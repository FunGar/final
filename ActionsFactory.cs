using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace final
{
    public class ActionsFactory
    {
        private IWebDriver driver = null!;
        public string CmdCtrl { get; set; } = null!;

        public ActionsFactory(IWebDriver driver) : this(driver, Keys.Control) { }

        public ActionsFactory(IWebDriver driver, string cmdCtrl)
        {
            this.driver = driver;
            CmdCtrl = cmdCtrl;
        }

        /// <summary>
        /// Creates <see cref="Actions"/> that sends CTRL + a (or Command + a) and delete to an element.
        /// Is used instead of <see cref="IWebElement.Clear"/>, because the clear not always works properly with input fields.
        /// </summary>
        /// <param name="element">Element that needs to be cleared</param>
        /// <returns>Safe clear action</returns>
        public Actions GetSafeClear(IWebElement element)
        {
            return new Actions(driver)
                .MoveToElement(element)
                .Click()
                .KeyDown(CmdCtrl)
                .SendKeys("a")
                .KeyUp(CmdCtrl)
                .SendKeys(Keys.Delete);
        }
    }
}
