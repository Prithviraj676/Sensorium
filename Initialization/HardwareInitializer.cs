using System;
using LibreHardwareMonitor.Hardware;
using serialize;

namespace Initializer
{


	public class HardwareInitializer
    {
		//public HashSet<SensorType> sComp = new HashSet<SensorType>();



		public void HInitializer(Computer computer, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>>> comp)
		{
			JsonUtility jsonizer = new JsonUtility();
			SHwareInitializer subHware = new SHwareInitializer();
			SensorInitializer senInit = new SensorInitializer();

			foreach (IHardware hardware in computer.Hardware)
			{
                Dictionary<string, object> Hware = new Dictionary<string, Dictionary<string, Dictionary<string, object>();
				
				subHware.SInitializer(hardware, Hware);

				senInit.SensorInit(hardware, Hware);

				comp.Add(hardware.Name.ToString()+"_"+hardware.HardwareType.ToString(), Hware);

			}
			//Console.WriteLine(jsonizer.Json(comp));
		}
	}
}

//SubhardareInit
//hardwareInit
