using Common.Mvvm;
using HassKnxConfigTool.Core.Model;
using System;
using System.Collections.ObjectModel;

namespace HassKnxConfigTool.Core.ViewModel
{
  public class ProjectsViewModel : ViewModelBase, IProjectChangedNotifier
  {
    #region ViewModelBase members
    public override string Header => "Projects";
    #endregion

    private readonly IUiService uiService;

    public ProjectsViewModel(IUiService uiService)
    {
      WireCommands();
      this.uiService = uiService;
      this.SetUnsavedChanges(false);  //initialize

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
      this.AddProjectCommand = new RelayCommand(AddProject);
      this.SaveProjectCommand = new RelayCommand(() => SaveProject(out bool _)); // ignore out parameter
    }

    public RelayCommand AddProjectCommand { get; private set; }
    public bool CanAddProject => string.IsNullOrEmpty(this.NewProjectName) == false;
    public void AddProject()
    {
      ProjectModel p = new ProjectModel
      {
        Name = this.NewProjectName
      };

      this.NewProjectName = string.Empty;

      this.Projects.Add(p);
    }


    public RelayCommand SaveProjectCommand { get; private set; }
    public bool CanSaveProject => this.SelectedProject != null;
    public void SaveProject(out bool success)
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
        this.SetUnsavedChanges(false);
        success = true;
      }
      catch (Exception ex)
      {
        this.uiService.DisplayBottomMessage(MessageSeverity.Error, $"Project {SelectedProject?.Name} could not be stored: {ex}");
        success = false;
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
        bool allowNewSelectedProject = true;

        // if null: allow switch selected project
        if(_selectedProject != null)
        {
          if(_selectedProject.HasUnsavedChanges)
          {
            this.SaveProject(out bool savingSuccess); // first save
            allowNewSelectedProject = !savingSuccess; // only allow switch if saving was successful
          }
        }

        // perform switch to new selected project
        if(allowNewSelectedProject)
        {
          _selectedProject = value;  // set new value
          this.SetUnsavedChanges(false);
          if (_selectedProject != null)
          {
            SelectedProjectChanged?.Invoke(this, _selectedProject); // inform Editor View Model
          }
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

    /// <summary>
    /// Sets the flag for unsaved changes.
    /// </summary>
    /// <param name="hasUnsavedChanges">true if there are unsaved changes.</param>
    private void SetUnsavedChanges(bool hasUnsavedChanges)
    {
      if (this.SelectedProject != null)
      {
        this.SelectedProject.HasUnsavedChanges = hasUnsavedChanges;
      }
      this.uiService.UpdateUnsavedChangesDisplay(hasUnsavedChanges);  // property changed event
    }
    #endregion
  }
}
