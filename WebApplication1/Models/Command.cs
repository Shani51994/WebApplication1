using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.IO;


namespace WebApplication1.Models
{
    public class Command
    {
        private bool isConnected;
        private static Command instance = null;
        private NetworkStream networkStream;
        private TcpClient client;
        private StreamReader reader;


        public Command()
        {
            this.isConnected = false;
        }

        /*
         * singleton to creat only one instance of the command
        */
        public static Command Instance
        {
            get
            {
                // if not exist create new else send the exist instance 
                if (instance == null)
                {
                    instance = new Command();
                }
                return instance;
            }
        }


        /*
         * open client and connect to the server
         */
        public void connectToServer(string ip, int port)
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            this.client = new TcpClient();
    
            // connect to server
            while (!client.Connected)
            {
                try
                {
                    client.Connect(endPoint);
                }
                catch (Exception)
                {
                }
            }

            isConnected = true;
            this.networkStream = client.GetStream();
            reader = new StreamReader(networkStream);
        }


        public string send(string textUser)
        {
            if (!isConnected)
            {
                return "not connected" ;
            }
            string totalCommands = textUser + "\r\n";
            byte[] buffer = Encoding.ASCII.GetBytes(totalCommands);
            networkStream.Write(buffer, 0, buffer.Length);
            string returnData = reader.ReadLine();
            string[] words = returnData.Split('\'');
            return words[1];
        }
   
        /*
         * close the connection to the client, and the server that are connected to him
         */
        public void closeClient()
        {
            this.isConnected = false;
            this.client.Close();
        }
    }
}
