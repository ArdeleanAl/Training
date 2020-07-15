using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Training_1.Steps
{
    class SignUpSteps
    {

        IWebDriver _driver;
        private WebDriverWait _wait;
        private PopUpSteps _popup;

        public SignUpSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            _popup = new PopUpSteps(_driver);
        }

        public string GetModalTitleText()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("signInModal")));
            return _driver.FindElement(By.Id("signInModalLabel")).Text;
        }

        public void TypeUserName(string UserName)
        {
            _driver.FindElement(By.Id("sign-username")).SendKeys(UserName);
        }

        public void TypeUserPassword(string UserPassword)
        {
            _driver.FindElement(By.Id("sign-password")).SendKeys(UserPassword);
        }

        public void ClickOnModalSignUpButton()
        {
            _driver.FindElement(By.XPath("//button[@type='button'][text()='Sign up']")).Click();
        }
    }
}
