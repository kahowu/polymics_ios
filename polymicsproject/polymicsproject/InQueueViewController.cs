using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using StudentDemo;

namespace polymicsproject
{
	partial class InQueueViewController : UIViewController
	{
		public FibeClass joined {
			get;
			set;
		}

        public PingRequest _request;

        System.Timers.Timer tt;

        int _secondsElapsed = 0;

   

		public PingRequest request {
            get 
            {
                return _request;
            }
            set 
            {
                _request = value;

                _request.onAudioAccept += (object sender, EventArgs e) => {
                    Console.WriteLine("Called!");
                    InvokeOnMainThread(() => {
                        btnStop.SetTitle("Stop", UIControlState.Normal);
                        UIView.Animate(0.5d, new NSAction(delegate {
                            imgQueue.Alpha = 0f;
                            imgQueued.Alpha = 1f;
                        })
                        );
                        tt = new System.Timers.Timer(1000d);
                        tt.Elapsed += (object senderX, System.Timers.ElapsedEventArgs eX) => {
                            InvokeOnMainThread(() =>
                                {
                                    labelCounter.Text = TimeSpan.FromSeconds(_secondsElapsed++).ToString();
                                });
                        };
                        tt.Start();
                    });
                };

                _request.onAudioStop += (object sender, EventArgs e) => {
                    CancelReq();
                };
            }
		}

		public InQueueViewController (IntPtr handle) : base (handle)
		{
		}

		public override void WillMoveToParentViewController (UIViewController parent)
		{
			base.WillMoveToParentViewController (parent);

			if (parent == null) {
				Console.WriteLine ("Popped!");
				request.stopTalking ();
				joined.CancelRequest();
			}
		}

		public override void ViewDidLoad ()
		{
            base.ViewDidLoad ();

            if (this.NavigationItem.BackBarButtonItem != null)
            {
                this.NavigationItem.BackBarButtonItem.TintColor = new UIColor(1.0f, 1.0f, 1.0f, 1.0f);
                this.NavigationItem.BackBarButtonItem.Title = " ";
            }

			btnStop.TouchUpInside += (object sender, EventArgs e) => {
				CancelReq();
			};

			btnStop.SetTitle ("Cancel", UIControlState.Normal);
		}

		public void CancelReq() {
			this.NavigationController.PopViewControllerAnimated(true);
		}

        public override void ViewWillDisappear(bool animated)
        {
            if (tt != null)
                tt.Stop();
            base.ViewWillDisappear(animated);
        }
	}
}
