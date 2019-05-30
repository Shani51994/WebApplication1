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
using System.Net;

namespace WebApplication1.Controllers
{
    public class FirstController : Controller
    {
        public ActionResult Welcome()
        {
            return View(); 
        }

        // GET: First
        public ActionResult Index(string ip, int port) {
            Command.Instance.connectToServer(ip, port);
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));

            ViewBag.lon = lon;
            ViewBag.lat = lat;
            
            return View("Index");
        }

        public ActionResult viewMapPath(string ip, int port, int time)
        {

            Command.Instance.connectToServer(ip, port);
            Session["time"] = time;
            return View();
        }


        [HttpPost]
        public string createXmlData() {
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
            Session["time"] = time;
            Session["seconds"] = seconds;
            Session["fileName"] = fileName;
            return View();
        }

        static bool isFirstWrite;

        [HttpPost]
        public string saveToFile()
        {
            string result = "";
            
            try
            {
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + Session["fileName"].ToString() + ".txt";
                Random rnd = new Random();
                string lon = (float.Parse(Command.Instance.send("get /position/longitude-deg")) + rnd.Next(50)).ToString();
                string lat = (float.Parse(Command.Instance.send("get /position/latitude-deg")) + rnd.Next(50)).ToString();
                string rudder = (float.Parse(Command.Instance.send("get /controls/flight/rudder")) + rnd.Next(50)).ToString();
                string throttle = (float.Parse(Command.Instance.send("get /controls/engines/current-engine/throttle")) + rnd.Next(50)).ToString();
               
                if(System.IO.File.Exists(fileName) && isFirstWrite)
                {
                    System.IO.File.Delete(fileName);
                }

                if (isFirstWrite)
                {
                    isFirstWrite = false;
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

            Session["time"] = time;
            Session["fileName"] = fileName;
            return View("ViewFilePath");
        }

        public string getValuesFromXML()
        {
            if (lineCounter == lines.Count)
            {
                return "nothing";
            }

            string line = lines[lineCounter];
            string[] splitLine = line.Split(',');

            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Data");

            writer.WriteElementString("Lon", splitLine[0]);
            writer.WriteElementString("Lat", splitLine[1]);
            writer.WriteElementString("Rudder", splitLine[2]);
            writer.WriteElementString("Throttle", splitLine[3]);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            lineCounter++;

            return sb.ToString();
        }

        public ActionResult Display(string stringToCheck, int number)
        {
            try
            {
                IPAddress.Parse(stringToCheck);
                return Index(stringToCheck, number);
            } catch
            {
                return ViewFilePath(stringToCheck, number);
            }
        }

        public void CloseConnection()
        {
            Command.Instance.closeClient();
            isFirstWrite = true;
        }
    }
}