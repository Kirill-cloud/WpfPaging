using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Pages;
using WpfPaging.Services;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading.Tasks;
using WpfPaging.Models.Enums;

namespace WpfPaging.ViewModels
{
    public class BankEditViewModel : BindableBase
    {
        private readonly MessageBus _messageBus;
        private readonly BankContext _db;
        private readonly PageService _pageService;
        public Bank Bank { get; set; } = new Bank();

        public List<ScoreItemViewModel> AgeItems => GetItemByType(ScoringItemType.Age);

        public List<ScoreItemViewModel> KidsItems => GetItemByType(ScoringItemType.KidsAmount);

        public List<ScoreItemViewModel> FamilyItems => GetItemByType(ScoringItemType.FamilyState);

        public List<ScoreItemViewModel> QualificationItems => GetItemByType(ScoringItemType.Qualification);

        public List<ScoreItemViewModel> JobItems => GetItemByType(ScoringItemType.JobType);

        public BankEditViewModel(MessageBus messageBus, BankContext db, PageService pageService)
        {
            _messageBus = messageBus;
            _db = db;
            _pageService = pageService;
            _messageBus.Receive<TextMessage>(this, ReciveHandler);
        }

        private List<ScoreItemViewModel> GetItemByType(ScoringItemType type)
        {
            if (Bank != null && Bank.ScoringSystemsItems != null)
            {
                return Bank.ScoringSystemsItems.Where(x => x.Type == type).ToList()
                    .Select(x => new ScoreItemViewModel(_messageBus, _pageService) { Item = x }).ToList();
            }
            else
            {
                return new List<ScoreItemViewModel>();
            }
        }

        public List<CreditPlanDetailedViewModel> CreditPlans
        {
            get
            {
                if (Bank != null && Bank.CreditPlans != null)
                {
                    return Bank.CreditPlans.Select(x => new CreditPlanDetailedViewModel(_messageBus, _pageService)
                    {
                        Item = x
                    }).ToList();
                }

                return null;
            }
        }

        private async Task ReciveHandler(TextMessage message)
        {
            if (message.Text == "update")
            {
                //TODO refactor
                RaisePropertyChanged(nameof(AgeItems));
                RaisePropertyChanged(nameof(KidsItems));
                RaisePropertyChanged(nameof(FamilyItems));
                RaisePropertyChanged(nameof(JobItems));
                RaisePropertyChanged(nameof(QualificationItems));
                RaisePropertiesChanged(nameof(CreditPlans));
            }
            else if (message.Text == "edit")
            {
                Bank = _db.Banks
                    .Where(b => b.Id == message.Id)
                    .Include(b => b.CreditPlans)
                    .Include(b => b.ScoringSystemsItems)
                    .FirstOrDefault();
                if (Bank == null)
                {
                    Bank = new Bank()
                    {
                        ScoringSystemsItems = new List<ScoringSystemItem>()
                    };
                    _db.Banks.Add(Bank);
                }
            }
            else if (message.Text == "removeScore" && message.Id.HasValue)
            {
                _db.ScoringSystems.Remove(_db.ScoringSystems.Find(message.Id));
                Bank.ScoringSystemsItems = Bank.ScoringSystemsItems.Where(b => b.Id != message.Id).ToList();
                await _db.SaveChangesAsync();
            }
            else if (message.Text == "removePlan" && message.Id.HasValue)
            {
                _db.CreditPlans.Remove(_db.CreditPlans.Find(message.Id));
                Bank.CreditPlans = Bank.CreditPlans.Where(b => b.Id != message.Id).ToList();
                await _db.SaveChangesAsync();
            }
        }

        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Loading());
            await _db.SaveChangesAsync();
            await _messageBus.SendTo<BanksViewModel>(new TextMessage("update"));
            _pageService.ChangePage(new Banks());
        });

        public ICommand<string> AddCriteria => new AsyncCommand<string>(async s =>
        {
            ScoringItemType type;

            switch (s)
            {
                case "age": type = ScoringItemType.Age; break;
                case "kids": type = ScoringItemType.KidsAmount; break;
                case "family": type = ScoringItemType.FamilyState; break;
                case "job": type = ScoringItemType.JobType; break;
                case "qualification": type = ScoringItemType.Qualification; break;
                default: throw new NotSupportedException();
            }

            if (Bank.Id == 0)
            {
                _db.Banks.Add(Bank);
                await _db.SaveChangesAsync();
            }

            await _messageBus.SendTo<ScoreItemEditViewModel>(new ScoreSystemMessage(type, Bank.Id, 0));
            _pageService.ChangePage(new ScoringItemEdit());
        });

        public ICommand<string> AddPlan => new AsyncCommand<string>(async s =>
        {
            if (Bank.Id == 0)
            {
                _db.Banks.Add(Bank);
                await _db.SaveChangesAsync();
            }

            await _messageBus.SendTo<PlanEditViewModel>(new DependedObjectMessage(Bank.Id, 0));
            _pageService.ChangePage(new PlanEdit());
        });
    }
}
