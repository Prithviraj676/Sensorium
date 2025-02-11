using System;

using LibreHardwareMonitor.Hardware;

using serialize;

namespace updater
{

	public class SubHardwareUpdater
	{

		public SubHardwareUpdater()
		{

		}

		public void SubHwareUpdater(IHardware hardware, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>> Hware)
		{

			JsonUtility jsonizer = new JsonUtility();

			SubSensorUpdater sSenUp = new SubSensorUpdater();
			Console.WriteLine(jsonizer.Json(Hware));
			foreach (IHardware subhardware in hardware.SubHardware)
			{
				if (Hware.ContainsKey(subhardware.Name.ToString() + "_" + subhardware.HardwareType.ToString()))
				{
                    Dictionary<string, Dictionary<string, Dictionary<string, float?>>> tempDict = Hware[subhardware.Name.ToString() + "_" + subhardware.HardwareType.ToString()] as Dictionary<string, Dictionary<string, Dictionary<string, float?>>>;

                    sSenUp.subSensorUpdater(subhardware, Hware[subhardware.Name.ToString() + "_" + subhardware.HardwareType.ToString()]);

				}
			}

		}

	}
}