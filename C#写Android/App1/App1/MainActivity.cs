using Android.App;
using Android.Widget;
using Android.OS;
using Android.Content;
using Android.Net;

namespace App1
{
    [Activity(Label = "App1", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button btnTest = null;
        private EditText txtNumber = null;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            txtNumber = (EditText)FindViewById(Resource.Id.txtNumber);
            btnTest = (Button)FindViewById(Resource.Id.btnTest);
            btnTest.Click += btnTest_Click;
        }

        void btnTest_Click(object sender, System.EventArgs e)
        {
            Intent dialIntent = new Intent(Intent.ActionDial, Uri.Parse("tel:" + this.txtNumber.Text));
            StartActivity(dialIntent);
        }
    }
}

