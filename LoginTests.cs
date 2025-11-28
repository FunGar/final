using final.pages;
using NUnit.Framework;

namespace final
{
    [Parallelizable(ParallelScope.Fixtures)]
    public class LoginTests : BaseTests
    {
        public LoginTests(Drivers driverType) : base(driverType) { }

        [Test]
        public void LoginWithNoUsernameAndPassword()
        {
            var loginPage = new LoginPage(driver, actionsFactory);
            loginPage.LoginWithNoUsernameAndPassword();

            var error = loginPage.GetError();

            Assert.That(error, Does.Contain("Username is required"));
        }

        [Test]
        public void LoginWithNoPassword()
        {
            var loginPage = new LoginPage(driver, actionsFactory);
            loginPage.LoginWithNoPassword();

            var error = loginPage.GetError();

            Assert.That(error, Does.Contain("Password is required"));
        }

        [Test]
        [TestCase("standard_user", "secret_sauce")]
        public void LoginWithCorrectData(string userNameToUse, string passwordToUse)
        {
            var loginPage = new LoginPage(driver, actionsFactory);
            var mainPage = loginPage.LoginWithCorrectData(userNameToUse, passwordToUse);

            var title = mainPage.GetTitle();

            Assert.That(title, Does.Contain("Swag Labs"));
        }
    }
}
