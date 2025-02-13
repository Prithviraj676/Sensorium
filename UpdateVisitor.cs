using System;
using LibreHardwareMonitor.Hardware;


namespace Visitor
{


    public class UpdateVisitor : IVisitor
    {
        public void VisitComputer(IComputer computer)
        {
            computer.Traverse(this);
        }
        public void VisitHardware(IHardware hardware)
        {
            hardware.Update();
            foreach (IHardware subHardware in hardware.SubHardware) subHardware.Accept(this);
        }
        public void VisitSensor(ISensor sensor) { }
        public void VisitParameter(IParameter parameter) { }
    }


}




//uniHardware(is nested dictionary) --> //multi-level nested dictionary < string (lists the hardware type), obj (nested dictionary) >
//    {
//        hardware(is key for uniHardware, represents a nested dictionary) --> // structure of the represented dict dictionary <string, dictionary <string, obj>>
//        { 

//            subhardware(is key for hardwaer, represents a nested dictionary) -- > //structure of the represented dict dictionary <string, obj>
//            {
//                //for the following key which is a dictionary --> multi-level nested dictionary < string, obj >
//                types(value) -- > //nested dictionary < string, dictionary<string, float> >
//                    {
//                        voltage(is key for types, represents as nested dictionary){ } --> //structure of the represented dict dictionary<string, dictionary<string, float>>
//                        temperature(is key for types, represents as nested dictionary) { } --> //structure of the represented dict dictionary<string, dictionary<string, float>>
//                        clock(is key for types, represents as nested dictionary) { } --> //structure of the represented dict dictionary<string, dictionary<string, float>>
//                    }
//                    name: (string / obj)
//                    timeStamp: (obj)
//            }

//        }
    
//        hardware(is key for uniHardware, represents a nested dictionary) --> //structure of the represented dict dictionary <string, dictionary<string, float>>
//        string : dictionary<string, dictionary<string, dictionary<string, float>>>

//    }
