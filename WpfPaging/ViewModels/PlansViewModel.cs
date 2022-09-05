using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using WpfPaging.Models;
using WpfPaging.Models.Enums;
using WpfPaging.Pages;
using WpfPaging.Services;

namespace WpfPaging.ViewModels
{
    public class PlansViewModel : BindableBase
    {
        public Visibility NoPlans
        {
            get
            {
                if (_changes && (CreditPlans == null || CreditPlans.Count == 0))
                {
                    return Visibility.Visible;
                }
                return Visibility.Collapsed;
            }
        }
        public List<CreditPlanViewModel> CreditPlans { get; set; }

        public string Age
        {
            get => _user.Age.ToString();
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    _user.Age = t;
                }
                else
                {
                    _user.Age = null;
                }
            }
        }
        public string Kids
        {
            get => _user.KidsAmount.ToString();
            set
            {
                int t;
                if (int.TryParse(value, out t))
                {
                    _user.KidsAmount = t;
                }
                else
                {
                    _user.KidsAmount = null;
                }
            }
        }

        public List<NamedValue> Jobs { get; } = EnumConverter.GetEnumMembersNumbers<JobTypes>().Select(x => new NamedValue()
        {
            Name = EnumConverter.GetString((JobTypes)x),
            Value = x,
        }).ToList();
        public NamedValue _job;
        public NamedValue Job
        {
            get
            {
                return _job;
            }

            set
            {
                _job = value;
                if (value is null)
                {
                    _user.JobType = null;
                }
                else
                {
                    _user.JobType = (JobTypes)value.Value;
                }
            }
        }

        public List<NamedValue> Families { get; } = EnumConverter.GetEnumMembersNumbers<FamilyStates>().Select(x => new NamedValue()
        {
            Name = EnumConverter.GetString((FamilyStates)x),
            Value = x,
        }).ToList();
        public NamedValue _family;
        public NamedValue Family
        {
            get
            {
                return _family;
            }

            set
            {
                _family = value;
                if (value is null)
                {
                    _user.FamilyState = null;
                }
                else
                {
                    _user.FamilyState = (FamilyStates)value.Value;
                }
            }
        }

        public List<NamedValue> Qualifications { get; } = EnumConverter.GetEnumMembersNumbers<QualificationTypes>().Select(x => new NamedValue()
        {
            Name = EnumConverter.GetString((QualificationTypes)x),
            Value = x,
        }).ToList();
        public NamedValue _qualification;
        public NamedValue Qualification
        {
            get
            {
                return _qualification;
            }

            set
            {
                _qualification = value;
                if (value is null)
                {
                    _user.Qualification = null;
                }
                else
                {
                    _user.Qualification = (QualificationTypes)value.Value;
                }
            }
        }

        private User _user = new User();
        private PageService _pageService;
        private readonly CreditPlanSelectorService _selectorService;
        private bool _changes = false;

        public PlansViewModel(PageService pageService, CreditPlanSelectorService selectorService)
        {
            _pageService = pageService;
            _selectorService = selectorService;
        }
        public ICommand GoToMain => new AsyncCommand(async () =>
        {
            _pageService.ChangePage(new MainPage());
            _changes = false;
        });

        public ICommand GetPlans => new AsyncCommand(async () =>
        {
            _changes = true;
            _pageService.ChangePage(new Loading());
            var availablePlans = await _selectorService.SelectAvailableCreditPlans(_user, CreditTarget.Other);

            CreditPlans = availablePlans.Select(x => new CreditPlanViewModel() { CreditPlan = x }).ToList();
            _pageService.ChangePage(new Plans());
        });

        public ICommand Reset => new AsyncCommand(async () =>
        {
            Age = null;
            Kids = null;
            Job = null;
            Family = null;
            Qualification = null;
        });
    }
}
