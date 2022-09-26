using DevExpress.Mvvm;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly MessageBus _messageBus;

        public string LogText { get; set; }

        public MainPageViewModel(PageService pageService, MessageBus messageBus)
        {
            _pageService = pageService;
            _messageBus = messageBus;
        }

        public ICommand GoToBanks => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Loading());

            await _messageBus.SendTo<BanksViewModel>(new TextMessage("update"));

            _pageService.ChangePage(new Banks());
        });

        public ICommand GoToPlans => new AsyncCommand(() =>
        {
            _pageService.ChangePage(new Plans());
            return Task.CompletedTask;
        });

        public ICommand SendLog => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<LogPageViewModel>(new TextMessage(LogText));
        });
    }
}
