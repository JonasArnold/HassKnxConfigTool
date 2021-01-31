using HassKnxConfigFileGenerator;
using HassKnxConfigFileGenerator.DeviceTypeDefinitions;
using HassKnxConfigFileGenerator.Knx;
using System.Collections.Generic;

namespace DemoConsoleApp
{
  class Program
  {
#pragma warning disable IDE0060 // Remove unused parameter
    static void Main(string[] args)
#pragma warning restore IDE0060 // Remove unused parameter
    {
      KnxConfigGenerator.GenerateConfigFile(DemoListOfLights(), @"C:\Users\jonas\Desktop\lights.yaml");
    }

    static List<Light> DemoListOfLights()
    {
      List<Light> lights = new List<Light>
      {
        new Light()
        {
          Name = "Testleuchte",
          Address = new GroupAddress(1, 2, 23),
          StateAddress = new GroupAddress(1, 2, 24)
        },
        new Light()
        {
          Name = "Deckenleuchte",
          Address = new GroupAddress(2, 2, 13),
          StateAddress = new GroupAddress(2, 2, 14)
        }

      };
      return lights;
    }
  }
}
