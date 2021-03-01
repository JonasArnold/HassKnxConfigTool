using Common.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class ProjectModel : ObservableObject, ICloneable
  {
    public string Name { get; set; }

    public ObservableCollection<LayerModel> Layers { get; set; }

    public DateTime LastSaved { get; set; }

    private bool hasUnsavedChanges;
    [JsonIgnore]
    public bool HasUnsavedChanges
    {
      get { return this.hasUnsavedChanges; }
      set { this.hasUnsavedChanges = value; OnPropertyChanged(nameof(this.HasUnsavedChanges)); }
    }

    public ProjectModel()
    {
      LastSaved = DateTime.Now;
      Layers = new ObservableCollection<LayerModel>();
      this.HasUnsavedChanges = false;
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
