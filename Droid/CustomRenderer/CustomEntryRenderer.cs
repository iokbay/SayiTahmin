using System;
using Android.App;
using Android.Content;
using Android.Util;
using Android.Views;
using Android.Views.Animations;
using SayiTahmin.CustomControls;
using SayiTahmin.Droid.CustomRenderer;
using SayiTahmin.Droid;
using Xamarin.Forms.Platform.Android;
using Java.Lang;
using Android.InputMethodServices;

[assembly: Xamarin.Forms.ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace SayiTahmin.Droid.CustomRenderer
{
    class CustomEntryRenderer : EntryRenderer
    {
        public Keyboard mKeyboard;
        public CustomKeyboardView mKeyboardView;
        public CustomEntryRenderer(Context context) : base(context)
        {
            mKeyboardView = (CustomKeyboardView)FindViewById(Resource.Id.keyboard_view);
            MainActivity deneme;
            MainActivity.mActivityRef.TryGetTarget(out deneme);
            mKeyboard = new Keyboard(deneme, Resource.Xml.keyboard2);

            mKeyboardView.Keyboard = mKeyboard;

            mKeyboardView.Key += (sender, e) => {
                long eventTime = JavaSystem.CurrentTimeMillis();
                KeyEvent ev = new KeyEvent(eventTime, eventTime, KeyEventActions.Down, e.PrimaryCode, 0, 0, 0, 0, KeyEventFlags.SoftKeyboard | KeyEventFlags.KeepTouchMode);

                this.DispatchKeyEvent(ev);
            };
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.Touch += (sender, ed) => {
                    Log.Info("onTouch", "true");
                    ShowKeyboardWithAnimation();
                    ed.Handled = true;
                };

            }
        }
        public void ShowKeyboardWithAnimation()
        {
            Log.Info("keyboardState", mKeyboardView.Visibility.ToString());
            if (mKeyboardView.Visibility == ViewStates.Gone)
            {
                MainActivity deneme;
                MainActivity.mActivityRef.TryGetTarget(out deneme);
                Animation animation = AnimationUtils.LoadAnimation(
                    deneme,
                    Resource.Animation.slide_in_bottom
                );
                mKeyboardView.ShowWithAnimation(animation);
            }
        }

    }
}
