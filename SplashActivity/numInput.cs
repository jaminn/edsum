using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;
using Android.Graphics;
using Android.Provider;
using Android.Content.PM;
using System.IO;
using Android.Preferences;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Label = "numInput",ScreenOrientation = ScreenOrientation.Portrait)]
    public class numInput : Activity
    {
        Java.IO.File _file;
        Java.IO.File _dir;
        ImageView _imgView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.input);

            if (IsThereAnAppToTakePictures())
            {
                CreateDirectoryForPictures();

                Button buttonOri = FindViewById<Button>(Resource.Id.buttonOri);

                _imgView = FindViewById<ImageView>(Resource.Id.imageOri);

                buttonOri.Click += TakeAPicture;
                
            }

            Button button = FindViewById<Button>(Resource.Id.button1);
            EditText edit1 = FindViewById<EditText>(Resource.Id.editText1);//max
            EditText edit2 = FindViewById<EditText>(Resource.Id.editText2);//경기이름
            EditText edit3 = FindViewById<EditText>(Resource.Id.editText3);//p1
            EditText edit4 = FindViewById<EditText>(Resource.Id.editText4);//p2

            button.Click += delegate {
                if(edit2.Text == "") {
                    edit2.Error = "경기 이름을 입력해 주세요";
                }
                else if (edit3.Text == "")
                {
                    edit3.Error = "플레이어 이름을 입력해 주세요";
                }
                else if (edit4.Text == "")
                {
                    edit4.Error = "플레이어 이름을 입력해 주세요";
                }
                else if (edit3.Text == edit4.Text)
                {
                    edit4.Error = "플레이어 이름이 중복됩니다.";
                }
                else if (edit1.Text == "")
                {
                    edit1.Error = "점수를 설정해 주세요.";
                }
                else {
                    ISharedPreferences prefs = PreferenceManager.GetDefaultSharedPreferences(Application.Context);
                    ISharedPreferencesEditor editor = prefs.Edit();

                    editor.PutInt("max", Int32.Parse(edit1.Text));
                    editor.PutString("gameName", edit2.Text);
                    editor.PutString("p1Name", edit3.Text);
                    editor.PutString("p2Name", edit4.Text);
                    if (_file != null)
                    {
                        editor.PutString("imgPath", _file.AbsolutePath);
                    }
                    editor.Apply();
                    StartActivity(typeof(NewTouch));
                }
            };
        }

    private bool IsThereAnAppToTakePictures()
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);
            IList<ResolveInfo> availableActivities = PackageManager.QueryIntentActivities(intent, PackageInfoFlags.MatchDefaultOnly);
            return availableActivities != null && availableActivities.Count > 0;
        }

        private void CreateDirectoryForPictures()
        {
            _dir = new Java.IO.File(Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures), "AndroidCameraVSDemo");
            if (!_dir.Exists())
            {
                _dir.Mkdirs();
            }
        }

        private void TakeAPicture(object sender, EventArgs eventArgs)
        {
            Intent intent = new Intent(MediaStore.ActionImageCapture);

            _file = new Java.IO.File(_dir, String.Format("myPhoto_{0}.jpg", Guid.NewGuid()));

            intent.PutExtra(MediaStore.ExtraOutput, Android.Net.Uri.FromFile(_file));

            StartActivityForResult(intent, 0);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Canceled) return;
            // make it available in the gallery
            var imagePath1 = _file.AbsolutePath;
            Intent mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
            if (!String.IsNullOrEmpty(imagePath1) && File.Exists(imagePath1))
            {
                Log.Info("justcheck", "input확인");
                Log.Info("justcheck", imagePath1);
                var myPath = imagePath1;
                int height = Resources.DisplayMetrics.HeightPixels;
                int width = _imgView.Height;
                Bitmap bitmap = BitmapHelpers.LoadAndResizeBitmap(myPath, width, height);
                _imgView.SetImageBitmap(bitmap);
            }
            else
            {
                //_imgView.SetImageResource(Resource.Drawable.newbase);
            }
            GC.Collect();
        }



    }
    

}