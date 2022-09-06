using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using EnumConverter = WpfPaging.Models.Enums.EnumConverter;
using System.Windows.Controls;

namespace WpfPaging.ViewModels
{
    public class ScoreItemEditViewModel : BindableBase, IDataErrorInfo
    {
        private readonly BankContext _db;
        private readonly MessageBus _messageBus;
        private readonly PageService _pageService;
        private Bank _bank;
        public ScoringSystemItem ScoreItem { get; set; }
        public Visibility RangeVisibility
        {
            get
            {
                if (ScoreItem.Type == ScoringItemType.Age || ScoreItem.Type == ScoringItemType.KidsAmount)
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
                if (ScoreItem.Type == ScoringItemType.FamilyState || ScoreItem.Type == ScoringItemType.Qualification || ScoreItem.Type == ScoringItemType.JobType)
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
                return ScoreItem.MinCondition.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    ScoreItem.MinCondition = t;
                }
            }
        }

        public string MaxCondition
        {
            get
            {
                return ScoreItem.MaxCondition.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    ScoreItem.MaxCondition = t;
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
            if (ScoreItem.Type == ScoringItemType.FamilyState)
            {
                return EnumConverter.GetEnumMembersNumbers<FamilyStates>().Select(x => new NamedValue()
                {
                    Name = EnumConverter.GetString((FamilyStates)x),
                    Value = x,
                }).ToList();
            }

            if (ScoreItem.Type == ScoringItemType.JobType)
            {
                return EnumConverter.GetEnumMembersNumbers<JobTypes>().Select(x => new NamedValue()
                {
                    Value = x,
                    Name = EnumConverter.GetString((JobTypes)x)
                }).ToList();
            }

            if (ScoreItem.Type == ScoringItemType.Qualification)
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
                    ScoreItem.ExactValue = _exactValue.Value;
                }
            }
        }

        public string Points
        {
            get
            {
                return ScoreItem.Points.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    ScoreItem.Points = t;
                }
            }
        }

        public ScoreItemEditViewModel(MessageBus messageBus, BankContext db, PageService pageService)
        {
            _messageBus = messageBus;
            _db = db;
            _pageService = pageService;
            _messageBus.Receive<ScoreSystemMessage>(this, ReciveTypeHandler);
        }

        private async Task ReciveTypeHandler(ScoreSystemMessage arg)
        {
            if (arg.ScoringItemId != 0)
            {
                ScoreItem = _db.ScoringSystems.FirstOrDefault(c => c.Id == arg.ScoringItemId);
                _exactValues = GetComboBoxValues();
                if (_exactValues != null)
                {
                    ExactValue = ExactValues.Find(x => x.Value == ScoreItem.ExactValue);
                }
            }
            else
            {
                ScoreItem = new ScoringSystemItem()
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
            _db.ScoringSystems.Update(ScoreItem);
            await _db.SaveChangesAsync();
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("update"));
            _pageService.ChangePage(new BankEdit());
        }, CanExecuteMethod);

        private bool CanExecuteMethod()
        {
            return ScoreItem.IsValid;
        }

        Func<string, string> selector = str => str.ToUpper();

        public string Error => throw new NotImplementedException();
        public string PointsError { get; set; }
        public string MinError { get; set; }
        public string MaxError { get; set; }
        public string ExactError { get; set; }
        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case nameof(Points):
                        if (Points != null && Points != string.Empty && Convert.ToInt32(Points) < 0)
                        {
                            error = "должен быть больше 0";
                            PointsError = error;
                        }
                        else
                        {
                            PointsError = null;
                        }
                        break;
                    case nameof(MinCondition):
                        if (MinCondition != null && MinCondition != string.Empty && Convert.ToInt32(MinCondition) < 0)
                        {
                            error = "должен быть больше 0";
                            MinError = error;
                        }
                        else
                        {
                            MinError = null;
                        }
                        break;
                    case nameof(MaxCondition):
                        if (MaxCondition != null && MaxCondition != string.Empty && Convert.ToInt32(MaxCondition) < 0)
                        {
                            error = "должен быть больше 0";
                            MaxError = error;
                        }
                        else
                        {
                            MaxError = null;
                        }
                        break;
                    //case nameof(ExactValue):
                    //    if (ExactValue != null && ExactValue != string.Empty && Convert.ToInt32(ExactValue) < 0)
                    //    {
                    //        error = "должен быть больше 0";
                    //        ExactError = error;
                    //    }
                    //    else
                    //    {
                    //        ExactError = null;
                    //    }
                    //    break;
                }
                return error;
            }
        }
    }
}
