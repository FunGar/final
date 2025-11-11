using Microsoft.Testing.Platform.Capabilities;
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Interactions;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace final
{
    public enum Drivers
    {
        Chrome,
        Firefox,
    }

    [TestFixture(Drivers.Chrome)]
    [TestFixture(Drivers.Firefox)]
    public class Tests
    {
        public static readonly string PageURL = @"https://www.saucedemo.com/";

        public Drivers driverType;
        public WebDriver driver = null!;
        public string CmdCtrl = null!;
        public ActionsFactory actionsFactory = null!;

        public Tests(Drivers driverType)
        {
            this.driverType = driverType;
        }

        [SetUp]
        public void Set()
        {
            driver = driverType switch
            {
                Drivers.Chrome => new ChromeDriver(),
                Drivers.Firefox => new FirefoxDriver(),
                _ => throw new InvalidOperationException($"Browser type {driverType} is not yet supported"),
            };

            var capabilities = driver.Capabilities;
            var platformName = (string) capabilities.GetCapability("platformName");
            CmdCtrl = platformName.Contains("mac") ? Keys.Command : Keys.Control;

            actionsFactory = new ActionsFactory(driver, CmdCtrl);

            driver.Navigate().GoToUrl(PageURL);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }

        [Test]
        public void LoginWithNoUsernameAndPassword()
        {
            var userName = driver.FindElement(By.XPath(@"//input[@id='user-name']"));
            var password = driver.FindElement(By.XPath(@"//input[@id='password']"));

            userName.SendKeys("test");
            password.SendKeys("test");

            actionsFactory.GetSafeClear(userName).Perform();
            actionsFactory.GetSafeClear(password).Perform();

            driver.FindElement(By.XPath(@"//input[@id='login-button']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var error = wait.Until(dr => dr.FindElement(By.XPath(@"//div[@class='error-message-container error']/h3[@data-test='error']")));

            Assert.That(error.Displayed);
            Assert.That(error.Text.Contains("Username is required"));
        }

        [Test]
        public void LoginWithNoPassword()
        {
            var userName = driver.FindElement(By.XPath(@"//input[@id='user-name']"));
            var password = driver.FindElement(By.XPath(@"//input[@id='password']"));

            userName.SendKeys("test");
            password.SendKeys("test");

            actionsFactory.GetSafeClear(password).Perform();

            password.Clear();

            var tete = password.Text;

            driver.FindElement(By.XPath(@"//input[@id='login-button']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var error = wait.Until(dr => dr.FindElement(By.XPath(@"//div[@class='error-message-container error']/h3[@data-test='error']")));
            tete = password.Text;

            Assert.That(error.Displayed);
            Assert.That(error.Text.Contains("Password is required"));
        }

        [Test]
        public void CorrectLogin()
        {
            var userName = driver.FindElement(By.XPath(@"//input[@id='user-name']"));
            var password = driver.FindElement(By.XPath(@"//input[@id='password']"));

            var userNameToUse = driver.FindElement(By.XPath(@"//div[@id='login_credentials']")).Text.Split(Environment.NewLine)[1];

            var passwordToUse = driver.FindElement(By.XPath(@"//div[@class='login_password']")).Text.Split(Environment.NewLine)[1];

            userName.SendKeys(userNameToUse);
            password.SendKeys(passwordToUse);

            driver.FindElement(By.XPath(@"//input[@id='login-button']")).Click();

            var wait = new WebDriverWait(driver, TimeSpan.FromSeconds(5));
            var title = wait.Until(dr => dr.FindElement(By.XPath(@"//div[@class='app_logo']")));

            Assert.That(title.Displayed);
            Assert.That(title.Text.Contains("Swag Labs"));
        }

    }
}
