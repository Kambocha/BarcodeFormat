using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using AndroidX.Core.Widget;
using ZXing.Mobile;

namespace BarcodeTypeTwo
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
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

            var button = FindViewById<Button>(Resource.Id.scanButton);

            button.Click += (s, e) =>
            {
                InitializeScanner();
            };
        }

        private async void InitializeScanner()
        {
            // Initialize the scanner first so it can track the current context
            MobileBarcodeScanner.Initialize(Application);

            //Tell our scanner to use the default overlay
            scanner.UseCustomOverlay = false;

            //We can customize the top and bottom text of the default overlay
            scanner.TopText = "Hold the camera up to the barcode\nAbout 6 inches away";
            scanner.BottomText = "Wait for the barcode to automatically scan!";

            //Start scanning
            var result = await scanner.Scan();

            HandleScanResult(result);
        }

        private void HandleScanResult(ZXing.Result result)
        {
            TextView barcodeTypeTextView = FindViewById<TextView>(Resource.Id.formatResult);

            if (result != null)
            {
                barcodeTypeTextView.Text = result.BarcodeFormat.ToString();
            }
            else
            {
                barcodeTypeTextView.Text = "Scanning Canceled!";
            }
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}