using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Xml;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class FirstController : Controller
    {

        // GET: First
        public ActionResult Index(string ip, int port)
        {
            Command.Instance.connectToServer(ip, port);
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));

            ViewBag.lon = lon;
            ViewBag.lat = lat;
            
            return View();
        }

        public ActionResult viewMapPath(string ip, int port, int time)
        {
            Command.Instance.connectToServer(ip, port);
            Session["time"] = time;
            return View();
        }


        [HttpPost]
        public string createXmlData()
        {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            Random rnd = new Random();

            string lon = Command.Instance.send("get /position/longitude-deg") + rnd.Next(50);
            string lat = Command.Instance.send("get /position/latitude-deg") + rnd.Next(50);

            writer.WriteElementString("Lon", lon);
            writer.WriteElementString("Lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return sb.ToString();


        }


    }
}