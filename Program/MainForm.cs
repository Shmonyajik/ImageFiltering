using System;

using System.Drawing;

using System.Windows.Forms;

namespace ImageFiltering
{
    public partial class MainForm : Form
    {
        public static Bitmap image;
        public static string imageName = "\0";
        public static UInt32[,] pixels;
        public MainForm()
        {
            InitializeComponent();
            
        }

        

        //открытие 
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog open_dialog = new OpenFileDialog();
            open_dialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";

            if (open_dialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    imageName = open_dialog.FileName;
                    image = new Bitmap(open_dialog.FileName);

                    this.Height = image.Height + 80;
                    this.Width = image.Width + 45;
                    
                    this.pictureBox1.Size = image.Size;
                    pictureBox1.Image = image;
                    pictureBox1.Invalidate(); 
                    //
                    pixels = new UInt32[image.Height, image.Width];
                    for (int x = 0; x < image.Width; x++)
                     for (int y = 0; y < image.Height; y++)
                        pixels[y,x] = (UInt32)(image.GetPixel(x, y).ToArgb());
                            
                }
                catch
                {
                    
                    DialogResult rezult = MessageBox.Show("Открытие выбранного файла невозможно","Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    imageName = "\0";
                }
            }
        }

        //сохранение 
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null)
            {
                
                SaveFileDialog savedialog = new SaveFileDialog();
               
                savedialog.CheckPathExists = true;
                savedialog.OverwritePrompt = true;
                 savedialog.Title = "Сохранить картинку как";
                savedialog.ShowHelp = true;
                savedialog.Filter = "Image Files(*.BMP)|*.BMP|Image Files(*.JPG)|*.JPG|Image Files(*.GIF)|*.GIF|Image Files(*.PNG)|*.PNG|All files (*.*)|*.*";
                
                if (savedialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        image.Save(savedialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch
                    {
                        MessageBox.Show("Сохранить изображение невозможно", "Ошибка",MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }  

        //размыть
        private void размытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainForm.imageName != "\0")
            {
                pixels = Filter.matrixFiltr(image.Height, image.Width,  pixels, Filter.blur,Filter.N2 );
                FromPixelToBitmap();
                FromBitmapToScreen();
            }
        }
        //Повышение резкости
        private void повыситьРезкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MainForm.imageName != "\0")
            {
                pixels = Filter.matrixFiltr(image.Height,image.Width,  pixels, Filter.sharpness ,Filter.N1 );
                FromPixelToBitmap();
                FromBitmapToScreen();
            }
        }

        //преобразование из UINT32 to Bitmap
        public static void FromPixelToBitmap()
        {
            for (int x = 0; x < image.Width; x++)
              for (int y = 0; y < image.Height; y++)               
                    image.SetPixel(x, y, Color.FromArgb((int)pixels[y, x]));
        }

        //преобразование UINT32 в Bitmap по пикселю
        public static void FromOnePixelToBitmap(int x, int y, UInt32 pixels)
        {
            image.SetPixel(y, x, Color.FromArgb((int)pixels));
        }
        private void яркостьконтрастностьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BrightnessForm BrightnessForm = new BrightnessForm(this);
            BrightnessForm.ShowDialog(); 
        }
        //вывод на экран
        public void FromBitmapToScreen()
        {
            pictureBox1.Image = image;
        }

        private void оПрограммеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Разработчик:\n Наугольный Денис Игоревич\n Написано на языке C# с использование Windows Forms. " +
                "Приложение, позволяющее открывать изображения из графических файлов, изменять яркость изображений, создавать размытие изображения (Box Blur)," +
                " сохранять измененные изображения в графические файлы.", "О программе");
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        
    }
}
