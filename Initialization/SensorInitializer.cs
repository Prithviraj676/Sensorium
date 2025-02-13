using System;

using LibreHardwareMonitor.Hardware;

public class SensorInitializer
{
	public SensorInitializer()
	{

	}

	public void SensorInit(IHardware hardware, Dictionary<string, object> Hware)
	{
		Dictionary<string, Dictionary<string, Dictionary<string, float?>>> sensorDict = new Dictionary<string, Dictionary<string, Dictionary<string, float?>>>();

		foreach (ISensor sensor in hardware.Sensors)
		{
			if (!sensorDict.ContainsKey(sensor.SensorType.ToString()))
			{
				sensorDict[sensor.SensorType.ToString()] = new Dictionary<string, Dictionary<string, float?>>();

			}
			sensorDict[sensor.SensorType.ToString()][sensor.Name] = new Dictionary<string, float?>
				{
						{ "Min", sensor.Min },
						{ "Current", sensor.Value },
						{ "Max", sensor.Max }
				};

			//	sensorDict[sensor.SensorType.ToString()] = new Dictionary<string, Dictionary<string, float?>>
			//	{
			//		{
			//			sensor.Name, new Dictionary<string, float?>
			//			{
			//				{ "Min", sensor.Min },
			//				{ "Current", sensor.Value },
			//				{ "Max", sensor.Max }
			//			}
			//		}
			//	};
			//}
			//else
			//{
			//	sensorDict.Add(sensor.SensorType.ToString(), new Dictionary<string, Dictionary<string, float?>>
			//	{
			//		{
			//			sensor.Name, new Dictionary<string, float?>
			//			{
			//				{ "Min", sensor.Min },
			//				{ "Current", sensor.Value },
			//				{ "Max", sensor.Max }
			//			}
			//		}
			//	});
			//}

		}
		Hware.Add("Sensors", sensorDict);

	}
}
