using System;
using System.Net;
using System.Net.Sockets;

namespace s_init { 
    public class ServerInit
    {
	    public ServerInit()
	    {
            StartServer();
	    }


        private static TcpListener _server;
        private static StreamWriter _writer;
        private static IPEndPoint _ipEndPoint;

        public async void StartServer()
        {

            try
            {
                _ipEndPoint = new IPEndPoint(IPAddress.Any, 1919);
                _server = new(_ipEndPoint);
                _server.Start();
                Console.WriteLine("The server has started at 1919");
                try
                {
                    using TcpClient handler = await _server.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected");
                    Thread serverThread = new Thread(() =>
                    {
                        /* TcpClient client = _server.AcceptTcpClient();*/
                        /*_writer = new StreamWriter(handler.GetStream(), Encoding.UTF8) 
                        { 
                            AutoFlush = true 
                        };*/
                    
                    });
                    serverThread.Start();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}