using System;
using LibreHardwareMonitor.Hardware;

namespace Initializer
{


	public class SHwareInitializer 
	{

		public SHwareInitializer() { 
		
		}

		public void SInitializer(IHardware hardware, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>> Hware)
		{
			sSensorInitializer subSensor = new sSensorInitializer();

			foreach (IHardware subhardware in hardware.SubHardware)
			{

                Dictionary<string, Dictionary<string, Dictionary<string, float?>>> sHware = new Dictionary<string, Dictionary<string, Dictionary<string, float?>>>();
				subSensor.subSensorInitializer(subhardware, sHware);
				Hware.Add(subhardware.Name.ToString() + "_" + subhardware.HardwareType.ToString(), sHware);
			}
		} 
	}
}