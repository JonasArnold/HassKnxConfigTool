using Common.Mvvm;
using HassKnxConfigTool.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class ProjectsViewModel : ViewModelBase, IProjectChangedNotifier
  {
    public string Header => "Projects";
    private readonly IUiService uiService;

    public ProjectsViewModel(IUiService uiService)
    {
      WireCommands();
      this.uiService = uiService;

      // import saved Projects
      try
      {
        var importedProjects = ProjectManager.LoadProjects();
        this.Projects = new ObservableCollection<ProjectModel>();
        foreach (var project in importedProjects)
        {
          this.Projects.Add(project);
        }
        this.uiService.DisplayBottomMessage(MessageSeverity.Success, $" Projects loaded successfully: {importedProjects?.Count}");
      }
      catch (Exception ex)
      {
        this.uiService.DisplayBottomMessage(MessageSeverity.Error, $"Some Projects could not be imported correctly: {ex}");
      }
    }


    #region Commands
    private void WireCommands()
    {
      AddProjectCommand = new RelayCommand(AddProject);
      SaveProjectCommand = new RelayCommand(SaveProject);
    }

    public RelayCommand AddProjectCommand { get; private set; }
    public bool CanAddProject => string.IsNullOrEmpty(NewProjectName) == false;
    public void AddProject()
    {
      ProjectModel p = new ProjectModel
      {
        Name = NewProjectName
      };

      NewProjectName = string.Empty;

      Projects.Add(p);
    }


    public RelayCommand SaveProjectCommand { get; private set; }
    public bool CanSaveProject => SelectedProject != null;
    public void SaveProject()
    {
      try
      {
        var clone = (ProjectModel)SelectedProject.Clone();  // clone instance
        var saveTime = DateTime.Now;
        clone.LastSaved = saveTime;
        ProjectManager.StoreProject(clone);
        // if successful:
        SelectedProject.LastSaved = saveTime;
        this.ReloadProjects();  // update project save times

        this.uiService.DisplayBottomMessage(MessageSeverity.Success, $"Project {clone.Name} was saved successfully.");
      }
      catch (Exception ex)
      {
        this.uiService.DisplayBottomMessage(MessageSeverity.Error, $"Project {SelectedProject?.Name} could not be stored: {ex}");
      }
    }
    #endregion

    #region Properties
    private ObservableCollection<ProjectModel> _projects = new ObservableCollection<ProjectModel>();
    public ObservableCollection<ProjectModel> Projects
    {
      get { return _projects; }
      set 
      { 
        _projects = value;
        OnPropertyChanged(nameof(Projects));
      }
    }

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

    public event EventHandler<ProjectModel> SelectedProjectChanged;
    private ProjectModel _selectedProject;
    public ProjectModel SelectedProject
    {
      get { return _selectedProject; }
      set
      {
        if(_selectedProject != null && _selectedProject.HasUnsavedChanges) // ignore if null, check for unsaved changes
        {
          this.SaveProject(); // first save
        }
        _selectedProject = value;  // set new value
        if (_selectedProject != null)
        {
          SelectedProjectChanged?.Invoke(this, _selectedProject); // inform Editor View Model
        }
        OnPropertyChanged(nameof(SelectedProject));
        OnPropertyChanged(nameof(CanSaveProject));
      }
    }

    #endregion

    #region Helpers
    private void ReloadProjects()
    {
      var projects = Projects;
      Projects = null;
      Projects = projects;
    }
    #endregion
  }
}
