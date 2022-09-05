using DevExpress.Mvvm;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPaging.Events;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class MainPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly EventBus _eventBus;
        private readonly MessageBus _messageBus;

        public string LogText { get; set; }

        public MainPageViewModel(PageService pageService, EventBus eventBus, MessageBus messageBus)
        {
            _pageService = pageService;
            _eventBus = eventBus;
            _messageBus = messageBus;
        }

        private bool banksReady = false;

        public ICommand GoToBanks => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Loading());

            await _messageBus.SendTo<BanksViewModel>(new TextMessage("update"));
            
            _pageService.ChangePage(new Banks());
        });

        public ICommand GoToPlans => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Plans());
        });
        public ICommand SendLog => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<LogPageViewModel>(new TextMessage(LogText));
        });
    }
}
