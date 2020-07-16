using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Threading;

namespace Training_1.Steps
{
    class NavbarSteps
    {
        IWebDriver _driver;
        private WebDriverWait _wait;
        
        //private CartPageSteps _cartPage;

        public NavbarSteps(IWebDriver driver)
        {
            _driver = driver; 
            _wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            //_cartPage = new CartPageSteps(driver);
        }

        public void ClickHome()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class,'nav-item active')]")));
            _driver.FindElement(By.XPath("//*[contains(@class,'nav-item active')]")).Click();
        }

        public void ClickContact()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@data-target,'exampleModal')]")));
            _driver.FindElement(By.XPath("//*[contains(@data-target,'exampleModal')]")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("exampleModalLabel")));
        }

        public void ClickAboutUs()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@data-target,'#videoModal')]")));
            _driver.FindElement(By.XPath("//*[contains(@data-target,'#videoModal')]")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("videoModalLabel")));

        }

        public void ClickCart()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
            _driver.FindElement(By.Id("cartur")).Click();
            while (new CartPageSteps(_driver).WeAreOnCartPage() != true) ;
        }

        public void ClickLogIn()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("login2")));
            _driver.FindElement(By.Id("login2")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("logInModal")));
        }

        public void ClickLogOut()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("logout2")));
            _driver.FindElement(By.Id("logout2")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("logInModal"))).GetAttribute("style.display").Equals("none");
        }

        public void clickUserName()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("nameofuser")));
            _driver.FindElement(By.Id("nameofuser")).Click();       //dummy button :))
        }

        public void ClickSignIn()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("signin2")));
            _driver.FindElement(By.Id("signin2")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("signInModal"))).GetAttribute("ariaHidden").Equals(null);
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'register')]")));
        }


    }
}
