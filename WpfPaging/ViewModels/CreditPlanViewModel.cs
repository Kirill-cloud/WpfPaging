using System;
using System.Collections.Generic;
using System.Text;
using WpfPaging.Models;

namespace WpfPaging.ViewModels
{
    public class CreditPlanViewModel
    {
        public CreditPlan CreditPlan { get; set; }

        public override string ToString()
        {
            return $"От ({CreditPlan.Bank.Name}) на сумму: {CreditPlan.Amount}";
        }
    }
}
