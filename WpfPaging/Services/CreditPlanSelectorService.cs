using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WpfPaging.Models;
using WpfPaging.Models.Enums;

namespace WpfPaging.Services
{
    public class CreditPlanSelectorService
    {
        private readonly BankContext _context;

        public CreditPlanSelectorService(BankContext context)
        {
            _context = context;
        }

        public async Task<List<CreditPlan>> SelectAvailableCreditPlans(User user, CreditTarget target)
        {
            var banks = await _context.Banks.Include(bank => bank.ScoringSystemsItems).Include(bank => bank.CreditPlans)
                .AsNoTracking().ToListAsync();

            Dictionary<Bank, int> scores = new Dictionary<Bank, int>();
            foreach (var item in banks)
            {
                int score = 0;

                var grouped = item.ScoringSystemsItems.GroupBy(x => x.Type);

                foreach (var group in grouped)
                {
                    switch (group.Key)
                    {
                        case ScoringItemType.Age:
                            var agePoints = group.Where(x =>user.Age.HasValue && user.Age >= x.MinCondition && user.Age <= x.MaxCondition)
                                .OrderBy(x => x.Points).FirstOrDefault();
                            if (agePoints != null)
                            {
                                score += agePoints.Points;
                            }
                            break;
                        case ScoringItemType.KidsAmount:
                            var kidsPoints = group.Where(x => user.KidsAmount.HasValue && user.KidsAmount >= x.MinCondition && user.KidsAmount <= x.MaxCondition)
                                .OrderBy(x => x.Points).FirstOrDefault();
                            if (kidsPoints != null)
                            {
                                score += kidsPoints.Points;
                            }
                            break;
                        case ScoringItemType.JobType:
                            var jobPoints = group.Where(x => user.JobType.HasValue && x.ExactValue == (int)user.JobType)
                                .OrderBy(x => x.Points).FirstOrDefault();
                            if (jobPoints != null)
                            {
                                score += jobPoints.Points;
                            }
                            break;
                        case ScoringItemType.FamilyState:
                            var familyPoints = group.Where(x => user.FamilyState.HasValue && x.ExactValue == (int)user.FamilyState)
                                .OrderBy(x => x.Points).FirstOrDefault();
                            if (familyPoints != null)
                            {
                                score += familyPoints.Points;
                            }
                            break;
                        case ScoringItemType.Qualification:
                            var qualificationPoints = group.Where(x => user.Qualification.HasValue && x.ExactValue == (int)user.Qualification)
                                .OrderBy(x => x.Points).FirstOrDefault();
                            if (qualificationPoints != null)
                            {
                                score += qualificationPoints.Points;
                            }
                            break;
                    }
                }
                scores.Add(item, score);
            }

            List<CreditPlan> result = new List<CreditPlan>();
            foreach (var score in scores)
            {
                result.AddRange(score.Key.CreditPlans.Where(x => x.Target == target && x.MinimalScore <= score.Value));
            }

            result = result.OrderBy(x => x.Amount).ToList();

            //Нужно чтобы показать анимацию...(имитация нагрузки)
            await Task.Run(() => { Thread.Sleep(2000); });

            return result;
        }
    }
}
