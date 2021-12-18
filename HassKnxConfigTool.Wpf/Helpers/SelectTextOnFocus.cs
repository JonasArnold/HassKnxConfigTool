using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace HassKnxConfigTool.Wpf.Helpers
{
  /// <summary>
  /// Helper to enable automatic Select all when textbox gains focus.
  /// Seen at: https://newbedev.com/how-to-automatically-select-all-text-on-focus-in-wpf-textbox
  /// 
  /// Usage:
  /// - To activate for one textbox add parameter: helpers:SelectTextOnFocus.Active = "True"
  /// - To activate for all Textboxes, add this to the Resources:
  ///   <Style TargetType="{x:Type TextBox}">
  ///     <Setter Property="helpers:SelectTextOnFocus.Active" Value="True"/>
  ///   </Style>
  /// - To deactivate for one textbox add parameter: helpers:SelectTextOnFocus.Active = "False"
  /// </summary>
  internal class SelectTextOnFocus : DependencyObject
  {
    public static readonly DependencyProperty ActiveProperty = DependencyProperty.RegisterAttached(
        "Active",
        typeof(bool),
        typeof(SelectTextOnFocus),
        new PropertyMetadata(false, ActivePropertyChanged));

    private static void ActivePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
      if (d is TextBox)
      {
        TextBox textBox = d as TextBox;
        if ((e.NewValue as bool?).GetValueOrDefault(false))
        {
          textBox.GotKeyboardFocus += OnKeyboardFocusSelectText;
          textBox.PreviewMouseLeftButtonDown += OnMouseLeftButtonDown;
        }
        else
        {
          textBox.GotKeyboardFocus -= OnKeyboardFocusSelectText;
          textBox.PreviewMouseLeftButtonDown -= OnMouseLeftButtonDown;
        }
      }
    }

    private static void OnMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
      DependencyObject dependencyObject = GetParentFromVisualTree(e.OriginalSource);

      if (dependencyObject == null)
      {
        return;
      }

      var textBox = (TextBox)dependencyObject;
      if (!textBox.IsKeyboardFocusWithin)
      {
        textBox.Focus();
        e.Handled = true;
      }
    }

    private static DependencyObject GetParentFromVisualTree(object source)
    {
      DependencyObject parent = source as UIElement;
      while (parent != null && !(parent is TextBox))
      {
        parent = VisualTreeHelper.GetParent(parent);
      }

      return parent;
    }

    private static void OnKeyboardFocusSelectText(object sender, KeyboardFocusChangedEventArgs e)
    {
      TextBox textBox = e.OriginalSource as TextBox;
      if (textBox != null)
      {
        textBox.SelectAll();
      }
    }

    [AttachedPropertyBrowsableForChildrenAttribute(IncludeDescendants = false)]
    [AttachedPropertyBrowsableForType(typeof(TextBox))]
    public static bool GetActive(DependencyObject @object)
    {
      return (bool)@object.GetValue(ActiveProperty);
    }

    public static void SetActive(DependencyObject @object, bool value)
    {
      @object.SetValue(ActiveProperty, value);
    }
  }
}