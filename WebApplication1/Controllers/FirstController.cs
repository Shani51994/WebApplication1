using System;
using System.IO;
using System.Collections.Generic;
using System.Diagnostics;
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
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));
            ViewBag.lon = lon;
            ViewBag.lat = lat;
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

            string lon = (float.Parse(Command.Instance.send("get /position/longitude-deg")) + rnd.Next(50)).ToString();
            string lat = (float.Parse(Command.Instance.send("get /position/latitude-deg")) + rnd.Next(50)).ToString();

            writer.WriteElementString("Lon", lon);
            writer.WriteElementString("Lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return sb.ToString();


        }

        public ActionResult Save(string ip, int port, int time, int seconds, string fileName)
        {
            Command.Instance.connectToServer(ip, port);
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));
            ViewBag.lon = lon;
            ViewBag.lat = lat;
            //ViewBag.fileName = fileName;
            Session["time"] = time;
            Session["fileName"] = fileName;
            return View();
        }


        [HttpPost]
        public string saveToFile()
        {
            string result = "";
            
            try
            {
                //string fileName = @"C:\Users\Home\source\repos\Shani51994\WebApplication1\WebApplication1\" + Session["fileName"].ToString() + ".txt";
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + Session["fileName"].ToString() + ".txt";
                Random rnd = new Random();
                string lon = (float.Parse(Command.Instance.send("get /position/longitude-deg")) + rnd.Next(50)).ToString();
                string lat = (float.Parse(Command.Instance.send("get /position/latitude-deg")) + rnd.Next(50)).ToString();
                string rudder = (float.Parse(Command.Instance.send("get /controls/flight/rudder")) + rnd.Next(50)).ToString();
                string throttle = (float.Parse(Command.Instance.send("get /controls/engines/current-engine/throttle")) + rnd.Next(50)).ToString();


                // checks if file isn't exist
                if (!System.IO.File.Exists(fileName))
                {
                    // creates a new file
                    using (FileStream fs = System.IO.File.Create(fileName))
                    {
                    }
                }

                // write data to the file
                using (StreamWriter writeText = System.IO.File.AppendText(fileName))
                {
                    writeText.WriteLine(lon + "," + lat + "," + rudder + "," + throttle);
                }

                //Initiate XML stuff
                StringBuilder sb = new StringBuilder();
                XmlWriterSettings settings = new XmlWriterSettings();
                XmlWriter writer = XmlWriter.Create(sb, settings);

                writer.WriteStartDocument();
                writer.WriteStartElement("Data");

                writer.WriteElementString("Lon", lon);
                writer.WriteElementString("Lat", lat);
                writer.WriteElementString("Rudder", rudder);
                writer.WriteElementString("Throttle", throttle);
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Flush();

                return sb.ToString();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return result;
        }


        static int lineCounter;
        static List<string> lines = new List<string>();

        public ActionResult ViewFilePath(string fileName, int time)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            lineCounter = 0;

            // read all file, and puts it in list
            using (StreamReader sr = System.IO.File.OpenText(path))
            {
                string s = "";

                while ((s = sr.ReadLine()) != null)
                {
                    lines.Add(s);    
                }
            }

            // get first values
            string line = lines[lineCounter];
            string[] splitLine = line.Split(',');

            ViewBag.lon = splitLine[0];
            ViewBag.lat = splitLine[1];
            ViewBag.rudder = splitLine[2];
            ViewBag.throttle = splitLine[3];

            lineCounter++;

            Session["time"] = time;
            Session["fileName"] = fileName;
            return View();
        }
        public void getValues()
        {
            string line = lines[lineCounter];
            string[] splitLine = line.Split(',');

            ViewBag.lon = splitLine[0];
            ViewBag.lat = splitLine[1];
            ViewBag.rudder = splitLine[2];
            ViewBag.throttle = splitLine[3];

            lineCounter++;
        }
    }
}