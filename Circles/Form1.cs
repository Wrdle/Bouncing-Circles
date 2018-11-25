using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace Circles
{
    public partial class Form1 : Form
    {
        public List<Circle> circles = new List<Circle>();
        Graphics g;
        int iCircles = 14;
        int iRadius = 300;
        Thread drawCircles = new Thread(new System.Threading.ThreadStart(animateCircles));

        public Form1()
        {
            InitializeComponent();
        }

        public void generateCircles()
        {
            circles.Clear();
            Random rand = new Random();
            for (int x = 0; x < iCircles; x++)
            {
                Circle circle = new Circle(rand.Next(1, canvas.Width), rand.Next(1, canvas.Height), (rand.Next(0, 2) == 1 ? -1 : 1), (rand.Next(0, 2) == 1 ? -1 : 1));
                circles.Add(circle);
            }
        }

        public void animateCircles()
        {
            g = canvas.CreateGraphics();
            g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            generateCircles();
            Brush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
            Pen pen = new Pen(Color.Black, 3);

            while (true)
            {
                Random rand = new Random();
                for (int x = 0; x < iCircles; x++)
                {
                    List<PointF> lines = new List<PointF>();

                    double newXPos = circles[x].xDir;
                    double newYPos = circles[x].yDir;
                    if (circles[x].yPos <= 0)
                    {
                        newYPos = -newYPos;
                    }
                    if (circles[x].yPos >= canvas.Height)
                    {
                        newYPos = -newYPos;
                    }
                    if (circles[x].xPos <= 0)
                    {
                        newXPos = -newXPos;
                    }
                    if (circles[x].xPos >= canvas.Width)
                    {
                        newXPos = -newXPos;
                    }
                    Circle circle = new Circle(circles[x].xPos + newXPos, circles[x].yPos + newYPos, newXPos, newYPos);
                    circles[x] = circle;
                    g.FillCircle(myBrush, (float)circles[x].xPos, (float)circles[x].yPos, 5);
                    foreach (Circle c in circles)
                    {
                        if (circles[x].xPos - c.xPos < iRadius && circles[x].yPos - c.yPos < iRadius)
                        {
                            if (circles[x].xPos - c.xPos > - iRadius && circles[x].yPos - c.yPos > -iRadius)
                            {
                                lines.Add(new PointF((float)circles[x].xPos, (float)circles[x].yPos));
                                lines.Add(new PointF((float)c.xPos, (float)c.yPos));
                            }
                        }
                    }
                    g.DrawLines(pen, lines.ToArray());
                }
                Thread.Sleep(1);
                g.Clear(Color.White);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            drawCircles = new Thread(new System.Threading.ThreadStart(animateCircles));
            drawCircles.Start();
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            drawCircles.Abort();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (drawCircles.IsAlive == true)
            {
                drawCircles.Abort();
            }
        }
    }
}
