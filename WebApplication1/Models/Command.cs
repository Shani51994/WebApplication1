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
        private TcpListener server;
        private TcpClient client;


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
            this.server = new TcpListener(endPoint);

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
            getFromServer();
            //this.networkStream = client.GetStream();
        }

        /*
         *sending commands from the auto pilot, after each command wait 2 sec for sending the next one
         */
        public void getFromServer()
        {
            NetworkStream stream = this.client.GetStream();
            BinaryReader reader = new BinaryReader(stream);
            String[] splitInput;

            while (!isConnected)
            {
                // read the input fron the simulator
                string input = "";
                char c;

                // read data untill \n
                while ((c = reader.ReadChar()) != '\n')
                {
                    input += c;
                }

                // splits the input
                splitInput = input.Split(',');

                // gets the lon and lat from the input and add them to the lon and lat in the instance
                //  FlightBoardViewModel.Instance.Lon = float.Parse(splitInput[0]);
                // FlightBoardViewModel.Instance.Lat = float.Parse(splitInput[1]);

                Console.WriteLine(splitInput[0]);
                Console.WriteLine(splitInput[1]);

            }
        }
        /*
         * close the connection to the client, and the server that are connected to him
         */
        public void closeClient()
        {
            this.isConnected = false;
            this.client.Close();
            this.server.Stop();
        }
    }
}
