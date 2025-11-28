using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace final.pages
{
    public class MainPage
    {
        protected WebDriver _driver;
        protected ActionsFactory _actionsFactory = null!;

        private By _titleBy = By.XPath(@"//div[@class='app_logo']");

        public MainPage(WebDriver driver, ActionsFactory actionsFactory)
        {
            _driver = driver;
            _actionsFactory = actionsFactory;
        }

        public string GetTitle()
        {
            var wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(5));
            return wait.Until(dr => dr.FindElement(_titleBy)).Text;
        }
    }
}
