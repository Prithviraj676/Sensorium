using System;

using LibreHardwareMonitor.Hardware;

public class SensorUpdater
{
	public SensorUpdater()
	{
	}

	public void hwareSensorUpdater(IHardware hardware,Dictionary<string, Dictionary<string, Dictionary<string, float?>>> Hware)
	{

		foreach (ISensor sensor in hardware.Sensors)
		{
            Dictionary<string, Dictionary<string, float?>> tempDict = Hware["Sensors"]; 
			if (tempDict.ContainsKey(sensor.SensorType.ToString()))
			{

				if (tempDict[sensor.SensorType.ToString()].ContainsKey(sensor.Name))
				{
                    Console.WriteLine(Hware["Sensors"][sensor.SensorType.ToString()][sensor.Name]);

					//Hware["Sensors"][sensor.SensorType.ToString()][sensor.Name]["Current"] = (float?)sensor.Value;

     //               Hware["Sensors"][sensor.SensorType.ToString()][sensor.Name]["Max"] = sensor.Max;
                }

			}
		}

	}

}
