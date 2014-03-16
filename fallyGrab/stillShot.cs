using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace fallyGrab
{
    public partial class stillShot : Form
    {
        public Bitmap picture;
        public stillShot()
        {
            InitializeComponent();
        }

        private void stillShot_Load(object sender, EventArgs e)
        {
            // set the bitmap object to the size of the screen
            this.BackgroundImage = picture;
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }
    }
}
