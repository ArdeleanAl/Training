using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;

namespace Training_1.Steps
{
    class LogInSteps
    {
        IWebDriver _driver;

        private NavbarSteps _navbar;

        public LogInSteps(IWebDriver driver)
        {
            _driver = driver;
            _navbar = new NavbarSteps(driver);
        }


        public void LogInWithCredentials(string UserName, string UserPassword)
        {
            _navbar.ClickLogIn();

            TypeUserName(UserName);
            TypeUserPassword(UserPassword);
            ClickOnModalLogInButton();

        }

        public void TypeUserName(string UserName)
        {
            _driver.FindElement(By.Id("loginusername")).SendKeys(UserName);
        }

        public void TypeUserPassword(string UserPassword)
        {
            _driver.FindElement(By.Id("loginpassword")).SendKeys(UserPassword);
        }

        public void ClickOnModalLogInButton()
        {
            _driver.FindElement(By.XPath("//button[@type='button'][text()='Log in']")).Click();
        }
    }
}
