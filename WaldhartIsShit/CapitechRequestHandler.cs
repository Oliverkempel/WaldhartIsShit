namespace WaldhartIsShit
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using OpenQA.Selenium.DevTools;
    using OpenQA.Selenium.Support.UI;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CapitechRequestHandler
    {
        public string Url { get; set; } = "https://hafjell.capitech.no/Apps/MinCapitech/";

        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public void RefreshTokens(string username, string password)
        {
            // Inits Chrome driver
            ChromeOptions chromeOptions = new ChromeOptions();
            var driver = new ChromeDriver(chromeOptions);

            // Navigates to specified url
            driver.Navigate().GoToUrl($"{Url}");

            // Wait for the browser to navigate to the specific URL (Timeout 120 seconds)
            WebDriverWait wait = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.Until(d => d.Url.Contains($"https://login.simployer.com/", StringComparison.OrdinalIgnoreCase));

            // Replace this with something smarter PLEASEEE GOD!!
            Thread.Sleep(3000);

            driver.FindElement(By.Id("username_login")).SendKeys(username);
            driver.FindElement(By.Id("password_login")).SendKeys(password);

            driver.FindElement(By.Id("btn-login")).Click();

            // Wait for the browser to navigate to the specific URL (Timeout 120 seconds)
            WebDriverWait wait2 = new WebDriverWait(driver, TimeSpan.FromSeconds(120));
            wait.Until(d => d.Url.Contains($"https://hafjell.capitech.no/", StringComparison.OrdinalIgnoreCase));

            Thread.Sleep(1000);

            Cookie accessToken = driver.Manage().Cookies.AllCookies.Where(x => x.Name == "my-capitech-access-token").FirstOrDefault();
            Cookie refreshToken = driver.Manage().Cookies.AllCookies.Where(x => x.Name == "refresh-token").FirstOrDefault();

            AccessToken = accessToken.Value;
            RefreshToken = refreshToken.Value;
        }
    }
}
