using AutoMapper;
using LearnNewWord.IViewModel;
using LearnNewWord.Managers;
using LearnNewWord.View;
using LearnNewWord.ViewModel;
using Prism.Ioc;
using Prism.Unity;
using Repository.IRepositories;
using Repository.Repositories;
using Service.IService;
using Service.Service;
using System.Windows;

namespace LearnNewWord
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IMapper>(() => new Mapper(Manager.RegisterAutoMapper()));

            #region ViewModels
            containerRegistry.Register<IBaseViewModel, BaseViewModel>();
            containerRegistry.Register<IPracticeViewModel, PracticeViewModel>();
            containerRegistry.Register<IUtilitiesViewModel, UtilitiesViewModel>();
            containerRegistry.Register<IMessageBoxViewModel, MessageBoxViewModel>();
            //containerRegistry.Register<ISettingViewModel, SettingViewModel>();
            //containerRegistry.Register<IFlashcardViewModel, FlashCardViewModel>();
            //containerRegistry.Register<IListWordViewModel, ListWordViewModel>();
            #endregion

            #region Repositories
            containerRegistry.Register<IWordRepository, WordRepository>();
            containerRegistry.Register<ICategoryRepository, CategoryReposity>();
            #endregion


            #region Services
            containerRegistry.Register<IWordService, WordService>();
            containerRegistry.Register<ICategoryService, CategoryService>();
            #endregion
            Manager.Configuration(Container);
        }
    }
}