using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace Training_1.Steps
{
    public class CartPageSteps
    {
        IWebDriver _driver;
        private WebDriverWait _wait;

        private NavbarSteps _navbar;
        private PopUpSteps _popup;

        public CartPageSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            _navbar = new NavbarSteps(driver);
            _popup = new PopUpSteps(driver);
        }

        public bool WeAreOnCartPage()
        {
            return _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@data-target,'orderModal')]"))).Displayed;
        }

        public int GetNumberOfProducts()
        {
            while (WeAreOnCartPage() != true) ;

            return _driver.FindElements(By.ClassName("success")).Count;
        }

        public string GetProductName()
        {
            while (WeAreOnCartPage() != true) ;
            return _driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/tr/td[2]")).Text;     //nu am gasit alta metoda de a prelua textul ala...
        }

        public int GetProductPrice()
        {
            while (WeAreOnCartPage() != true) ;
            return int.Parse(_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/tr/td[3]")).Text);     //nu am gasit alta metoda de a prelua textul ala...
        }

        public bool BuyAll()
        {
            bool status = false;
            IWebElement element;

            _navbar.ClickCart();
            ClickPlaceOrder();
            CompleteAllFieldsNeeded("AlexG", "Romania", "Timisoasa", "4444-5555-6666-8777");
            ClickModalPurchase();

            element = _driver.FindElement(By.XPath("//*[contains(class,'sweet-alert  showSweetAlert visible')]"));
            status = element.GetAttribute("innerText").ToLower().Contains("thank you for your purchase!");

            ClickSweetOK();

            return status;
        }

        public void ClickPlaceOrder()
        {
            _driver.FindElement(By.XPath("//*[contains(@data-target,'orderModal')]")).Click();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("orderModal"))).GetAttribute("exampleModal").Equals(null);
        }

        public void CompleteAllFieldsNeeded(string Name, string Country, string City, string cardNo)
        {
            _driver.FindElement(By.Id("name")).SendKeys(Name);
            _driver.FindElement(By.Id("country")).SendKeys(Country);
            _driver.FindElement(By.Id("city")).SendKeys(City);
            _driver.FindElement(By.Id("card")).SendKeys(cardNo);
            _driver.FindElement(By.Id("month")).SendKeys(DateTime.Today.Month.ToString());
            _driver.FindElement(By.Id("year")).SendKeys(DateTime.Today.Year.ToString());
        }

        public void ClickModalPurchase()
        {
            _driver.FindElement(By.XPath("//*[contains(@onclick, 'purchaseOrder')]")).Click();
            while (_wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.XPath("//*[contains(class,'sweet-alert  showSweetAlert visible')]"))).Displayed != true) ;
        }

        public void ClickSweetOK()
        {
            _driver.FindElement(By.XPath("//*[contains(@class,'confirm btn btn-lg btn-primary')]")).Click();
            while (_wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("sweet-overlay"))).Displayed) ;
        }

    }
}