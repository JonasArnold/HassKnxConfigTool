using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HassKnxConfigTool.Core
{
  public interface IUiService
  {
    void DisplayBottomMessage(MessageSeverity severity, string message);

    void UpdateUnsavedChangesDisplay(bool hasUnsavedChanges);
  }
  public enum MessageSeverity
  {
    Unknown = 0,
    Success,
    Warning,
    Error,
    Information
  }
}
