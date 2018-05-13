using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Foundation.Metadata;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace UINotUpdatingFromDispatcher
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;

        bool test1On = false;
        bool test2On = false;
        bool test3On = false;

        public MainPage()
        {
            this.InitializeComponent();

            // load toggleSwitcch values
            Loaded += delegate
            {
                toggleSwitchTest1.IsOn = Convert.ToBoolean(localSettings.Values["Test1"]);
                toggleSwitchTest2.IsOn = Convert.ToBoolean(localSettings.Values["Test2"]);
                toggleSwitchTest3.IsOn = Convert.ToBoolean(localSettings.Values["Test3"]);

                // init event handlers
                toggleSwitchTest1.Toggled += toggleSwitchTest1_Toggled;
                toggleSwitchTest3.Toggled += toggleSwitchTest2_Toggled;
                toggleSwitchTest3.Toggled += toggleSwitchTest3_Toggled;
            };
        }

        private void toggleSwitchTest1_Toggled(object sender, RoutedEventArgs e)
        {
            // set setting
            localSettings.Values["Test1"] = (toggleSwitchTest1.IsOn).ToString();
        }

        private ValueSet UpdateWin32()
        {
            // generate int[] to send
            int[] arrayToSend = null;
            try
            {
                arrayToSend = new int[] { (localSettings.Values["Test1"].ToString() == "True") ? 1 : 0,
                                          (localSettings.Values["Test2"].ToString() == "True") ? 1 : 0,
                                          (localSettings.Values["Test3"].ToString() == "True") ? 1 : 0};
            }
            catch (NullReferenceException)
            {
                localSettings.Values["Test1"] = false.ToString();
                localSettings.Values["Test2"] = false.ToString();
                localSettings.Values["Test3"] = false.ToString();

                /*
                await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
                {
                    localSettings.Values["Test1"] = (toggleSwitchTest1.IsOn).ToString();
                    localSettings.Values["Test2"] = (toggleSwitchTest3.IsOn).ToString();
                    localSettings.Values["Test3"] = (toggleSwitchTest3.IsOn).ToString();
                });
                
                 * This also anyway returns false
                 */
            }


            // generate ValueSet to send to Win32
            ValueSet msg = new ValueSet();
            msg.Add("content", arrayToSend);
            return msg;
        }

        private void toggleSwitchTest2_Toggled(object sender, RoutedEventArgs e)
        {
            // set setting
            localSettings.Values["Test2"] = (toggleSwitchTest2.IsOn).ToString();
        }

        private void toggleSwitchTest3_Toggled(object sender, RoutedEventArgs e)
        {
            // set setting
            localSettings.Values["Test3"] = (toggleSwitchTest3.IsOn).ToString();
        }

        public async Task updateUI()
        {
            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, () =>
            {
                // set Test1 state in UI
                toggleSwitchTest1.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest1"])) ? "It is: On" : "It is: Off";
                toggleSwitchTest1.OffContent = toggleSwitchTest1.OnContent;

                // set Test2 state in UI
                toggleSwitchTest2.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest2"])) ? "It is: On" : "It is: Off";
                toggleSwitchTest2.OffContent = toggleSwitchTest2.OnContent;

                // set Test3 state in UI
                toggleSwitchTest3.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest3"])) ? "It is: On" : "It is: Off";
                toggleSwitchTest3.OffContent = toggleSwitchTest3.OnContent;

                string absoluteString = "ms-appx:///Assets/";
                // set image1 state in UI
                image1.Source = Convert.ToBoolean(localSettings.Values["WinFormTest1"]) ?
                        new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));

                // set image2 state in UI
                image2.Source = Convert.ToBoolean(localSettings.Values["WinFormTest2"]) ?
                        new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));

                // set image3 state in UI
                image3.Source = Convert.ToBoolean(localSettings.Values["WinFormTest3"]) ?
                        new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));
            });

            // it is as if the code does not run, however, breakpoints are hit and, according to Visual Studio, the properties change, but
            // no visual change occurs. the same happens with Checkboxes. works perfectly with button. what to do??
        }

        public async void Connection_OnRequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // read content
            if (args.Request.Message.ContainsKey("content"))
            {
                object message = null;
                args.Request.Message.TryGetValue("content", out message);
                // if message is an int[]
                if (message is int[])
                {
                    // init field vars
                    int indexInArray = 0;
                    foreach (int trueorfalse in (int[])message)
                    {
                        // set bool state based on index
                        switch (indexInArray)
                        {
                            case 0:
                                test1On = Convert.ToBoolean(trueorfalse);
                                localSettings.Values["winFormTest1"] = (Convert.ToBoolean(trueorfalse)).ToString();
                                break;
                            case 1:
                                test2On = Convert.ToBoolean(trueorfalse);
                                localSettings.Values["winFormTest2"] = (Convert.ToBoolean(trueorfalse)).ToString();
                                break;
                            case 2:
                                test3On = Convert.ToBoolean(trueorfalse);
                                localSettings.Values["winFormTest3"] = (Convert.ToBoolean(trueorfalse)).ToString();
                                break;
                            default:
                                break;
                        }
                        indexInArray++;
                    }

                    await updateUI();
                }
            }
            else if (args.Request.Message.ContainsKey("request"))
            {
                // send current settings as response
                AppServiceResponseStatus responseStatus = await args.Request.SendResponseAsync(UpdateWin32());
            }
            else if (args.Request.Message.ContainsKey("exit"))
            {
                // exit
                Application.Current.Exit();
            }
        }

        private async void Grid_Loading(FrameworkElement sender, object args)
        {
            // launch WinForm
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Unfortunately, this OS does not meet the minimum requirements. " +
                    "App will now exit...", "Requirements Not Met");
                await messageDialog.ShowAsync();

                Application.Current.Exit();
            }
        }

        private async void btnLaunchWinform_Click(object sender, RoutedEventArgs e)
        {
            // launch WinForm
            if (ApiInformation.IsApiContractPresent("Windows.ApplicationModel.FullTrustAppContract", 1, 0))
            {
                await FullTrustProcessLauncher.LaunchFullTrustProcessForCurrentAppAsync();
            }
            else
            {
                MessageDialog messageDialog = new MessageDialog("Unfortunately, this OS does not meet the minimum requirements. " +
                    "App will now exit...", "Requirements Not Met");
                await messageDialog.ShowAsync();

                Application.Current.Exit();
            }
        }

        private void btnManuallyUpdateUI_Click(object sender, RoutedEventArgs e)
        {
            // set Test1 state in UI
            toggleSwitchTest1.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest1"])) ? "It is: On" : "It is: Off";
            toggleSwitchTest1.OffContent = toggleSwitchTest1.OnContent;

            // set Test2 state in UI
            toggleSwitchTest2.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest2"])) ? "It is: On" : "It is: Off";
            toggleSwitchTest2.OffContent = toggleSwitchTest2.OnContent;

            // set Test3 state in UI
            toggleSwitchTest3.OnContent = (Convert.ToBoolean(localSettings.Values["winFormTest3"])) ? "It is: On" : "It is: Off";
            toggleSwitchTest3.OffContent = toggleSwitchTest3.OnContent;

            string absoluteString = "ms-appx:///Assets/";
            // set image1 state in UI
            image1.Source = Convert.ToBoolean(localSettings.Values["WinFormTest1"]) ?
                    new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));

            // set image2 state in UI
            image2.Source = Convert.ToBoolean(localSettings.Values["WinFormTest2"]) ?
                    new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));

            // set image3 state in UI
            image3.Source = Convert.ToBoolean(localSettings.Values["WinFormTest3"]) ?
                    new BitmapImage(new Uri(absoluteString + "On.png")) : new BitmapImage(new Uri(absoluteString + "Off.png"));

            // works perfectly
        }
    }
}
