using System;

using LibreHardwareMonitor.Hardware;

using serialize;


namespace updater
{

	public class HardwareUpdater
	{
		public HardwareUpdater()
		{
		}

		public void HwareUpdater(IComputer computer, Dictionary<string, object> comp)
		{

			JsonUtility jsonizer = new JsonUtility();

			SubHardwareUpdater subHwareUpdater = new SubHardwareUpdater();
			SensorUpdater senUpdater = new SensorUpdater();

			foreach (IHardware hardware in computer.Hardware)
			{
					//Console.WriteLine(hardware.ToString() + "\n\n");
					//Console.WriteLine(comp[hardware.Name + "_" + hardware.HardwareType.ToString()].GetType() + "\n\n\n");
                Console.WriteLine(((Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>>)comp[hardware.Name + "_" + hardware.HardwareType.ToString()]).GetType() + "\n\n\n");
                if ((comp.ContainsKey(hardware.Name + "_" + hardware.HardwareType.ToString())) &&  hardware.HardwareType.ToString() == "Motherboard")
				{
                    subHwareUpdater.SubHwareUpdater(hardware, (Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>>)comp[hardware.Name + "_" + hardware.HardwareType.ToString()]);
				}
				else
				{
					senUpdater.hwareSensorUpdater(hardware, comp[hardware.Name + "_" + hardware.HardwareType.ToString()] as Dictionary<string, Dictionary<string, Dictionary<string, float?>>>);
				}

			}

		}




	}
}