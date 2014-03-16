using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using fallyToast;

namespace fallyGrab
{
    public partial class cropperForm : Form
    {
        static public stillShot ssForm;

        public string ssfolder = "";
        private static Bitmap bmpScreenshot;
        private static Graphics gfxScreenshot;
        public string urlCrop = "";

        // set the thickness of the virtual form border that should catch the resizing 
        // if below 1, the resize function does not work!
        private int VIRTUALBORDER = 5;
        private bool SHOWVIRTUALBORDERS = true;

        // declare the limits for the form size
        private int MINHEIGHT = 10;
        private int MAXHEIGHT = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height;
        private int MINWIDTH = 20;
        private int MAXWIDTH = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width;


        private Point RESIZESTART;
        private Point RESIZEDESTINATION;
        private Point MOUSEPOS;

        // define Rectangles & Booleans for all 9 + 1 areas of the Form.
        private Rectangle R0;
        private Rectangle R1;
        private Rectangle R2;
        private Rectangle R3;
        private Rectangle R4;
        private Rectangle R5;
        private Rectangle R6;
        private Rectangle R7;
        private Rectangle R8;
        private Rectangle R9;


        // bool to determine if the form is being moved (True when the form is clicked in the center area (R5))
        private bool ISMOVING;

        // bool to determine if the form is being rezised (True when the form is clicked in all areas except the center (R5))
        private bool ISREZISING;

        // bool's to determine in which direction the form is moving
        private bool ISRESIZINGLEFT;
        private bool ISRESIZINGRIGHT;

        private bool ISRESIZINGTOP;
        private bool ISRESIZINGBOTTOM;

        private bool ISRESIZINGTOPRIGHT;
        private bool ISRESIZINGTOPLEFT;

        private bool ISRESIZINGBOTTOMRIGHT;
        private bool ISRESIZINGBOTTOMLEFT;
        mainForm ths;

        public cropperForm(mainForm frm)
        {
            InitializeComponent();
            // for smoother drawing of the form...
            DoubleBuffered = true;
            ths = frm;
        }

        private void cropper_Load(object sender, EventArgs e)
        {
            Bitmap picture = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height, PixelFormat.Format32bppRgb);
            ssForm = new stillShot();
            ssForm.picture = picture;
            Graphics gfxScreenshot = Graphics.FromImage(picture);
            gfxScreenshot.CopyFromScreen(Screen.PrimaryScreen.Bounds.X, Screen.PrimaryScreen.Bounds.Y, 0, 0, Screen.PrimaryScreen.Bounds.Size, CopyPixelOperation.SourceCopy);
            ssForm.Show();
            ssForm.Dispose();

            // garbage collector
            System.GC.Collect();
            System.GC.WaitForPendingFinalizers();

            // fix this to be always on top
            //this.Focus();
            this.Activate();

        }

        private void cropper_MouseMove(object sender, MouseEventArgs e)
        {
            RESIZEDESTINATION = PointToScreen(new Point(e.X, e.Y));
            R0 = Bounds;

            // if the form has captured the mouse...
            if (Capture)
            {
                if (ISMOVING == true)
                {
                    ISREZISING = false;
                    // ISMOVING is true if the R5 rectangle is pressed. Allow the form to be moved around the screen.
                    Location = new Point(MousePosition.X - MOUSEPOS.X, MousePosition.Y - MOUSEPOS.Y);
                }
                if (ISREZISING == true)
                {
                    ISMOVING = false;

                    if (ISRESIZINGTOPLEFT)
                    { Bounds = new Rectangle(R0.X + RESIZEDESTINATION.X - RESIZESTART.X, R0.Y + RESIZEDESTINATION.Y - RESIZESTART.Y, R0.Width - RESIZEDESTINATION.X + RESIZESTART.X, R0.Height - RESIZEDESTINATION.Y + RESIZESTART.Y); }
                    if (ISRESIZINGTOP)
                    { Bounds = new Rectangle(R0.X, R0.Y + RESIZEDESTINATION.Y - RESIZESTART.Y, R0.Width, R0.Height - RESIZEDESTINATION.Y + RESIZESTART.Y); }
                    if (ISRESIZINGTOPRIGHT)
                    { Bounds = new Rectangle(R0.X, R0.Y + RESIZEDESTINATION.Y - RESIZESTART.Y, R0.Width + RESIZEDESTINATION.X - RESIZESTART.X, R0.Height - RESIZEDESTINATION.Y + RESIZESTART.Y); }
                    if (ISRESIZINGLEFT)
                    { Bounds = new Rectangle(R0.X + RESIZEDESTINATION.X - RESIZESTART.X, R0.Y, R0.Width - RESIZEDESTINATION.X + RESIZESTART.X, R0.Height); }
                    if (ISRESIZINGRIGHT)
                    { Bounds = new Rectangle(R0.X, R0.Y, R0.Width + RESIZEDESTINATION.X - RESIZESTART.X, R0.Height); }
                    if (ISRESIZINGBOTTOMLEFT)
                    { Bounds = new Rectangle(R0.X + RESIZEDESTINATION.X - RESIZESTART.X, R0.Y, R0.Width - RESIZEDESTINATION.X + RESIZESTART.X, R0.Height + RESIZEDESTINATION.Y - RESIZESTART.Y); }
                    if (ISRESIZINGBOTTOM)
                    { Bounds = new Rectangle(R0.X, R0.Y, R0.Width, R0.Height + RESIZEDESTINATION.Y - RESIZESTART.Y); }
                    if (ISRESIZINGBOTTOMRIGHT)
                    { Bounds = new Rectangle(R0.X, R0.Y, R0.Width + RESIZEDESTINATION.X - RESIZESTART.X, R0.Height + RESIZEDESTINATION.Y - RESIZESTART.Y); }

                    RESIZESTART = RESIZEDESTINATION;
                    Invalidate();
                }
            }

            // if the form has not captured the mouse; the mouse is just hovering the form...
            else
            {
                MOUSEPOS = new Point(e.X, e.Y);

                // changes Cursor depending where the mousepointer is at the form...
                if (R1.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNWSE; }
                if (R2.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNS; }
                if (R3.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNESW; }
                if (R4.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeWE; }
                if (R5.Contains(MOUSEPOS))
                { Cursor = Cursors.Default; }
                if (R6.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeWE; }
                if (R7.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNESW; }
                if (R8.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNS; }
                if (R9.Contains(MOUSEPOS))
                { Cursor = Cursors.SizeNWSE; }
            }
        }

        private void cropper_MouseDown(object sender, MouseEventArgs e)
        {
            switch (e.Button)
            {
                case MouseButtons.Left:

                    if (R1.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGTOPLEFT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R2.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGTOP = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R3.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGTOPRIGHT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R4.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGLEFT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R5.Contains(MOUSEPOS))
                    {
                        // if the center area of the form is pressed (R5), then we should be able to move the form.
                        ISMOVING = true;
                        ISREZISING = false;
                        MOUSEPOS = new Point(e.X, e.Y);
                        Cursor = Cursors.SizeAll;
                    }
                    if (R6.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGRIGHT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R7.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGBOTTOMLEFT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R8.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGBOTTOM = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    if (R9.Contains(MOUSEPOS))
                    {
                        ISREZISING = true;
                        ISRESIZINGBOTTOMRIGHT = true;
                        RESIZESTART = PointToScreen(new Point(e.X, e.Y));
                    }
                    break;
            }
        }

        private void cropper_MouseUp(object sender, MouseEventArgs e)
        {
            ISMOVING = false;
            ISREZISING = false;

            ISRESIZINGLEFT = false;
            ISRESIZINGRIGHT = false;

            ISRESIZINGTOP = false;
            ISRESIZINGBOTTOM = false;

            ISRESIZINGTOPRIGHT = false;
            ISRESIZINGTOPLEFT = false;

            ISRESIZINGBOTTOMRIGHT = false;
            ISRESIZINGBOTTOMLEFT = false;

            Invalidate();
        }

        private void cropper_Paint(object sender, PaintEventArgs e)
        {
            #region DIVIDE THE FORM INTO 9 SUB AREAS
            R1 = new Rectangle(new Point(ClientRectangle.X, ClientRectangle.Y), new Size(VIRTUALBORDER, VIRTUALBORDER));
            R2 = new Rectangle(new Point(ClientRectangle.X + R1.Width, ClientRectangle.Y), new Size(ClientRectangle.Width - (R1.Width * 2), R1.Height));
            R3 = new Rectangle(new Point(ClientRectangle.X + R1.Width + R2.Width, ClientRectangle.Y), new Size(VIRTUALBORDER, VIRTUALBORDER));

            R4 = new Rectangle(new Point(ClientRectangle.X, ClientRectangle.Y + R1.Height), new Size(R1.Width, ClientRectangle.Height - (R1.Width * 2)));
            R5 = new Rectangle(new Point(ClientRectangle.X + R4.Width, ClientRectangle.Y + R1.Height), new Size(R2.Width, R4.Height));
            R6 = new Rectangle(new Point(ClientRectangle.X + R4.Width + R5.Width, ClientRectangle.Y + R1.Height), new Size(R3.Width, R4.Height));

            R7 = new Rectangle(new Point(ClientRectangle.X, ClientRectangle.Y + R1.Height + R4.Height), new Size(VIRTUALBORDER, VIRTUALBORDER));
            R8 = new Rectangle(new Point(ClientRectangle.X + R7.Width, ClientRectangle.Y + R1.Height + R4.Height), new Size(ClientRectangle.Width - (R7.Width * 2), R7.Height));
            R9 = new Rectangle(new Point(ClientRectangle.X + R7.Width + R8.Width, ClientRectangle.Y + R1.Height + R4.Height), new Size(VIRTUALBORDER, VIRTUALBORDER));
            #endregion

            #region SET FILL COLORS FOR THE VIRTUAL BORDER
            if (SHOWVIRTUALBORDERS)
            {
                Graphics GFX = e.Graphics;
                GFX.FillRectangle(Brushes.Azure, R5);

                GFX.FillRectangle(Brushes.Gold, R1);
                GFX.FillRectangle(Brushes.Gold, R3);
                GFX.FillRectangle(Brushes.Gold, R7);
                GFX.FillRectangle(Brushes.Gold, R9);

                GFX.FillRectangle(Brushes.Red, R2);
                GFX.FillRectangle(Brushes.Red, R8);
                GFX.FillRectangle(Brushes.Red, R4);
                GFX.FillRectangle(Brushes.Red, R6);
            }
            #endregion
        }

        private void cropper_Resize(object sender, EventArgs e)
        {
            //CODE TO LIMIT THE SIZE OF THE FORM
            if (Height > MAXHEIGHT)
            {
                Capture = false;
                Height = MAXHEIGHT;
            }
            if (Height < MINHEIGHT)
            {
                Capture = false;
                Height = MINHEIGHT;
            }
            if (Width > MAXWIDTH)
            {
                Capture = false;
                Width = MAXWIDTH;
            }
            if (Width < MINWIDTH)
            {
                Capture = false;
                Width = MINWIDTH;
            }
        }

        private void cropper_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                
                // check if save folder exists
                try
                {
                    // get file name
                    string file = commonFunctions.fileName();
                    // set the bitmap object to the size of the screen
                    ImageCodecInfo myImageCodecInfo;
                    System.Drawing.Imaging.Encoder myEncoder;
                    EncoderParameter myEncoderParameter;
                    EncoderParameters myEncoderParameters;
                    bmpScreenshot = new Bitmap(this.Size.Width - 2 * VIRTUALBORDER, this.Size.Height - 2 * VIRTUALBORDER, PixelFormat.Format32bppRgb);
                    // create a graphics object from the bitmap
                    gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                    // remove background
                    this.Opacity = 0;
                    // take the screenshot from the upper left corner to the right bottom corner
                    gfxScreenshot.CopyFromScreen(this.Location.X + VIRTUALBORDER, this.Location.Y + VIRTUALBORDER, 0, 0, new Size(this.Size.Width - 2 * VIRTUALBORDER, this.Size.Height - 2 * VIRTUALBORDER), CopyPixelOperation.SourceCopy);
                    // close stillshot
                    ssForm.Close();
                    this.Hide();
                    // save the screenshot to the specified path that the user has chosen
                    if (fallyGrab.Properties.Settings.Default.imageFormat == "JPG")
                        myImageCodecInfo = GetEncoderInfo("image/jpeg");
                    else
                        myImageCodecInfo = GetEncoderInfo("image/png");
                    myEncoder = System.Drawing.Imaging.Encoder.Quality;
                    myEncoderParameters = new EncoderParameters(1);
                    myEncoderParameter = new EncoderParameter(myEncoder, Convert.ToInt64(fallyGrab.Properties.Settings.Default.quality));
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    bmpScreenshot.Save(ssfolder + "\\fallyGrab-" + file, myImageCodecInfo, myEncoderParameters);
                    // add link to history if returned
                    urlCrop = file;
                    // close form
                    this.Dispose();
                    this.Close();
                }
                catch (Exception ex)
                {
                    fallyToast.Toaster alertformfolder = new fallyToast.Toaster();
                    alertformfolder.Show("fallyGrab", "Error: " + ex.Message, -1, "Fade", "Up", "", "", "error");
                    commonFunctions.writeLog(ex.Message, ex.StackTrace);
                    // garbage collector
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                }
            }
            else if (e.KeyChar == (char)Keys.Escape)
            {
                // close stillshot
                ssForm.Close();
                // close form
                this.Close();
                this.Dispose();
            }
        }

        public void Dispose()
        {
            System.GC.SuppressFinalize(this);
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void cropper_DoubleClick(object sender, EventArgs e)
        {
            // check if save folder exists
            try
            {
                // get file name
                string file = commonFunctions.fileName();
                // set the bitmap object to the size of the screen
                ImageCodecInfo myImageCodecInfo;
                System.Drawing.Imaging.Encoder myEncoder;
                EncoderParameter myEncoderParameter;
                EncoderParameters myEncoderParameters;
                bmpScreenshot = new Bitmap(this.Size.Width - 2 * VIRTUALBORDER, this.Size.Height - 2 * VIRTUALBORDER, PixelFormat.Format32bppRgb);
                // create a graphics object from the bitmap
                gfxScreenshot = Graphics.FromImage(bmpScreenshot);
                // remove background
                this.Opacity = 0;
                // Take the screenshot from the upper left corner to the right bottom corner
                gfxScreenshot.CopyFromScreen(this.Location.X + VIRTUALBORDER, this.Location.Y + VIRTUALBORDER, 0, 0, new Size(this.Size.Width - 2 * VIRTUALBORDER, this.Size.Height - 2 * VIRTUALBORDER), CopyPixelOperation.SourceCopy);
                // close stillshot
                ssForm.Close();
                this.Hide();
                // save the screenshot to the specified path that the user has chosen
                if (fallyGrab.Properties.Settings.Default.imageFormat == "JPG")
                    myImageCodecInfo = GetEncoderInfo("image/jpeg");
                else
                    myImageCodecInfo = GetEncoderInfo("image/png");
                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                myEncoderParameters = new EncoderParameters(1);
                myEncoderParameter = new EncoderParameter(myEncoder, Convert.ToInt64(fallyGrab.Properties.Settings.Default.quality));
                myEncoderParameters.Param[0] = myEncoderParameter;
                bmpScreenshot.Save(ssfolder + "\\fallyGrab-" + file, myImageCodecInfo, myEncoderParameters);
                // add link to history if returned
                urlCrop = file;
                // close form
                this.Dispose();
                this.Close();
            }
            catch (Exception ex)
            {
                fallyToast.Toaster alertformfolder = new fallyToast.Toaster();
                alertformfolder.Show("fallyGrab", "Error: " + ex.Message, -1, "Fade", "Up", "", "", "error");
                commonFunctions.writeLog(ex.Message, ex.StackTrace);
                // garbage collector
                System.GC.Collect();
                System.GC.WaitForPendingFinalizers();
            }
        }

        private void cropper_Deactivate(object sender, EventArgs e)
        {
            // bring back focus
            this.TopMost = false;
            ssForm.TopMost = false;
            this.TopMost = true;
            this.Activate();
        }
    }
}
