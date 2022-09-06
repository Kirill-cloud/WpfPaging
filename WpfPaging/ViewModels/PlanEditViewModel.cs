using DevExpress.Mvvm;
using Microsoft.EntityFrameworkCore;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WpfPaging.Messages;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;
using EnumConverter = WpfPaging.Models.Enums.EnumConverter;

namespace WpfPaging.ViewModels
{
    public class PlanEditViewModel :BindableBase, IDataErrorInfo
    {
        private readonly PageService _pageService;
        private readonly BankContext _context;
        private readonly MessageBus _messageBus;
        public CreditPlan CreditPlanItem { get; set; } = new CreditPlan();
        private string _minimalScore;
        public string MinimalScore
        {
            get
            {
                return CreditPlanItem.MinimalScore.ToString();
            }
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    CreditPlanItem.MinimalScore = t;
                }
            }
        }
        public string Amount
        {
            get
            {
                return CreditPlanItem.Amount.ToString();
            }
            set
            {
                decimal t;
                if (decimal.TryParse(value, out t))
                {
                    CreditPlanItem.Amount = t;
                }
            }
        }

        public NamedValue _creditType;
        public List<NamedValue> CreditTypes { get; set; }

        public NamedValue CreditType
        {
            get
            {
                return _creditType;
            }
            set
            {
                _creditType = value;
                if (value != null)
                {
                    CreditPlanItem.Target = (CreditTarget)value.Value;
                }
            }
        }

        public PlanEditViewModel(PageService pageService, MessageBus messageBus, BankContext context)
        {
            _pageService = pageService;
            _context = context;
            _messageBus = messageBus;
            CreditTypes = EnumConverter.GetEnumMembersNumbers<CreditTarget>().Select(x => new NamedValue()
            {
                Name = EnumConverter.GetString((CreditTarget)x),
                Value = x,
            }).ToList();
            _messageBus.Receive<DependedObjectMessage>(this, async message =>
            {
                if (message.DependItemId == 0)
                {
                    CreditPlanItem = new CreditPlan()
                    {
                        Bank = _context.Banks.Include(b => b.CreditPlans).FirstOrDefault(b => b.Id == message.SourceItemId)
                    };
                }
                else
                {
                    CreditPlanItem = _context.CreditPlans.FirstOrDefault(b => b.Id == message.DependItemId);
                }

                CreditType = CreditTypes.Find(x => x.Value == (int)CreditPlanItem.Target);
            });
        }


        public ICommand SaveCommand => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new Loading());
            _context.CreditPlans.Update(CreditPlanItem);
            await _context.SaveChangesAsync();
            await _messageBus.SendTo<BankEditViewModel>(new TextMessage("update"));
            _pageService.ChangePage(new BankEdit());
        }, CanExecuteMethod);

        private bool CanExecuteMethod()
        {
            return CreditPlanItem.IsValid;
        }

        public string Error => throw new NotImplementedException();
        public string AmountError { get; set; }
        public string MinError { get; set; }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                switch (columnName)
                {
                    case nameof(Amount):
                        if (Amount != null && Amount != string.Empty && Convert.ToDecimal(Amount) <= 0)
                        {
                            error = "должен быть больше 0";
                            AmountError = error;
                        }
                        else
                        {
                            AmountError = null;
                        }
                        break;
                    case nameof(MinimalScore):
                        if (MinimalScore != null && MinimalScore != string.Empty && Convert.ToDecimal(MinimalScore) <= 0)
                        {
                            error = "должен быть больше 0";
                            MinError = error;
                        }
                        else
                        {
                            MinError = null;
                        }
                        break;
                }

                return error;
            }
        }
    }
}
