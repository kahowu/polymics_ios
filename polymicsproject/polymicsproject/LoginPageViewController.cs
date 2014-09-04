using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using System.Threading.Tasks;
using System.Collections.Generic;
using StudentDemo;

namespace polymicsproject
{
	partial class LoginPageViewController : UIViewController
	{
		public LoginPageViewController (IntPtr handle) : base (handle)
		{
		}

		async void ConnectToServer ()
		{
			bool x = false;
			while (!x) {
				LoadSpinner loadingOverlay = new LoadSpinner (UIScreen.MainScreen.Bounds);
				View.Add (loadingOverlay);

				await Task.Run (() => {
					x = Application.engine.ConnectTo ("hysw.org");
                    Console.WriteLine("Connection established!");
				});

				loadingOverlay.Hide ();

				if (!x) {
					new UIAlertView ("Unable to connect to server!", "Please check your connection.", null, "Retry", null);
				}
			}
		}

        public override void ViewDidAppear(bool animated)
        {
            this.NavigationController.SetNavigationBarHidden(true, false);
            base.ViewDidAppear(animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            this.NavigationController.SetNavigationBarHidden(false, true);
            this.NavigationController.NavigationBar.Alpha = 0.75f;
            base.ViewDidDisappear(animated);
        }

		public override void ViewDidLoad ()
		{
            base.ViewDidLoad ();


            if (this.NavigationItem.BackBarButtonItem != null)
            {
                this.NavigationItem.BackBarButtonItem.TintColor = new UIColor(1.0f, 1.0f, 1.0f, 1.0f);
                this.NavigationItem.BackBarButtonItem.Title = " ";
            }

            UIImage image = UIImage.FromBundle("Background.jpg");
            image = image.Scale(new System.Drawing.SizeF(320f, 580f));
            this.View.BackgroundColor = UIColor.FromPatternImage(image);
            //this.NavigationController.NavigationBar.SetBackgroundImage(image, UIBarMetrics.Default);
//            this.NavigationController.NavigationBaZr.Alpha = 0.1f;

            ConnectToServer ();
			btnLogin.TouchUpInside += LoginBtnClick;
			txtUsername.ShouldReturn += JumptoPassword;
			txtPassword.ShouldReturn += LoginReturn;
		}

		bool JumptoPassword (UITextField textField)
		{
			txtPassword.Select (textField);
			return true;
		}

		bool LoginReturn (UITextField textField)
		{
			textField.ResignFirstResponder ();
			LoginBtnClick (null, null);
			return true;
		}

		async void LoginBtnClick (object sender, EventArgs e)
		{
			bool x = false;
			String username = txtUsername.Text;
			String password = txtPassword.Text;
			LoadSpinner loadingOverlay = new LoadSpinner (UIScreen.MainScreen.Bounds);
			View.Add (loadingOverlay);

			await Task.Run (() => {
				x = Application.engine.login(username, password);
			});

			UIApplication.SharedApplication.NetworkActivityIndicatorVisible = false;

			if (!x) {
				loadingOverlay.Hide ();
				UIAlertView err = new UIAlertView ("Login failed", Application.engine.CurrentError, null, "OK", null);
				err.Show ();
			} else {

				x = await DoGetRooms ();

				loadingOverlay.Hide ();
				if (x) {
					this.NavigationController.PopViewControllerAnimated (false);
					RoomSelectController roomView = Storyboard.InstantiateViewController ("RoomSelectController") as RoomSelectController;
					roomView.classList = new List<ClassItem> (Application.engine.classAvailable);

					this.NavigationController.PushViewController (roomView, true);
				}
			}
		}

		async Task<bool> DoGetRooms() {
			bool x = false;
			await Task.Run (() => {
				try {
					x = Application.engine.list();
                } catch (Exception ex) {
					x = false;
                    Console.WriteLine(ex.StackTrace);
				}
			});
			if (!x) {
				UIAlertView err = new UIAlertView ("Failed to retrieve classes!", Application.engine.CurrentError, null, "OK", null);
				err.Show ();
			}
			return x;
		}
	}
}
