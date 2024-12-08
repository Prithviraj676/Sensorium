//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Text;

//namespace s_init { 
//    internal class ServerInit
//    {
//        private static TcpListener _server;
//        private static StreamWriter _writer;
//        private static IPEndPoint _ipEndPoint;

//        private static bool _exec = true;
//        protected static string valJson = "";

//	    public ServerInit()
//	    {
//            StartServer();
//	    }

//        public ServerInit(bool _exec, string valJson)
//        {
//            this._exec = _exec;
//            this.valJson = valJson;
//        }

//        internal async void StartServer(string vJson)
//        {
//            try
//            {
//                _ipEndPoint = new IPEndPoint(IPAddress.Any, 1919);
//                _server = new(_ipEndPoint);
//                _server.Start();
//                Console.WriteLine("The server has started at 1919");
//                try
//                {
//                    TcpClient handler = await _server.AcceptTcpClientAsync();
//                    Console.WriteLine("Client connected");
//                    _writer = new StreamWriter(handler.GetStream(), Encoding.UTF8)
//                    {
//                        AutoFlush = true
//                };
//                    Thread serverThread = new Thread(
//                        TransmitData
//                        /* TcpClient client = _server.AcceptTcpClient();*/
//                        );
//                    serverThread.Start();
//                }
//                catch (Exception ex)
//                {
//                    Console.WriteLine(ex.ToString());
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.ToString());
//            }
//        }

        

//    }
//}