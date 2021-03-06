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
    private readonly ProjectsViewModel ProjectsVM;
    private readonly EditorViewModel EditorVM;

    public ObservableCollection<ViewModelBase> TabList { get; set; }

    public MainWindow()
    {
      InitializeComponent();
      this.ProjectsVM = new ProjectsViewModel(this);
      this.EditorVM = new EditorViewModel(this, this.ProjectsVM);

      this.TabList = new ObservableCollection<ViewModelBase> { this.ProjectsVM, this.EditorVM };
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

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      if(this.ProjectsVM.SelectedProject != null &&
         this.ProjectsVM.SelectedProject.HasUnsavedChanges)
      {
        var userDecision = MessageBox.Show("Do you want to save the changes to the current project?", 
          "Unsaved Changes", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);

        switch (userDecision)
        {
          case MessageBoxResult.Yes:
            this.ProjectsVM.SaveProject(out bool savingSuccess, true);
            e.Cancel = !savingSuccess;  // cancel closing if unsuccessful saving
            break;
          case MessageBoxResult.Cancel:
            // cancel program closure
            e.Cancel = true;
            break;
          case MessageBoxResult.No:
          case MessageBoxResult.None:
          default:
            e.Cancel = false;
            // close program
            break;
        }

      }
    }

    public void UpdateUnsavedChangesDisplay(bool hasUnsavedChanges)
    {
      if(hasUnsavedChanges)
      {
        this.tbUnsavedChanges.Visibility = Visibility.Visible;
      }
      else
      {
        this.tbUnsavedChanges.Visibility = Visibility.Hidden;
      }
    }
  }
}
