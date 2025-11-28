using OpenQA.Selenium;

namespace final.pages
{
    public class LoginPage
    {
        protected WebDriver _driver;
        protected ActionsFactory _actionsFactory = null!;

        private By _usernameBy = By.XPath(@"//input[@id='user-name']");
        private By _passwordBy = By.XPath(@"//input[@id='password']");
        private By _signinBy = By.XPath(@"//input[@id='login-button']");
        private By _errorBy = By.XPath(@"//div[@class='error-message-container error']/h3[@data-test='error']");

        public LoginPage(WebDriver driver, ActionsFactory actionsFactory)
        {
            _driver = driver;
            _actionsFactory = actionsFactory;
        }

        public void LoginWithNoUsernameAndPassword()
        {
            var userNameField = _driver.FindElement(_usernameBy);
            var passwordField = _driver.FindElement(_passwordBy);

            userNameField.SendKeys("test");
            passwordField.SendKeys("test");

            _actionsFactory.GetSafeClear(userNameField).Perform();
            _actionsFactory.GetSafeClear(passwordField).Perform();

            _driver.FindElement(_signinBy).Click();
        }

        public void LoginWithNoPassword()
        {
            var userNameField = _driver.FindElement(_usernameBy);
            var passwordField = _driver.FindElement(_passwordBy);

            userNameField.SendKeys("test");
            passwordField.SendKeys("test");

            _actionsFactory.GetSafeClear(passwordField).Perform();

            _driver.FindElement(_signinBy).Click();
        }

        public MainPage LoginWithCorrectData(String userName, String password)
        {
            _driver.FindElement(_usernameBy).SendKeys(userName);
            _driver.FindElement(_passwordBy).SendKeys(password);
            _driver.FindElement(_signinBy).Click();

            return new MainPage(_driver, _actionsFactory);
        }

        public string GetError()
        {
            return _driver.FindElement(_errorBy).Text;
        }
    }
}
