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
using System.Text.Json;

using Visitor;
using serialize;
using Initializer;
using updater;
//using s_init;



namespace Main
{


    public class Program
    {

        private static bool _running = true;
        private static HardwareInitializer hardwareInitializer;
        private static HardwareUpdater hardwareUpdater;
        private static JsonUtility jsonizer;

        public static void Main()
        {
            Program obj = new Program();
            hardwareInitializer = new HardwareInitializer();
            hardwareUpdater = new HardwareUpdater();
            jsonizer = new JsonUtility();
            obj.Monitor();
            Console.ReadKey();
            Console.CancelKeyPress += new ConsoleCancelEventHandler(OnExit);
            Console.ReadLine();
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
            Dictionary<string, object> data = new Dictionary<string, object>(); 
            Console.WriteLine(jsonizer.Json(data));
            hardwareInitializer.HInitializer(computer, data);
            //while (_running){
            Console.WriteLine("\n\n\t\tUpdated\n\n");
                //hardwareUpdater.HwareUpdater(computer, data);
                //Console.WriteLine(jsonizer.Json(data));
            //}
            //Initializer ini = new Initializer(computer, sComp);


            HashSet<HardwareType> hardwareComp = new HashSet<HardwareType>
            {
                HardwareType.Motherboard,
                HardwareType.Cpu,
                HardwareType.Memory,
                HardwareType.GpuNvidia,
                HardwareType.Storage,
                HardwareType.Network
            };
            Dictionary<string, object> uniHardware = new Dictionary<string, object>();

            foreach (IHardware hardware in computer.Hardware)
            {
                //Console.WriteLine("Hardware: {0} :: {1}", hardware.Name, hardware.HardwareType);
               
                Dictionary<string, object> Hware = new Dictionary<string, object>()
                {
                    { "hName", hardware.Name },
                    { "hType", hardware.HardwareType.ToString() },
                    { "Sware", null}
                };

                foreach (IHardware subhardware in hardware.SubHardware)
                {
                    //Console.WriteLine("\tSubhardware: {0}  ::  {1}", subhardware.Name, subhardware.HardwareType);

                    Dictionary<string, object> Sware = new Dictionary<string, object>()
                    {
                        { "sbName", subhardware.Name },
                        { "sbType", subhardware.HardwareType.ToString() },
                        { "sensors", null }
                    };

                    foreach (ISensor sensor in subhardware.Sensors)
                    {   
                        if(sensor.Value != 0)
                        {
                            Dictionary<string, object> Sensors = new Dictionary<string, object>
                            {
                                { "snName", sensor.Name },
                                { "snType", sensor.SensorType },
                                { "Values", null }
                            };

                            //Console.WriteLine("\t\tSensor: Type: {2} :: {0}, value: {1}", sensor.Name, sensor.Value, sensor.SensorType.GetType());
                        }
                    }

                    //Sware["sensors"] = Sensors;
                }

                //Hware["Sware"] = Sware;
                //uniHardware.Add(hardware.HardwareType.ToString(), Hware);

                foreach (ISensor sensor in hardware.Sensors)
                {   
                    if (sensor.Value != 0)
                    {

                    //Console.WriteLine("\tSensor: {2} :: {0}, value: {1}", sensor.Name, sensor.Value, sensor.SensorType);
                    }
                }

                //Thread.Sleep(2000);
            }


            string Jsonized = JsonConvert.SerializeObject(uniHardware, Formatting.Indented);
            //Console.WriteLine(Jsonized);
            computer.Close();



        }


    private static void OnExit(Object sender, ConsoleCancelEventArgs args)
    {
        _running = false;
        args.Cancel = true;
        //_server.Stop();
    }
    }


}

    














/*
 
 
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

        HashSet<HardwareType> hardwareComp = new HashSet<HardwareType>
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
                            if (sensor.Value != 0)
                            {
                                Console.WriteLine("\n\t\t\tSensor: ", sensor, "  Vlaue: ", sensor.Value);
                            }

                        }
                            foreach (ISensor sensor in hardware.Sensors)
                            {
                                if (sensor.Value != 0)
                                {
                                    Console.WriteLine(sensor.Name + ": \t" + sensor.SensorType + "\tMin: " + sensor.Min + "\tMax: " + sensor.Max + "\tValue: " + sensor.Value);
                                    jsonizer.JsonParse(sensor.Name, sensor.SensorType.ToString(), sensor.Min, sensor.Max, sensor.Value, sensorsDict);
                                }
                            }
                        
                        Console.WriteLine("\n\nStart: \n\n" + jsonizer.Json(sensorsDict));

                    }


                    Thread.Sleep(200);

                    computer.Accept(new UpdateVisitor());

                    //Console.WriteLine(SensorType.Fan);

                }

                computer.Close();
            }
        }
    }

 */