using HassKnxConfigFileGenerator.Helpers;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HassKnxConfigFileGenerator
{
  public class KnxConfigGenerator
  {
    /// <summary>
    /// Generates a HASS Config File for the instances given with the list.
    /// </summary>
    /// <typeparam name="T">Type of an instance</typeparam>
    /// <param name="instances">the instances to add to yaml</param>
    /// <param name="filePath">path where the file should be stored (e.g. @"c:\temp\lights.yaml")</param>
    public static void GenerateConfigFile<T>(List<T> instances, string filePath)
    {
      Console.WriteLine($"Starting Generation of config....");

      Stopwatch sw = new Stopwatch();
      sw.Start();
      Type typeList = instances.GetType();
      Type typeItem;

      // check if the given list is really a generic list   //TEST
      if (!typeList.IsGenericType || typeList.GetGenericTypeDefinition() != typeof(List<>))
      {
        throw new ArgumentException("Argument 'instances' is not a generic type.");
      }
      typeItem = typeList.GetGenericArguments()[0];

      // check if the item type exists in the DeviceTypeDefinitions namespace   //TEST
      string @namespace = "HassKnxConfigFileGenerator.DeviceTypeDefinitions";
      var myClassType = Type.GetType(String.Format("{0}.{1}", @namespace, typeItem.Name));
      object instance = myClassType == null ? null : Activator.CreateInstance(myClassType); //Check if exists, instantiate if so.
      if(instance == null)
      {
        throw new ArgumentException($"Type of instance does not exist in {@namespace}.");
      }

      // create content
      string content = "";
      foreach (var listItem in instances)
      {
        content += YamlStringify.CreateInstanceString(listItem);
      }
      sw.Stop();

      Console.WriteLine($"Done. Milliseconds to generate: {sw.Elapsed.TotalMilliseconds}");
      Console.WriteLine("Finished content: \n\n");
      Console.WriteLine(content);

      // store file
      FileGenerator.CreateFile(filePath, content);
    }
  }
}
