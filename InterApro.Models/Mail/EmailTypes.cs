using System;
using System.Collections.Generic;
using System.Text;

namespace InterApro.Models.Mail
{
    /// <summary>
    /// Enumerador para tipos de plantillas de correo a usar
    /// </summary>
    public enum EmailTypes
    {
        None,
        CreatedToBuyer,
        CreatedToBoss,
        RejectedByBoss,
        ApprovedByBoss,
        ApprovedToFinance,
        RejectedByFinance,
        ApprovedByFinance,
    }
}
