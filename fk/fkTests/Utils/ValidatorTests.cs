using Microsoft.VisualStudio.TestTools.UnitTesting;
using fk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fk.Utils.Tests
{
    [TestClass()]
    public class ValidatorTests
    {
        [TestMethod]
        public void ValidateDigitTest1()
        {
            Boolean actual = Validator.ValidateDigit("5", 4, 6);
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void ValidateDigitTest2()
        {
            Boolean actual = Validator.ValidateDigit("3", 4, 6);
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public void ValidateDigitTest3()
        {
            Boolean actual = Validator.ValidateEmail("email@mail.ru");
            Assert.IsTrue(actual);
        }
        [TestMethod]
        public void ValidateDigitTest4()
        {
            Boolean actual = Validator.ValidateEmail("no_email");
            Assert.IsFalse(actual);
        }
        [TestMethod]
        public void ErrorEmailTest()
        {
            ErrorsContext error = new ErrorsContext();
            error.ErrorEmail = "nothing";
            String actual = error.ErrorEmail;
            Assert.AreEqual(actual, "nothing");
        }
    }
}