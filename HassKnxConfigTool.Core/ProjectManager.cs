using System.IO;
using System.Collections.Generic;
using Newtonsoft.Json;
using HassKnxConfigTool.Core.Model;

namespace HassKnxConfigTool.Core
{
  public static class ProjectManager
  {
    public static void StoreProject(ProjectModel project)
    {
      // Create serializer
      JsonSerializer serializer = new JsonSerializer()
      {
        NullValueHandling = NullValueHandling.Ignore,
        Formatting = Formatting.Indented
      };

      // create file path if it does not exist yet
      if (Directory.Exists(Constants.ProjectFilesLocation) == false)
      {
        Directory.CreateDirectory(Constants.ProjectFilesLocation);
      }

      // TODO Normalize string project.Name to make sure it is compatible with Windows file path
      // open stream writer at file path and store the file, streawriter will overwrite existing file or create new
      using StreamWriter sw = new StreamWriter($"{Constants.ProjectFilesLocation}{project.Name}{Constants.ProjectFilesExtension}", false);
      using JsonWriter writer = new JsonTextWriter(sw);
      serializer.Serialize(writer, project);
    }

    public static IList<ProjectModel> LoadProjects()
    {
      // check if directory exists
      if (Directory.Exists(Constants.ProjectFilesLocation) == false)
      {
        // if it does not exist, create it and return an empty list
        Directory.CreateDirectory(Constants.ProjectFilesLocation);
        return new List<ProjectModel>();
      }

      var files = Directory.GetFiles(Constants.ProjectFilesLocation);
      List<ProjectModel> importedProjects = new List<ProjectModel>();
      foreach (var file in files)
      {
        var fileInfo = new FileInfo(file);
        // ignore files that do not have the correct extension
        if (fileInfo.Extension != Constants.ProjectFilesExtension) continue;
        // deserialize project
        importedProjects.Add(JsonConvert.DeserializeObject<ProjectModel>(File.ReadAllText(file)));
      }

      return importedProjects;
    }
  }
}
