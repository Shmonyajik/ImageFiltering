using System;

namespace ImageFiltering
{
    struct RGB
    {
        public float R;
        public float G;
        public float B;
    }

    class Filter
    {
        public static UInt32[,] matrixFiltr(int height, int width,  UInt32[,] pixels,double[,] matryx, int n )
        {
            int a, b, k, c, space = (int)(n / 2);
            int tmpH = height + 2 * space;
            int tmpW = width + 2 * space;

            UInt32[,] tpixel = new UInt32[tmpH, tmpW];
            UInt32[,] cpixel = new UInt32[height, width];
            //крайние левая и правая 
            for (a = space; a < tmpH - space; a++)
                for (b = 0; b < space; b++)
                {
                    tpixel[a, b] = pixels[a - space, b];
                    tpixel[a, tmpW - 1 - b] = pixels[a - space, width - 1 - b];
                }
            //крайние верхняя и нижняя 
            for (a = 0; a < space; a++)
                for (b = space; b < tmpW - space; b++)
                {
                    tpixel[a, b] = pixels[a, b - space];
                    tpixel[tmpH - 1 - a, b] = pixels[height - 1 - a, b - space];
                }
            
            //углы
            for (a = 0; a < space; a++)
                for (b = 0; b < space; b++)
                {
                    tpixel[a, tmpW - 1 - b] = pixels[0, width - 1];
                    tpixel[tmpH - 1 - a, b] = pixels[height - 1, 0];
                    tpixel[a, b] = pixels[0, 0];                   
                    tpixel[tmpH - 1 - a, tmpW - 1 - b] = pixels[height - 1, width - 1];
                }
            //центр
            for (a = 0; a < height; a++)
                for (b = 0; b < width; b++)
                    tpixel[a + space, b + space] = pixels[a, b];
            
            
            
            //применение ядра свертки
            RGB ColorOfPixel = new RGB();

            RGB ColorOfCell= new RGB();
            
            
            for (a = space; a < tmpH - space; a++)
                for (b = space; b < tmpW - space; b++)
                {
                    ColorOfPixel.R = 0;
                    ColorOfPixel.G = 0;
                    ColorOfPixel.B = 0;
                    for (k = 0; k < n; k++)
                        for (c = 0; c < n; c++)
                        {
                            ColorOfCell = calculationOfColor(tpixel[a - space + k, b - space + c], matryx[k, c]);
                            ColorOfPixel.R += ColorOfCell.R;
                            ColorOfPixel.G += ColorOfCell.G;
                            ColorOfPixel.B += ColorOfCell.B;
                        }
                    //контролируем переполнение переменных
                    if (ColorOfPixel.R < 0) ColorOfPixel.R = 0;
                    if (ColorOfPixel.R > 255) ColorOfPixel.R = 255;
                    if (ColorOfPixel.G < 0) ColorOfPixel.G = 0;
                    if (ColorOfPixel.G > 255) ColorOfPixel.G = 255;
                    if (ColorOfPixel.B < 0) ColorOfPixel.B = 0;
                    if (ColorOfPixel.B > 255) ColorOfPixel.B = 255;

                    cpixel[a - space, b - space] = buildChanals(ColorOfPixel);
                }

            return cpixel;
        }

        //сборка каналов
        public static UInt32 buildChanals(RGB ColorOfPixel)
        {
            UInt32 Color;
            Color = 0xFF000000 | ((UInt32)ColorOfPixel.B) | ((UInt32)ColorOfPixel.R << 16) | ((UInt32)ColorOfPixel.G << 8);
            return Color;
        }
        //вычисление нового цвета
        public static RGB calculationOfColor(UInt32 pixels, double rate)
        {
            RGB сolor = new RGB();
            сolor.R = (float)(rate * ((pixels & 0x00FF0000) >> 16));
            сolor.G = (float)(rate * ((pixels & 0x0000FF00) >> 8));
            сolor.B = (float)(rate * (pixels & 0x000000FF));
            return сolor;
        }

        //размытие
        public const int N2 = 3;
        public static double[,] blur = new double[N1, N1] {{0.111, 0.111, 0.111},
                                                               {0.111, 0.111, 0.111},
                                                               {0.111, 0.111, 0.111}};
        //повышение резкости
        public const int N1 = 3;
        public static double[,] sharpness = new double[N1, N1] {{-1, -1, -1},
                                                               {-1,  9, -1},
                                                               {-1, -1, -1}};

    }
}