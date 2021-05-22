using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Rasterization
{
    public partial class Form1 : Form
    {
        CanvasLogic CanvasLogic = new CanvasLogic();
        CircleDrawing CircleDrawing = new CircleDrawing();
        LineDrawing LineDrawing = new LineDrawing();
        PolygonDrawing PolygonDrawing = new PolygonDrawing();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            comboBox1.Items.Add("Black");
            comboBox1.Items.Add("Red");
            comboBox1.Items.Add("Green");
            comboBox1.Items.Add("Blue");
            comboBox1.SelectedIndex = 0;

            comboBox2.Items.Add("1");
            comboBox2.Items.Add("3");
            comboBox2.Items.Add("5");
            comboBox2.Items.Add("7");
            comboBox2.SelectedIndex = 0;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 2;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            CanvasLogic.DrawingMode = 3;
            CanvasLogic.tmpPolygon.Points = new List<Point>();
            CanvasLogic.tmpPolygon.Color = GetSelectedColor();
            CanvasLogic.tmpPolygon.Antialiasing = checkBox3.Checked ? true : false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (CanvasLogic.tmpPolygon.Antialiasing == false)
            {
                pictureBox1.Image = PolygonDrawing.DrawPolygon_DDA(CanvasLogic.tmpPolygon, pictureBox1.Image);
            }
            if (CanvasLogic.tmpPolygon.Antialiasing == true)
            {
                pictureBox1.Image = PolygonDrawing.DrawPolygon_Antialiasing(CanvasLogic.tmpPolygon, pictureBox1.Image);
            }

            CanvasLogic.tmpPolygon = new Polygon();
            CanvasLogic.DrawingMode = 0;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            switch (CanvasLogic.DrawingMode)
            {
                case 1:
                    if (e.X + GetValueFromTextBox(textBox1) < pictureBox1.Width &&
                        e.X - GetValueFromTextBox(textBox1) > 0 &&
                        e.Y + GetValueFromTextBox(textBox1) < pictureBox1.Height &&
                        e.Y - GetValueFromTextBox(textBox1) > 0)
                    {
                        CanvasLogic.tmpCircle.Center = new Point(e.X, e.Y);
                        CanvasLogic.tmpCircle.Radius = int.Parse(textBox1.Text.ToString());
                        CanvasLogic.tmpCircle.Color = GetSelectedColor();
                        CanvasLogic.tmpCircle.Antialiasing = checkBox2.Checked ? true : false;

                        if (CanvasLogic.tmpCircle.Antialiasing)
                        {
                            pictureBox1.Image = CircleDrawing.WuCircle(CanvasLogic.tmpCircle, pictureBox1.Image);
                        }
                        else
                        {
                            pictureBox1.Image = CircleDrawing.MidpointCircle(CanvasLogic.tmpCircle, pictureBox1.Image);
                        }

                        CanvasLogic.tmpCircle = new Circle();
                        CanvasLogic.DrawingMode = 0;
                    }
                    break;
                case 2:
                    if (CanvasLogic.tmpLine.StartPoint == ((new Line()).StartPoint))
                    {
                        CanvasLogic.tmpLine.StartPoint = new Point(e.X, e.Y);
                    }
                    else
                    {
                        CanvasLogic.tmpLine.EndPoint = new Point(e.X, e.Y);
                        CanvasLogic.tmpLine.Color = GetSelectedColor();
                        CanvasLogic.tmpLine.Thickness = int.Parse(comboBox2.Items[comboBox2.SelectedIndex].ToString());
                        CanvasLogic.tmpLine.Antialiasing = checkBox1.Checked ? true : false;

                        if (CanvasLogic.tmpLine.Antialiasing == false &&
                            comboBox2.SelectedIndex == 0)
                        {
                            pictureBox1.Image = LineDrawing.lineDDA(CanvasLogic.tmpLine, pictureBox1.Image);
                        }
                        else if (CanvasLogic.tmpLine.Antialiasing == false &&
                                 comboBox2.SelectedIndex != 0)
                        {
                            pictureBox1.Image = LineDrawing.lineDDA_thick(CanvasLogic.tmpLine, pictureBox1.Image);
                        }
                        else if (CanvasLogic.tmpLine.Antialiasing == true &&
                                 comboBox2.SelectedIndex == 0)
                        {
                            pictureBox1.Image = LineDrawing.WuLine(CanvasLogic.tmpLine, pictureBox1.Image);
                        }

                        CanvasLogic.tmpLine = new Line();
                        CanvasLogic.DrawingMode = 0;
                    }
                    break;
                case 3:
                    CanvasLogic.tmpPolygon.Points.Add(new Point(e.X, e.Y));
                    break;
                default:
                    break;
            }
        }

        private Color GetSelectedColor()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    return Color.FromArgb(255, 0, 0, 0);
                case 1:
                    return Color.FromArgb(255, 255, 0, 0);
                case 2:
                    return Color.FromArgb(255, 0, 255, 0);
                case 3:
                    return Color.FromArgb(255, 0, 0, 255);
                default:
                    return Color.FromArgb(255, 0, 0, 0);
            }
        }

        private int GetValueFromTextBox(TextBox textBox)
        {
            return int.Parse(textBox.Text.ToString());
        }
    }
}
