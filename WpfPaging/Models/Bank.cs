using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using WpfPaging.Models.Enums;

namespace WpfPaging.Models
{
    public class Bank : IdEntity
    {
        public string Name { get; set; }
        public List<CreditPlan> CreditPlans { get; set; }

        public List<ScoringSystemItem> ScoringSystemsItems { get; set; }

        public int GetScore(User user)
        {
            return 1;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
