using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace WinForm
{
    public class CommunicationPath : ApplicationContext
    {
        AppServiceConnection connection = null;

        bool canUpdateAgain = true;
        bool Test1On = false;
        bool Test2On = false;
        bool Test3On = false;

        MainForm mainForm;

        public CommunicationPath()
        {
            //if (Debugger.IsAttached) Debugger.Break();

            // get current statuses of switches
            updateUWP(true, null, false, false, false);

            // launch MainForm
            mainForm = new MainForm(this);
            Application.Run(mainForm);
        }

        public async Task<bool> updateUWP(bool request, int[] statuses, bool _Test1On, bool _Test2On, bool _Test3On)
        {
            if (canUpdateAgain)
            {
                canUpdateAgain = false;
                ValueSet message = new ValueSet();

                if (request == false)
                {
                    message.Add("content", statuses);

                    if (connection == null)
                    {
                        // init
                        connection = new AppServiceConnection();
                        connection.AppServiceName = "UWPSibling";
                        connection.PackageFamilyName = Package.Current.Id.FamilyName;
                        connection.RequestReceived += Connection_RequestReceived;
                        connection.ServiceClosed += Connection_ServiceClosed;

                        // attempt connection 
                        AppServiceConnectionStatus connectionStatus = AppServiceConnectionStatus.Unknown;

                        try
                        {
                            connectionStatus = await connection.OpenAsync();
                        }
                        catch (InvalidOperationException)
                        {
                            return false;
                        }

                        // if UWP isn't running
                        if (connectionStatus == AppServiceConnectionStatus.AppUnavailable) { canUpdateAgain = true; return false; }
                    }

                    AppServiceResponse serviceResponse = await connection.SendMessageAsync(message);

                    // update local variables
                    Test1On = _Test1On;
                    Test2On = _Test2On;
                    Test3On = _Test3On;
                }
                else
                {
                    message.Add("request", null);

                    if (connection == null)
                    {
                        // init
                        connection = new AppServiceConnection();
                        connection.AppServiceName = "UWPSibling";
                        connection.PackageFamilyName = Package.Current.Id.FamilyName;
                        connection.RequestReceived += Connection_RequestReceived;
                        connection.ServiceClosed += Connection_ServiceClosed;

                        // attempt connection 
                        AppServiceConnectionStatus connectionStatus = await connection.OpenAsync();

                        // if UWP isn't running
                        if (connectionStatus == AppServiceConnectionStatus.AppUnavailable) { canUpdateAgain = true; return false; }
                    }

                    AppServiceResponse serviceResponse = await connection.SendMessageAsync(message);

                    // if UWP isn't running
                    if (serviceResponse.Status == AppServiceResponseStatus.Failure) return false;

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
                            bool test1On = false;
                            bool test2On = false;
                            bool test3On = false;

                            foreach (int trueorfalse in (int[])newMessage)
                            {
                                // set bool state based on index
                                switch (indexInArray)
                                {
                                    case 0:
                                        test1On = Convert.ToBoolean(trueorfalse);
                                        break;
                                    case 1:
                                        test2On = Convert.ToBoolean(trueorfalse);
                                        break;
                                    case 2:
                                        test3On = Convert.ToBoolean(trueorfalse);
                                        break;
                                    default:
                                        break;
                                }
                                indexInArray++;
                            }

                            mainForm.SetStatuses(test1On, test2On, test3On);

                            // update local variables
                            Test1On = _Test1On;
                            Test2On = _Test2On;
                            Test3On = _Test3On;
                        }
                    }
                }
            }
            canUpdateAgain = true;
            return true;
        }

        private void Connection_ServiceClosed(AppServiceConnection sender, AppServiceClosedEventArgs args)
        {
            // exit
            mainForm.Exit();
            Application.Exit();
        }

        public int[] GetStatuses() => new int[] { Convert.ToInt32(Test1On), Convert.ToInt32(Test2On), Convert.ToInt32(Test3On) };

        private void Connection_RequestReceived(AppServiceConnection sender, AppServiceRequestReceivedEventArgs args)
        {
            // read content
            if (args.Request.Message.ContainsKey("content"))
            {
                object message = null;
                args.Request.Message.TryGetValue("content", out message);

                // init field vars
                bool _Test1On = false;
                bool _Test2On = false;
                bool _Test3On = false;

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
                                _Test1On = Convert.ToBoolean(trueorfalse);
                                break;
                            case 1:
                                _Test2On = Convert.ToBoolean(trueorfalse);
                                break;
                            case 2:
                                _Test3On = Convert.ToBoolean(trueorfalse);
                                break;
                            default:
                                break;
                        }
                        indexInArray++;
                    }

                    mainForm.SetStatuses(_Test1On, _Test2On, _Test3On);

                    // update local variables
                    Test1On = _Test1On;
                    Test2On = _Test2On;
                    Test3On = _Test3On;
                }
            }
            else if (args.Request.Message.ContainsKey("request"))
            {
                updateUWP(false, GetStatuses(), Test1On, Test2On, Test3On);
            }
            else if (args.Request.Message.ContainsKey("exit"))
            {
                // exit
                mainForm.Exit();
                Application.Exit();
            }
        }
    }
}
