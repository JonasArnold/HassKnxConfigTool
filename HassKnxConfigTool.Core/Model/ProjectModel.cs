using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.Model
{
  public class ProjectModel : ICloneable
  {
    public string Name { get; set; }

    ObservableCollection<LayerModel> Layers { get; set; }

    public DateTime LastSaved { get; set; }

    public bool HasUnsavedChanges { get; set; }

    public ProjectModel()
    {
      LastSaved = DateTime.Now;
      Layers = new ObservableCollection<LayerModel>();
    }

    public object Clone()
    {
      return this.MemberwiseClone();
    }
  }
}
