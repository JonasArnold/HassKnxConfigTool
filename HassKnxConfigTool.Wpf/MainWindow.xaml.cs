using HassKnxConfigTool.Core;
using HassKnxConfigTool.Core.ViewModel;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace HassKnxConfigTool.Wpf
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IUiService
  {

    public ObservableCollection<ViewModelBase> TabList { get; set; }

    //private readonly ProjectsViewModel projectVM;
    //private readonly EditorViewModel editorVM;

    public MainWindow()
    {
      InitializeComponent();
      this.TabList = new ObservableCollection<ViewModelBase>();
      this.TabList.Add(new ProjectsViewModel(this));
      this.TabList.Add(new EditorViewModel(this));

      this.tabControlMain.ItemsSource = this.TabList;
    }

    //private void TabControlMain_SelectionChanged(object sender, SelectionChangedEventArgs e)
    //{
    //  if (ProjectsTab.IsSelected)
    //  {
    //    this.DataContext = this.projectVM;
    //  }
    //  if (EditorTab.IsSelected)
    //  {
    //    this.DataContext = this.editorVM;
    //  }
    //}
  }
}
