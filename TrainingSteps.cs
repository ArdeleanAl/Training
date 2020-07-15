/*using OpenQA.Selenium;
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
        private readonly ITestOutputHelper scenario; //de obicei variabilele de aici le numesti cu _numeVariabila
        IWebDriver driver = new ChromeDriver();
        private int budget;
        string slide;
        string tempPhoneName;
        int tempPhonePrice;  // e.g. _tempPhonePrice

        public TrainingSteps(ITestOutputHelper sc)
        {
            scenario = sc;
        }


        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            
            string title = driver.Title;
            Assert.Equal("STORE", title);
        }


        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            Trace.WriteLine("we started");// de obicei dupa ce termini cu debugging ii ok sa scoti comentariile
            Debug.WriteLine("we started");
            driver.Navigate().GoToUrl("https://www.demoblaze.com/");//odata ce ai facut pasu de go to home, puteai refolosi pasul respectiv si aici sa ai doar partea de login
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
            Thread.Sleep(500); //Pe cat se poate sa eviti sa folosesti thread sleep. Alternativa ar fi waint.until(conditie care ai nevoie (displayed/clickable/etc)
            string element = driver.FindElement(By.Id("signInModalLabel")).Text;

            Assert.Equal("Sign up", element);
        }


        [Given(@"I select the first available product")]
        public void GivenISelectTheFirstAvailableProduct()
        {
            var wait = new WebDriverWait(driver, new TimeSpan(0, 0, 30));//puteai sa declari wait intr-o clasa separata in care o mostenesti si nu trebuie sa o declari in fiecare metoda sau o pui la inceput cum ai pus si variabilele de la inceputul clasei
            wait.Until(SeleniumExtras.WaitHelpers.ExpectedConditions.ElementToBeClickable(By.XPath("//*[@id=\"tbodyid\"]/div[1]/div/div/h4/a")));// xpathurile nu is cei mai buni identificatori deoarece la cea mai mica schimbare ele se pot schimba si devin invalide
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
	        // se poate simplifica un pic codul de aici incat sa fie mai usor de urmarit ce se intampla
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

            driver.Close(); // de obicei inchiz driverul doar in after scenario

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
*/

using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Text.RegularExpressions;
using System.Threading;
using TechTalk.SpecFlow;
using Xunit;
using Xunit.Abstractions;

using Training_1.Steps;

namespace Training
{
    [Binding]
    public class TrainingSteps
    {
        IWebDriver _driver;

        private readonly ITestOutputHelper _scenario; //de obicei variabilele de aici le numesti cu _numeVariabila           --> done
        private int _budget;
        private string _slide;
        private string _tempPhoneName;
        private int _tempPhonePrice;  // e.g. _tempPhonePrice                                                                        --> done   
        private HomePageSteps _homePage;
        private LogInSteps _logIn;
        private NavbarSteps _navbar;
        private SignUpSteps _signUp;
        private ProductPageSteps _productPage;
        private PopUpSteps _popup;
        private CartPageSteps _cartPage;


        public TrainingSteps(ITestOutputHelper scenario)
        {
            _scenario = scenario;
            _driver = new ChromeDriver();
            _homePage = new HomePageSteps(_driver);
            _logIn = new LogInSteps(_driver);
            _navbar = new NavbarSteps(_driver);
            _signUp = new SignUpSteps(_driver);
            _productPage = new ProductPageSteps(_driver);
            _popup = new PopUpSteps(_driver);
            _cartPage = new CartPageSteps(_driver);
        }

        [BeforeScenario]
        public void Init()
        {
            //do nothing
        }


        [Given(@"I am on the homepage")]
        public void GivenIAmOnTheHomepage()
        {
            _homePage.GoToHomePage();

            Assert.True(_homePage.ImageSlideAvailable());
        }


        [Given(@"I am logged in")]
        public void GivenIAmLoggedIn()
        {
            _homePage.GoToHomePage();
            _logIn.LogInWithCredentials("AlexG", "123456");

            Assert.Equal("Welcome AlexG", _homePage.GetUserName());
        }

        [Given(@"I click on Sign Up button")]
        public void GivenIClickOnSignUpButton()
        {
            _navbar.ClickSignIn();

            Assert.Equal("Sign up", _signUp.GetModalTitleText());
        }


        [Given(@"I select the first available product")]
        public void GivenISelectTheFirstAvailableProduct()
        {
            _homePage.SelectFirstItem();
        }


        [Given(@"I add it to cart")]
        public void GivenIAddItToCart()
        {
            _productPage.AddToCart();

            _tempPhoneName = _productPage.GetProductName();
            _tempPhonePrice = _productPage.GetProductPrice();
        }


        [Given(@"I have a budget of (.*)")]
        public void GivenIHaveABudgetOf(int p0)
        {
            this._budget = p0;
            Assert.True(this._budget == p0);
        }


        [When(@"I click (.*)")]
        public void WhenIClick(string p0)
        {
            switch (p0)
            {
                case "Home":
                    _navbar.ClickHome();
                    break;
                case "Contact":
                    _navbar.ClickContact();
                    break;
                case "About us":
                    _navbar.ClickAboutUs();
                    break;
                case "Cart":
                    _navbar.ClickCart();
                    break;
                case "Log in":
                    _navbar.ClickLogIn();
                    break;
                case "Log Out":
                    _logIn.LogInWithCredentials("AlexG", "123456");
                    _navbar.ClickLogOut();
                    break;
                case "User Name":
                    _logIn.LogInWithCredentials("AlexG", "123456");
                    _navbar.clickUserName();
                    break;
                case "Sign up":
                    _navbar.ClickSignIn();
                    break;
            }
        }


        [When(@"I fill in required data")]
        public void WhenIFillInRequiredData()
        {
            _signUp.TypeUserName("AlexG");
            _signUp.TypeUserPassword("123456");
            _signUp.ClickOnModalSignUpButton();
        }


        [When(@"I filter by (.*)")]
        public void WhenIFilterBy(string p0)
        {
            switch (p0)
            {
                case "Phones":
                    _homePage.FilterByPhones();
                    break;
                case "Laptops":
                    _homePage.FilterByLaptops();
                    break;
                case "Monitors":
                    _homePage.FilterByMonitors();
                    break;
                default:
                    _scenario.WriteLine("FAIL");
                    break;
            }
        }


        [When(@"Image Slider (.*) button is clicked")]
        public void WhenImageSliderButtonIsClicked(string p0)
        {

            _slide = _homePage.GetCurrentSliderImageText();

            switch (p0)
            {
                case "Previous":
                    _homePage.ClickSliderPrevious();
                    break;
                case "Next":
                    _homePage.ClickSliderNext();
                    break;
                default:
                    _scenario.WriteLine("ERROR");
                    break;
            }
        }


        [When(@"I navigate to Cart page")]
        public void GivenINavigateToCartPage()
        {
            _navbar.ClickCart();
        }


        [When(@"I can add A DELL laptop from 2017")]
        public void WhenIAddADellLaptopFrom()
        {
            Assert.True(_homePage.BuyDellLaptopFrom2017());
        }


        [When(@"I filter by Phones")]
        public void WhenIFilterByPhones()
        {
            _homePage.FilterByPhones();
        }


        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            _popup.PopUpAvailable();
            Assert.Equal("Sign up successful.", _popup.GetPopUpText());
        }


        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            int noOfProducts;
            int sum = 0;
            int index = 1;

            noOfProducts = _driver.FindElements(By.XPath("//*[contains(@class, 'col-lg-4 col-md-6 mb-4')]")).Count;
            while (index <= noOfProducts)
            {
                sum += int.Parse(Regex.Match(_driver.FindElement(By.XPath("//*[@id=\"tbodyid\"]/div[" + index + "]/div/div/h5")).Text, @"\d+").Value);
                index++;
            }

            _scenario.WriteLine("Sum is:\t" + sum + "\nNo Of Products:\t" + noOfProducts + "\n Mean VALUE:\t" + sum / noOfProducts);

        }


        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {
            Assert.NotEqual(_slide, _homePage.GetCurrentSliderImageText());
            _scenario.WriteLine(_slide + "  " + _homePage.GetCurrentSliderImageText());

        }

        [Then(@"I can see the correct page title is displayed")]
        public void ThenICanSeeTheCorrectPageTitleIsDisplayed()
        {
            string title = _driver.Title;
            Assert.Equal("STORE", title);
        }


        [Then(@"Cart page should be displayed")]
        public void ThenCartPageShouldBeDisplayed()
        {
            Assert.True(_cartPage.WeAreOnCartPage());
        }


        [Then(@"Display the product price in console")]
        public void ThenDisplayTheProductPriceInConsole()
        {
            _scenario.WriteLine("Product Price is: " + _productPage.GetProductPrice());
        }


        [Then(@"The selected product and correct price should be displayed")]
        public void ThenTheSelectedProductAndCorrectPriceShouldBeDisplayed()
        {
            Assert.Equal(1, _cartPage.GetNumberOfProducts());
            Assert.Equal(_tempPhoneName, _cartPage.GetProductName());
            Assert.Equal(_tempPhonePrice, _cartPage.GetProductPrice());
        }


        [Then(@"I get the correct page/popup for that (.*)")]
        public void ThenIGetTheCorrectPagePopupForThat(string p0)
        {
            Thread.Sleep(3000);
            switch (p0)
            {
                case "Home":
                    Assert.Equal("STORE", _driver.Title);
                    break;
                case "Contact":
                    Assert.Equal("New message", _driver.FindElement(By.Id("exampleModalLabel")).Text);
                    break;
                case "About us":
                    Assert.Equal("About us", _driver.FindElement(By.Id("videoModalLabel")).Text);
                    break;
                case "Cart":
                    Assert.Equal("Place Order", _driver.FindElement(By.XPath("//*[contains(@data-target,'orderModal')]")).Text);
                    Assert.Equal("Total", _driver.FindElement(By.XPath("//*[contains(@class,'col-lg-1')]/h2")).Text);
                    break;
                case "Log in":
                    Assert.Equal("Log in", _driver.FindElement(By.Id("logInModalLabel")).Text);
                    break;
                case "Sign up":
                    Assert.Equal("Sign up", _driver.FindElement(By.Id("signInModalLabel")).Text);
                    break;
            }
        }


        [Then(@"I can Buy all from cart")]
        public void ICanBuyAllFromCart()
        {
            Assert.True(_cartPage.BuyAll());
        }


        [Then(@"Cart is empty")]
        public void ThenCartIsEmpty()
        {
            while (_cartPage.WeAreOnCartPage() != true) ;

            Assert.Equal(0, _cartPage.GetNumberOfProducts());
        }


        [Then(@"I can buy a phone, laptop and a monitor within the budget")]
        public void ThenICanBuyAPhoneLaptopAndAMonitorWithinTheBudget()
        // se poate simplifica un pic codul de aici incat sa fie mai usor de urmarit ce se intampla
        {
            Assert.True(_homePage.BuyAPhoneALaptopAndAMonitorWithinBudget(this._budget));
            ICanBuyAllFromCart();
        }


        [Then(@"I can add to cart (.*) random phones that don't exceed my budget")]
        public void ThenICanAddToCartRandomPhonesThatDonTExceedMyBudget(int p0)
        {
            Assert.True(_homePage.AddANumberOfPhoneWithinTheBudget(p0, this._budget));
            _navbar.ClickCart();

            Assert.Equal(p0, _cartPage.GetNumberOfProducts());
        }


        [AfterScenario]
        public void Kill()
        {
            _driver.Quit();
        }

    }
}
