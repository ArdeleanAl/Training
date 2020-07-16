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
using Training_1.Steps;
using Xunit;
using Xunit.Abstractions;

namespace Training_1
{
    [Binding]
    public class TrainingSteps
    {
        IWebDriver _driver;
        private WebDriverWait _wait;

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
            _wait = new WebDriverWait(_driver, new TimeSpan(0, 0, 5));
            _homePage = new HomePageSteps(_driver, scenario);
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
            _scenario.WriteLine(_signUp.GetModalTitleText());
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


        //[When(@"I filter by Phones")]
        //public void WhenIFilterByPhones()
        //{
        //    _homePage.FilterByPhones();
        //}


        [Then(@"I get registered")]
        public void ThenIGetRegistered()
        {
            _popup.PopUpAvailable();
            Assert.Equal("Sign up successful.", _popup.GetPopUpText());
        }


        [Then(@"I can see in the test output the mean value of each product")]
        public void ThenICanSeeInTheTestOutputTheMeanValueOfEachProduct()
        {
            _scenario.WriteLine(_homePage.CalculateMeanValue());

        }


        [Then(@"I see a different product")]
        public void ThenISeeADifferentProduct()
        {
            _scenario.WriteLine(_slide + "  " + _homePage.GetCurrentSliderImageText());
            Assert.NotEqual(_slide, _homePage.GetCurrentSliderImageText());
            

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
            _navbar.ClickCart();
            
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
            while (_cartPage.AllProductsWereUpdated() == 0) ;
            Assert.Equal(p0, _cartPage.GetNumberOfProducts());
        }


        [AfterScenario]
        public void Kill()
        {
            _driver.Quit();
        }

    }
}
