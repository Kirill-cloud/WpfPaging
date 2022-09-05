using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class ScoreItemViewModel
    {
        public ScoringSystemItem Item { get; set; }

        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;

        public ScoreItemViewModel(MessageBus messageBus, PageService pageService)
        {
            _messageBus = messageBus;
            _pageService = pageService;
        }

        public string ExactValue
        {
            get
            {
                return GetComplexValue(Item);
            }
        }

        private string GetComplexValue(ScoringSystemItem Item)
        {
            if (Item.ExactValue.HasValue)
            {
                if (Item.Type == ScoringItemType.FamilyState)
                {
                    var state = (FamilyStates)Item.ExactValue;
                    return EnumConverter.GetString(state);
                }
                if (Item.Type == ScoringItemType.JobType)
                {
                    var state = (JobTypes)Item.ExactValue;
                    return EnumConverter.GetString(state);
                }
                if (Item.Type == ScoringItemType.Qualification)
                {
                    var state = (QualificationTypes)Item.ExactValue;
                    return EnumConverter.GetString(state);
                }
            }

            return null;
        }

        public ICommand RemoveCommand => new AsyncCommand(async () =>
            {
                await _messageBus.SendTo<BankEditViewModel>(new TextMessage("remove", Item.Id));
                await _messageBus.SendTo<BankEditViewModel>(new TextMessage("update"));
            });

        public ICommand EditCommand => new AsyncCommand(async () =>
        {
            await _messageBus.SendTo<ScoreItemEditViewModel>(new ScoreSystemMessage(Item.Type, Item.BankId, Item.Id));
            _pageService.ChangePage(new ScoringItemEdit());
        });
    }
}
