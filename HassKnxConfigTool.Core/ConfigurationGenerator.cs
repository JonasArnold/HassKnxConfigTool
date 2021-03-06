using Common.DeviceTypes;
using HassKnxConfigFileGenerator;
using HassKnxConfigTool.Core.Model;
using System;
using System.Collections.Generic;
using System.IO;

namespace HassKnxConfigTool.Core
{
  internal static class ConfigurationGenerator
  {
    public static void GenerateAllConfigs(ICollection<LayerModel> layers, string exportFolderName)
    {
      // create output folder path
      string outputFolder = $"{Constants.ExportLocation}{exportFolderName}_{DateTime.Now.ToString(Constants.ExportFolderDateTimeFormat)}\\";

      // ensure that the directory does not exist yet
      if(Directory.Exists(outputFolder))
      {
        throw new ArgumentException($"Cannot generate configs, directory already exists: {outputFolder}");
      }
      else // create directory if it does not yet exist
      {
        Directory.CreateDirectory(outputFolder);
      }

      // Generate config of every single device types (EXTEND_DEVICETYPES)
      ExtractDevicesAndGenerateConfigOfType<Light>(layers, $"{outputFolder}lights{Constants.ExportFilesExtension}");
      ExtractDevicesAndGenerateConfigOfType<Switch>(layers, $"{outputFolder}switch{Constants.ExportFilesExtension}");
      ExtractDevicesAndGenerateConfigOfType<BinarySensor>(layers, $"{outputFolder}binary_sensor{Constants.ExportFilesExtension}");
      ExtractDevicesAndGenerateConfigOfType<Scene>(layers, $"{outputFolder}scene{Constants.ExportFilesExtension}");
    }

    /// <summary>
    /// Extracts devices of type T out of the given layer structure.
    /// Then generates a config file for all devices of type T at the given file location.
    /// </summary>
    /// <typeparam name="T">type of device to search and extract</typeparam>
    /// <param name="layers">layers to search for devices</param>
    /// <param name="fileOutputLocation">file location to write the config to</param>
    /// <returns>list of devices that were extracted</returns>
    private static void ExtractDevicesAndGenerateConfigOfType<T>(ICollection<LayerModel> layers, string fileOutputLocation) where T : IDevice
    {
      var listOfDevices = ExtractDevicesOfType<T>(layers);
      if (listOfDevices != null && listOfDevices.Count > 0)
      {
        KnxConfigGenerator.GenerateConfigFile(listOfDevices, fileOutputLocation);
      }
    }

    /// <summary>
    /// Extracts devices of type T out of the given layer structure.
    /// </summary>
    /// <typeparam name="T">type of device to search and extract</typeparam>
    /// <param name="layers">layers to search for devices</param>
    /// <returns>list of devices that were extracted</returns>
    private static List<T> ExtractDevicesOfType<T>(ICollection<LayerModel> layers) where T : IDevice
    {
      return LayerHelpers.FindAllDevicesOfType<T>(layers);
    }
  }
}
