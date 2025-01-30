using System;
using LibreHardwareMonitor.Hardware;

namespace Initializer
{


	public class SHwareInitializer 
	{

		public SHwareInitializer() { 
		
		}

		public void SInitializer(IHardware hardware, Dictionary<string, object> Hware)
		{
			sSensorInitializer subSensor = new sSensorInitializer();

			foreach (IHardware subhardware in hardware.SubHardware)
			{
			
				Dictionary<string, object> sHware = new Dictionary<string, object>();
				subSensor.subSensorInitializer(subhardware, sHware);
				Hware.Add(subhardware.Name.ToString() + "_" + subhardware.HardwareType.ToString(), sHware);
			}
		} 
	}
}