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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private Image org_img;
        //constants
        float gamma = 1.5f;
        int brightness = 30;
        float contrast = 1.8f;
        int[,] boxblur =
        {
            {1,1,1 },
            {1,1,1 },
            {1,1,1 }
        };
        int[,] gaussblur =
        {
            {1,2,1 },
            {2,4,2 },
            {1,2,1 }
        };
        int[,] sharpen =
        {
            {-1,-1,-1 },
            {-1,9,-1 },
            {-1,-1,-1 }
        };
        int[,] edge =
        {
            {0,0,0 },
            {-1,1,0 },
            {0,0,0 }
        };
        int[,] emboss =
        {
            {-1,0,1 },
            {-1,1,1 },
            {-1,0,1 }
        };

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog();
            open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;)|*.jpg; *.jpeg; *.gif; *.bmp;";
            if(open.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.Image = new Bitmap(open.FileName);
                org_img = pictureBox1.Image;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog save = new SaveFileDialog();
            save.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp;)|*.jpg; *.jpeg; *.gif; *.bmp;";
            if(save.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = (FileStream)save.OpenFile();
                switch(save.FilterIndex)
                {
                    case 1:
                        pictureBox1.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 2:
                        pictureBox1.Image.Save(fs, ImageFormat.Jpeg);
                        break;
                    case 3:
                        pictureBox1.Image.Save(fs, ImageFormat.Gif);
                        break;
                    case 4:
                        pictureBox1.Image.Save(fs, ImageFormat.Bmp);
                        break;

                }
                fs.Close();
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            for(int i = 0; i <checkedListBox1.CheckedIndices.Count; i++)
            {
                switch(checkedListBox1.CheckedIndices[i])
                {
                    case 0:
                        pictureBox1.Inversion();
                        break;
                    case 1:
                        pictureBox1.BrightnessCorrection(brightness);
                        break;
                    case 2:
                        pictureBox1.ContrastEnhancement(contrast);
                        break;
                    case 3:
                        pictureBox1.GammaCorrection(gamma); 
                        break;
                    case 4:
                        pictureBox1.ApplyFilter(boxblur, 0);
                        break;
                    case 5:
                        pictureBox1.ApplyFilter(gaussblur, 0);
                        break;
                    case 6:
                        pictureBox1.ApplyFilter(sharpen, 0);
                        break;
                    case 7:
                        pictureBox1.ApplyFilter(edge, 128);
                        break;
                    case 8:
                        pictureBox1.ApplyFilter(emboss, 0);
                        break;
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = org_img;
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {
            
            int row = Int16.Parse(textBox3.Text);
            int column = Int16.Parse(textBox4.Text);
            var control = sender as Control;
            NumericUpDown[,] numpd = new NumericUpDown[row,column];

            for (int i =0;i<row; i++)
            {
                for(int j =0;j<column;j++)
                {

                    numpd[i, j] = new NumericUpDown();
                    numpd[i, j].Name = $"{i} + {j}";
                    numpd[i, j].Value = 1;
                    numpd[i, j].Size = new Size(30, 20);
                    numpd[i, j].Location = new Point(523 + 30 * j, 285 + 20 * i);
                    numpd[i, j].Visible = true;
                    numpd[i, j].Minimum = -100;
                    this.Controls.Add(numpd[i, j]);
                }
            }
        }


        public static byte Clamp(int min, int max, float val)
        {
            if (val > max)
                val = max;
            if (val < min)
                val = min;
            return (byte)val;
        }

        public Bitmap ApplyCustomFilter(Image img)
        {
            Bitmap bm = new Bitmap(img);
            int row = Int16.Parse(textBox3.Text);
            int column = Int16.Parse(textBox4.Text);
            int R, G, B;
            R = G = B = 0;
            int divisor = Int16.Parse(textBox2.Text);
            int offset = Int16.Parse(textBox1.Text);
            int[,] kernel = new int[row, column];
            for(int i=0;i<row;i++)
            {
                for (int j = 0; j < column; j++)
                {
                    NumericUpDown numpd = this.Controls.Find($"{i} + {j}", false).First() as NumericUpDown;
                    kernel[i, j] = (int)numpd.Value;
                }
            }
            
            
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
                        int sourceY = ((int)counter / ((int)bm.Width * 4)) * 4 + y * 4;
                        if (sourceX < 0)
                            sourceX = 0;
                        if (sourceX >= bm.Width * 4)
                            sourceX = (bm.Width - 1) * 4;
                        if (sourceY < 0)
                            sourceY = 0;
                        if (sourceY >= bm.Height * 4)
                            sourceY = (bm.Height - 1) * 4;
                        R += (rgbValues[sourceX + sourceY * bm.Width] * kernel[y + (kernel.GetLength(1) / 2), x + (kernel.GetLength(0) / 2)]);
                        G += (rgbValues[sourceX + sourceY * bm.Width + 1] * kernel[y + (kernel.GetLength(1) / 2), x + (kernel.GetLength(0) / 2)]);
                        B += (rgbValues[sourceX + sourceY * bm.Width + 2] * kernel[y + (kernel.GetLength(1) / 2), x + (kernel.GetLength(0) / 2)]);
                    }
                }
                resultValues[counter] = Clamp(0, 255, (R / divisor) + offset);
                resultValues[counter + 1] = Clamp(0, 255, (G / divisor) + offset);
                resultValues[counter + 2] = Clamp(0, 255, (B / divisor) + offset);
                resultValues[counter + 3] = 255;


            }
            System.Runtime.InteropServices.Marshal.Copy(resultValues, 0, ptr, bytes);
            bm.UnlockBits(bmData);
            return bm;

        }

        private void button6_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = ApplyCustomFilter(pictureBox1.Image);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            int row = Int16.Parse(textBox3.Text);
            int column = Int16.Parse(textBox4.Text);
            int[,] kernel = new int[row, column];
            for (int i = 0; i < row; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    NumericUpDown numpd = this.Controls.Find($"{i} + {j}", false).First() as NumericUpDown;
                    kernel[i, j] = (int)numpd.Value;
                }
            }
            int divisor = kernel.Cast<int>().Sum();
            if (divisor == 0)
                divisor = 1;
            textBox2.Text = divisor.ToString();

        }
    }
}
