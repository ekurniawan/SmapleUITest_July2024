﻿using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        [Fact]
        public void LoadApplicationPage()
        {
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                Helpers.Pause();

                string pageTitle = driver.Title;
                Assert.Equal(HomeTitle, pageTitle);
                Assert.Equal(HomeUrl, driver.Url);
            }
        }

        [Fact]
        public void ReloadHomePage()
        {
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
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
            using (IWebDriver driver = new ChromeDriver())
            {
                driver.Navigate().GoToUrl(HomeUrl);

                ReadOnlyCollection<IWebElement> productCells = driver.FindElements(By.TagName("td"));
                Assert.Equal("Easy Credit Card", productCells[0].Text);
                Assert.Equal("20% APR", productCells[1].Text);
                Assert.Equal("Silver Credit Card", productCells[2].Text);
            }
        }


    }
}
