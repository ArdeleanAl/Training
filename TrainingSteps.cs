using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

namespace Training_1
{
    [Binding]
    public class TrainingSteps
    {
        private readonly ITestOutputHelper scenario;
        IWebDriver driver = new ChromeDriver();
        private int budget;
        string slide;
        string tempPhoneName;
        int tempPhonePrice;

        public TrainingSteps(ITestOutputHelper sc)
        {
            scenario = sc;
        }


        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            driver.Navigate().GoToUrl("https://www.demoblaze.com/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
            driver.FindElement(By.Id("nava")).Click();

            string title = driver.Title;
            Assert.Equal("STORE", title);
        }


        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            Trace.WriteLine("we started");
            Debug.WriteLine("we started");
            driver.Navigate().GoToUrl("https://www.demoblaze.com/");
            driver.Manage().Window.Maximize();
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(5);
            driver.FindElement(By.Id("nava")).Click();

            driver.FindElement(By.Id("login2")).Click();
            Thread.Sleep(500);

            driver.FindElement(By.Id("loginusername")).SendKeys("AlexG");
            driver.FindElement(By.Id("loginpassword")).SendKeys("123456");
            driver.FindElement(By.XPath("//button[@type='button'][text()='Log in']")).Click();
            Thread.Sleep(1500);

            string info = driver.FindElement(By.Id("nameofuser")).Text;

            Assert.Equal("Welcome AlexG", info);


        }

        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            driver.FindElement(By.Id("signin2")).Click();
            Thread.Sleep(500);
            string element = driver.FindElement(By.Id("signInModalLabel")).Text;

            Assert.Equal("Sign up", element);
        }


        [Given(@"I select the first available product")]
        public void GivenISelectTheFirstAvailableProduct()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/div/h4/a")));
            driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/div/h4/a")).Click();

        }


        [Given(@"I add it to cart")]
        public void GivenIAddItToCart()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]")));
            driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
            Thread.Sleep(500);
            driver.SwitchTo().Alert().Accept();

            //save name and price
            tempPhoneName = driver.FindElement(By.XPath("//div[contains(@id,'tbodyid')]/h2")).Text;
            tempPhonePrice = int.Parse(Regex.Match(driver.FindElement(By.ClassName("price-container")).Text, @"\d+").Value);
        }


        [Given(@"I have a budget of (.*)")]
        public void GivenIHaveABudgetOf(int p0)
        {
            this.budget = p0;
            Assert.True(this.budget == 1500);
        }


        [When(@"I click (.*)")]
        public void WhenIClick(string p0)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            switch (p0)
            {
                case "Home":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class,'nav-item active')]")));
                    driver.FindElement(By.XPath("//*[contains(@class,'nav-item active')]")).Click();
                    break;
                case "Contact":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@data-target,'exampleModal')]")));
                    driver.FindElement(By.XPath("//*[contains(@data-target,'exampleModal')]")).Click();
                    break;
                case "About us":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@data-target,'videoModal')]")));
                    driver.FindElement(By.XPath("//*[contains(@data-target,'videoModal')]")).Click();
                    break;
                case "Cart":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
                    driver.FindElement(By.Id("cartur")).Click();
                    break;
                case "Log in":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("login2")));
                    driver.FindElement(By.Id("login2")).Click();
                    break;
                case "Sign up":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("signin2")));
                    driver.FindElement(By.Id("signin2")).Click();
                    break;
            }
        }


        [When(@"I fill in required data")]
        public void WhenIFillInRequiredData()
        {
            driver.FindElement(By.Id("sign-username")).SendKeys("Alex");
            driver.FindElement(By.Id("sign-password")).SendKeys("123456");
            //Thread.Sleep(500);
            driver.FindElement(By.XPath("//button[@type='button'][text()='Sign up']")).Click();

            Thread.Sleep(500);
            string info = driver.SwitchTo().Alert().Text;

            Assert.Equal("This user already exist.", info);

        }


        [When(@"I filter by “(.*)”")]
        public void WhenIFilterBy(string p0)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cat")));

            switch (p0)
            {
                case "Phones":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'phone')]")));
                    driver.FindElement(By.XPath("//*[contains(@onclick,'phone')]")).Click();
                    Thread.Sleep(2000);
                    break;
                case "Laptops":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'notebook')]")));
                    driver.FindElement(By.XPath("//*[contains(@onclick,'notebook')]")).Click();
                    Thread.Sleep(2000);
                    break;
                case "Monitors":
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'monitor')]")));
                    driver.FindElement(By.XPath("//*[contains(@onclick,'monitor')]")).Click();
                    Thread.Sleep(2000);
                    break;
                default:
                    Debug.WriteLine("FAIL");
                    break;
            }
            Thread.Sleep(3500);
        }


        [When(@"I click on the (.*) from Image Slider")]
        public void WhenIClickOnTheFromImageSlider(string p0)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'carousel-item active')]/img")));
            slide = driver.FindElement(By.XPath("//div[contains(@class,'carousel-item active')]/img")).GetProperty("alt");

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class,'carousel-control-prev-icon')]")));
            Thread.Sleep(1000);
            switch (p0)
            {
                case "Previous":
                    driver.FindElement(By.XPath("//*[contains(@class,'carousel-control-prev-icon')]")).Click();
                    Thread.Sleep(2500);
                    break;
                case "Next":
                    driver.FindElement(By.XPath("//*[contains(@class,'carousel-control-next-icon')]")).Click();
                    Thread.Sleep(2500);
                    break;
                default:
                    scenario.WriteLine("ERROR");
                    break;
            }
        }


        [When(@"I navigate to Cart page")]
        public void GivenINavigateToCartPage()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
            driver.FindElement(By.Id("cartur")).Click();
            Thread.Sleep(2000);

        }


        [When(@"I can add A DELL laptop from 2017")]
        public void WhenIAddADellLaptopFrom()
        {
            int i;
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            for (i = 1; i <= driver.FindElements(By.ClassName("card-title")).Count; i++)
            {
                try
                {
                    if (driver.FindElements(By.ClassName("card-title")).ElementAt(i).Text.Contains("2017") && driver.FindElements(By.ClassName("card-title")).ElementAt(i).Text.ToLower().Contains("dell"))      //can use also XPath //*[@id="tbodyid"]/div[i]/div/div/h4/a
                    {
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("card-title")));
                        driver.FindElements(By.ClassName("card-title")).ElementAt(i).Click();

                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]")));
                        driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();

                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                        Thread.Sleep(500);
                        driver.SwitchTo().Alert().Accept();

                    }
                }
                catch (Exception)
                {
                    scenario.WriteLine("No Element found");
                }
            }
        }


        [When(@"I filter by Phones")]
        public void WhenIFilterByPhones()
        {
            driver.FindElement(By.XPath("//*[contains(@onclick,'phone')]")).Click();
            Thread.Sleep(2000);

            Boolean found = true;
            try
            {
                if (driver.FindElement(By.XPath("//*[contains(text(),'Sony vaio i7')]")).Displayed)
                {
                    found = true;
                }
            }
            catch (NoSuchElementException)
            {
                found = false;
            }
            Assert.False(found);

            try
            {
                if (driver.FindElement(By.XPath("//*[contains(text(),'Apple monitor 24')]")).Displayed)
                {
                    found = true;
                }
            }
            catch (NoSuchElementException)
            {
                found = false;
            }
            Assert.False(found);

            try
            {
                if (driver.FindElement(By.XPath("//*[contains(text(),'Sony xperia z5')]")).Displayed)
                {
                    found = true;
                }
            }
            catch (NoSuchElementException)
            {
                found = false;
            }
            Assert.True(found);

        }


        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            Thread.Sleep(500);
            string info = driver.SwitchTo().Alert().Text;

            Assert.Equal("Sign up successful.", info);
            //Assert.Equal("This user already exist.", info);  //Sign up successful.
            driver.SwitchTo().Alert().Accept();
            driver.Close();
        }


        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            int noOfProducts;
            int sum = 0;
            int index = 1;

            noOfProducts = driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count;
            while (index <= noOfProducts)
            {
                sum += int.Parse(Regex.Match(driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + index + "]/div/div/h5")).Text, @"\d+").Value);
                index++;
            }

            scenario.WriteLine("Sum is:\t" + sum + "\nNo Of Products:\t" + noOfProducts + "\n Mean VALUE:\t" + sum / noOfProducts);

        }


        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {
            string slideToCompare;
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//div[contains(@class,'carousel-item active')]/img")));
            slideToCompare = driver.FindElement(By.XPath("//div[contains(@class,'carousel-item active')]/img")).GetProperty("alt");

            scenario.WriteLine(slideToCompare);
            Assert.NotEqual(slide, slideToCompare);

        }

        [Then(@"I can see the correct page title is displayed")]
        public void ThenICanSeeTheCorrectPageTitleIsDisplayed()
        {
            string title = driver.Title;
            Assert.Equal("STORE", title);
        }


        [Then(@"Cart page should be displayed")]
        public void ThenCartPageShouldBeDisplayed()
        {
            string tempText;
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@data-target,'orderModal')]")));
            tempText = driver.FindElement(By.XPath("//*[contains(@data-target,'orderModal')]")).Text;

            Assert.Equal("Place Order", tempText);
        }


        [Then(@"Display the product price in console")]
        public void ThenDisplayTheProductPriceInConsole()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("price-container")));

            scenario.WriteLine(driver.FindElement(By.ClassName("price-container")).Text);
        }


        [Then(@"The selected product and correct price should be displayed")]
        public void ThenTheSelectedProductAndCorrectPriceShouldBeDisplayed()
        {
            int noOfProducts;

            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("success")));
            noOfProducts = driver.FindElements(By.ClassName("success")).Count;
            Assert.Equal(1, noOfProducts);

            Assert.Equal(tempPhoneName, driver.FindElement(By.XPath("//*[contains(@id,'tbodyid')]/tr/td[2]")).Text);//*[@id="tbodyid"]/tr/td[2]
            Assert.Equal(tempPhonePrice, int.Parse(driver.FindElement(By.XPath("//*[contains(@id,'tbodyid')]/tr/td[3]")).Text));

        }


        [Then(@"I get the correct page/popup for that (.*)")]
        public void ThenIGetTheCorrectPagePopupForThat(string p0)
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            switch (p0)
            {
                case "Home":
                    Thread.Sleep(3000);
                    Assert.Equal("STORE", driver.Title);
                    break;
                case "Contact":
                    Thread.Sleep(3000);
                    Assert.Equal("New message", driver.FindElement(By.Id("exampleModalLabel")).Text);
                    break;
                case "About us":
                    Thread.Sleep(3000);
                    Assert.Equal("About us", driver.FindElement(By.Id("videoModalLabel")).Text);
                    break;
                case "Cart":
                    Thread.Sleep(3000);
                    Assert.Equal("Place Order", driver.FindElement(By.XPath("//*[contains(@data-target,'orderModal')]")).Text);
                    Assert.Equal("Total", driver.FindElement(By.XPath("//*[contains(@class,'col-lg-1')]/h2")).Text);
                    //driver.FindElement(By.Id("nava")).Click();
                    //Thread.Sleep(3000);
                    break;
                case "Log in":
                    Thread.Sleep(3000);
                    Assert.Equal("Log in", driver.FindElement(By.Id("logInModalLabel")).Text);
                    break;
                case "Sign up":
                    Thread.Sleep(3000);
                    Assert.Equal("Sign up", driver.FindElement(By.Id("signInModalLabel")).Text);
                    break;


            }
        }



        [Then(@"I can Buy all from cart")]
        public void ICanBuyAllFromCart()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0,0,30));
            string name = "AlexG";
            string country = "Romaina";
            string city = "Timisoara";
            string card = "4444-5555-6666-7777";

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
            driver.FindElement(By.Id("cartur")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'deleteItem')]")));
            Thread.Sleep(3000);
            driver.FindElement(By.XPath("//*[contains(@class, 'btn btn-success')]")).Click();

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("orderModalLabel")));
            Assert.Equal("Place order",driver.FindElement(By.Id("orderModalLabel")).Text );

            driver.FindElement(By.Id("name")).SendKeys(name);
            driver.FindElement(By.Id("country")).SendKeys(country);
            driver.FindElement(By.Id("city")).SendKeys(city);
            driver.FindElement(By.Id("card")).SendKeys(card);
            driver.FindElement(By.Id("month")).SendKeys("06");
            driver.FindElement(By.Id("year")).SendKeys("22");
            driver.FindElement(By.XPath("//*[contains(@onclick, 'purchaseOrder')]")).Click();


            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@class, 'confirm btn btn-lg btn-primary')]")));
            Assert.True(driver.FindElement(By.XPath("/ html / body / div[10] / h2")).Text.ToLower().Equals("thank you for your purchase!"));
            Assert.True(driver.FindElement(By.XPath("//*[contains(@class,'lead text-muted')]")).Text.Contains(name) && driver.FindElement(By.XPath("//*[contains(@class,'lead text-muted')]")).Text.Contains(card));

            Thread.Sleep(1500);
            driver.FindElement(By.XPath("//*[contains(@class, 'confirm btn btn-lg btn-primary')]")).Click();
        }


        [Then(@"Cart is empty")]
        public void ThenCartIsEmpty()
        {
            Thread.Sleep(3000);

            int noOfObjects;
            var wait = new WebDriverWait(driver, new TimeSpan(0,0,30));
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
            driver.FindElement((By.Id("cartur"))).Click();

            Thread.Sleep(3000);
            noOfObjects = driver.FindElements(By.XPath("//*[contains(@onclick,'deleteItem')]")).Count();
            Assert.Equal(0, noOfObjects);
            
        }


        [Then(@"I can buy a phone, laptop and a monitor within the budget")]
        public void ThenICanBuyAPhoneLaptopAndAMonitorWithinTheBudget()
        {
            int currentSum = 0;
            int i,j,k;
            Boolean found = false;
            string productXPath;
            int noOfPhones, noOfLaptops, noOfMonitors;
            int tempPhoneValue, tempLaptopValue, tempMonitorValue;

            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));

            //Init tempValues
            WhenIFilterBy("Phone");
            noOfPhones = driver.FindElements(By.ClassName("card-title")).Count();
            WhenIFilterBy("Laptop");
            noOfLaptops = driver.FindElements(By.ClassName("card-title")).Count();
            WhenIFilterBy("Monitor");
            noOfMonitors = driver.FindElements(By.ClassName("card-title")).Count();

            for (i = 1 ; i< noOfPhones && !found; i++)
            {
                WhenIFilterBy("Phones");
                productXPath = "//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h5";
                tempPhoneValue = int.Parse(Regex.Match(driver.FindElement(By.XPath(productXPath)).Text, @"\d+").Value);
                currentSum += tempPhoneValue;

                for (j = 1 ; j < noOfLaptops && !found; j++)
                {
                    WhenIFilterBy("Laptops");
                    productXPath = "//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h5";
                    tempLaptopValue = int.Parse(Regex.Match(driver.FindElement(By.XPath(productXPath)).Text, @"\d+").Value);
                    currentSum += tempLaptopValue;

                    for (k = 1 ; k < noOfMonitors && !found; k++)
                    {
                        WhenIFilterBy("Monitors");
                        productXPath = "//*[@id=\"tbodyid\"]/div[" + k + "]/div/div/h5";
                        tempMonitorValue = int.Parse(Regex.Match(driver.FindElement(By.XPath(productXPath)).Text, @"\d+").Value);
                        currentSum += tempMonitorValue;

                        if (currentSum < this.budget)
                        {
                            found = true;
                            scenario.WriteLine("" + k);
                            productXPath = "//*[@id=\"tbodyid\"]/div[" + k + "]/div/div/h4/a";
                            driver.FindElement(By.XPath(productXPath)).Click();
                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]")));
                            driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();
                            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                            Thread.Sleep(500);
                            driver.SwitchTo().Alert().Accept();
                            Thread.Sleep(500);
                            driver.FindElement(By.Id("nava")).Click();
                            Thread.Sleep(2000);
                        }
                        currentSum -= tempMonitorValue;
                    }
                    if (found)
                    {
                        WhenIFilterBy("Laptops");
                        productXPath = "//*[@id=\"tbodyid\"]/div[" + j + "]/div/div/h4/a";
                        driver.FindElement(By.XPath(productXPath)).Click();
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]")));
                        driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();
                        wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                        Thread.Sleep(500);
                        driver.SwitchTo().Alert().Accept();
                        Thread.Sleep(500);
                        driver.FindElement(By.Id("nava")).Click();
                        Thread.Sleep(2000);
                    }
                    currentSum -= tempLaptopValue;
                }
                if (found)
                {
                    WhenIFilterBy("Phones");
                    productXPath = "//*[@id=\"tbodyid\"]/div[" + i + "]/div/div/h4/a";
                    driver.FindElement(By.XPath(productXPath)).Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[contains(@onclick,'addToCart')]")));
                    driver.FindElement(By.XPath("//*[contains(@onclick,'addToCart')]")).Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.AlertIsPresent());
                    Thread.Sleep(500);
                    driver.SwitchTo().Alert().Accept();
                    
                }
                currentSum -= tempPhoneValue;
            }

            ICanBuyAllFromCart();


        }


        [Then(@"I can add to cart (.*) random phones that don't exceed my budget")]
        public void ThenICanAddToCartRandomPhonesThatDonTExceedMyBudget(int p0)
        {
            int i;
            i = p0;
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            while (i > 0 && budget > 0)
            {
                AddInCart(ref i);
                i--;
            }

            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.Id("cartur")));
            driver.FindElement(By.Id("cartur")).Click(); //CART
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.ClassName("success")));
            Thread.Sleep(1500);
            Assert.Equal(p0, driver.FindElements(By.ClassName("success")).Count);

            driver.Close();
        }

        public void AddInCart(ref int p0)
        {
            int objPrice;
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));
            Random rand = new Random();
            string productToBuy;

            try
            {
                productToBuy = "//*[@id=\"tbodyid\"]/div[" + (rand.Next(driver.FindElements(By.XPath("//*[contains(@class,'col-lg-4 col-md-6 mb-4')]")).Count - 1) + 1) + "]/div/div/h4/a";
                wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath(productToBuy)));

                if (driver.FindElement(By.XPath(productToBuy)).Displayed)
                {
                    driver.FindElement(By.XPath(productToBuy)).Click();
                    wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.ClassName("price-container")));
                    objPrice = int.Parse(Regex.Match(driver.FindElement(By.ClassName("price-container")).Text, @"\d+").Value);

                    if (objPrice > budget)
                    {
                        p0++;
                    }
                    else
                    {
                        budget -= objPrice;
                        driver.FindElement(By.XPath("//*[contains(@class,'btn btn-success btn-lg')]")).Click();
                        Thread.Sleep(2000);
                        driver.SwitchTo().Alert().Accept();
                        Thread.Sleep(500);
                        Debug.WriteLine(budget);

                    }
                }
            }
            catch (Exception)
            {
                Debug.WriteLine("Error Occured");
                p0++;
            }


            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.Id("nava")));
            driver.FindElement(By.Id("nava")).Click(); //home page
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementIsVisible(By.XPath("//*[contains(@onclick,'phone')]")));
            driver.FindElement(By.XPath("//*[contains(@onclick,'phone')]")).Click();


        }


        [AfterScenario]
        public void Kill()
        {
            driver.Quit();
        }

    }
}
