using System;
using LibreHardwareMonitor.Hardware;

public class sSensorInitializer
{
	public sSensorInitializer()
	{

	}

	public void subSensorInitializer(IHardware subhardware, Dictionary<string, Dictionary<string, Dictionary<string, float?>>> sHware)
	{

		Dictionary<string, Dictionary<string, Dictionary<string, float?>>> subSensorsDict = new Dictionary<string, Dictionary<string, Dictionary<string, float?>>>();
        //foreach (ISensor sensor in subhardware.Sensors)
        //{
        //	if (subSensorsDict.ContainsKey(sensor.SensorType.ToString()))
        //	{
        //		subSensorsDict[sensor.SensorType.ToString()][sensor.Name] = new Dictionary<string, Dictionary<string, float?>>
        //		{

        //			{
        //				sensor.Name, new Dictionary<string, float?>
        //				{
        //					{ "Min", sensor.Min },
        //					{ "Current", sensor.Value },
        //					{ "Max", sensor.Max }
        //				}
        //			}
        //		};
        //	}
        //	else
        //	{
        //		subSensorsDict.Add(sensor.SensorType.ToString(), new Dictionary<string, Dictionary<string, float?>>
        //		{
        //			{
        //				sensor.Name, new Dictionary<string, float?>
        //				{
        //					{ "Min", sensor.Min },
        //					{ "Current", sensor.Value },
        //					{ "Max", sensor.Max }
        //				}
        //			}
        //		});
        //	}
        //}



        foreach (ISensor sensor in subhardware.Sensors)
        {
            // Check if the sensor type already exists in the dictionary
            if (!subSensorsDict.ContainsKey(sensor.SensorType.ToString()))
            {
                // If the sensor type does not exist, create a new entry for it
                subSensorsDict[sensor.SensorType.ToString()] = new Dictionary<string, Dictionary<string, float?>>();
            }

            // Add the sensor data to the dictionary for the corresponding sensor type
            subSensorsDict[sensor.SensorType.ToString()][sensor.Name] = new Dictionary<string, float?>
    {
        { "Min", sensor.Min },
        { "Current", sensor.Value },
        { "Max", sensor.Max }
    };
        }

        sHware.Add("SubSensors",  subSensorsDict);

	}
}