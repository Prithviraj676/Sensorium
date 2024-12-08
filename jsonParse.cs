using System;
using System.Collections.Generic;

using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;



namespace serialize
{
    public class JsonUtility
    {
        public void JsonParse(string sensorName, string sensorType, float? min, float? max, float? current, Dictionary<string, Dictionary<string, object>> sensors) 
        {

            Dictionary<string, object> val = new Dictionary<string, object>();
            val.Add("TYPE", sensorType);
            val.Add("min", min);
            val.Add("max", max);
            val.Add("Current", current);

            if (!sensors.ContainsKey(sensorName))
            {
                sensors.Add(sensorName, val);
            }
            else
            {
                sensors[sensorName] = val;
            }

        }


        public string Json(Dictionary<string, Dictionary<string, object>> val)
        {
            string valJson = JsonConvert.SerializeObject(val, Formatting.Indented);

            return valJson;

        }

       

    }
}   