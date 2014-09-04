using System;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.CodeDom.Compiler;
using MonoTouch.CoreGraphics;
using MonoTouch.CoreAnimation;
using System.Drawing;

namespace polymicsproject
{
    partial class SwipeButton : UIButton
    {
        private const float padding = 2;
        private CALayer slider;

        public event EventHandler Swiped;

        public SwipeButton(IntPtr handle)
            : base(handle)
        {
            this.Layer.BackgroundColor = new CGColor(1.0f, 1.0f, 1.0f);

            slider = new CALayer();
            slider.BackgroundColor = new CGColor(0.0f, 0.0f, 0.0f);
            slider.Frame = new RectangleF(padding, padding, this.Layer.Frame.Height - padding * 2, this.Layer.Frame.Height - padding * 2);
            this.Layer.AddSublayer(slider);
        }
            
        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            base.TouchesMoved(touches, evt);

            var touch = touches.AnyObject as UITouch;
            if (touch == null)
                return;

            var position = touch.LocationInView(this);
            var frame = slider.Frame;
            var x = position.X - frame.Width / 2;
            x = Math.Min(x, Frame.Width - frame.Width - padding);
            x = Math.Max(x, padding);
            frame.Location = new PointF(x, frame.Location.Y);
            slider.Frame = frame;
            slider.Speed = 5;
       }

        public override void TouchesEnded(NSSet touches, UIEvent evt)
        {
            base.TouchesEnded(touches, evt);

            var touch = touches.AnyObject as UITouch;
            if (touch == null)
                return;

            var frame = slider.Frame;
            if (frame.Location.X > Frame.Width / 2 && Swiped != null)
                Swiped(this, EventArgs.Empty);

            frame.Location = new PointF(padding, frame.Location.Y);
            slider.Frame = frame;
            slider.Speed = 1;
        }
    }
}
