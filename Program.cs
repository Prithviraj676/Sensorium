// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using LibreHardwareMonitor.Hardware;
using System.Runtime.CompilerServices;

public partial class UpdateVisitor : IVisitor
{
    public void VisitComputer(IComputer computer)
    {
        computer.Traverse(this);
    }
    public void VisitHardware(IHardware hardware)
    {
        hardware.Update();
        foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
    }
    public void VisitSensor(ISensor sensor) { }
    public void VisitParameter(IParameter parameter) { }
}


public partial class Program
{
    private static bool _running = true;
    private static TcpListener _server;
    private static StreamWriter _writer;
    private static IPEndPoint _ipEndPoint;

    public static void Main()
    {
        Program one = new Program();
        one.StartServer();
        one.Monitor();
    }

    private async void StartServer()
    {

        try
        {
            _ipEndPoint = new IPEndPoint(IPAddress.Any, 1919);
            _server = new (_ipEndPoint);
            _server.Start();
            Console.WriteLine("The server has started at 1919");
            try
            {
                using TcpClient handler = await _server.AcceptTcpClientAsync();
                Console.WriteLine("Client connected");
                Thread serverThread = new Thread(() =>
                {
                   /* TcpClient client = _server.AcceptTcpClient();*/
                    _writer = new StreamWriter(handler.GetStream(), Encoding.UTF8) 
                    { 
                        AutoFlush = true 
                    };
                });
                serverThread.Start();
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.ToString());
        }
    }

    public void Monitor()
    {
        Computer computer = new Computer
        {
            IsCpuEnabled = true,
            IsGpuEnabled = true,
            IsMemoryEnabled = true,
            IsMotherboardEnabled = true,
            IsControllerEnabled = true,
            IsNetworkEnabled = true,
            IsStorageEnabled = true,
            IsPsuEnabled = true
        };

        computer.Open();
        computer.Accept(new UpdateVisitor());

        Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);

        while (_running)
        {
            Dictionary<string, Dictionary<string, float>> val = new Dictionary<string, Dictionary<string, float>>();
            StringBuilder sb = new StringBuilder();

            foreach (IHardware hardware in computer.Hardware)
            {
/*                Console.WriteLine("Hardware: {0}", hardware.Name);
*/                hardware.Update();
                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("\tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        if((sensor.SensorType == SensorType.Fan) && sensor.Value != 0)
                        {
                            Dictionary<string, float?> val= new Dictionary<string, float?>();
                            /*sb.AppendLine($"\t\tSensor: {sensor.Name}, value: {sensor.Value}, MinValue: {sensor.Min}, MaxValue: {sensor.Max}");
                            Console.WriteLine($"TYPE\t\tSensor: {sensor.Name},          Min: {sensor.Min},        value: {sensor.Value},            Max: {sensor.Max}, Index{sensor.Index}");
                        }

                    }
                }

                foreach (ISensor sensor in hardware.Sensors)
                {
                    /*Console.WriteLine($"EXTERNAL:           \tSensor Type: {sensor.SensorType},         Sensor: {sensor.Name}, value: {sensor.Value}");*/
                }
            }
           /* Console.WriteLine("\n\n\n\n");
           */
            string data = sb.ToString();
            _writer?.WriteLine(data);

            Thread.Sleep(2000);

            computer.Accept(new UpdateVisitor());

            Console.WriteLine(SensorType.Fan);

        }

        computer.Close();

    }
        private static void OnExit(Object sender, ConsoleCancelEventArgs args)
        {
            _running = false;
            args.Cancel = true;
            _server.Stop();
        }
}