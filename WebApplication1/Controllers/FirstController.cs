using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class FirstController : Controller
    {
        // GET: First
        public ActionResult Index(string ip, int port)
        {
            ViewBag.ip = ip;
            ViewBag.port = port;
            return View();
        }

    }
}