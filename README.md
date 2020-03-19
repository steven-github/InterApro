# Instrutions

Instructions to run project locally.

## 1. Install Dependencies

Go to Solution Explorer > InterApro > Packages [EXAMPLE](https://i.imgur.com/mEyVq7N.png) and install following dependencies:

[Microsoft.EntityFrameworkCore](https://i.imgur.com/zijB99L.png)

```bash
Microsoft.EntityFrameworkCore
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
```

## 2. Add Migrations and Update DataBase

Go to Tools > Nuget Package Manager > Package Manager Console [EXAMPLE](https://i.imgur.com/iaVTaC9.png) and run following commands:

[Add-Migration InitialCreate](https://i.imgur.com/6JZXzqQ.png)

[Update-Database](https://i.imgur.com/TSBwUPv.png)

```bash
Add-Migration InitialCreate
Update-Database
```

If you have any issues please let me know!