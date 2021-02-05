using HassKnxConfigTool.LayerDefinitions;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool
{
  public class ToolImplementation
  {
    #region UI Bindings
    ObservableCollection<Project> ProjectList { get; set; }
    ObservableCollection<MainLayer> CurrentProjectNodes { get; set; }


    #endregion

    private IUserInterfaceActions userInterface;

    public ToolImplementation(IUserInterfaceActions ui)
    {
      this.CurrentProjectNodes = DemoTree();
      this.userInterface = ui;
    }

    public void Demo()
    {
      this.CurrentProjectNodes = DemoTree();

    }

    private static ObservableCollection<MainLayer> DemoTree()
    {
      ObservableCollection<MainLayer> mainLayers = new ObservableCollection<MainLayer>
      {
        new MainLayer()
        {
          Name = "UG",
          Members = new ObservableCollection<ILayer>()
        {
          new MiddleLayer()
          {
            Name = "Aufenthaltsraum",
            Members = new ObservableCollection<ILayer>()
            {
              new SubLayer() { Name = "Schalten Küchenlicht" },
              new SubLayer() { Name = "Dimmen Designerleuchte" },
              new SubLayer() { Name = "Storen Rigi" }
            }
          },
          new MiddleLayer()
          {
            Name = "Orchestergraben",
            Members = new ObservableCollection<ILayer>()
            {
              new SubLayer() { Name = "Schalten Orchestergraben Licht" },
              new SubLayer() { Name = "Schalten Grün Dirigent" },
              new SubLayer() { Name = "Schalten Rot Dirigent" }
            }
          }
        }
        },

        new MainLayer()
        {
          Name = "EG",
          Members = new ObservableCollection<ILayer>()
        {
          new MiddleLayer()
          {
            Name = "Bühnenhaus",
            Members = new ObservableCollection<ILayer>()
            {
              new SubLayer() { Name = "Schalten Arbeitslicht Seitenbühne" },
              new SubLayer() { Name = "Schalten Arbeitslicht Hinterbühne" },
              new SubLayer() { Name = "Schalten Arbeitslicht Estrich" },
              new SubLayer() { Name = "Schalten Arbeitslicht Bühne" },
              new SubLayer() { Name = "Dimmen Saallicht Parkett" },
              new SubLayer() { Name = "Dimmen Saallicht Estrade" },
              new SubLayer() { Name = "Dimmen Saallicht Balkon" },
            }
          },
          new MiddleLayer()
          {
            Name = "Foyer",
            Members = new ObservableCollection<ILayer>()
            {
              new SubLayer() { Name = "Schalten Deckenlicht"},
              new SubLayer() { Name = "Dimmen Wandleuchten" },
            }
          },
          new MiddleLayer()
          {
            Name = "Testraum",
            Members = new ObservableCollection<ILayer>()
            {
              new SubLayer() { Name = "Schalten Test1" },
              new SubLayer() { Name = "Schalten Test2" },
              new SubLayer() { Name = "Schalten Test3" }
            }
          }
        }
        }
      };

      return mainLayers;
    }

  }
}
