using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FirstController : Controller
    {

        // GET: First
        public ActionResult Index(string ip, int port)
        {
            ViewBag.ip = ip;
            ViewBag.port = port;
            Command.Instance.connectToServer(ip, port);
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));

            ViewBag.lon = lon;
            ViewBag.lat = lat;

            Console.WriteLine(lon);
            Console.WriteLine(lat);


            return View();
        }

    }
}