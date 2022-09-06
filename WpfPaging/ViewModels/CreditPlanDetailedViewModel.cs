using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class CreditPlanDetailedViewModel
    {
        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;
        public CreditPlan Item { get; set; }
        public string Target
        {
            get
            {
                if (Item != null)
                {
                    return EnumConverter.GetString(Item.Target);
                }
                return null;
            }
        }

        public CreditPlanDetailedViewModel(MessageBus messageBus, PageService pageService)
        {
            _pageService = pageService;
            _messageBus = messageBus;
        }
        public ICommand RemoveCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("removePlan", Item.Id));
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("update"));
        });

        public ICommand EditCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<PlanEditViewModel>(new DependedObjectMessage(Item.BankId, Item.Id));
            _pageService.ChangePage(new PlanEdit());
        });
    }
}
