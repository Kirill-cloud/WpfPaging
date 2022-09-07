using DevExpress.Mvvm;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class LogPageViewModel : BindableBase
    {
        private readonly PageService _pageService;
        private readonly MessageBus _messageBus;

        public ObservableCollection<string> Logs { get; set; } = new ObservableCollection<string>();


        public LogPageViewModel(PageService pageService, MessageBus messageBus)
        {
            _pageService = pageService;
            _messageBus = messageBus;

            _messageBus.Receive<TextMessage>(this, async message =>
            {
                await Task.Delay(3000);
                Logs.Add(message.Text);
            });
            //_messageBus.Receive<TextMessage>(new object(), async message => Logs.Add(message.Text));
        }

        public ICommand AppendLog => new DelegateCommand(() =>
        {
            Logs.Add(Guid.NewGuid().ToString());
        });

        public ICommand ChangePage => new DelegateCommand(async () =>
        {
            _pageService.ChangePage(new MainPage());
        });
    }
}
