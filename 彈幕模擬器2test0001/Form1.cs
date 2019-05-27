using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using Barrage1;
using 彈幕模擬器2test0001.Properties;
using BallPoint;
using System.Threading.Tasks;

namespace 彈幕模擬器2test0001
{
    unsafe public partial class Form1 : Form
    {
        Barrage[] gs = new Barrage[10];//控制類別   
        BallDraw bg;
        int tra1, tra2, tra3, tra4, tra5, tra6;
        int bitmapwidth, bitmapheight;
        int width, height, BallW, BallH;
        byte* checkstart;
        byte* checkendd;
        byte* BallPF;
        byte* BallByte;
        Image image0;
        Bitmap nowimage;
        Bitmap tempbitmap;
        Bitmap Ball;
        int i = 0;
        int time = 0;
        int total = 1000;
        int time2 = 0;
        int speed = 1;
        Boolean fastfor = true;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            tra1 = trackBar1.Value;
            tra2 = trackBar2.Value;
            tra3 = trackBar3.Value;
            tra4 = trackBar4.Value;
            tra5 = trackBar5.Value;
            tra6 = trackBar6.Value;
            bitmapheight = 900;
            bitmapwidth = 700;
            this.WindowState = FormWindowState.Maximized;
            Bitmap MyNewBmp = new Bitmap(bitmapwidth, bitmapheight);
            Rectangle MyRec = new Rectangle(0, 0, MyNewBmp.Width, MyNewBmp.Height);
            BitmapData MyBmpData = MyNewBmp.LockBits(MyRec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            IntPtr MyPtr = MyBmpData.Scan0;
            int MyByteCount = MyBmpData.Stride * MyNewBmp.Height;
            byte[] MyNewColor = new byte[MyByteCount];
            Marshal.Copy(MyPtr, MyNewColor, 0, MyByteCount);
            width = MyNewBmp.Width;
            height = MyNewBmp.Height;
            for (int n = 0; n < MyByteCount; n++)
                MyNewColor[n] = 255;
            Marshal.Copy(MyNewColor, 0, MyPtr, MyByteCount);
            MyNewBmp.UnlockBits(MyBmpData);
            pictureBox1.Image = MyNewBmp;
            image0 = MyNewBmp;
            pictureBox1.Image = MyNewBmp;
            nowimage = MyNewBmp;
            tempbitmap = new Bitmap(nowimage, nowimage.Width, nowimage.Height);
            bg = new BallDraw(checkstart, checkendd, BallW, BallH, width);
            yellowball();
            gsStart();
            timer1.Enabled = true;
        }
        private void yellowball()
        {
            Ball = new Bitmap(Resources.光球, tra3, tra3);
            Rectangle MyRec = new Rectangle(0, 0, Ball.Width, Ball.Height);
            BitmapData MyBmpData = Ball.LockBits(MyRec, ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int MyByteCount = MyBmpData.Stride * Ball.Height;
            BallByte = (byte*)MyBmpData.Scan0;
            bg.BallCount(BallByte, MyByteCount);
            Ball.UnlockBits(MyBmpData);
            BallW = Ball.Width;
            BallH = Ball.Height;
            bg.Check(BallW, BallH);
        }
        private void draw(byte* point)
        {
            byte* point2 = point;
            byte* BallPF2 = BallPF;
            int iW, iH;
            byte color3;
            for (iH = 0; iH < BallH; iH++)
            {
                for (iW = 0; iW < BallW; iW++)
                {
                    point2 = (point + (iH * width + iW) * 3);
                    BallPF2 = (BallPF + (iH * BallW + iW) * 4);
                    color3 = *(BallPF2 + 3);
                    if (!(point2 <= checkstart || point2 >= checkendd))
                    {
                        *(point2) = (byte)((*(point2 + 0) * (255 - color3) / 255 + *(BallPF2 + 0) * color3 / 255));
                        *(point2 + 1) = (byte)((*(point2 + 1) * (255 - color3) / 255 + *(BallPF2 + 1) * color3 / 255));
                        *(point2 + 2) = (byte)((*(point2 + 2) * (255 - color3) / 255 + *(BallPF2 + 2) * color3 / 255));
                    }

                }
            }
        }
        private void gsStart()
        {
            timer1.Enabled = false;
            yellowball();
            time = 1;
            gs = new Barrage[total];
            for (i = 0; i < gs.Length; i++)
                gs[i] = new Barrage(tra2, tra4, tra5, (i + 1) * tra1, width, height);
            timer1.Enabled = true;
        }
        private void newtimer()
        {

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            time2++;
            tempbitmap = nowimage;
            if (time < gs.Length - 1)
                time++;
            Rectangle MyRec = new Rectangle(0, 0, tempbitmap.Width, tempbitmap.Height);
            BitmapData MyBmpData = tempbitmap.LockBits(MyRec, ImageLockMode.ReadWrite, PixelFormat.Format24bppRgb);
            byte* MyNewColor = (byte*)MyBmpData.Scan0;
            checkstart = MyNewColor;
            int MyByteCount = MyBmpData.Stride * tempbitmap.Height;
            checkendd = checkstart + MyByteCount;
            bg.Check(checkstart, checkendd);
            byte*[] point = new byte*[gs.Length];
            for (i = 0; i < MyByteCount; i += 3)
                *(MyNewColor + i) = *(MyNewColor + i + 1) = *(MyNewColor + i + 2) = 0;
            int[] gsCheck = new int[time];
            if (fastfor == true)
            {
                Parallel.For(0, time, j =>
              {
                  gsCheck[j] = gs[j].Game();
              });
                Array.Sort(gsCheck);
                Parallel.For(0, time, j =>
                {
                    point[j] = MyNewColor + (gsCheck[j]) * 3;
                });
                Parallel.For(0, time, j =>
                {
                    if (gsCheck[j] > 0 && time2 >= speed)
                        bg.draw(point[j]);
                });
            }
            else
            {
                for (i = 0; i < time; i++)
                    gsCheck[i] = gs[i].Game();
                Array.Sort(gsCheck);
                for (i = 0; i < time; i++)
                    point[i] = MyNewColor + (gsCheck[i]) * 3;
                for (i = 0; i < time; i++)
                    if (gsCheck[i] > 0 && time2 >= speed)
                        bg.draw(point[i]);
            }
            tempbitmap.UnlockBits(MyBmpData);
            if (time2 >= speed)
            {
                pictureBox1.Image = tempbitmap;
                time2 = 0;
            }
            timer1.Enabled = true;
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            tra1 = trackBar1.Value;
            gsStart();
        }
        private void trackBar2_Scroll(object sender, EventArgs e)
        {
            tra2 = trackBar2.Value;
            gsStart();
        }
        private void trackBar3_Scroll(object sender, EventArgs e)
        {
            tra3 = trackBar3.Value;
            gsStart();
        }
        private void trackBar4_Scroll(object sender, EventArgs e)
        {
            tra4 = trackBar4.Value;
            gsStart();
        }
        private void trackBar5_Scroll(object sender, EventArgs e)
        {
            tra5 = trackBar5.Value;
            gsStart();
        }
        private void trackBar6_Scroll(object sender, EventArgs e)
        {
            tra6 = trackBar6.Value;
            gsStart();
        }
    }
}
////////////////////////
/*
byte* point2 = point;
byte* BallPF2 = BallPF;
int iW, iH;
byte color3;
            for (iH = 0; iH<BallH; iH++)
            {
                for (iW = 0; iW<BallW; iW++)
                {
                    point2 = (point + (iH* width + iW) * 3);
                    BallPF2 = (BallPF + (iH* BallW + iW) * 4);
                    color3 = * (BallPF2 + 3);
                    if (!(point2 <= checkstart || point2 >= checkendd))
                    {
                        * (point2) = (byte) (*(point2 + 0) + * (BallPF2 + 0));
                        * (point2 + 1) = (byte) (*(point2 + 1) + * (BallPF2 + 1));
                        * (point2 + 2) = (byte) (*(point2 + 2) + * (BallPF2 + 2));
                    }

                }
            }
    */
