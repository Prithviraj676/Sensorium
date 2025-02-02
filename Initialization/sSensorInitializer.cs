using System;
using LibreHardwareMonitor.Hardware;

public class sSensorInitializer
{
	public sSensorInitializer()
	{

	}

	public void subSensorInitializer(IHardware subhardware, Dictionary<string, object> sHware)
	{

		Dictionary<string, Dictionary<string, Dictionary<string, float?>>> sensorsDict = new Dictionary<string, Dictionary<string, Dictionary<string, float?>>>();
		foreach (ISensor sensor in subhardware.Sensors)
		{
			if (sensorsDict.ContainsKey(sensor.SensorType.ToString()))
			{
				sensorsDict[sensor.SensorType.ToString()] = new Dictionary<string, Dictionary<string, float?>>
				{

					{
						sensor.Name, new Dictionary<string, float?>
						{
							{ "Min", sensor.Min },
							{ "Current", sensor.Value },
							{ "Max", sensor.Max }
						}
					}
				};
			}
			else
			{
				sensorsDict.Add(sensor.SensorType.ToString(), new Dictionary<string, Dictionary<string, float?>>
				{
					{
						sensor.Name, new Dictionary<string, float?>
						{
							{ "Min", sensor.Min },
							{ "Current", sensor.Value },
							{ "Max", sensor.Max }
						}
					}
				});
			}
		}

	}
}