using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Windows.Navigation;

namespace WpfPaging.Models.Enums
{
    public class EnumConverter
    {
        private static string GetStringState(FamilyStates state) => state switch
        {
            FamilyStates.Single => "Холост (не замужем)",
            FamilyStates.Married => "Женат (замужем)",
            FamilyStates.PlanningDivorce => "В браке, но живет раздельно",
            FamilyStates.Divorced => "В разводе",
            FamilyStates.Widow => "Вдовец (вдова)",
            _ => throw new NotSupportedException("не известное состояние")
        };
        private static string GetStringState(JobTypes state) => state switch
        {
            JobTypes.PublicService => "Госслужба",
            JobTypes.CommercialStructure => "Коммерческая структура",
            JobTypes.Pensioner => "Пенсионер",
            JobTypes.Other => "Другие",
            _ => throw new NotSupportedException("не известное состояние")
        };

        private static string GetStringState(QualificationTypes state) => state switch
        {
            QualificationTypes.None => "Нет квалификации",
            QualificationTypes.Servants => "Обслуживающий персонал",
            QualificationTypes.Specialist => "Специалист",
            QualificationTypes.Supervisor => "Руководитель",
            _ => throw new NotSupportedException("не известное состояние")
        };

        private static string GetStringState(CreditTarget state) => state switch
        {
            CreditTarget.Other => "Другие",
            CreditTarget.Auto => "Авто",
            CreditTarget.Mortgage => "Ипотека",
            _ => throw new NotSupportedException("не известное состояние")
        };

        public static string GetString<T>(T state)
        {
            if (state is FamilyStates family)
            {
                return GetStringState(family);
            }

            if (state is JobTypes job)
            {
                return GetStringState(job);
            }

            if (state is QualificationTypes qualification)
            {
                return GetStringState(qualification);
            }

            if (state is CreditTarget target)
            {
                return GetStringState(target);
            }

            throw new NotSupportedException();
        }

        public static List<int> GetEnumMembersNumbers<T>() where T : Enum
        {
            List<int> result = new List<int>();
            foreach (var member in Enum.GetValues(typeof(T)))
            {
                result.Add((int)member);
            }

            return result;
        }
    }
}
