using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using JustTrade.Database;

namespace JustTrade.Controllers
{
    public class UserController : Controller
    {
        public ActionResult Index()
        {
            return View ();
        }

        [HttpPost]
        public ActionResult Add(User user)
        {
            Repository<User>.Add(user);

            return View ();
        }

    }
}
