using System;

using LibreHardwareMonitor.Hardware;

namespace updater {
	public class SubSensorUpdater
	{
		public SubSensorUpdater()
		{
		}

        //public void subSensorUpdater(IHardware subhardware, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, Dictionary<string, float?>>>>> subHware)

        public void subSensorUpdater(IHardware subhardware, Dictionary<string, Dictionary<string, Dictionary<string, float?>>> subHware)
        {
			foreach (ISensor subSensor in subhardware.Sensors)
			{
    //            var tempDict = (Dictionary<string, Dictionary<string, Dictionary<string, float?>>>)subHware["SubSensors"];

    //            if (tempDict.ContainsKey(subSensor.SensorType.ToString()))
				//{
				//	if (tempDict[subSensor.SensorType.ToString()].ContainsKey(subSensor.Name))
				//	{
    //                    ((Dictionary<string, Dictionary<string, Dictionary<string, float?>>>)subHware["SubSensors"])[subSensor.SensorType.ToString()][subSensor.Name]["Min"] = subSensor.Min;

    //                    ((Dictionary<string, Dictionary<string, Dictionary<string, float?>>>)subHware["SubSensors"])[subSensor.SensorType.ToString()][subSensor.Name]["Current"] = subSensor.Value;

    //                    ((Dictionary<string, Dictionary<string, Dictionary<string, float?>>>)subHware["SubSensors"])[subSensor.SensorType.ToString()][subSensor.Name]["Max"] = subSensor.Max;
    //                }
				//}
			}

		}
	}
}