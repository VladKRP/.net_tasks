using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace RoslynAnalyzer.TestWebApp.Controllers
{
    public class UserManageController : Controller
    {
        [Authorize]
        public ActionResult EditUserProfile(int userId)
        {
            return View();
        }

        [Authorize]
        public ActionResult GetFullUserProfile(int userId)
        {
            return View();
        }


    }
}