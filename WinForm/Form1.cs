using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Windows.ApplicationModel;
using Windows.ApplicationModel.AppService;
using Windows.Foundation.Collections;

namespace WinForm
{
    public partial class MainForm : Form
    {
        CommunicationPath commPath;

        bool test1 = false;
        bool test2 = false;
        bool test3 = false;

        System.Timers.Timer tCheckLockedBoxes = new System.Timers.Timer();

        public MainForm(CommunicationPath _commPath)
        {
            InitializeComponent();

            commPath = _commPath;
        }

        private void checkBoxTest1_CheckedChanged(object sender, EventArgs e)
        {
            // update UWP
            commPath.updateUWP(false, GetStatuses(), checkBoxTest1.Checked, checkBoxTest2.Checked, checkBoxTest3.Checked);
        }

        private void checkBoxTest2_CheckedChanged(object sender, EventArgs e)
        {
            // update UWP
            commPath.updateUWP(false, GetStatuses(), checkBoxTest1.Checked, checkBoxTest2.Checked, checkBoxTest3.Checked);
        }

        private void checkBoxTest3_CheckedChanged(object sender, EventArgs e)
        {
            // update UWP
            commPath.updateUWP(false, GetStatuses(), checkBoxTest1.Checked, checkBoxTest2.Checked, checkBoxTest3.Checked);
        }

        public void SetStatuses(bool Test1On, bool Test2On, bool Test3On)
        {
            this.Invoke((MethodInvoker)delegate
            {
                lblTest1.Text = (Test1On) ? "UWP Test 1: Is On" : "UWP Test 1: Is Off";
                lblTest2.Text = (Test2On) ? "UWP Test 2: Is On" : "UWP Test 2: Is Off";
                lblTest3.Text = (Test3On) ? "UWP Test 3: Is On" : "UWP Test 3: Is Off";
            });
        }

        public int[] GetStatuses() => new int[] { Convert.ToInt32(checkBoxTest1.Checked), Convert.ToInt32(checkBoxTest2.Checked),
            Convert.ToInt32(checkBoxTest3.Checked) };

        public void Exit()
        {
            // quit
            Application.Exit();
        }
    }
}
