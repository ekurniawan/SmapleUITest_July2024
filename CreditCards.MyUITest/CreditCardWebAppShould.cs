using CreditCards.MyUITest.Models;
using CsvHelper;
using FluentAssertions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreditCards.MyUITest
{
    public class CreditCardWebAppShould
    {
        private const string HomeUrl = "https://localhost:7014/";
        private const string HomeTitle = "Home Page - Credit Cards";
        private const string AboutTitle = "About - Credit Cards";
        private const string AboutUrl = "https://localhost:7014/Home/About";
        private const string ApplyUrl = "https://localhost:7014/Apply";
        private ChromeOptions options;

        public CreditCardWebAppShould()
        {
            options = new ChromeOptions();
            options.AddArgument("--headless=new");
        }

        [Fact]
        public void LoadApplicationPage()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                Helpers.Pause();

                string pageTitle = driver.Title;
                Assert.Equal(HomeTitle, pageTitle);
                Assert.Equal(HomeUrl, driver.Url);

                driver.Quit();
            }
        }

        [Fact]
        public void ReloadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                Helpers.Pause();
                driver.Navigate().Refresh();

                string pageTitle = driver.Title;
                Assert.Equal(HomeTitle, pageTitle);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void ReloadHomePageOnBack()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                Helpers.Pause();

                IWebElement tokenElement = driver.FindElement(By.Id("GenerationToken"));
                string strToken = tokenElement.Text;

                driver.Navigate().GoToUrl(AboutUrl);

                Helpers.Pause();

                driver.Navigate().Back();

                Helpers.Pause();

                string pageTitle = driver.Title;

                string reloadToken = driver.FindElement(By.Id("GenerationToken")).Text;

                Assert.Equal(HomeTitle, pageTitle);
                Assert.Equal(HomeUrl, driver.Url);

                Assert.NotEqual(strToken, reloadToken);
            }
        }

        [Fact]
        public void ReloadHomePageOnForward()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(AboutUrl);

                Helpers.Pause();

                driver.Navigate().GoToUrl(HomeUrl);

                Helpers.Pause();

                driver.Navigate().Back();

                Helpers.Pause();

                driver.Navigate().Forward();

                Helpers.Pause();

                string pageTitle = driver.Title;
                Assert.Equal(HomeTitle, pageTitle);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }


        // Test untuk menekan tombol Apply Now Low Rate
        [Fact]
        public void HomePage_TekanTombol_ApplyLowRate()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Helpers.Pause(1000);

                IWebElement applyLowRateButton = driver.FindElement(By.Name("ApplyLowRate"));
                applyLowRateButton.Click();

                Helpers.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void HomePage_TekanTombol_EasyApplyNow()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Helpers.Pause(11000);

                var elementApplyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                elementApplyLink.Click();

                Helpers.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }


        [Fact]
        public void HomePage_TekanTombol_EasyApplyNow_WithCssSelector()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Helpers.Pause(1000);

                var caraouselNextButton = driver.FindElement(By.CssSelector("[data-slide='next']"));
                caraouselNextButton.Click();

                Helpers.Pause(1000);

                var elementApplyLink = driver.FindElement(By.LinkText("Easy: Apply Now!"));
                elementApplyLink.Click();

                Helpers.Pause();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void HomePage_AmbilBarisPertamaDariTabel()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Helpers.Pause();

                var firstRow = driver.FindElement(By.CssSelector("table tbody tr td"));
                //var firstRow = driver.FindElement(By.TagName("td"));
                string firstRowText = firstRow.Text;

                Helpers.Pause();

                Assert.Equal("Easy Credit Card", firstRowText);
            }
        }

        [Fact]
        public void AmbilHyperlink_XPATH()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);
                Helpers.Pause(21000);

                var elementApplyLink = driver.FindElement(By.XPath("/html/body/div/div[4]/div/p/a"));
                elementApplyLink.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void WaitUntilButtonEasyNowShow()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(20));
                var applyLink = wait.Until(d => d.FindElement(By.LinkText("Easy: Apply Now!")));
                applyLink.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void WaitUntilButtonEasyNowShow_ElementToBeClickable()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(11));
                var applyLink = wait.Until(ExpectedConditions.ElementToBeClickable(By.LinkText("Easy: Apply Now!")));

                applyLink.Click();

                Assert.Equal("Credit Card Application - Credit Cards", driver.Title);
                Assert.Equal(ApplyUrl, driver.Url);
            }
        }

        [Fact]
        public void DisplayProductsAndRates()
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {
                driver.Navigate().GoToUrl(HomeUrl);

                ReadOnlyCollection<IWebElement> productCells = driver.FindElements(By.TagName("td"));
                Assert.Equal("Easy Credit Card", productCells[0].Text);
                Assert.Equal("20% APR", productCells[1].Text);
                Assert.Equal("Silver Credit Card", productCells[2].Text);
            }
        }


        //[Fact]
        //public void Form_IsiTextBox()
        //{
        //    List<ApplyCredits> records = new List<ApplyCredits>();
        //    using (var reader = new StreamReader(@"C:\Workshop\2024\Selenium UI\CreditCards\CreditCards.MyUITest\csv\ApplyCredits.csv"))
        //    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
        //    {
        //        records = csv.GetRecords<ApplyCredits>().ToList();
        //    }

        //    using (IWebDriver driver = new ChromeDriver(options))
        //    {
        //        foreach (var data in records)
        //        {
        //            driver.Navigate().GoToUrl(ApplyUrl);

        //            driver.FindElement(By.Id("FirstName")).SendKeys(data.FirstName);
        //            Helpers.Pause(1000);
        //            driver.FindElement(By.Id("LastName")).SendKeys(data.LastName);
        //            Helpers.Pause(1000);
        //            driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(data.FrequentFlyerNumber);
        //            Helpers.Pause(1000);
        //            driver.FindElement(By.Id("Age")).SendKeys(data.Age.ToString());
        //            Helpers.Pause(1000);
        //            driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(data.GrossAnnualIncome.ToString());
        //            Helpers.Pause(1000);
        //            driver.FindElement(By.Id(data.Relationship)).Click();
        //            Helpers.Pause(1000);
        //            IWebElement businessSource = driver.FindElement(By.Id("BusinessSource"));
        //            SelectElement businessSourceSelect = new SelectElement(businessSource);
        //            businessSourceSelect.SelectByValue(data.BusinessSource);
        //            Helpers.Pause(1000);
        //            if (data.TermsAccepted == "Yes")
        //                driver.FindElement(By.Id("TermsAccepted")).Click();

        //            Helpers.Pause(1000);
        //            driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        //            Helpers.Pause();

        //            if (data.Expected == "true")
        //            {
        //                Assert.Equal("Application Complete - Credit Cards", driver.Title);
        //            }
        //            else
        //            {
        //                var validationSummary = driver.FindElement(By.CssSelector(".validation-summary-errors ul li"));
        //                Assert.Equal(data.Expected, validationSummary.Text);
        //            }
        //        }
        //    }
        //}

        //[Theory]
        //[InlineData("Sarah", "Smith", "123456-A", 41, 50000, "Married", "Email", "Yes", "true")]
        //[InlineData("Erick", "Smith", "123456-B", 16, 50000, "Single", "Email", "Yes", "You must be at least 18 years old")]
        //[InlineData("Scott", "Smith", "123456-C", 20, 65000, "Single", "Email", "No", "You must accept the terms and conditions to continue")]
        //public void Form_IsiTextBoxInline(string firstName, string lastName, string frequentFlyerNumber, int age, decimal grossanualincome,
        //    string relationship, string businesssource, string termaccepted, string expected)
        //{
        //    using (IWebDriver driver = new ChromeDriver(options))
        //    {

        //        driver.Navigate().GoToUrl(ApplyUrl);

        //        driver.FindElement(By.Id("FirstName")).SendKeys(firstName);
        //        Helpers.Pause(1000);
        //        driver.FindElement(By.Id("LastName")).SendKeys(lastName);
        //        Helpers.Pause(1000);
        //        driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(frequentFlyerNumber);
        //        Helpers.Pause(1000);
        //        driver.FindElement(By.Id("Age")).SendKeys(age.ToString());
        //        Helpers.Pause(1000);
        //        driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(grossanualincome.ToString());
        //        Helpers.Pause(1000);
        //        driver.FindElement(By.Id(relationship)).Click();
        //        Helpers.Pause(1000);
        //        IWebElement businessSource = driver.FindElement(By.Id("BusinessSource"));
        //        SelectElement businessSourceSelect = new SelectElement(businessSource);
        //        businessSourceSelect.SelectByValue(businesssource);
        //        Helpers.Pause(1000);
        //        if (termaccepted == "Yes")
        //            driver.FindElement(By.Id("TermsAccepted")).Click();

        //        Helpers.Pause(1000);
        //        driver.FindElement(By.CssSelector("input[type='submit']")).Click();
        //        Helpers.Pause();

        //        if (expected == "true")
        //        {
        //            Assert.Equal("Application Complete - Credit Cards", driver.Title);
        //        }
        //        else
        //        {
        //            var validationSummary = driver.FindElement(By.CssSelector(".validation-summary-errors ul li"));
        //            Assert.Equal(expected, validationSummary.Text);
        //        }
        //    }
        //}


        [Theory]
        [SqlServerData("ACTUAL", "TestDatabase", "select FirstName, LastName,FrequentFlyerNumber,Age,GrossAnualIncome,Relationship,BusinessSource,TermAccepted,Expected from UserCreditCard")]
        public void Form_IsiTextBoxDatabase(string FirstName, string LastName, string FrequentFlyerNumber, int Age, decimal GrossAnualIncome,
           string Relationship, string BusinessSource, string TermAccepted, string Expected)
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {

                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(FirstName);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("LastName")).SendKeys(LastName);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(FrequentFlyerNumber);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("Age")).SendKeys(Age.ToString());
                Helpers.Pause(1000);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(GrossAnualIncome.ToString());
                Helpers.Pause(1000);
                driver.FindElement(By.Id(Relationship)).Click();
                Helpers.Pause(1000);
                IWebElement businessSource = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSourceSelect = new SelectElement(businessSource);
                businessSourceSelect.SelectByValue(BusinessSource);
                Helpers.Pause(1000);
                if (TermAccepted == "Yes")
                    driver.FindElement(By.Id("TermsAccepted")).Click();

                Helpers.Pause(1000);
                driver.FindElement(By.CssSelector("input[type='submit']")).Click();
                Helpers.Pause();

                if (Expected == "true")
                {
                    Assert.Equal("Application Complete - Credit Cards", driver.Title);
                }
                else
                {
                    var validationSummary = driver.FindElement(By.CssSelector(".validation-summary-errors ul li"));
                    Assert.Equal(Expected, validationSummary.Text);
                }
            }
        }


        [Theory]
        [ExcelData(@"C:\Workshop\2024\Selenium UI\CreditCards\CreditCards.MyUITest\ApplyCredits.xlsx", "select * from [Sheet1$A1:I4]")]
        public void Form_IsiTextBoxExcel(string FirstName, string LastName, string FrequentFlyerNumber, int Age, decimal GrossAnualIncome,
   string Relationship, string BusinessSource, string TermAccepted, string Expected)
        {
            using (IWebDriver driver = new ChromeDriver(options))
            {

                driver.Navigate().GoToUrl(ApplyUrl);

                driver.FindElement(By.Id("FirstName")).SendKeys(FirstName);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("LastName")).SendKeys(LastName);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("FrequentFlyerNumber")).SendKeys(FrequentFlyerNumber);
                Helpers.Pause(1000);
                driver.FindElement(By.Id("Age")).SendKeys(Age.ToString());
                Helpers.Pause(1000);
                driver.FindElement(By.Id("GrossAnnualIncome")).SendKeys(GrossAnualIncome.ToString());
                Helpers.Pause(1000);
                driver.FindElement(By.Id(Relationship)).Click();
                Helpers.Pause(1000);
                IWebElement businessSource = driver.FindElement(By.Id("BusinessSource"));
                SelectElement businessSourceSelect = new SelectElement(businessSource);
                businessSourceSelect.SelectByValue(BusinessSource);
                Helpers.Pause(1000);
                if (TermAccepted == "Yes")
                    driver.FindElement(By.Id("TermsAccepted")).Click();

                Helpers.Pause(1000);
                driver.FindElement(By.CssSelector("input[type='submit']")).Click();
                Helpers.Pause();

                if (Expected.ToLower() == "true")
                {
                    Assert.Equal("Application Complete - Credit Cards", driver.Title);
                }
                else
                {
                    var validationSummary = driver.FindElement(By.CssSelector(".validation-summary-errors ul li"));
                    Assert.Equal(Expected, validationSummary.Text);
                }
            }
        }

        //dotnet test --logger "html;logfilename=testResult.html"
    }

}
