using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject1
{
    public class ScrapeData
    {
        private IWebDriver driver;
        private string csvFile = "BooksData.csv";

        //1. Setup: Preparing the test (launch browser, set options, navigate).
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
            File.WriteAllText(csvFile, "Title,Price,Availability\n");
        }

        //2.Invocation: Performing actions (clicks, typing, navigation).
        //and Assessment: Verifying expected results.
        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("https://books.toscrape.com/");

            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            int countPageNumber = 3;
            TestContext.WriteLine("Number of testing pages: " + countPageNumber);

            int n = 1;

            while (n!= countPageNumber+1)
            {
                wait.Until(drv => drv.FindElements(By.CssSelector(".product_pod")).Count > 0);

                IList<IWebElement> books = driver.FindElements(By.CssSelector(".product_pod"));

                foreach (var book in books)
                {
                    string title = book.FindElement(By.CssSelector("h3 > a")).GetAttribute("title");
                    string price = book.FindElement(By.CssSelector(".price_color")).Text;
                    string availability = book.FindElement(By.CssSelector(".availability")).Text.Trim();

                    string line = $"\"{title}\",\"{price}\",\"{availability}\"\n";
                    File.AppendAllText(csvFile, line);

                    TestContext.WriteLine(line);
                }

                IWebElement nextButton = driver.FindElement(By.CssSelector(".next > a"));
                nextButton.Click();
                n++;
            }

            TestContext.WriteLine($"Data saved to {csvFile}");
        }

        //3. Tear Down: Cleaning up (close browser, release resources).
        [TearDown]
        public void TearDown()
        {
            driver.Quit();
            driver.Dispose();
        }
    }
}