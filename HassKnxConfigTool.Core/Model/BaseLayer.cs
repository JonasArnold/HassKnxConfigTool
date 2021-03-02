using Common.Mvvm;
using Newtonsoft.Json;
using System;

namespace HassKnxConfigTool.Core.Model
{
  public class BaseLayer : ObservableObject, ILayer
  {
    public BaseLayer(ILayer parentLayer)
    {
      this.Depth = CalcLayerDepth(parentLayer);
      Id = Guid.NewGuid().ToString();  // generate unique id
    }

    private static int CalcLayerDepth(ILayer parentLayer)
    {
      // if top layer => return 0
      if(parentLayer == null)
      {
        return 0;
      }
      else  // otherwise one deeper than the previous
      {
        return parentLayer.Depth + 1;
      }
    }

    #region ILayer members
    private string name;
    public string Name
    {
      get { return this.name; }
      set { this.name = value; OnPropertyChanged(nameof(this.Name)); }
    }
    
    public int Depth { get; set; }
    public string Id { get; set; }
    #endregion
  }
}
