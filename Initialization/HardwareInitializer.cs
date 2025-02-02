using System;
using LibreHardwareMonitor.Hardware;
using serialize;

namespace Initializer
{


	public class HardwareInitializer
    {
		//public HashSet<SensorType> sComp = new HashSet<SensorType>();



		public void HInitializer(Computer computer, Dictionary<string, object> comp)
		{
			JsonUtility jsonizer = new JsonUtility();
			SHwareInitializer subHware = new SHwareInitializer();


			foreach (IHardware hardware in computer.Hardware)
			{
				Dictionary<string, object> Hware = new Dictionary<string, object>();
				subHware.SInitializer(hardware, Hware);
				comp.Add(hardware.Name.ToString()+"_"+hardware.HardwareType.ToString(), Hware);

			}
			Console.WriteLine(jsonizer.Json(comp));
		}
	}
}

//SubhardareInit
//hardwareInit
