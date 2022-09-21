using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Widget;
using Xamarin.Essentials;
using ZXing.Mobile;

namespace BarcodeTypeTwo
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true, Label = "TUBarcode")]
    public class MainActivity : AppCompatActivity
    {
        MobileBarcodeScanner scanner = new MobileBarcodeScanner();

        protected override async void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            InitializeScanner();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var scanButton = FindViewById<Button>(Resource.Id.scanButton);
            var formatButtom = FindViewById<Button>(Resource.Id.copyFormat);
            var urlButton = FindViewById<Button>(Resource.Id.copyUrl);

            scanButton.Click += (s, e) =>
            {
                InitializeScanner();
            };

            formatButtom.Click += (s, e) =>
            {
                var formatResult = FindViewById<TextView>(Resource.Id.formatResult);
                Clipboard.SetTextAsync(formatResult.Text);

                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = builder.Create();
                alert.SetMessage("Copied barcode format");
                alert.Show();
            };

            urlButton.Click += (s, e) =>
            {
                var urlResult = FindViewById<TextView>(Resource.Id.urlTextView);
                Clipboard.SetTextAsync(urlResult.Text);

                Android.App.AlertDialog.Builder builder = new Android.App.AlertDialog.Builder(this);
                Android.App.AlertDialog alert = builder.Create();
                alert.SetMessage("Copied url / text");
                alert.Show();
            };
        }

        private async void InitializeScanner()
        {
            MobileBarcodeScanner.Initialize(Application);

            scanner.UseCustomOverlay = false;

            scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            scanner.BottomText = "Wait for the barcode to automatically scan!";

            ZXing.Result result = await scanner.Scan();

            HandleScanResult(result);
        }

        private void HandleScanResult(ZXing.Result result)
        {
            var barcodeTypeTextView = FindViewById<TextView>(Resource.Id.formatResult);
            var urlTextView = FindViewById<TextView>(Resource.Id.urlTextView);

            var formatButtom = FindViewById<Button>(Resource.Id.copyFormat);
            var urlButton = FindViewById<Button>(Resource.Id.copyUrl);

            if (result != null)
            {
                barcodeTypeTextView.Text = result.BarcodeFormat.ToString();
                formatButtom.Enabled = true;
                formatButtom.SetTextColor(Color.White);

                if (!string.IsNullOrEmpty(result.Text))
                {
                    urlTextView.Text = result.Text;
                    urlButton.Enabled = true;
                    urlButton.SetTextColor(Color.White);
                }
            }
            else
            {
                barcodeTypeTextView.Text = "Canceled";
                formatButtom.Enabled = false;
                formatButtom.SetTextColor(Color.Gray);

                urlTextView.Text = "Please scan for result";
                urlButton.Enabled = false;
                urlButton.SetTextColor(Color.Gray);
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}