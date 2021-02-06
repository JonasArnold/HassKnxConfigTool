using HassKnxConfigTool.Core.Model;
using System;

namespace HassKnxConfigTool.Core
{
  public interface IProjectChangedNotifier
  {
    /// <summary>
    /// Notifies about the change of the selected project.
    /// </summary>
    event EventHandler<ProjectModel> SelectedProjectChanged;
  }
}
