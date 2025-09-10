using Microsoft.Playwright;
using Global360.PageObjects.Base;
using Global360.Constants;

namespace Global360.PageObjects
{
    public class LoginPage : BasePage
    {
        // Selectors
        private readonly string _usernameField = "[id='username']";
        private readonly string _passwordField = "[id='password']";
        private readonly string _loginButton = "[id='submit']";

        public LoginPage(IPage page) : base(page) { }

        public async Task NavigateAsync()
        {
            await NavigateToAsync(TestConstants.LoginPage);
        }


        public async Task LoginAsync(string username, string password)
        {
            await FillAsync(_usernameField, username);
            await FillAsync(_passwordField, password);
            await ClickAsync(_loginButton);
        }

        public async Task<bool> IsLoginSuccessfulAsync()
        {
            try
            {
                var header = await GetTextAsync("h1");
                if (header.Trim().Equals("Dashboard"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                
            }
            catch
            {
                return false;
            }
        }
    }
}