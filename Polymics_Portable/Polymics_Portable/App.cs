using System;
using Xamarin.Forms;

namespace Polymics_Portable
{
    public class App
    {
        static IClient _engine;

        public static IClient engine
        {
            get
            {
                if (_engine == null)
                {
                    #if __IOS__
                    _engine = new FibeClient();
                    #endif
                }
                return _engine;
            }
        }

        public static Page GetMainPage()
        {	
            return new ContentPage
            { 
                Content = new Label
                {
                    Text = "Hello, Forms!",
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    HorizontalOptions = LayoutOptions.CenterAndExpand,
                },
            };
        }
    }
}

