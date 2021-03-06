using Common.Mvvm;
using HassKnxConfigTool.Core.Serializing;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  [Serializable]
  [JsonConverter(typeof(ProjectConverter))] // using a specific converter to check for data version
  public class ProjectModel : ObservableObject, ICloneable
  {
    /// <summary>
    /// Identification of this project.
    /// </summary>
    public string Id { get; }

    /// <summary>
    /// Project name.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Indicates the version of data structure.
    /// </summary>
    public int DataVersion { get; set; }

    /// <summary>
    /// Layers stored in this Project.
    /// </summary>
    public ObservableCollection<LayerModel> Layers { get; set; }

    /// <summary>
    /// When the project was saved latest.
    /// </summary>
    public DateTime LastSaved { get; set; }

    private bool hasUnsavedChanges;
    /// <summary>
    /// Indicates if the project has any unsaved changes.
    /// </summary>
    [JsonIgnore]
    public bool HasUnsavedChanges
    {
      get { return this.hasUnsavedChanges; }
      set { this.hasUnsavedChanges = value; OnPropertyChanged(nameof(this.HasUnsavedChanges)); }
    }

    public ProjectModel()
    {
      this.LastSaved = DateTime.Now;
      this.Layers = new ObservableCollection<LayerModel>();
      this.HasUnsavedChanges = false;
      this.DataVersion = Constants.DataVersion;
      this.Id = Guid.NewGuid().ToString();  // generate unique id
    }

    /// <summary>
    /// Creates a memberwise clone of this instance of a project.
    /// </summary>
    /// <returns>clone</returns>
    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
