using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class BankViewModel
    {
        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;
        public Bank Bank { get; set; }

        public int AgeParamsCount
        {
            get
            {
                if (Bank?.ScoringSystemsItems != null)
                {
                    return Bank.ScoringSystemsItems.Count(b => b.Type == ScoringItemType.Age);
                }

                return 0;
            }
        }
        public int KidsParamsCount
        {
            get
            {
                if (Bank?.ScoringSystemsItems != null)
                {
                    return Bank.ScoringSystemsItems.Count(b => b.Type == ScoringItemType.KidsAmount);
                }

                return 0;
            }
        }
        public int JobParamsCount
        {
            get
            {
                if (Bank?.ScoringSystemsItems != null)
                {
                    return Bank.ScoringSystemsItems.Count(b => b.Type == ScoringItemType.JobType);
                }

                return 0;
            }
        }
        public int QualificationParamsCount
        {
            get
            {
                if (Bank?.ScoringSystemsItems != null)
                {
                    return Bank.ScoringSystemsItems.Count(b => b.Type == ScoringItemType.Qualification);
                }

                return 0;
            }
        }
        public int FamilyParamsCount
        {
            get
            {
                if (Bank?.ScoringSystemsItems != null)
                {
                    return Bank.ScoringSystemsItems.Count(b => b.Type == ScoringItemType.FamilyState);
                }

                return 0;
            }
        }

        public BankViewModel(MessageBus messageBus, PageService pageService)
        {
            _messageBus = messageBus;
            _pageService = pageService;
        }

        public ICommand RemoveCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<BanksViewModel>(new TextMessage("remove", Bank.Id));
        });

        public ICommand EditCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("edit", Bank.Id));
            _pageService.ChangePage(new BankEdit());
        });
    }
}
