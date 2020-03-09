using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Imaging;

namespace CG_Project1
{
    public static class Filters
    {
        public static byte Clamp(int min, int max, float val)
        {
            if (val > max)
                val = max;
            if (val < min)
                val = min;
            return (byte)val;
        }

        public static byte SlidingWindow(int[,] kernel, byte[] src, byte[] dest, int divisor)
        {
            for(int i = 0; i < kernel.GetLength(0); i++)
            {
                for(int j = 0; j < kernel.GetLength(1); j++)
                {

                }
            }
            return 1;
        }
        
        public static void Inversion(this PictureBox pctrBox)
        {
            Bitmap bm = new Bitmap(pctrBox.Image);
            Point[] points =
            {
                new Point(0, 0),
                new Point(bm.Width, 0),
                new Point(0, bm.Height),
            };
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            IntPtr ptr = bmData.Scan0;
            int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
            int bytes = Math.Abs(bmData.Stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < bytes; counter+= bitsPerPixel/8)
            {
                rgbValues[counter] = (byte)(255 - rgbValues[counter]);
                rgbValues[counter+1] = (byte)(255 - rgbValues[counter+1]);
                rgbValues[counter+2] = (byte)(255 - rgbValues[counter+2]);
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            pctrBox.Image = bm;
            
            
        }

        public static void BrightnessCorrection(this PictureBox pctrBox, int brightness)
        {
            Bitmap bm = new Bitmap(pctrBox.Image);
            Point[] points =
            {
                new Point(0, 0),
                new Point(bm.Width, 0),
                new Point(0, bm.Height),
            };
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            IntPtr ptr = bmData.Scan0;
            int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
            int bytes = Math.Abs(bmData.Stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < bytes; counter += bitsPerPixel / 8)
            {

                rgbValues[counter] = Clamp(0, 255, rgbValues[counter] + brightness);
                rgbValues[counter + 1] = Clamp(0, 255, rgbValues[counter + 1] + brightness);
                rgbValues[counter + 2] = Clamp(0, 255, rgbValues[counter + 2] + brightness);
               
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            pctrBox.Image = bm;

        }

        public static void ContrastEnhancement(this PictureBox pctrBox, float contrast)
        {
            Bitmap bm = new Bitmap(pctrBox.Image);
            Point[] points =
            {
                new Point(0, 0),
                new Point(bm.Width, 0),
                new Point(0, bm.Height),
            };
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            IntPtr ptr = bmData.Scan0;
            int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
            int bytes = Math.Abs(bmData.Stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < bytes; counter += bitsPerPixel / 8)
            {
                rgbValues[counter] = Clamp(0, 255, (rgbValues[counter] - 128) * contrast + 128);
                rgbValues[counter + 1] = Clamp(0, 255, (rgbValues[counter + 1] - 128) * contrast + 128);
                rgbValues[counter + 2] = Clamp(0, 255, (rgbValues[counter + 2] - 128)* contrast + 128);
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            pctrBox.Image = bm;

        }

        public static void GammaCorrection(this PictureBox pctrBox, float gamma)
        {
            
            Bitmap bm = new Bitmap(pctrBox.Image);
            Point[] points =
            {
                new Point(0, 0),
                new Point(bm.Width, 0),
                new Point(0, bm.Height),
            };
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            IntPtr ptr = bmData.Scan0;
            int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
            int bytes = Math.Abs(bmData.Stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < bytes; counter += bitsPerPixel / 8)
            {
                rgbValues[counter] = (byte)(255 * Math.Pow((rgbValues[counter] / (double)255), gamma));
                rgbValues[counter+1] = (byte)(255 * Math.Pow((rgbValues[counter+1] / (double)255), gamma));
                rgbValues[counter+2] = (byte)(255 * Math.Pow((rgbValues[counter+2] / (double)255), gamma));
            }
            System.Runtime.InteropServices.Marshal.Copy(rgbValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            pctrBox.Image = bm;
        }

        //public static void ConvFilter(this PictureBox pctrBox, int[,] kernel)
        //{
        //    int R, G, B;
        //    R = G = B = 0;
        //    int avgR, avgG, avgB;
           
            

        //    int divisor = kernel.Cast<int>().Sum();
        //    if (divisor == 0)
        //        divisor = 1;
        //    Bitmap bm = new Bitmap(pctrBox.Image);

        //    int top = (kernel.GetLength(1) / 2) * bm.Width * 4;
        //    int bot = (bm.Height - (kernel.GetLength(1) / 2)) * bm.Width * 4;
        //    int left = (kernel.GetLength(0) / 2) * 4 - 4;
        //    int right = (bm.Width - (kernel.GetLength(0) / 2)) * 4;
        //    Point[] points =
        //    {
        //        new Point(0, 0),
        //        new Point(bm.Width, 0),
        //        new Point(0, bm.Height),
        //    };
        //    Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
        //    BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
        //    IntPtr ptr = bmData.Scan0;
        //    int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
        //    int bytes = Math.Abs(bmData.Stride) * bm.Height;
        //    byte[] rgbValues = new byte[bytes];
        //    byte[] resultValues = rgbValues;
        //    System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);

        //    for (int counter = 0; counter < bytes; counter += bitsPerPixel / 8)
        //    {
        //        for(int y = -kernel.GetLength(1)/2; y <= kernel.GetLength(1)/2; y++)
        //        {
        //            for(int x = -kernel.GetLength(0)/2; x <= kernel.GetLength(0)/2; x++)
        //            {
        //                if((counter < top) & (counter % (4*bm.Width) <= left))
        //                {
        //                    if (x < 0 & y < 0)
        //                    {
        //                        R += rgbValues[counter    ];
        //                        G += rgbValues[counter + 1];
        //                        B += rgbValues[counter + 2];
        //                    }
        //                    else if (x >= 0 & y < 0)
        //                    {
        //                        R += rgbValues[counter     + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else if (x < 0 & y >= 0)
        //                    {
        //                        R += rgbValues[counter     + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else if (x >= 0 & y >= 0)
        //                    {
        //                        R += rgbValues[counter     + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                }

        //                else if ((counter < top) & (counter % (4*bm.Width) >= right))
        //                {
        //                    if (x <= 0 & y >= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                    else if (x > 0 & y >= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else if (x <= 0 & y < 0)
        //                    {

        //                        R += rgbValues[counter + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else if (x > 0 & y < 0)
        //                    {

        //                        R += rgbValues[counter];
        //                        G += rgbValues[counter + 1];
        //                        B += rgbValues[counter + 2];
        //                    }
        //                }

        //                else if ((counter < top) & !(counter % (4*bm.Width) >= right) & !(counter % (4*bm.Width) <= left))
        //                {
        //                    if (y < 0)
        //                    {
        //                        R += rgbValues[counter + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                }
        //                else if ((counter % (4*bm.Width) <= left) & !(counter >= bot) & !(counter < top))
        //                {
        //                    if (x < 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                }
        //                else if ((counter % (4*bm.Width) >= left) & !(counter >= bot) & !(counter < top))
        //                {
        //                    if (x > 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                }

        //                else if ((counter >= bot) & (counter % (4*bm.Width) <= left))
        //                {
        //                    if (x >= 0 & y <= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                    else if (x < 0 & y <= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else if (x >= 0 & y > 0)
        //                    {

        //                        R += rgbValues[counter + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else if (x < 0 & y > 0)
        //                    {

        //                        R += rgbValues[counter];
        //                        G += rgbValues[counter + 1];
        //                        B += rgbValues[counter + 2];
        //                    }
        //                }

        //                else if ((counter >= bot) & (counter % (4*bm.Width) >= right))
        //                {
        //                    if (x <= 0 & y <= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                    else if (x > 0 & y <= 0)
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width) * 4];
        //                    }
        //                    else if (x <= 0 & y > 0)
        //                    {

        //                        R += rgbValues[counter + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else if (x > 0 & y > 0)
        //                    {

        //                        R += rgbValues[counter];
        //                        G += rgbValues[counter + 1];
        //                        B += rgbValues[counter + 2];
        //                    }
        //                }

        //                else if ((counter >= bot) & !(counter % (4*bm.Width) >= right) & !(counter % (4*bm.Width) <= left))
        //                {
        //                    if (y > 0)
        //                    {
        //                        R += rgbValues[counter + (x) * 4];
        //                        G += rgbValues[counter + 1 + (x) * 4];
        //                        B += rgbValues[counter + 2 + (x) * 4];
        //                    }
        //                    else
        //                    {
        //                        R += rgbValues[counter + (y * bm.Width + x) * 4];
        //                        G += rgbValues[counter + 1 + (y * bm.Width + x) * 4];
        //                        B += rgbValues[counter + 2 + (y * bm.Width + x) * 4];
        //                    }
        //                }


        //            }
        //        }
        //        avgR = R / divisor;
        //        avgG = G / divisor;
        //        avgB = B / divisor;
        //        resultValues[counter    ] = (byte)(avgR);
        //        resultValues[counter + 1] = (byte)(avgG);
        //        resultValues[counter + 2] = (byte)(avgB);
                
        //        R = G = B = 0;

        //    }
        //    System.Runtime.InteropServices.Marshal.Copy(resultValues, 0, ptr, bytes);
        //    bm.UnlockBits(bmData);
        //    pctrBox.Image = bm;
        //}

        public static void ApplyFilter(this PictureBox pctrBox, int[,] kernel, int offset)
        {
            int R, G, B;
            R = G = B = 0;
            int divisor = kernel.Cast<int>().Sum();
            if (divisor == 0)
                divisor = 1;
            Bitmap bm = new Bitmap(pctrBox.Image);
            Rectangle rect = new Rectangle(0, 0, bm.Width, bm.Height);
            BitmapData bmData = bm.LockBits(rect, ImageLockMode.ReadWrite, bm.PixelFormat);
            IntPtr ptr = bmData.Scan0;
            int bitsPerPixel = Image.GetPixelFormatSize(bm.PixelFormat);
            int bytes = Math.Abs(bmData.Stride) * bm.Height;
            byte[] rgbValues = new byte[bytes];
            byte[] resultValues = new byte[bytes]; ;
            System.Runtime.InteropServices.Marshal.Copy(ptr, rgbValues, 0, bytes);
            for (int counter = 0; counter < bytes; counter += 4)
            {
                R = G = B = 0;
                for (int y = -kernel.GetLength(1) / 2; y <= kernel.GetLength(1) / 2; y++)
                {
                    for (int x = -kernel.GetLength(0) / 2; x <= kernel.GetLength(0) / 2; x++)
                    {
                        int sourceX = counter % (bm.Width * 4) + x * 4;
                        int sourceY = ((int)counter / ((int)bm.Width*4)) * 4 + y * 4;
                        if (sourceX < 0)
                            sourceX = 0;
                        if (sourceX >= bm.Width * 4)
                            sourceX = (bm.Width - 1) * 4;
                        if (sourceY < 0)
                            sourceY = 0;
                        if (sourceY >= bm.Height * 4)
                            sourceY = (bm.Height - 1) * 4;
                        R += (rgbValues[sourceX + sourceY * bm.Width] * kernel[y + (kernel.GetLength(1)/2), x + (kernel.GetLength(0)/2)]); 
                        G += (rgbValues[sourceX + sourceY * bm.Width + 1] * kernel[y + (kernel.GetLength(1)/2), x + (kernel.GetLength(0)/2)]);
                        B += (rgbValues[sourceX + sourceY * bm.Width + 2] * kernel[y + (kernel.GetLength(1)/2), x + (kernel.GetLength(0)/2)]);
                    }
                }
                resultValues[counter] = Clamp(0, 255, (R / divisor) + offset);
                resultValues[counter + 1] = Clamp(0, 255, (G / divisor) + offset);
                resultValues[counter + 2] = Clamp(0, 255, (B / divisor) + offset);
                resultValues[counter + 3] = 255;
                

            }
            System.Runtime.InteropServices.Marshal.Copy(resultValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            pctrBox.Image = bm;

        }

        
    }
}
