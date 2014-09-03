using System;
using System.Collections.Generic;
using System.Linq;

using MonoTouch.Foundation;
using MonoTouch.UIKit;

using Xamarin.Forms;

namespace Polymics_Portable.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : UIApplicationDelegate
    {
        UIWindow window;

        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();

            window = new UIWindow(UIScreen.MainScreen.Bounds);

            var loginPage = new LoginPage();
			
            window.RootViewController = loginPage.CreateViewController();
            window.MakeKeyAndVisible();
			
            return true;
        }
    }
}

