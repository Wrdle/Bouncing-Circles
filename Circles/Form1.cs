using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Circles
{
    public partial class Form1 : Form
    {
        public List<Circle> circles = new List<Circle>();
        public Graphics g;

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread drawCircles = new Thread(new System.Threading.ThreadStart(animateCircles));
            drawCircles.Start();
        }

        public void generateCircles()
        {
            circles.Clear();
            Random rand = new Random();
            for (int x = 0; x < 1; x++)
            {
                Circle circle = new Circle(rand.Next(1, 300), rand.Next(1, 300), (rand.Next(0, 2) == 1 ? -1 : 1), (rand.Next(0, 2) == 1 ? -1 : 1));
                circles.Add(circle);
            }
        }

        public void animateCircles()
        {
            g = canvas.CreateGraphics();
            generateCircles();
            Brush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);

            for (int iterations = 0; iterations < 1500; iterations++)
            {
                Random rand = new Random();
                for (int x = 0; x < 1; x++)
                {
                    int newXPos = circles[x].xDir;
                    int newYPos = circles[x].yDir;
                    if (circles[x].yPos <= 0)
                    {
                        newYPos = 1;
                    }
                    if (circles[x].yPos >= canvas.Height)
                    {
                        newYPos = -1;
                    }
                    if (circles[x].xPos <= 0)
                    {
                        newXPos = 1;
                    }
                    if (circles[x].xPos >= canvas.Width)
                    {
                        newXPos = -1;
                    }
                    Circle circle = new Circle(circles[x].xPos + newXPos, circles[x].yPos + newYPos, newXPos, newYPos);
                    circles[x] = circle;
                    g.FillCircle(myBrush, circles[x].xPos, circles[x].yPos, 5);
                }
                Thread.Sleep(5);
                g.Clear(Color.White);
            }
        }
    }

    public struct Circle
    {
        public int xPos;
        public int yPos;
        public int xDir;
        public int yDir;

       public Circle(int _xPos, int _yPos, int _xDir, int _yDir)
        {
            xPos = _xPos;
            yPos = _yPos;
            xDir = _xDir;
            yDir = _yDir;
        }
    }


    public static class GraphicsExtensions
    {
        public static void DrawCircle(this Graphics g, Pen pen,
                                      float centerX, float centerY, float radius)
        {
            g.DrawEllipse(pen, centerX - radius, centerY - radius,
                          radius + radius, radius + radius);
        }

        public static void FillCircle(this Graphics g, Brush brush,
                                      float centerX, float centerY, float radius)
        {
                g.FillEllipse(brush, centerX - radius, centerY - radius, radius + radius, radius + radius);
        }
        public static void clearScreen(this Graphics g)
        {
            g.Clear(Color.White);
        }
    }
}
