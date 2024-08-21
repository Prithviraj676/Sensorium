﻿using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace s_init { 
    internal class ServerInit
    {
	    public ServerInit()
	    {
            StartServer();
	    }


        private static TcpListener _server;
        private static StreamWriter _writer;
        private static IPEndPoint _ipEndPoint;
        private static bool _exe = true;

        internal async void StartServer()
        {

            try
            {
                _ipEndPoint = new IPEndPoint(IPAddress.Any, 1919);
                _server = new(_ipEndPoint);
                _server.Start();
                Console.WriteLine("The server has started at 1919");
                try
                {
                    TcpClient handler = await _server.AcceptTcpClientAsync();
                    Console.WriteLine("Client connected");
                    _writer = new StreamWriter(handler.GetStream(), Encoding.UTF8)
                    {
                        AutoFlush = true
                };
                    Thread serverThread = new Thread(
                        TransmitData
                        /* TcpClient client = _server.AcceptTcpClient();*/
                        );
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

        static void TransmitData()
        {
            while (exe)
            {
                _writer.WriteLine(valJson);
                Console.WriteLine("Sensor Data sent!");
                Thread.sleep(1000);
            }
        }

    }
}