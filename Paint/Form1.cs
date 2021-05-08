using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint
{
    enum Tool
    {
        Line,
        Rectangle,
        Pen,
        Circle
    }
    public partial class Form1 : Form
    {
        Bitmap bitmap = default(Bitmap);
        Graphics graphics = default(Graphics);
        Pen pen = new Pen(Color.Black);
        Point prevPoint = default(Point);
        Point curPoint = default(Point);
        bool mousePressed = false;
        Tool curTool = Tool.Pen;
        public Form1()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            pictureBox1.Image = bitmap;
            graphics.Clear(Color.White);
            button3.Select();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            curTool = Tool.Line;
        }
        Rectangle GetMRectangle(Point pPoint, Point cPoint)
        {
            return new Rectangle
            {
                X = Math.Min(pPoint.X, cPoint.X),
                Y = Math.Min(pPoint.Y, cPoint.Y),
                Width = Math.Abs(pPoint.X - cPoint.X),
                Height = Math.Abs(pPoint.Y - cPoint.Y)
            };
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            toolStripStatusLabel1.Text = e.Location.ToString();
            if (mousePressed)
            {
                switch (curTool)
                {
                    case Tool.Line:
                    case Tool.Rectangle:
                        curPoint = e.Location;
                        break;
                    case Tool.Pen:
                        prevPoint = curPoint;
                        curPoint = e.Location;
                        graphics.DrawLine(pen, prevPoint, curPoint);
                        break;
                    case Tool.Circle:
                        curPoint = e.Location;
                        break;
                    default:
                        break;
                }
                pictureBox1.Refresh();
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            prevPoint = e.Location;
            curPoint = e.Location;
            mousePressed = true;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            mousePressed = false;
            switch (curTool)
            {
                case Tool.Line:
                    graphics.DrawLine(pen, prevPoint, curPoint);
                    break;
                case Tool.Rectangle:
                    graphics.DrawRectangle(pen, GetMRectangle(prevPoint, curPoint));
                    break;
                case Tool.Pen:
                    break;
                case Tool.Circle:
                    graphics.DrawEllipse(pen, GetMRectangle(prevPoint, curPoint));
                    break;
                default:
                    break;
            }
            prevPoint = e.Location;
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
            switch (curTool)
            {
                case Tool.Line:
                    e.Graphics.DrawLine(pen, prevPoint, curPoint);
                    break;
                case Tool.Rectangle:
                    e.Graphics.DrawRectangle(pen, GetMRectangle(prevPoint, curPoint));
                    break;
                case Tool.Pen:
                    break;
                case Tool.Circle:
                    e.Graphics.DrawEllipse(pen, GetMRectangle(prevPoint, curPoint));
                    break;
                default:
                    break;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            curTool = Tool.Rectangle;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            curTool = Tool.Pen;
        }

        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                bitmap = Bitmap.FromFile(openFileDialog1.FileName) as Bitmap;
                pictureBox1.Image = bitmap;
                graphics = Graphics.FromImage(bitmap);
            }
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Displays a SaveFileDialog so the user can save the Image
            // assigned to Button2.
            saveFileDialog1.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            saveFileDialog1.Title = "Save an Image File";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Saves the Image via a FileStream created by the OpenFile method.
                System.IO.FileStream fs =
                    (System.IO.FileStream)saveFileDialog1.OpenFile();
                // Saves the Image in the appropriate ImageFormat based upon the
                // File type selected in the dialog box.
                // NOTE that the FilterIndex property is one-based.
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Jpeg);
                        break;

                    case 2:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Bmp);
                        break;

                    case 3:
                        bitmap.Save(fs,
                          System.Drawing.Imaging.ImageFormat.Gif);
                        break;
                }

                fs.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            curTool = Tool.Circle;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            pen.Color = ((Button)sender).BackColor;
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if(colorDialog1.ShowDialog() == DialogResult.OK)
            {
                pen.Color = colorDialog1.Color;
                ((Button)sender).BackColor = colorDialog1.Color;
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            graphics.Clear(pictureBox1.BackColor);
            pictureBox1.Image = bitmap;
        }

        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            pen.Width = trackBar1.Value;
        }
    }
}
