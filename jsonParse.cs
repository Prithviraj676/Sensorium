using System;
using System.Collections.Generic;

using LibreHardwareMonitor.Hardware;
using Newtonsoft.Json;



namespace serialize
{
    public class JsonUtility
    {
        public void JsonParse(string sensor, string type, float? min, float? max, float? value, Dictionary<string, Dictionary<string, object>> sensors) 
        {

            Dictionary<string, object> val = new Dictionary<string, object>();
            val.Add("TYPE", type);
            val.Add("min", min);
            val.Add("max", max);
            val.Add("Current", value);

            if (!sensors.ContainsKey(sensor))
            {
                sensors.Add(sensor, val);
            }
            else
            {
                sensors[sensor] = val;
            }

        }


        public string Json(Dictionary<string, Dictionary<string, object>> val)
        {
            string valJson = JsonConvert.SerializeObject(val, Formatting.Indented);

            return valJson;

        }

       

    }
}   