using System;

using System.Windows.Forms;

namespace ImageFiltering
{
    public partial class BrightnessForm : Form
    {
        MainForm OwnersForm;
        public BrightnessForm(MainForm ownerForm)
        {   
            InitializeComponent();
            this.OwnersForm = ownerForm;
            this.FormClosing += new FormClosingEventHandler(Form2Closing);
            this.button1.Click += new System.EventHandler(this.button_Click);
            
            
        }

        //изменение яркости
        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            if (MainForm.imageName!="\0")
            {
                UInt32 pix;
                for (int i = 0; i < MainForm.image.Height; i++)
                    for (int j = 0; j < MainForm.image.Width; j++)
                    {
                        pix = Brightness.brightness(MainForm.pixels[i,j], trackBar1.Maximum, trackBar1.Value);
                        MainForm.FromOnePixelToBitmap(i, j, pix);
                    }

                FromBitmapToScreen();
            }
        }


        //сохранение изменений яркости или контрастности
        private void button_Click(object sender, EventArgs e)
        {
            if (MainForm.imageName != "\0")
            {
                for (int i = 0; i < MainForm.image.Height; i++)
                    for (int j = 0; j < MainForm.image.Width; j++)
                        MainForm.pixels[i, j] = (UInt32)(MainForm.image.GetPixel(j, i).ToArgb());
                trackBar1.Value = 0;
                
            }
        }
        //обновление изображения в Bitmap и pictureBox при закрытии окна
        private void Form2Closing(object sender, System.EventArgs e)
        {
            if (MainForm.imageName != "\0")
            {
                MainForm.FromPixelToBitmap();
                FromBitmapToScreen();
            }
        }

        //вывод изображения на экран
        void FromBitmapToScreen()
        {
            OwnersForm.FromBitmapToScreen();
        }
    }
}
