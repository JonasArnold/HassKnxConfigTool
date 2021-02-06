﻿using System.Collections.ObjectModel;

namespace Common.Structure
{
  public interface ILayer
  {
    public string Name { get; set; }

    ObservableCollection<ILayer> SubLayers { get; }

    ObservableCollection<IDevice> Devices { get; }
  }
}
