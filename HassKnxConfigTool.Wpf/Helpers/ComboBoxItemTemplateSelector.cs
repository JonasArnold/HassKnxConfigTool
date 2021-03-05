using System.Windows;
using System.Windows.Controls;

namespace HassKnxConfigTool.Wpf.Helpers
{
  /// <summary>
  /// As described in:
  /// https://wpf.2000things.com/2014/02/19/1012-using-a-different-data-template-for-the-face-of-a-combobox/
  /// </summary>
  public class ComboBoxItemTemplateSelector : DataTemplateSelector
  {
    // Can set both templates from XAML
    public DataTemplate SelectedItemTemplate { get; set; }
    public DataTemplate ItemTemplate { get; set; }

    public override DataTemplate SelectTemplate(object item, DependencyObject container)
    {
      bool selected = false;

      // container is the ContentPresenter
      if (container is FrameworkElement fe)
      {
        DependencyObject parent = fe.TemplatedParent;
        if (parent != null)
        {
          if (parent is ComboBox)
            selected = true;
        }
      }

      if (selected)
        return SelectedItemTemplate;
      else
        return ItemTemplate;
    }
  }
}
