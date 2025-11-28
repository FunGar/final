using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using System.Drawing;

namespace final
{
    [TestFixture(Drivers.Chrome)]
    [TestFixture(Drivers.Firefox)]
    [Parallelizable(ParallelScope.Fixtures)]
    public class BaseTests
    {
        public const string PageURL = @"https://www.saucedemo.com/";

        public Drivers driverType;
        public WebDriver driver = null!;
        public string CmdCtrl = null!;
        public ActionsFactory actionsFactory = null!;

        public BaseTests(Drivers driverType)
        {
            this.driverType = driverType;
        }

        [SetUp]
        public void Set()
        {
            switch (driverType)
            {
                case Drivers.Chrome:
                    {
                        ChromeOptions options = new ChromeOptions();
                        options.AddArgument("headless");
                        driver = new ChromeDriver(options);
                    }
                    break;
                case Drivers.Firefox:
                    {
                        FirefoxOptions options = new FirefoxOptions();
                        options.AddArgument("--headless");
                        driver = new FirefoxDriver(options);
                    }
                    break;
                default:
                    throw new InvalidOperationException($"Browser type {driverType} is not yet supported");
            }

            var capabilities = driver.Capabilities;
            var platformName = (string) capabilities.GetCapability("platformName");
            CmdCtrl = platformName.Contains("mac") ? Keys.Command : Keys.Control;

            actionsFactory = new ActionsFactory(driver, CmdCtrl);

            driver.Navigate().GoToUrl(PageURL);
            driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(1);
            MaximizeWindow();
        }

        protected void ResizeWindow(int width, int height)
        {
            driver.Manage().Window.Size = new Size(width, height);
        }

        protected void MaximizeWindow()
        {
            driver.Manage().Window.Maximize();
        }

        [TearDown]
        public void TearDown()
        {
            driver?.Dispose();
        }
    }
}
