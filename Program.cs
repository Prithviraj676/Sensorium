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
using serialize;
//using s_init;




public partial class Program
{
    private static bool _running = true;

    public static void Main()
    {
        Program one = new Program();
        //ServerInit s_obj = new ServerInit();
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

        HashSet< HardwareType> hardwareComp = new HashSet<HardwareType> 
        { 
            HardwareType.Motherboard, 
            HardwareType.Cpu, 
            HardwareType.Memory,
            HardwareType.GpuNvidia,
            HardwareType.Storage,
            HardwareType.Network
        };

        while (_running)
        {
            Dictionary<string, Dictionary<string, object>> sensorsDict = new Dictionary<string, Dictionary<string, object>>();

            Dictionary<string, Dictionary<string, float?>> vFanDict = new Dictionary<string, Dictionary<string, float?>>();

            JsonUtility jsonizer = new JsonUtility();

            foreach (IHardware hardware in computer.Hardware)
            {
                Console.WriteLine("\t\t\tHardware: {0}", hardware.Name + " : " + hardware.HardwareType);
                
                hardware.Update();
                if (hardwareComp.Contains(hardware.HardwareType))
                {
                    foreach (IHardware subhardware in hardware.SubHardware)
                    {
                        //Console.WriteLine("Entered");
                        Console.WriteLine("\tSubhardware: {0}", subhardware.Name + "\n");
                            foreach (ISensor sensor in subhardware.Sensors)
                            {
                                if(sensor.Value != 0)
                                {
                                    //jsonizer.JsonParse(sensor.Name, sensor.SensorType.ToString(), sensor.Min, sensor.Max, sensor.Value, sensorsDict);
                                }
                                if ((sensor.SensorType == SensorType.Fan) && sensor.Value != 0)
                                {

                                //    Console.WriteLine("\n\nUpdated Approach: " + jsonizer.Json(sensorsDict));



                                //        Dictionary<string, float?> val = new Dictionary<string, float?>();
                                //        sb.AppendLine($"\t\tSensor: {sensor.Name}, value: {sensor.Value}, MinValue: {sensor.Min}, MaxValue: {sensor.Max}");
                                    //Console.WriteLine($"TYPE: {sensor.SensorType},\t\tSensor: {sensor.Name},          Min: {sensor.Min},        value: {sensor.Value},            Max: {sensor.Max}, Index{sensor.Index}");

                                //        val.Add("Min", sensor.Min);
                                //        val.Add("Max", sensor.Max);
                                //        val.Add("Value", sensor.Value);


                                //        if (vFanDict.ContainsKey(sensor.Name))
                                //        {
                                //            vFanDict[sensor.Name] = val;
                                //        }
                                //        else
                                //        {
                                //            vFanDict.Add(sensor.Name, val);
                                //        }


                                //Console.WriteLine("\n\nInitial Approach: " + jsonizer.Json(vFanDict));
                                //    foreach(var s in vFanDict)
                                //    {
                                //        Console.WriteLine("Sensor: " + s.Key + ": \n");
                                //        foreach(var v in s.Value)
                                //        {
                                //            Console.WriteLine("\t\t" + v.Key + " : " + v.Value + "\n");
                                //        }

                                //    }

                                }

                            }
                        //Console.WriteLine("\n\nThe Dictionary: ");
                        //Console.Write("\n\nThe Dictionary: " + JsonUtility.JsonParse(vFanDict));

                    }

                        foreach (ISensor sensor in hardware.Sensors)
                        {
                            if (sensor.Value != 0)
                            {
                                //Console.WriteLine(sensor.Name + ": \t" + sensor.SensorType + "\tMin: " + sensor.Min + "\tMax: " + sensor.Max + "\tValue: " + sensor.Value);
                                jsonizer.JsonParse(sensor.Name, sensor.SensorType.ToString(), sensor.Min, sensor.Max, sensor.Value, sensorsDict);
                            }
                        }
                }
                Console.WriteLine("\n\nStart: \n\n" + jsonizer.Json(sensorsDict));
            }
            /* Console.WriteLine("\n\n\n\n");
            */
            //string data = sb.ToString();
            //_writer?.WriteLine(data);

            Thread.Sleep(200000);

            computer.Accept(new UpdateVisitor());

            //Console.WriteLine(SensorType.Fan);

        }

        computer.Close();

    }
    private static void OnExit(Object sender, ConsoleCancelEventArgs args)
    {
        _running = false;
        args.Cancel = true;
        //_server.Stop();
    }
}