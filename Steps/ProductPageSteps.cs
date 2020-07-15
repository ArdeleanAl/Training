using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Training_1.Steps
{
    class ProductPageSteps
    {
        IWebDriver _driver;
        private WebDriverWait _wait;
        PopUpSteps _popup;

        public ProductPageSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(driver, new TimeSpan(0, 0, 5));
            _popup = new PopUpSteps(driver);
        }


        public void AddToCart()
        {
            ClickAddToCartButton();
            _popup.PopUpAccept();
        }

        public void ClickAddToCartButton()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]"))).Text.Equals("Add to cart");
            _driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();

        }

        public string GetProductName()
        {
            return _driver.FindElement(By.ClassName("name")).Text;
        }

        public int GetProductPrice()
        {
            return int.Parse(Regex.Match(_driver.FindElement(By.ClassName("price-container")).Text, @"\d+").Value);
        }

        public bool WeAreonProductPage()
        {
            try
            {
                return _driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Text.ToLower().Contains("add to cart");                
            }
            catch
            {
                return false;
            }
        }
    }
}
