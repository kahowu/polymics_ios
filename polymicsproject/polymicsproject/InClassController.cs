using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using StudentDemo;
using System.Threading.Tasks;

namespace polymicsproject
{
	partial class InClassController : UIViewController
	{

		public FibeClass joined;

		public InClassController (IntPtr handle) : base (handle)
		{
		}

		public override void ViewDidLoad ()
		{
            base.ViewDidLoad ();

            if (this.NavigationItem.BackBarButtonItem != null)
            {
                this.NavigationItem.BackBarButtonItem.TintColor = new UIColor(1.0f, 1.0f, 1.0f, 1.0f);
                this.NavigationItem.BackBarButtonItem.Title = " ";
            }

			if (joined != null) {
				labelRequest.Text = joined.Title;
			}

			btnRequest.TouchUpInside += MakeRequest;
		}

		void MakeRequest (object sender, EventArgs e)
		{
			PingRequest x = joined.addPing("");

			InQueueViewController queueView = Storyboard.InstantiateViewController ("InQueueViewController") as InQueueViewController;
			queueView.request = x;
			queueView.joined = joined;
			this.NavigationController.PushViewController (queueView, true);
		}
	}
}
