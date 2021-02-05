using System.Windows;
using HassKnxConfigTool;
using HassKnxConfigTool.LayerDefinitions;

namespace UserInterface
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window, IUserInterfaceActions
  {
    ToolImplementation tool;
    public string NewSubNodeText { get; set; }
    private ILayer currentLayer;

    public MainWindow()
    {
      InitializeComponent();
      this.tool = new ToolImplementation(this);
      DataContext = this.tool;
      this.tool.Demo();
    }

    private void trvLocations_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
    {
      if (e.NewValue is ILayer layer)
      {
        currentLayer = layer;
        tbCurrentItem.Text = currentLayer.Name;
      }
    }

    private void btnAddSubNode_Click(object sender, RoutedEventArgs e)
    {
      if (this.currentLayer is MainLayer)
      {
        this.currentLayer.Members.Add(new MiddleLayer() { Name = tbNewSubNode.Text });
      }
      if (this.currentLayer is MiddleLayer)
      {
        this.currentLayer.Members.Add(new SubLayer() { Name = tbNewSubNode.Text });
      }
      //if (this.currentLayer is SubLayer)
      //{
      //  this.parentLayer.Members.Add(new SubLayer() { Name = tbNewSubNode.Text });
      //}
    }
  }
}
