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
        private readonly string _successAlert = "[id='success-notification']";

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
            await WaitForLoadingToDisappearAsync();
            var successLogin = await IsVisibleAsync(_successAlert);

            if (successLogin)
            {
                return true;
            }
            return false;
        }
    }
}