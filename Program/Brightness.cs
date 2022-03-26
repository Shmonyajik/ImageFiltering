using System;

namespace ImageFiltering
{
    class Brightness
    {
        //якрость
        public static UInt32 brightness (UInt32 dot, int lenght,int pozition)
        {
            int R;
            int G;
            int B;

            int N = (100 / lenght) * pozition; //кол-во процентов

            R = (int)(((dot & 0x00FF0000) >> 16) + N * 128 / 100);
            G = (int)(((dot & 0x0000FF00) >> 8) + N * 128 / 100);
            B = (int)((dot & 0x000000FF) + N * 128 / 100);

            //контролируем переполнение переменных
            if (R > 255) R = 255;
            if (R < 0) R = 0;
            if (G > 255) G = 255;
            if (G < 0) G = 0;
            if (B > 255) B = 255;
            if (B < 0) B = 0;
            

            dot = 0xFF000000| ((UInt32)B) | ((UInt32)R << 16) | ((UInt32)G << 8);

            return dot;
        }
 
    }
}