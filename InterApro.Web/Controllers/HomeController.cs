using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using InterApro.Database;
using InterApro.Database.Tables;
using InterApro.Web.Models;

using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity;
using InterApro.Web.Data;
using Microsoft.AspNetCore.Authorization;
using InterApro.Models.Mail;
using InterApro.Business.Mail;

namespace InterApro.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly InterAproDBContext _context;
        private readonly ILogger<HomeController> _logger;
        private RoleManager<InterAproWebRole> _roleManager;
        private UserManager<InterAproWebUser> _userManager;
        private InterAproWebUser _currentUser;
        private IEmailSender _emailSender;

        public HomeController(ILogger<HomeController> logger, InterAproDBContext context, UserManager<InterAproWebUser> userManager, RoleManager<InterAproWebRole> roleManager, IEmailSender emailSender)
        {
            _logger = logger;
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _emailSender = emailSender;
        }


        public async Task<IActionResult> Index()
        {
            _currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var newRequests = _context.Request.AsQueryable();
            var approvedRequests = _context.Request.AsQueryable();
            var rejectedRequests = _context.Request.AsQueryable();

            if (await _userManager.IsInRoleAsync(_currentUser, "Buyer"))
            {
                newRequests = newRequests.Where(x => x.BuyerId == _currentUser.Id);
                approvedRequests = approvedRequests.Where(x => x.BuyerId == _currentUser.Id);
                rejectedRequests = rejectedRequests.Where(x => x.BuyerId == _currentUser.Id);


            }
            else if (await _userManager.IsInRoleAsync(_currentUser, "Manager"))
            {
                newRequests = newRequests.Where(x => x.ManagerId == _currentUser.Id);
                approvedRequests = approvedRequests.Where(x => x.ManagerId == _currentUser.Id);
                rejectedRequests = rejectedRequests.Where(x => x.ManagerId == _currentUser.Id);
            }
            else // Todos los roles de finance
            {
                newRequests = newRequests.Where(x => x.FinanceId == _currentUser.Id);
                approvedRequests = approvedRequests.Where(x => x.FinanceId == _currentUser.Id);
                rejectedRequests = rejectedRequests.Where(x => x.FinanceId == _currentUser.Id);
            }

            newRequests = newRequests.Where(x => x.RequestStatusId == 1);
            approvedRequests = approvedRequests.Where(x => x.RequestStatusId == 3 || x.RequestStatusId == 5);
            rejectedRequests = rejectedRequests.Where(x => x.RequestStatusId == 2 || x.RequestStatusId == 4);

            newRequests = newRequests.Include(r => r.RequestStatus);
            approvedRequests = approvedRequests.Include(r => r.RequestStatus);
            rejectedRequests = rejectedRequests.Include(r => r.RequestStatus);

            var viewModel = new RequestListsViewModel()
            {
                NewRequests = newRequests.ToList(),
                ApprovedRequests = approvedRequests.ToList(),
                RejectedRequests = rejectedRequests.ToList()
            };

            return View(viewModel);
        }

        // GET: Requests/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var request = await _context.Request
                .Include(r => r.RequestStatus)
                .FirstOrDefaultAsync(m => m.RequestId == id);
            if (request == null)
            {
                return NotFound();
            }

            return View(request);
        }

        // GET: Requests/Create
        public IActionResult Create()
        {
            ViewData["RequestStatusId"] = new SelectList(_context.RequestStatus, "RequestStatusId", "RequestStatusId");
            return View();
        }

        // POST: Requests/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RequestId,RequestDescription,RequestAmount,BuyerId,ManagerId,FinanceId,RequestStatusId,Created,Updated")] Request request)
        {
            if (ModelState.IsValid)
            {
                _currentUser = await _userManager.GetUserAsync(HttpContext.User);

                request.BuyerId = _currentUser.Id;
                request.RequestStatusId = 1;
                request.ManagerId = _currentUser.ManagerId;
                request.Created = DateTime.Now;

                _context.Add(request);
                await _context.SaveChangesAsync();
                await SendEmail(request);
                TempData["LastActionResult"] = "Request Created";
                return RedirectToAction(nameof(Index));
            }
            ViewData["RequestStatusId"] = new SelectList(_context.RequestStatus, "RequestStatusId", "RequestStatusId", request.RequestStatusId);

            
            return RedirectToAction("Index");
        }

        /// <summary>
        /// Aprovacion de solicitud
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Approve(long? id)
        {
            var request = await _context.Request.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            try
            {

                _currentUser = await _userManager.GetUserAsync(HttpContext.User);

                request.RequestStatus = null;
                if (await _userManager.IsInRoleAsync(_currentUser, "Manager"))
                {
                    request.ManagerId = _currentUser.Id;
                    request.RequestStatusId = 3;

                    //Asignacion de finanzas segun el monto
                    var roles = _roleManager.Roles.ToList();

                    foreach (var role in roles)
                    {
                        if(request.RequestAmount >= role.MinAmout && request.RequestAmount <= role.MaxAmount)
                        {
                            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

                            //Elije un usuario de finanzas al azar
                            Random r = new Random();
                            int rInt = r.Next(0, usersInRole.Count); //for ints

                            var finUser = usersInRole[rInt];

                            request.FinanceId = finUser.Id;
                        }
                    }
                }
                else // Todos los roles de finance
                {
                    request.RequestStatusId = 5;
                }

                request.Updated = DateTime.Now;
                _context.Update(request);
                await _context.SaveChangesAsync();
                await SendEmail(request);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["LastActionResult"] = "Request Approved";
            return RedirectToAction(nameof(Index));

        }

        /// <summary>
        /// Rechazo de solicitud
        /// </summary>
        /// <param name="id">Id de solicitud</param>
        /// <returns></returns>
        public async Task<IActionResult> Reject(long? id)
        {
            var request = await _context.Request.FindAsync(id);

            if (request == null)
            {
                return NotFound();
            }

            try
            {

                _currentUser = await _userManager.GetUserAsync(HttpContext.User);

                request.RequestStatus = null;
                if (await _userManager.IsInRoleAsync(_currentUser, "Manager"))
                {
                    request.ManagerId = _currentUser.Id;
                    request.RequestStatusId = 2;
                }
                else // Todos los roles de finance
                {
                    request.FinanceId = _currentUser.Id;
                    request.RequestStatusId = 4;
                }
                request.Updated = DateTime.Now;
                _context.Update(request);
                await _context.SaveChangesAsync();
                await SendEmail(request);

            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RequestExists(request.RequestId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            TempData["LastActionResult"] = "Request Rejected";
            return RedirectToAction(nameof(Index));

        }

        /// <summary>
        /// Envio de notificaciones por email
        /// </summary>
        /// <param name="request">Solicitud a Notificar</param>
        /// <returns></returns>
        private async Task SendEmail(Request request)
        {
            _currentUser = await _userManager.GetUserAsync(HttpContext.User);

            var callbackUrl = Url.Action("Details",
                        "Home",
                        values: new { id = request.RequestId},
                        protocol: Request.Scheme);

            if (await _userManager.IsInRoleAsync(_currentUser, "Buyer"))
            {
                if(request.RequestStatusId == 1)
                {
                    var manager = await _userManager.FindByIdAsync(_currentUser.ManagerId);

                    string[] tokens = new string[] { _currentUser.FullName, manager.FullName, callbackUrl};
                    var message = new Message(new string[] { _currentUser.Email }, "Request Created", "", EmailTypes.CreatedToBuyer, tokens);
                    await _emailSender.SendEmailAsync(message);

                    tokens = new string[] { manager.FullName,_currentUser.FullName, callbackUrl };
                    var managerMessage = new Message(new string[] { manager.Email }, "Request Created", "", EmailTypes.CreatedToManager, tokens);
                    await _emailSender.SendEmailAsync(managerMessage);
                }
            }
            else if (await _userManager.IsInRoleAsync(_currentUser, "Manager"))
            {
                if (request.RequestStatusId == 2)
                {
                    var buyer = await _userManager.FindByIdAsync(request.BuyerId);

                    string[] tokens = new string[] { buyer.FullName, _currentUser.FullName, callbackUrl };
                    var message = new Message(new string[] { buyer.Email }, "Request Rejected by Manager", "", EmailTypes.RejectedByManager, tokens);
                    await _emailSender.SendEmailAsync(message);

                }
                else if(request.RequestStatusId == 3)
                {
                    var buyer = await _userManager.FindByIdAsync(request.BuyerId);
                    var finance = await _userManager.FindByIdAsync(request.FinanceId);

                    string[] tokens = new string[] { buyer.FullName, _currentUser.FullName, callbackUrl };
                    var message = new Message(new string[] { buyer.Email }, "Request Approved by Manager", "", EmailTypes.ApprovedByManager, tokens);
                    await _emailSender.SendEmailAsync(message);

                    tokens = new string[] { finance.FullName, buyer.FullName, _currentUser.FullName, callbackUrl };
                    message = new Message(new string[] { finance.Email }, "Request Approved by Manager", "", EmailTypes.ApprovedToFinance, tokens);
                    await _emailSender.SendEmailAsync(message);
                }
            }
            else // Todos los roles de finance
            {
                if (request.RequestStatusId == 4)
                {
                    var buyer = await _userManager.FindByIdAsync(request.BuyerId);

                    string[] tokens = new string[] { buyer.FullName, _currentUser.FullName, callbackUrl };
                    var message = new Message(new string[] { buyer.Email }, "Request Rejected by Finance", "", EmailTypes.RejectedByFinance, tokens);
                    await _emailSender.SendEmailAsync(message);

                }
                else if (request.RequestStatusId == 5)
                {
                    var buyer = await _userManager.FindByIdAsync(request.BuyerId);

                    string[] tokens = new string[] { buyer.FullName, _currentUser.FullName, callbackUrl };
                    var message = new Message(new string[] { buyer.Email }, "Request Approved by Finance", "", EmailTypes.ApprovedByFinance, tokens);
                    await _emailSender.SendEmailAsync(message);
                }
            }

            
        }

        private bool RequestExists(long id)
        {
            return _context.Request.Any(e => e.RequestId == id);
        }

    }
}
