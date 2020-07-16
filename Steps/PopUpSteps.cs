using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;

namespace Training_1.Steps
{
    class PopUpSteps
    {

        IWebDriver _driver;
        private WebDriverWait _wait;

        public PopUpSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
        }

        public void PopUpAccept()
        {
            PopUpAvailable();
            _driver.SwitchTo().Alert().Accept();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertState(false));

        }

        public string GetPopUpText()
        {
            return _driver.SwitchTo().Alert().Text;
        }

        public void PopUpAvailable()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
        }
    }
}
