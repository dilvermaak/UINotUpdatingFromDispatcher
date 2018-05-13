using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Collections;

namespace WinForm
{
    public partial class MainForm : Form
    {
        private AppServiceConnection connection = null;

        bool test1 = false;
        bool test2 = false;
        bool test3 = false;

        System.Timers.Timer tCheckLockedBoxes = new System.Timers.Timer();

        public MainForm()
        {
            InitializeComponent();

            // init timer
            tCheckLockedBoxes.Elapsed += tCheckLockedBoxes_Elapsed;
            tCheckLockedBoxes.Interval = 250;

            // send details to UWP
            int[] lockedKeysStatuses = new int[] { Convert.ToInt32(test1), Convert.ToInt32(test2), Convert.ToInt32(test3) };
            Start(lockedKeysStatuses);
        }

        private async void Start(int[] lockedKeysStatuses)
        {
            await SendToUWPVoidAsync(lockedKeysStatuses);
        }

        private async void tCheckLockedBoxes_Elapsed(object sender, EventArgs e)
        {
            // init bool
            bool success = false;

            // update all bools
            test1 = checkBoxTest1.Checked;
            test2 = checkBoxTest2.Checked;
            test3 = checkBoxTest3.Checked;

            // build string from bool values
            int[] lockedKeysStatus = new int[] { Convert.ToInt32(test1), Convert.ToInt32(test2), Convert.ToInt32(test3) };

            // update UWP sibling
            success = await SendToUWPVoidAsync(lockedKeysStatus);

            // ask for new settngs
            if (success) await SendToUWPVoidAsync("request");
        }

        private async Task<bool> SendToUWPVoidAsync(object content)
        {
            ValueSet message = new ValueSet();
            if (content != "request") message.Add("content", content);
            else message.Add(content as string, "");

            #region SendToUWP

            // if connection isn't inited
            if (connection == null)
            {
                // init
                connection = new AppServiceConnection();
                connection.PackageFamilyName = Package.Current.Id.FamilyName;
                connection.AppServiceName = "UWPSibling";
                connection.ServiceClosed += Connection_ServiceClosed;
                //connection.RequestReceived += Connection_OnRequestReceived;

                // attempt connection 
                AppServiceConnectionStatus connectionStatus = await connection.OpenAsync();
            }

            AppServiceResponse serviceResponse = null;
            try
            {
                // send message
                serviceResponse = await connection.SendMessageAsync(message);
            }
            catch (Exception)
            {
                try
                {
                    // sleep for 1 sec
                    Thread.Sleep(1000);

                    // retry
                    serviceResponse = await connection.SendMessageAsync(message);
                }
                catch (Exception)
                {
                    // exit 
                    test1 = false;
                    test2 = false;
                    test3 = false;

                    Application.Exit();
                }
            }

            // get response
            if (serviceResponse.Message.ContainsKey("content"))
            {
                object newMessage = null;
                serviceResponse.Message.TryGetValue("content", out newMessage);
                // if message is an int[]
                if (newMessage is int[])
                {
                    // init field vars
                    int indexInArray = 0;
                    foreach (int trueorfalse in (int[])newMessage)
                    {
                        // set bool state based on index
                        switch (indexInArray)
                        {
                            case 0:
                                this.Invoke((MethodInvoker)delegate
                                {
                                    lblTest1.Text = (Convert.ToBoolean(trueorfalse)) ? "UWP Test 1: Is On" : "UWP Test 1: Is Off";
                                });
                                break;
                            case 1:
                                this.Invoke((MethodInvoker)delegate
                                {
                                    lblTest2.Text = (Convert.ToBoolean(trueorfalse)) ? "UWP Test 2: Is On" : "UWP Test 2: Is Off";
                                });
                                break;
                            case 2:
                                this.Invoke((MethodInvoker)delegate
                                {
                                    lblTest3.Text = (Convert.ToBoolean(trueorfalse)) ? "UWP Test 3: Is On" : "UWP Test 3: Is Off";
                                });
                                break;
                            default:
                                break;
                        }
                        indexInArray++;
                    }
                }
            }
            #endregion

            if (!tCheckLockedBoxes.Enabled) tCheckLockedBoxes.Start();
            return true;
        }

        void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            connection.ServiceClosed -= Connection_ServiceClosed;
            connection = null;

            // close
            Application.Exit();
        }
    }
}
