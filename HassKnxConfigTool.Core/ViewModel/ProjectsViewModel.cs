using Common.Mvvm;
using HassKnxConfigTool.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class ProjectsViewModel : ViewModelBase
  {
    public string Header => "Projects";
    private readonly IUiService uiService;

    public ProjectsViewModel(IUiService uiService)
    {
      WireCommands();
      this.uiService = uiService;
    }

    private void WireCommands()
    {
      AddProjectCommand = new RelayCommand(AddProject);
    }

    public RelayCommand AddProjectCommand
    {
      get;
      private set;
    }

    private ObservableCollection<ProjectModel> _projects = new ObservableCollection<ProjectModel>();

    public ObservableCollection<ProjectModel> Projects
    {
      get { return _projects; }
      set { _projects = value; }
    }

    public bool CanAddProject => string.IsNullOrEmpty(NewProjectName) == false;

    private string _newProjectName;

    public string NewProjectName
    {
      get { return _newProjectName; }
      set
      {
        if (_newProjectName != value)
        {
          _newProjectName = value;
          OnPropertyChanged(nameof(NewProjectName));
          OnPropertyChanged(nameof(CanAddProject));
        }
      }
    }

    public void AddProject()
    {
      ProjectModel p = new ProjectModel
      {
        Name = NewProjectName
      };

      NewProjectName = string.Empty;

      Projects.Add(p);
    }
  }
}
