using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RayCasting
{
    public partial class Form1 : Form
    {
        Graphics gfx;
        Bitmap canvas;
        RayCaster rayCaster;
        List<Line> barriers;
        double angleStep = 0.005;
        double rayCastDistance = 5000;

        Point cursor => pictureBox1.PointToClient(Cursor.Position);
        public Form1()
        {
            InitializeComponent();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            gfx.Clear(Color.Black);
            RayCaster.DrawBarriers(gfx, barriers, Pens.White);
            rayCaster.Update(gfx,cursor,barriers);
            pictureBox1.Image = canvas;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            canvas = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            gfx = Graphics.FromImage(canvas);
            rayCaster = new RayCaster(Pens.White,angleStep,rayCastDistance);
            barriers = RayCaster.GenerateBarriers(gfx, 10, 500,canvas);
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            barriers = RayCaster.GenerateBarriers(gfx, 10, 500, canvas);
        }
    }
}
