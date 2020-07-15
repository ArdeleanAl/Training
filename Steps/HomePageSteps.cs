using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace Training_1.Steps
{
    public class HomePageSteps
    {
        IWebDriver _driver;
        private WebDriverWait _wait;
        private int _noOfImageSlides;
        private ProductPageSteps _productPage;

        private static string Url = "https://www.demoblaze.com/";

        public HomePageSteps(IWebDriver driver)
        {
            _driver = driver;
            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 30));
            _productPage = new ProductPageSteps(_driver);
        }

        public void GoToHomePage()
        {
            _driver.Navigate().GoToUrl(Url);
            _driver.Manage().Window.Maximize();
            _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            //_wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementExists(By.Id("nava")));
        }

        public bool ImageSlideAvailable()
        {
            return _driver.FindElement(By.ClassName("carousel-item")).Displayed;
        }

        public string GetUserName()
        {
            while (_wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("nameofuser"))).GetAttribute("scrollHeight").Equals(0)) ;
            return _driver.FindElement(By.Id("nameofuser")).Text;
        }

        public void SelectFirstItem()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("card-title")));// xpathurile nu is cei mai buni identificatori deoarece la cea mai mica schimbare ele se pot schimba si devin invalide --> done
            _driver.FindElement(By.ClassName("card-title")).Click();
            while (_productPage.WeAreonProductPage() != true) ;
        }

        public void FilterByPhones()
        {
            GoToHomePage();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'phone')]")));
            _driver.FindElement(By.XPath("//*[contains(@onclick,'phone')]")).Click();
            WaitForFilterToBeApplied();
        }

        public void FilterByLaptops()
        {
            GoToHomePage();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'notebook')]")));
            _driver.FindElement(By.XPath("//*[contains(@onclick,'notebook')]")).Click();
            WaitForFilterToBeApplied();
        }

        public void FilterByMonitors()
        {
            GoToHomePage();
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'monitor')]")));
            _driver.FindElement(By.XPath("//*[contains(@onclick,'monitor')]")).Click();
            WaitForFilterToBeApplied();
        }

        public void WaitForFilterToBeApplied()
        {
            int numberOfProducts;
            int index = 0;

            numberOfProducts = _driver.FindElements(By.ClassName("card-block")).Count;
            while ((numberOfProducts == _driver.FindElements(By.ClassName("card-block")).Count) && index < 100)
            {
                index++;
            }
        }

        public string GetCurrentSliderImageText()
        {
            _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@class,'carousel-item active')]")));
            _noOfImageSlides = _driver.FindElements(By.XPath("//*[(@class='carousel-item')]")).Count;
            return _driver.FindElement(By.XPath("//div[contains(@class,'carousel-item active')]/img")).GetProperty("alt");
        }

        public void ClickSliderPrevious()
        {
            _driver.FindElement(By.XPath("//*[contains(@class,'carousel-control-prev-icon')]")).Click();
            while (_driver.FindElements(By.XPath("//*[(@class='carousel-item')]")).Count < _noOfImageSlides) ;
        }

        public void ClickSliderNext()
        {
            _driver.FindElement(By.XPath("//*[contains(@class,'carousel-control-next-icon')]")).Click();
            while (_driver.FindElements(By.XPath("//*[(@class='carousel-item')]")).Count < _noOfImageSlides) ;
        }

        public bool BuyDellLaptopFrom2017()
        {
            int i;
            int noOfProducts;
            IWebElement currentProduct;

            noOfProducts = _driver.FindElements(By.ClassName("card-title")).Count;

            for (i = 1; i <= noOfProducts; i++)
            {
                try
                {
                    currentProduct = _driver.FindElements(By.ClassName("card-title")).ElementAt(i);

                    if (currentProduct.Text.Contains("2017") && currentProduct.Text.ToLower().Contains("dell"))
                    {
                        _wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("card-title")));
                        currentProduct.Click();

                        _productPage.AddToCart();
                        return true;

                    }
                }
                catch (Exception)
                {
                    //Console.WriteLine("No Element found");
                    return false;
                }
            }
            return false;
        }

        public bool AddANumberOfPhoneWithinTheBudget(int numberOfPhones, int budget)
        {
            int objPrice;
            int index = 1;
            int sum = 0;
            IWebElement currentProduct;
            Random rand = new Random();

            while (index != numberOfPhones)
                try
                {
                    FilterByPhones();
                    currentProduct = _driver.FindElements(By.ClassName("card-block")).ElementAt(rand.Next(GetTheNumberOfProducts() - 1) + 1);
                    objPrice = GetProductPrice(currentProduct);

                    if (sum + objPrice < budget)
                    {
                        currentProduct.Click();
                        _productPage.AddToCart();
                        sum += objPrice;
                        index++;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            return true;

        }

        public int GetTheNumberOfProducts()
        {
            return _driver.FindElements(By.XPath("//*[contains(@class,'col-lg-4 col-md-6 mb-4')]")).Count;
        }

        public int GetProductPrice(IWebElement element)
        {
            int startIndex;
            int endIndex;
            startIndex = element.GetAttribute("innerHTML").ToString().IndexOf('$');
            endIndex = element.GetAttribute("innerHTML").ToString().IndexOf("<//h5>");
            return int.Parse(element.GetAttribute("innerHTML").Substring(startIndex, (endIndex - startIndex)));
        }

        public bool BuyAPhoneALaptopAndAMonitorWithinBudget(int Budget)
        {
            bool found = false;
            int noOfPhones, noOfLaptops, noOfMonitors;
            int tempPhoneValue, tempLaptopValue, tempMonitorValue;
            IWebElement currentProduct;

            //Init tempValues
            FilterByPhones();
            noOfPhones = GetTheNumberOfProducts();
            FilterByPhones();
            noOfLaptops = GetTheNumberOfProducts();
            FilterByPhones();
            noOfMonitors = GetTheNumberOfProducts();

            for (int i = 1; i < noOfPhones && !found; i++)
            {
                FilterByPhones();
                currentProduct = GetProductSpecifiedByIndex(i);
                tempPhoneValue = GetProductPrice(currentProduct);

                for (int j = 1; j < noOfLaptops && !found; j++)
                {
                    FilterByLaptops();
                    currentProduct = GetProductSpecifiedByIndex(j);
                    tempLaptopValue = GetProductPrice(currentProduct);

                    for (int k = 1; k < noOfMonitors && !found; k++)
                    {
                        FilterByPhones();
                        currentProduct = GetProductSpecifiedByIndex(k);
                        tempMonitorValue = GetProductPrice(currentProduct);

                        if (tempMonitorValue + tempLaptopValue + tempPhoneValue < Budget)
                        {
                            found = true;
                            currentProduct.Click();
                            _productPage.AddToCart();
                        }
                    }
                    if (found)
                    {
                        FilterByLaptops();
                        currentProduct = GetProductSpecifiedByIndex(j);
                        currentProduct.Click();
                        _productPage.AddToCart();
                    }
                }
                if (found)
                {
                    FilterByPhones();
                    currentProduct = GetProductSpecifiedByIndex(i);
                    currentProduct.Click();
                    _productPage.AddToCart();

                }
            }

            if (found)
            {
                return true;
            }
            return false;
        }

        public IWebElement GetProductSpecifiedByIndex(int index)
        {
            return _driver.FindElements(By.ClassName("card-block")).ElementAt(index);
        }

    }

}
