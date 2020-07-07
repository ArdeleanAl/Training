Feature: Training_1
	In order to avoid silly mistakes
	As a math idiot
	I want to be told the sum of two numbers

@mytag
Scenario: Register new user
	Given I am on the homepage
	And I click on Sign Up button
	When I fill in required data
	Then I get registered

Scenario Outline: Get mean value product cost
	Given I am on the homepage
	When I filter by “<Product>”
	Then I can see in the test output the mean value of each product

	Examples:
		| Product  |
		| Phones   |
		| Laptops  |
		| Monitors |

Scenario: Check that Image Slider change the content
	Given I am on the homepage
	When I click <Buttons> from Image Slider
	Then I see a different product

	Examples:
		| Buttons  |
		| Previous |
		| Next     |

Scenario: Access the home page and check that the correct page title is displayed
	Given I am on the homepage
	Then I can see the correct page title is displayed

Scenario: Access the home page and navigate to the cart page, then the cart page should be displayed
	Given I am on the homepage
	When I navigate to Cart page
	Then Cart page should be displayed

Scenario: Access the home page, select the first available product, then display the products price in the console.
	Given I am on the homepage
	And I select the first available product
	Then Display the product price in console

Scenario: Access the home page, select the first available product, add it to your cart, navigate to the shopping cart page, then the selected product with the correct price should be displayed
	Given I am on the homepage
	And I select the first available product
	And I add it to cart
	When I navigate to Cart page
	Then The selected product and correct price should be displayed

Scenario: Check that the correct page/popup is displayed
	Given I am on the homepage
	When I click <headerlink>
	Then I get the correct page/popup for that <headerlink>

	Examples:
		| headerlink  |
		| Home  |
		| Contact     |
		| About us  |
		| Cart     |
		| Log in  |
		| Sign up     |

Scenario: Buy a Dell laptop model from 2017
	Given I am logged in
	When I filter by “Laptops”
	And I can add A DELL laptop from 2017
	Then I can Buy all from cart

Scenario: Check Cart is empty after a complete Buy
	Given I am logged in
	When I filter by “Laptops”
	And I can add A DELL laptop from 2017
	Then I can Buy all from cart
	And Cart is empty

Scenario: Buy a Phone, Laptop and Monitor within a given budget
	Given I am logged in
	And I have a budget of 1500 
	Then I can buy a phone, laptop and a monitor within the budget

	
Scenario: Buy random phones using given budget
	Given I am logged in
	And I have a budget of 1500
	When I filter by Phones
	Then I can add to cart 2 random phones that don't exceed my budget