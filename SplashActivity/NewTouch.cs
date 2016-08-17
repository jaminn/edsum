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
using Android.Util;
using Android.Content.PM;
using Android.Graphics;
using Android.Preferences;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Label = "NewTouch", ScreenOrientation = ScreenOrientation.SensorLandscape)]
    public class NewTouch : Activity
    {
        int cnt1 = 0;
        int cnt2 = 0;
        int END = 13;
        string gameName;
        string p1Name;
        string p2Name;
        string imagePath1;

        bool gameOver = false;

        private ImageView _img;
        private TextView _txtView1;
        private TextView _txtView2;

        private TextView _txtView3;
        private TextView _txtView4;

        float startY = 0;
        float endY = 0;
        string tag = "TouchCheck";


        public override void OnBackPressed()
        {
            if (gameOver)
            {
                AlertDialog.Builder alert = new AlertDialog.Builder(this);
                alert.SetTitle("알림");
                Toast.MakeText(this, "goto", ToastLength.Short);
                alert.SetMessage("승자를 선택해 주세요.");

                alert.SetNegativeButton(p1Name, (senderAlert, args) =>
                {
                    Toast.MakeText(this, p1Name + "의 승리", ToastLength.Short).Show();
                    StartActivity(typeof(Activity1));
                });

                alert.SetPositiveButton(p2Name, (senderAlert, args) =>
                {
                    Toast.MakeText(this, p2Name + "의 승리", ToastLength.Short).Show();
                    StartActivity(typeof(Activity1));
                });
                Dialog dialog = alert.Create();
                dialog.Show();
            }else
            {
                Finish();
            }
            //Finish();
        }



        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Log.Info(tag, "this is an info message");
            SetContentView(Resource.Layout.layoutTouch);

            ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
            END = prefs.GetInt("max", 1);
            gameName = prefs.GetString("gameName","nope");
            p1Name = prefs.GetString("p1Name","nope");
            p2Name = prefs.GetString("p2Name","nope");
            imagePath1 = prefs.GetString("imgPath","");

            Log.Info(tag, "justcheck" + "|" + gameName+ "|" + p1Name+ "|" + p2Name);
            Toast.MakeText(this, gameName +"시작", ToastLength.Short);

            
            _img = FindViewById<ImageView>(Resource.Id.imageView1);
            _txtView1 = FindViewById<TextView>(Resource.Id.textView1);
            _txtView2 = FindViewById<TextView>(Resource.Id.textView2);

            _txtView3 = FindViewById<TextView>(Resource.Id.textView3);
            _txtView4 = FindViewById<TextView>(Resource.Id.textView4);
            
            if (!String.IsNullOrEmpty(imagePath1)  &&  File.Exists(imagePath1))
            {
                Log.Info("justcheck", "확인");
                Log.Info("justcheck", imagePath1);
                var myPath = imagePath1;
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _img.Height;
                Bitmap bitmap = BitmapHelpers.LoadAndResizeBitmap(myPath, width, height);
                _img.SetImageBitmap(bitmap);
            } else
            {
                _img.SetImageResource(Resource.Drawable.newbase);
            }
            GC.Collect();



            //_img.SetImageResource(Resource.Drawable.newbase);
            _txtView1.Text = cnt1.ToString();
            _txtView2.Text = cnt2.ToString();

            _txtView3.Text = p1Name;
            _txtView4.Text = p2Name;


            _txtView1.Touch += TouchViewOnTouch;
            _txtView2.Touch += TouchView2OnTouch;
        }

        private void TouchViewOnTouch(object sender, View.TouchEventArgs e)
        {
            string message = "";

            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    startY = e.Event.GetY();
                    break;
                case MotionEventActions.Move:
                    message = "Touch second Begins";
                    break;
                case MotionEventActions.Up:
                    endY = e.Event.GetY();
                    Log.Info(tag, e.Event.GetX().ToString() + "," + (endY - startY).ToString());
                    if ((endY - startY) > 50)
                    {
                        message = "Down";
                        if (cnt1 != 0)
                        {
                            cnt1--;
                        }

                    }
                    else if ((startY - endY) > 50)
                    {
                        message = "Up";
                        cnt1++;
                    }
                    else
                    {
                        message = "Nope";
                    }
                    startY = 0;
                    endY = 0;
                    _txtView1.Text = cnt1.ToString();
                    if (END == cnt1 && !gameOver)
                    {
                        gameOver = true;
                        Toast.MakeText(this, p1Name+"의 승리", ToastLength.Short).Show();
                        ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                        ISharedPreferencesEditor editor = prefs.Edit();
                        editor.PutInt("p1Cnt", cnt1);
                        editor.Apply();
                        Intent intent = new Intent(this,typeof(p1win));
                        intent.SetFlags(ActivityFlags.NoAnimation);
                        
                        StartActivity(intent);
                    }
                    else {
                        //Toast.MakeText(this, message, ToastLength.Short).Show();
                    }
                    break;
                default:
                    message = string.Empty;
                    break;
            }
        }

        

        private void TouchView2OnTouch(object sender, View.TouchEventArgs e)
        {
            string message = "";

            switch (e.Event.Action & MotionEventActions.Mask)
            {
                case MotionEventActions.Down:
                    startY = e.Event.GetY();
                    break;
                case MotionEventActions.Move:
                    message = "Touch second Begins";
                    break;
                case MotionEventActions.Up:
                    endY = e.Event.GetY();
                    Log.Info(tag, e.Event.GetX().ToString() + "," + (endY - startY).ToString());
                    if ((endY - startY) > 50)
                    {
                        message = "Down";
                        if (cnt2 != 0) {
                            cnt2--;
                        }
                        
                    }
                    else if ((startY - endY) > 50)
                    {
                        message = "Up";
                        cnt2++;
                    }
                    else
                    {
                        message = "Nope";
                    }
                    startY = 0;
                    endY = 0;
                    _txtView2.Text = cnt2.ToString();
                    if (END == cnt2 && !gameOver)
                    {
                        gameOver = true;
                        Toast.MakeText(this, p2Name+"의 승리", ToastLength.Short).Show();
                        ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                        ISharedPreferencesEditor editor = prefs.Edit();
                        editor.PutInt("p2Cnt", cnt2);
                        editor.Apply();
                        Intent intent = new Intent(this, typeof(p2win));
                        intent.SetFlags(ActivityFlags.NoAnimation);
                        StartActivity(intent);
                    }
                    else
                    {
                        //Toast.MakeText(this, message, ToastLength.Short).Show();
                    }
                    break;
                default:
                    message = string.Empty;
                    break;
            }

        }

    }
}