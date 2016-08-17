using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;
using Android.Util;
using Android.Widget;
using System;

namespace com.xamarin.sample.splashscreen
{
    [Activity(Label = "START")]
    public class Activity1 : AppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            button.Click += delegate
            {
                StartActivity(typeof(numInput));
            };
        }

        public override void OnBackPressed()
        {
            Android.App.AlertDialog.Builder alert = new Android.App.AlertDialog.Builder(this);
            alert.SetTitle("�˸�");
            Toast.MakeText(this, "goto", ToastLength.Short);
            alert.SetMessage("�����ðڽ��ϱ�?");

            alert.SetNegativeButton("���", (senderAlert, args) =>
            {
            });

            alert.SetPositiveButton("Ȯ��", (senderAlert, args) =>
            {
                Intent intent = new Intent(Intent.ActionMain);
                intent.AddCategory(Intent.CategoryHome);
                intent.SetFlags(ActivityFlags.NewTask);
                StartActivity(intent);
            });
            Dialog dialog = alert.Create();
            dialog.Show();
        }

    }

}