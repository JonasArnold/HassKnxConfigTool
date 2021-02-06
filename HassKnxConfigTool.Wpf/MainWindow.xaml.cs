using HassKnxConfigTool.Core;
using HassKnxConfigTool.Core.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace HassKnxConfigTool.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IUiService
  {

    public ObservableCollection<ViewModelBase> TabList { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      this.TabList = new ObservableCollection<ViewModelBase>
      {
        new ProjectsViewModel(this),
        new EditorViewModel(this)
      };

      this.tabControlMain.ItemsSource = this.TabList;
    }

    public void DisplayBottomMessage(MessageSeverity severity, string message)
    {
      Brush col;
      switch (severity)
      {
        case MessageSeverity.Success:
          col = Brushes.Green;
          break;
        case MessageSeverity.Warning:
          col = Brushes.Orange;
          break;
        case MessageSeverity.Error:
          col = Brushes.Red;
          break;
        case MessageSeverity.Information:
          col = Brushes.Blue;
          break;
        case MessageSeverity.Unknown:
        default:
          col = Brushes.DarkGray;
          break;
      }

      tbBottomMessage.Foreground = col;
      tbBottomMessage.Text = message;
    }
  }
}
