using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace dodo
{
    public partial class Form1 : Form
    {
        List<int> localxlist = new List<int>();
        List<int> localylist = new List<int>();
        List<int> roillist = new List<int>();

        private Bitmap bmp;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //load image
            pictureBox1.Image = Image.FromFile("dodo.jpg");
        }
        /// <summary>
        /// test button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
                       
        }

        /// <summary>
        /// 生成按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            int AllStar = 200;
            Random rd = new Random(DateTime.Now.Second);
            Bitmap pic1 = (Bitmap)pictureBox1.Image;            
            for (int i = 0; i < AllStar; i++)
            {
                //W:1519 H:657
                roillist.Add(rd.Next(1, 10));
                localxlist.Add(rd.Next(pic1.Width));
                localylist.Add(rd.Next(pic1.Height));
            }
            this.pictureBox1.Invalidate();
        }
        /// <summary>
        /// 重新绘制界面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            bmp = new Bitmap(this.pictureBox1.Width, this.pictureBox1.Height, e.Graphics);
            var g = Graphics.FromImage(bmp);
            g.Clear(Color.White);
            Pen pen = new Pen(Color.Black,1);
            for (int i = 0; i < localxlist.Count; i++)
            {
                g.FillEllipse(new SolidBrush(Color.Black), localxlist[i], localylist[i], roillist[i], roillist[i]);                
                g.DrawEllipse(pen, localxlist[i], localylist[i], roillist[i], roillist[i]);
            }
            e.Graphics.DrawImage(bmp, 0, 0);
        }
    }
    public class EarthInfo
    {
        //const define
        public enum DIRECT_SPEED
        {
            SPEED_UP,
            SPEED_DOWN,
            SPEED_RIGHT,
            SPEED_LEFT,
            SPEED_UP_LEFT,
            SPEED_UP_RIGHT,
            SPEED_DOWN_LEFT,
            SPEED_DOWN_RIGHT,
            SPEED_STOP,          
        }

        //value define
        public string name;
        public int localx;
        public int localy;
        public int roil;
        public int speed;
        public int direct;
        public List<string> oldName = new List<string>();

    }
}
