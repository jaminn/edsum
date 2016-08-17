using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Content.PM;
using Android.Util;
using Android.Graphics;
using Android.Preferences;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Label = "p1win", ScreenOrientation = ScreenOrientation.SensorLandscape, NoHistory = true)]
    public class p1win : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.layoutTouchP1Win);

            TextView txtName = FindViewById<TextView>(Resource.Id.p1WinName);
            TextView txtCnt = FindViewById<TextView>(Resource.Id.p1WinCnt);
            ImageView _img = FindViewById<ImageView>(Resource.Id.imageView1);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            String imagePath1 = prefs.GetString("imgPath", "");
            if (!string.IsNullOrEmpty(imagePath1) && File.Exists(imagePath1))
            {
                Log.Info("justcheck", "p1win확인");
                Log.Info("justcheck", imagePath1);
                var myPath = imagePath1;
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _img.Height;
                Bitmap bitmap = BitmapHelpers.LoadAndResizeBitmap(myPath, width, height);
                _img.SetImageBitmap(bitmap);
            }
            else
            {
                _img.SetImageResource(Resource.Drawable.newbase);
            }
            GC.Collect();

            txtName.Text = prefs.GetString("p1Name", "nope")+": WIN";
            txtCnt.Text = prefs.GetInt("p1Cnt", 0).ToString();
            txtCnt.Touch += touched;


        }

        private void touched(object sender, View.TouchEventArgs e)
        {
            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Up:
                    AlertDialog.Builder alert = new AlertDialog.Builder(this);
                    alert.SetTitle("알림");
                    alert.SetMessage("결과를 확정하시겠습니까?");
                    alert.SetPositiveButton("확인", (senderAlert, args) => {
                        Toast.MakeText(this, "확인", ToastLength.Short).Show();
                        StartActivity(typeof(Activity1));
                    });

                    alert.SetNegativeButton("취소", (senderAlert, args) => {
                        Toast.MakeText(this, "취소", ToastLength.Short).Show();
                        var ACTtouch = new Intent(this, typeof(NewTouch));
                        ACTtouch.SetFlags(ActivityFlags.ReorderToFront);
                        StartActivity(ACTtouch);
                    });
                    Dialog dialog = alert.Create();
                    dialog.Show();
                    break;
            }
        }
    }
}