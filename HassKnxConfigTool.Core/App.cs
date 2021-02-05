using HassKnxConfigTool.Core.ViewModel;
using MvvmCross.ViewModels;

namespace HassKnxConfigTool.Core
{
  public class App : MvxApplication
  {
    public override void Initialize()
    {
      RegisterAppStart<ProjectsViewModel>();
    }
  }
}
