using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Polymics_Portable
{	
	public partial class LoginPage : ContentPage
	{	
		public LoginPage ()
		{
			InitializeComponent ();

            ConnectToServer();

            btnLogin.Clicked += LoginClicked;
		}

        async void ConnectToServer() 
        {
            var loadIndi = new ActivityIndicator() {
                Color = Color.Black
            };
            loadIndi.IsVisible = loadIndi.IsRunning = true;

            bool x = false;
            await Task.Run(() =>
                {
                    x = App.engine.ConnectTo("hysw.org");
                });
            if (x)
            {
                loadIndi.IsVisible = loadIndi.IsRunning = false;
            }
        }

        async void LoginClicked(object sender, EventArgs e)
        {
            String username = txtUsername.Text;
            String password = txtPassword.Text;

            bool s = false;
            await Task.Run(() =>
                {
                    s = App.engine.login(username, password);
                });
            if (s)
            {
            }
            else
            {
                DisplayAlert("Error", App.engine.getError(), "OK", "");
            }

        }
	}
}

