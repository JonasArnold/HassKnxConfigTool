using HassKnxConfigTool.Core.Model;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class ProjectsViewModel : MvxViewModel
  {
    public ProjectsViewModel()
    {
      AddProjectCommand = new MvxCommand(AddProject);
    }

    public IMvxCommand AddProjectCommand { get; set; }

    private ObservableCollection<ProjectModel> _projects = new ObservableCollection<ProjectModel>();

    public ObservableCollection<ProjectModel> Projects
    {
      get { return _projects; }
      set
      {
        SetProperty(ref _projects, value);
      }
    }

    public bool CanAddProject => string.IsNullOrEmpty(NewProjectName) == false;

    private string _newProjectName;

    public string NewProjectName
    {
      get { return _newProjectName; }
      set
      {
        SetProperty(ref _newProjectName, value);
        RaisePropertyChanged(() => CanAddProject);
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
