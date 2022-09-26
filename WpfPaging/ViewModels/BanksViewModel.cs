using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfPaging.Models;
using WpfPaging.Pages;
using WpfPaging.Services;
using WpfPaging.Messages;
using System.Windows.Input;
using DevExpress.Mvvm;

namespace WpfPaging.ViewModels
{
    public class BanksViewModel : BindableBase
    {
        private BankContext _db;
        private PageService _pageService;
        private MessageBus _messageBus;
        public List<BankViewModel> Banks { get; set; }

        public BanksViewModel(BankContext context, PageService pageService, MessageBus messageBus)
        {
            _db = context;
            _pageService = pageService;
            _messageBus = messageBus;

            _messageBus.Receive<TextMessage>(this, ReciveMessage);
        }

        private async Task ReciveMessage(TextMessage message)
        {
            if (message.Text == "update")
            {
                var banks = await _db.Banks
                    .Include(b => b.CreditPlans)
                    .Include(b => b.ScoringSystemsItems)
                    .AsNoTracking().ToListAsync();
                Banks = banks.Select(x => new BankViewModel(_messageBus, _pageService) { Bank = x }).ToList();
            }
            else if (message.Text == "remove" && message.Id.HasValue)
            {
                Banks = Banks.Where(x => x.Bank.Id != message.Id).ToList();
                _db.Banks.Remove(_db.Banks.Find(message.Id));
                await _db.SaveChangesAsync();
            }
        }

        public ICommand GotoMainPage => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainPage());
        });


        public ICommand AddCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("edit", 0));
            _pageService.ChangePage(new BankEdit());
        });
    }
}
