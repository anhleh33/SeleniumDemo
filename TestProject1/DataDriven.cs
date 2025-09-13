using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;

namespace TestProject1
{
    public class DataDriven
    {
        private IWebDriver driver;
        private string csvFile = "price_dicrepancies.csv";

        //1. Setup: Preparing the test (launch browser, set options, navigate).
        [SetUp]
        public void Setup()
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();
        }

        public List<Book> getBookDataFromFile(string fileName)
        {
            List<Book> bookData = new List<Book>();
            var lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++)
            {
                var parts = lines[i].Split(",");
                
                if (parts.Length == 3)
                {
                    string title = parts[0];
                    string price = parts[1];
                    string availability = parts[2];

                    bookData.Add(new Book
                    {
                        Title = title,
                        Price = price,
                        Availability = availability
                    });
                    
                }
            }
            return bookData;
        }

        [Test]
        public void printBookList()
        {
            List <Book> data = getBookDataFromFile("BooksData.csv");
            foreach (Book book in data)
            {
                book.printBook();
            }
        }

        //2.Invocation: Performing actions (clicks, typing, navigation).
        //and Assessment: Verifying expected results.
        [Test]
        public void Test()
        {
            driver.Navigate().GoToUrl("https://books.toscrape.com/");

            // Open the CSV file once with StreamWriter
            using (var writer = new StreamWriter(csvFile, false)) // false = overwrite file
            {
                writer.WriteLine("Title,SavedPrice,CurrentPrice");

                WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
                int n = 1;
                int countPageNumber = 3;

                List<Book> bookData = getBookDataFromFile("BooksData.csv");
                TestContext.WriteLine("Length of book list: " + bookData.Count);
                int lengthOfBookData = 0;

                while (n != countPageNumber + 1)
                {
                    wait.Until(drv => drv.FindElements(By.CssSelector(".product_pod")).Count > 0);
                    IList<IWebElement> books = driver.FindElements(By.CssSelector(".product_pod"));

                    foreach (var book in books)
                    {
                        string title = book.FindElement(By.CssSelector("h3 > a")).GetAttribute("title");
                        string price = book.FindElement(By.CssSelector(".price_color")).Text;

                        if (lengthOfBookData < bookData.Count)
                        {
                            Book savedBook = bookData[lengthOfBookData];

                            if (savedBook.IsDifferentPrice(price) && savedBook.isTheSameTitle(title))
                            {
                                string line = $"\"{title}\",\"{savedBook.Price}\",\"{price}\"";
                                TestContext.WriteLine(line);
                                writer.WriteLine(line);
                            }
                        }
                        else
                        {
                            TestContext.WriteLine($"No saved book found for index {lengthOfBookData}");
                        }

                        lengthOfBookData++;
                    }

                    IWebElement nextButton = driver.FindElement(By.CssSelector(".next > a"));
                    nextButton.Click();
                    n++;
                }
            }
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
