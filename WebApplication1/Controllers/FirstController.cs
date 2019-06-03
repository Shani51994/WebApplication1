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
        /**
         * This action returns the default view.
         */
        public ActionResult Welcome()
        {
            return View(); 
        }

        /**
         * This action deals with shows the current position on the map image.
         */
        // GET: First
        public ActionResult Index(string ip, int port) {
            // connects to the server
            Command.Instance.connectToServer(ip, port);

            // gets the lon and lat values from the server
            float lon = float.Parse(Command.Instance.send("get /position/longitude-deg"));
            float lat = float.Parse(Command.Instance.send("get /position/latitude-deg"));

            ViewBag.lon = lon;
            ViewBag.lat = lat;
            
            return View("Index");
        }

        /**
         * This action deals with shows the whole path's positions of the plane on the map.
         */
        public ActionResult ViewMapPath(string ip, int port, int time)
        {
            // connects to the server
            Command.Instance.connectToServer(ip, port);

            Session["time"] = time;
            return View();
        }

        /**
         * This function deals with creation of XML file with the lon and lat values.
         */
        [HttpPost]
        public string CreateXmlData() {
            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Location");
            Random rnd = new Random();

            // get the lon and lat values
            string lon = (float.Parse(Command.Instance.send("get /position/longitude-deg")) + rnd.Next(50)).ToString();
            string lat = (float.Parse(Command.Instance.send("get /position/latitude-deg")) + rnd.Next(50)).ToString();

            // write the values to the XML
            writer.WriteElementString("Lon", lon);
            writer.WriteElementString("Lat", lat);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            return sb.ToString();
           }

        /**
         * This function deals with show and save to file the plane's positions 
         */
        public ActionResult Save(string ip, int port, int time, int seconds, string fileName)
        {
            // connets to the server
            Command.Instance.connectToServer(ip, port);
            Session["time"] = time;
            Session["seconds"] = seconds;
            Session["fileName"] = fileName;
            return View();
        }

        static bool isFirstWrite = true;

        [HttpPost]
        /**
         * This function deals with save the planne's details into a file, and also write them to XML
         */
        public string SaveToFile()
        {
            string result = "";
            
            try
            {
                // get the full path for the file
                string fileName = AppDomain.CurrentDomain.BaseDirectory + @"\" + Session["fileName"].ToString() + ".txt";
                Random rnd = new Random();

                // gets the plane's details: lon, lat, ruuder and throttle
                string lon = (float.Parse(Command.Instance.send("get /position/longitude-deg")) + rnd.Next(50)).ToString();
                string lat = (float.Parse(Command.Instance.send("get /position/latitude-deg")) + rnd.Next(50)).ToString();
                string rudder = (float.Parse(Command.Instance.send("get /controls/flight/rudder")) + rnd.Next(50)).ToString();
                string throttle = (float.Parse(Command.Instance.send("get /controls/engines/current-engine/throttle")) + rnd.Next(50)).ToString();
               
                // checks if need to delete an ezist file for new flight
                if(System.IO.File.Exists(fileName) && isFirstWrite)
                {
                    System.IO.File.Delete(fileName);
                }

                // checks if it's the first time we write to the file
                if (isFirstWrite)
                {
                    isFirstWrite = false;
                }

                // write (append) data to the file
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

                // write the values to the XML
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

        /**
         * This function deals with show the flight path from a given file
         */
        public ActionResult ViewFilePath(string fileName, int time)
        {
            // gets the full path of the file
            string path = AppDomain.CurrentDomain.BaseDirectory + @"\" + fileName + ".txt";
            lineCounter = 0;

            // read all file, and puts each line in a list
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

        /**
         * This function deals with put the lines of the list in an XML, each line every time
         */
        public string GetValuesFromXML()
        {
            if (lineCounter == lines.Count)
            {
                return "nothing";
            }

            // gets the current line
            string line = lines[lineCounter];

            // split the line by ',' to get all values
            string[] splitLine = line.Split(',');

            //Initiate XML stuff
            StringBuilder sb = new StringBuilder();
            XmlWriterSettings settings = new XmlWriterSettings();
            XmlWriter writer = XmlWriter.Create(sb, settings);

            writer.WriteStartDocument();
            writer.WriteStartElement("Data");

            // puts all values in the XML
            writer.WriteElementString("Lon", splitLine[0]);
            writer.WriteElementString("Lat", splitLine[1]);
            writer.WriteElementString("Rudder", splitLine[2]);
            writer.WriteElementString("Throttle", splitLine[3]);
            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Flush();

            // goes to the next line
            lineCounter++;

            return sb.ToString();
        }

        /**
         * This function deals with the check of which view to return, becaues same values
         */
        public ActionResult Display(string stringToCheck, int number)
        {
            try
            {
                // checks if the given string is an IP address
                IPAddress.Parse(stringToCheck);
                return Index(stringToCheck, number);
            } catch
            {
                return ViewFilePath(stringToCheck, number);
            }
        }

        /**
         * This function deals with the things should be done between each pass from view to view
         */
        public void CloseConnection()
        {
            // closes the client
            Command.Instance.closeClient();

            // initialzes values
            isFirstWrite = true;
            lines = new List<string>();
        }
    }
}