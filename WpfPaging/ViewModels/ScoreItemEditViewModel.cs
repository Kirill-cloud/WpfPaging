using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Microsoft.EntityFrameworkCore;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class ScoreItemEditViewModel : BindableBase
    {
        private readonly BankContext _db;
        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;
        private Bank _bank;
        public ScoringSystemItem Item { get; set; }
        public Visibility RangeVisibility
        {
            get
            {
                if (Item.Type == ScoringItemType.Age || Item.Type == ScoringItemType.KidsAmount)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }
        public Visibility ExactVisibility
        {
            get
            {
                if (Item.Type == ScoringItemType.FamilyState || Item.Type == ScoringItemType.Qualification || Item.Type == ScoringItemType.JobType)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
        }

        public string MinCondition
        {
            get
            {
                return Item.MinCondition.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    Item.MinCondition = t;
                }
            }
        }

        public string MaxCondition
        {
            get
            {
                return Item.MaxCondition.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    Item.MaxCondition = t;
                }
            }
        }

        public List<NamedValue> _exactValues;
        public List<NamedValue> ExactValues
        {
            get { return _exactValues; }
        }

        private List<NamedValue> GetComboBoxValues()
        {
            if (Item.Type == ScoringItemType.FamilyState)
            {
                return EnumConverter.GetEnumMembersNumbers<FamilyStates>().Select(x => new NamedValue()
                {
                    Name = EnumConverter.GetString((FamilyStates)x),
                    Value = x,
                }).ToList();
            }

            if (Item.Type == ScoringItemType.JobType)
            {
                return EnumConverter.GetEnumMembersNumbers<JobTypes>().Select(x => new NamedValue()
                {
                    Value = x,
                    Name = EnumConverter.GetString((JobTypes)x)
                }).ToList();
            }

            if (Item.Type == ScoringItemType.Qualification)
            {
                return EnumConverter.GetEnumMembersNumbers<QualificationTypes>().Select(x => new NamedValue()
                {
                    Value = x,
                    Name = EnumConverter.GetString((QualificationTypes)x)
                }).ToList();
            }

            return null;
        }


        private NamedValue _exactValue;

        public NamedValue ExactValue
        {
            get
            {
                return _exactValue;
            }
            set
            {
                _exactValue = value;
                if (value != null)
                {
                    Item.ExactValue = _exactValue.Value;
                }
            }
        }

        public string Points
        {
            get
            {
                return Item.Points.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    Item.Points = t;
                }
            }
        }

        public ScoreItemEditViewModel(MessageBus messageBus, BankContext db, PageService pageService)
        {
            _messageBus = messageBus;
            _db = db;
            _pageService = pageService;
            _messageBus.Receive<ScoreSystemMessage>(this, ReciveTypeHandler);
            _messageBus.Receive<TextMessage>(this, ReciveTextHandler);
        }

        private async Task ReciveTextHandler(TextMessage arg)
        {
            if (arg.Text == "edit" && arg.Id.HasValue)
            {
            }
        }

        private async Task ReciveTypeHandler(ScoreSystemMessage arg)
        {
            if (arg.ScoringItemId != 0)
            {
                Item = _db.ScoringSystems.FirstOrDefault(c => c.Id == arg.ScoringItemId);
                _exactValues = GetComboBoxValues();
                ExactValue = ExactValues.Find(x => x.Value == Item.ExactValue);

            }
            else
            {
                Item = new ScoringSystemItem()
                {
                    Type = arg.ScoringItemType,
                    Bank = _db.Banks.Include(b => b.ScoringSystemsItems).FirstOrDefault(b => b.Id == arg.BankId)
                };
                _exactValues = GetComboBoxValues();
            }
        }
        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Loading());
            _db.ScoringSystems.Update(Item);
            await _db.SaveChangesAsync();
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("update"));
            _pageService.ChangePage(new BankEdit());
        });
    }
}
