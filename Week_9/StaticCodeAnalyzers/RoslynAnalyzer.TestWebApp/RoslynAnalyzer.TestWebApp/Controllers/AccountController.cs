using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoslynAnalyzer.TestWebApp.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult BlockUserAccount(int userId)
        {
            //actions
            return View();
        }

        public ActionResult UnblockUserAccount(int userId)
        {
            //actions
            return View();
        }
    }
}