// See https://aka.ms/new-console-template for more information
using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

/*using System.Threading;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;*/

using LibreHardwareMonitor.Hardware;

using Newtonsoft.Json;

using Visitor;
using s_init;




public partial class Program
{
    private static bool _running = true;
    

    public static void Main()
    {
        Program one = new Program();
        ServerInit s_obj = new ServerInit();
        s_obj.StartServer();
        one.Monitor();
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
            Dictionary<string, Dictionary<string, float?>> Vsensor = new Dictionary<string, Dictionary<string, float?>>();
            StringBuilder sb = new StringBuilder();

            foreach (IHardware hardware in computer.Hardware)
            {
/*                Console.WriteLine("Hardware: {0}", hardware.Name);
*/                hardware.Update();
                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    Console.WriteLine("OUTER:           \tSubhardware: {0}", subhardware.Name);

                    foreach (ISensor sensor in subhardware.Sensors)
                    {
                        if((sensor.SensorType == SensorType.Fan) && sensor.Value != 0)
                        {
                            Dictionary<string, float?> val= new Dictionary<string, float?>();
                            /*sb.AppendLine($"\t\tSensor: {sensor.Name}, value: {sensor.Value}, MinValue: {sensor.Min}, MaxValue: {sensor.Max}");
                            Console.WriteLine($"TYPE\t\tSensor: {sensor.Name},          Min: {sensor.Min},        value: {sensor.Value},            Max: {sensor.Max}, Index{sensor.Index}");
*/
                            val.Add("Min", sensor.Min);
                            val.Add("Max", sensor.Max);
                            val.Add("Value", sensor.Value);

                            if (Vsensor.ContainsKey(sensor.Name))
                            {
                                Vsensor[sensor.Name] = val;
                            }
                            else
                            {
                                Vsensor.Add(sensor.Name, val);
                            }

                            /*foreach(var s in Vsensor)
                            {
                                Console.WriteLine("Sensor: " + s.Key + ": \n");
                                foreach(var v in s.Value)
                                {
                                    Console.WriteLine("\t\t" + v.Key + " : " + v.Value + "\n");
                                }

                            }*/

                        }

                    }
                            string valJson = JsonConvert.SerializeObject(Vsensor);
                            Console.WriteLine(valJson + "\n\n");
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