using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SQLite;
using Microsoft.VisualBasic;
using System.Diagnostics;

namespace fallyGrab
{
    public partial class historyManagerForm : Form
    {
        public historyManagerForm()
        {
            InitializeComponent();
        }

        private void historyManager_Load(object sender, EventArgs e)
        {
            // set control sizes
            splitContainer1.SplitterDistance = Properties.Settings.Default.hsplitterdistance;
            columnHeader3.Width = Properties.Settings.Default.hcol1size;
            columnHeader1.Width = Properties.Settings.Default.hcol2size;
            columnHeader2.Width = Properties.Settings.Default.hcol3size;
            this.Size = new Size(Properties.Settings.Default.hformwidth, Properties.Settings.Default.hformheight);
            if (Properties.Settings.Default.hformx!=0 && Properties.Settings.Default.hformy!=0)
                this.Location = new Point(Properties.Settings.Default.hformx, Properties.Settings.Default.hformy);
            this.BringToFront();
            this.Focus();
            // load items
            loadItems();
        }

        private void clickItem(string link)
        {
            try
            {
                webBrowser1.Navigate(link);
            }
            catch (Exception webex) {
                fallyToast.Toaster alertdb = new fallyToast.Toaster();
                alertdb.Show("fallyGrab", webex.Message, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(webex.Message, webex.StackTrace);
            }
            toolStripStatusLabel2.Text = link;
        }

        private void loadItems(string where="")
        {
            listView1.Items.Clear();
            toolStripButton1.Enabled = false;
            toolStripButton2.Enabled = false;
            toolStripStatusLabel2.Text = "";
            webBrowser1.Navigate("");
            try
            {
                SQLiteDatabase db = new SQLiteDatabase();
                String query;
                if (where!="")
                    query = "select * from `history` WHERE "+where+" ORDER BY `date` DESC;";
                else query = "select * from `history` ORDER BY `date` DESC;";
                DataTable history = db.GetDataTable(query);
                foreach (DataRow r in history.Rows)
                {
                    ListViewItem item = new ListViewItem();
                    item.Text = r["label"].ToString();
                    item.SubItems.Add(r["link"].ToString());
                    item.SubItems.Add(r["date"].ToString());
                    item.SubItems.Add(r["id"].ToString());

                    listView1.Items.Add(item);
                }
                toolStripStatusLabel1.Text = history.Rows.Count.ToString() + " item(s) in history";
            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                fallyToast.Toaster alertdb = new fallyToast.Toaster();
                alertdb.Show("fallyGrab", error, -1, "Fade", "Up", "", "", "error");
                toolStripStatusLabel1.Text = "0 item(s) in history";
                commonFunctions.writeLog(fail.Message, fail.StackTrace);
            }
        }

        private void listView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            ListViewItem item = listView1.SelectedItems[0];
            clickItem(item.SubItems[1].Text);
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            // reload
            loadItems();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            // check if there is a selection
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    SQLiteDatabase db = new SQLiteDatabase();
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        ListViewItem item = listView1.SelectedItems[i];
                        db.Delete("history", String.Format("id = {0}", item.SubItems[3].Text));
                    }
                    // reload
                    loadItems();
                }
                catch (Exception fail)
                {
                    String error = "The following error has occurred:\n\n";
                    error += fail.Message.ToString() + "\n\n";
                    fallyToast.Toaster alertdb = new fallyToast.Toaster();
                    alertdb.Show("fallyGrab", error, -1, "Fade", "Up", "", "", "error");
                    commonFunctions.writeLog(fail.Message, fail.StackTrace);
                }
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // check if there is a selection
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string x = Interaction.InputBox("Type a label for the screenshot", "Label", item.Text);
                if (x.Trim() != "")
                {
                    SQLiteDatabase db = new SQLiteDatabase();
                    Dictionary<String, String> data = new Dictionary<String, String>();
                    data.Add("label", x);
                    try
                    {
                        db.Update("history", data, String.Format("history.id = {0}", item.SubItems[3].Text));
                        // reload
                        loadItems();
                    }
                    catch (Exception crap)
                    {
                        fallyToast.Toaster alertdb = new fallyToast.Toaster();
                        alertdb.Show("fallyGrab", crap.Message, -1, "Fade", "Up", "", "", "error");
                        commonFunctions.writeLog(crap.Message, crap.StackTrace);
                    }
                }
            }
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // check if there is a selection
            if (listView1.SelectedItems.Count > 0)
            {
                toolStripButton1.Enabled = true;
                toolStripButton2.Enabled = true;
            }
            else
            {
                toolStripButton1.Enabled = false;
                toolStripButton2.Enabled = false;
            }
        }

        private void toolStripTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (toolStripTextBox1.Text.Trim() != "") loadItems("`link` LIKE '%" + toolStripTextBox1.Text.Replace("'", "\"") + "%' OR `label` LIKE '%" + toolStripTextBox1.Text.Replace("'", "\"") + "%' OR `date` LIKE '%" + toolStripTextBox1.Text.Replace("'", "\"") + "%'");
                else loadItems();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure you want to clear your history?", "fallyGrab", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    SQLiteDatabase db = new SQLiteDatabase();
                    db.Delete("history", "1=1");
                    // reload
                    loadItems();
                }
            }
            catch (Exception fail)
            {
                String error = "The following error has occurred:\n\n";
                error += fail.Message.ToString() + "\n\n";
                fallyToast.Toaster alertdb = new fallyToast.Toaster();
                alertdb.Show("fallyGrab", error, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(fail.Message, fail.StackTrace);
            }
        }

        private void listView1_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            if (e.Label == null)
                return;
            ListViewItem item = listView1.Items[e.Item];
            SQLiteDatabase db = new SQLiteDatabase();
            Dictionary<String, String> data = new Dictionary<String, String>();
            data.Add("label", e.Label.ToString());
            try
            {
                db.Update("history", data, String.Format("history.id = {0}", item.SubItems[3].Text));
                // reload
                loadItems();
            }
            catch (Exception crap)
            {
                fallyToast.Toaster alertdb = new fallyToast.Toaster();
                alertdb.Show("fallyGrab", crap.Message, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(crap.Message, crap.StackTrace);
            }
        }

        private void historyManager_FormClosing(object sender, FormClosingEventArgs e)
        {
            // save control sizes to settings
            Properties.Settings.Default.hformwidth = this.Width;
            Properties.Settings.Default.hformheight = this.Height;
            Properties.Settings.Default.hsplitterdistance = splitContainer1.SplitterDistance;
            Properties.Settings.Default.hcol1size = columnHeader3.Width;
            Properties.Settings.Default.hcol2size = columnHeader1.Width;
            Properties.Settings.Default.hcol3size = columnHeader2.Width;
            Properties.Settings.Default.hformx = this.Location.X;
            Properties.Settings.Default.hformy = this.Location.Y;
            Properties.Settings.Default.Save();
        }

        private void listView1_KeyPress(object sender, KeyPressEventArgs e)
        {
          
        }

        private void listView1_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Control && e.KeyCode == Keys.C && listView1.SelectedItems.Count > 0)
                {
                    ListViewItem item = listView1.SelectedItems[0];
                    System.Windows.Forms.Clipboard.SetText(item.SubItems[1].Text);
                    toolStripStatusLabel1.Text = "The URL has been copied to your clipboard";
                    timer1.Enabled = true;
                }
            }
            catch (Exception crap)
            {
                fallyToast.Toaster alertdb = new fallyToast.Toaster();
                alertdb.Show("fallyGrab", crap.Message, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(crap.Message, crap.StackTrace);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = listView1.Items.Count.ToString() + " item(s) in history";
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                clickItem(item.SubItems[1].Text);
            }
        }

        private void openInBrowserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                Process.Start(item.SubItems[1].Text);
            }
        }

        private void copyURLToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                System.Windows.Forms.Clipboard.SetText(item.SubItems[1].Text);
                toolStripStatusLabel1.Text = "The URL has been copied to your clipboard";
                timer1.Enabled = true;
            }
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if there is a selection
            if (listView1.SelectedItems.Count > 0)
            {
                ListViewItem item = listView1.SelectedItems[0];
                string x = Interaction.InputBox("Type a label for the screenshot", "Label", item.Text);
                if (x.Trim() != "")
                {
                    SQLiteDatabase db = new SQLiteDatabase();
                    Dictionary<String, String> data = new Dictionary<String, String>();
                    data.Add("label", x);
                    try
                    {
                        db.Update("history", data, String.Format("history.id = {0}", item.SubItems[3].Text));
                        // reload
                        loadItems();
                    }
                    catch (Exception crap)
                    {
                        fallyToast.Toaster alertdb = new fallyToast.Toaster();
                        alertdb.Show("fallyGrab", crap.Message, -1, "Fade", "Up", "", "", "error");
                        commonFunctions.writeLog(crap.Message, crap.StackTrace);
                    }
                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // check if there is a selection
            if (listView1.SelectedItems.Count > 0)
            {
                try
                {
                    SQLiteDatabase db = new SQLiteDatabase();
                    for (int i = 0; i < listView1.SelectedItems.Count; i++)
                    {
                        ListViewItem item = listView1.SelectedItems[i];
                        db.Delete("history", String.Format("id = {0}", item.SubItems[3].Text));
                    }
                    // reload
                    loadItems();
                }
                catch (Exception fail)
                {
                    String error = "The following error has occurred:\n\n";
                    error += fail.Message.ToString() + "\n\n";
                    fallyToast.Toaster alertdb = new fallyToast.Toaster();
                    alertdb.Show("fallyGrab", error, -1, "Fade", "Up", "", "", "error");
                    commonFunctions.writeLog(fail.Message, fail.StackTrace);
                }
            }
        }
    }
}
