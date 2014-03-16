using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Web;
using System.Text.RegularExpressions;

namespace fallyToast
{
    public partial class Notification : Form
    {
        private static List<Notification> openNotifications = new List<Notification>();
        private bool allowFocus = false;
        private ToastNotifications.FormAnimator animator;
        private IntPtr currentForegroundWindow;
        private string lurl = "";
        private string limage = "";

        /// <summary>
        /// 
        /// </summary>
        /// <param name="title"></param>
        /// <param name="body"></param>
        /// <param name="duration"></param>
        /// <param name="animation"></param>
        /// <param name="direction"></param>
        /// <param name="url"></param>
        public Notification(string title, string body, int duration, ToastNotifications.FormAnimator.AnimationMethod animation, ToastNotifications.FormAnimator.AnimationDirection direction, string url="", string image="", string type="")
        {
            this.Visible = false;
            InitializeComponent();
            if (duration < 0)
                duration = int.MaxValue;
            else
                duration = duration * 1000;

            this.lifeTimer.Interval = duration;
            this.labelTitle.Text = title;
            this.labelBody.Text = body;

            this.animator = new ToastNotifications.FormAnimator(this, animation, direction, 500);

            // fore color
            if (type == "error")
            {
                labelTitle.ForeColor = System.Drawing.Color.Gold;
                labelBody.ForeColor = System.Drawing.Color.Gold;
            }

            // twitter and facebook sharing
            if (url != "")
            {
                pictureBox1.Visible = true;
                pictureBox2.Visible = true;
                lurl = url;
            }
            
            // open picture
            if (image != "" && File.Exists(image))
            {
                if (url == "")
                    pictureBox3.Location = new Point(335, 2);
                pictureBox3.Visible = true;
                limage = image;
            }
        }

        #region Methods

        /// <summary>
        /// Displays the form
        /// </summary>
        /// <remarks>
        /// Required to allow the form to determine the current foreground window before being displayed
        /// </remarks>
        public new void Show()
        {
            // Determine the current foreground window so it can be reactivated each time this form tries to get the focus
            this.currentForegroundWindow = ToastNotifications.NativeMethods.GetForegroundWindow();

            base.Show();
        }

        #endregion // Methods

        #region Event Handlers

        private void Notification_Load(object sender, EventArgs e)
        {
            this.Visible = true;
            // Display the form just above the system tray.
            this.Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - this.Width-5,
                                      Screen.PrimaryScreen.WorkingArea.Height - this.Height-5);

            // Move each open form upwards to make room for this one
            foreach (Notification openForm in Notification.openNotifications)
            {
                openForm.Top -= this.Height+5;
            }

            Notification.openNotifications.Add(this);
            this.lifeTimer.Start();
        }

        private void Notification_Activated(object sender, EventArgs e)
        {
            // Prevent the form taking focus when it is initially shown
            if (!this.allowFocus)
            {
                // Activate the window that previously had focus
                ToastNotifications.NativeMethods.SetForegroundWindow(this.currentForegroundWindow);
            }
        }

        private void Notification_Shown(object sender, EventArgs e)
        {
            // Once the animation has completed the form can receive focus
            this.allowFocus = true;

            // Close the form by sliding down.
            this.animator.Duration = 0;
            this.animator.Direction = ToastNotifications.FormAnimator.AnimationDirection.Down;
        }

        private void Notification_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Move down any open forms above this one
            foreach (Notification openForm in Notification.openNotifications)
            {
                if (openForm == this)
                {
                    // Remaining forms are below this one
                    break;
                }
                openForm.Top += this.Height;
            }

            Notification.openNotifications.Remove(this);
        }

        private void lifeTimer_Tick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Notification_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void messageIcon_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelTitle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelTitle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelRO_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void labelUpdated_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void panelIconBackground_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion // Event Handlers
        // facebook
        private void pictureBox2_Click(object sender, EventArgs e)
        {
            string encode = HttpUtility.UrlEncode(lurl);
            Process.Start("https://www.facebook.com/sharer.php?u="+encode);
        }
        // twitter
        private void pictureBox1_Click(object sender, EventArgs e)
        {
            string encode = HttpUtility.UrlEncode(lurl);
            Process.Start("http://twitter.com/home?status="+encode);
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Process.Start(limage);
        }

        internal static string UrlEncode(string oldString)
        {
            if (oldString == null) return String.Empty;
            StringBuilder sb = new StringBuilder(oldString.Length * 2);
            Regex reg = new Regex("[a-zA-Z0-9$-_.+!*'(),]");

            foreach (char c in oldString)
            {
                if (reg.IsMatch(c.ToString()))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append(ToHex(c));
                }
            }
            return sb.ToString();
        }

        private static string ToHex(char c)
        {
            return ((int)c).ToString("X");
        }
    }
}
