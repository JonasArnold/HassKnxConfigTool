﻿using HassKnxConfigTool.Core.Model;
using System.Collections.Generic;
using System.Linq;

namespace HassKnxConfigTool.Core
{
  internal static class LayerHelpers
  {
    /// <summary>
    /// Searches and removes the first layer which ID matches the ID passed in.
    /// Also recursively searches sublayers of layer.
    /// </summary>
    /// <param name="id">Identification of the layer to remove</param>
    /// <param name="layers">Layer collection to search</param>
    /// <returns>true = removed successfully</returns>
    public static bool FindAndRemoveLayer(string id, ICollection<LayerModel> layers)
    {
      bool removed = false;

      // try to remove in current layer
      removed |= layers.Remove(layers.FirstOrDefault(d => d.Id == id));

      // if the id was not found in the top layer
      if (removed == false)
      {
        // go through every layer 
        foreach (var layer in layers)
        {
          // check if the maximum serach depth is not yet reached (if not checked creates endless recursive loop)
          if ((layer.Depth + 1) <= Constants.MaxLayerDepth)
          {          
            // recursively call this method and try to remove the layer
            removed |= FindAndRemoveLayer(id, layer.SubLayers);
          }

          // break if the layer could be removed
          if (removed)
          {
            break;
          }
        }
      }

      return removed;
    }

    /// <summary>
    /// Searches and removes the first device which ID matches the ID passed in.
    /// Also recursively searches sublayers of layer.
    /// </summary>
    /// <param name="id">Identification of the device to remove</param>
    /// <param name="layers">Layer collection to search</param>
    /// <returns>true = removed successfully</returns>
    public static bool FindAndRemoveDevice(string id, ICollection<LayerModel> layers)
    {
      bool removed = false;

      // go through every layer 
      foreach (var layer in layers)
      {
        // try to remove device from current layer
        removed |= layer.Devices.Remove(layer.Devices.FirstOrDefault(d => d.Id == id));

        // break if the device could be removed or the maximum search depth is reached (if not checked creates endless recursive loop)
        if (removed || (layer.Depth + 1) > Constants.MaxLayerDepth)
        {
          break;
        }

        // if it was not found in this layer => start recursive search in the sub layers
        removed |= FindAndRemoveDevice(id, layer.SubLayers);
      }

      return removed;
    }

  }
}
