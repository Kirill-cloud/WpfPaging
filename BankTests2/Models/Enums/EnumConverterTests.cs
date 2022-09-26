using Microsoft.VisualStudio.TestTools.UnitTesting;
using WpfPaging.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfPaging.Models.Enums.Tests
{
    [TestClass()]
    public class EnumConverterTests
    {
        [TestMethod()]
        public void GetStringNotSupportedTest()
        {
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                EnumConverter.GetString(new object());
            });

            Assert.ThrowsException<NotSupportedException>(() =>
            {
                EnumConverter.GetString<object>(null);
            });

            FamilyStates family = (FamilyStates)99;
            Assert.ThrowsException<NotSupportedException>(() =>
            {
                EnumConverter.GetString(family);
            });
        }

        [TestMethod()]
        public void GetStringTest()
        {
            Assert.AreEqual("Холост (не замужем)", EnumConverter.GetString(FamilyStates.Single));
            Assert.AreEqual("Другие", EnumConverter.GetString(JobTypes.Other));
            Assert.AreEqual("Нет квалификации", EnumConverter.GetString(QualificationTypes.None));
            Assert.AreEqual("Авто", EnumConverter.GetString(CreditTarget.Auto));
        }
    }
}