using System;
using System.Collections.Generic;
using System.Diagnostics;
using Common.Exceptions;
using Common.FileHelpers;

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
      Debug.WriteLine($"Starting Generation of config....");

      Stopwatch sw = new Stopwatch();
      sw.Start();
      Type typeList = instances.GetType();
      Type typeItem;

      // check if the given list is really a generic list   //TEST
      if (!typeList.IsGenericType || typeList.GetGenericTypeDefinition() != typeof(List<>))
      {
        throw new ImplementationException("Argument 'instances' is not a generic type.");
      }
      typeItem = typeList.GetGenericArguments()[0];

      // check if the item type exists in the DeviceTypes namespace (including assembly)  //TEST
      var myClassType = Type.GetType($"{Constants.DeviceTypeDefinitionNamespace}.{typeItem.Name}, {typeItem.Assembly.FullName}");
      object instance = myClassType == null ? null : Activator.CreateInstance(myClassType); //Check if exists, instantiate if so.
      if(instance == null)
      {
        throw new ImplementationException($"Type of instance does not exist in {Constants.DeviceTypeDefinitionNamespace}.");
      }

      // create content
      string content = "";
      foreach (var listItem in instances)
      {
        content += YamlStringify.CreateInstanceString(listItem);
      }
      sw.Stop();

      Debug.WriteLine($"Done. Milliseconds to generate: {sw.Elapsed.TotalMilliseconds}");
      Debug.WriteLine("Finished content: \n\n");
      Debug.WriteLine(content);

      // store file
      FileGenerator.CreateFile(filePath, content);
    }
  }
}
