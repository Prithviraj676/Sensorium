using System;

using s_init;

public class TransmitData
{
	public TransmitData()
	{
	}

    static void TransmitData()
    {
        while (_exec)
        {
            _writer.WriteLine(valJson);
            Console.WriteLine("Sensor Data sent!");
            Thread.sleep(1000);
        }
    }
}
