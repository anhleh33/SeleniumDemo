using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace TestProject1
{
    public class FormSubmission
    {
        private IWebDriver driver;

        //1. Setup: Preparing the test (launch browser, set options, navigate).
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        //2.Invocation: Performing actions (clicks, typing, navigation).
        //and Assessment: Verifying expected results.
        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("https://scholar.google.com/?hl=vi");
            TestContext.WriteLine("This test gonna form submission from GG Scholar");

            IWebElement searchBox = driver.FindElement(By.Name("q"));
            searchBox.SendKeys("GNN" + Keys.Enter);
            Thread.Sleep(3000);

            IWebElement yearGap = driver.FindElement(By.CssSelector("#gs_res_sb_yyl > li:nth-child(4) > a"));
            yearGap.Click();
            Thread.Sleep(3000);

            Assert.IsTrue(driver.Title.Contains("GNN"), "Page did not contain the search text");
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