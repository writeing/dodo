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
        List<EarthInfo> g_listEarth = new List<EarthInfo>();
        private Bitmap bmp;


        public int bmpWidth;
        public int bmpHigth;
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
            timer1.Start();
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
            bmpWidth = pic1.Width;
            bmpHigth = pic1.Height;
            for (int i = 0; i < AllStar; i++)
            {

                //W:1519 H:657
                int roil = rd.Next(1, 10);
                int localx = rd.Next(pic1.Width- 10);
                int localy = rd.Next(pic1.Height - 10);
                int direct = rd.Next(0, 360);
                roillist.Add(roil);
                localxlist.Add(localx);
                localylist.Add(localy);
                EarthInfo eif = new EarthInfo(roil,localx, localy,i, direct);
                g_listEarth.Add(eif);
            }
            this.pictureBox1.Invalidate();
            //get all earth 
            EarthCacl.calcAllEarthDiss(ref g_listEarth);

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
            foreach (EarthInfo eti in g_listEarth)
            {
                g.FillEllipse(new SolidBrush(Color.Black), eti.localx, eti.localy, eti.roil, eti.roil);
                g.DrawEllipse(pen, eti.localx, eti.localy, eti.roil, eti.roil);
            }
            e.Graphics.DrawImage(bmp, 0, 0);
        }
        public int clacCount = 0;
        private void timer1_Tick(object sender, EventArgs e)
        {
            //combine earth 
            EarthCacl.combineEarth(ref g_listEarth);
            //repoint
            this.pictureBox1.Invalidate();
            //clac next point local
            EarthCacl.calcEarthExec(ref g_listEarth);
            //get disconect data
            EarthCacl.calcAllEarthDiss(ref g_listEarth);
            //get earth direct
            //F = G*M*m/R2
            //V = V0 + at;
            //direct = 

            clacCount++;
            richTextBox1.AppendText(clacCount.ToString() + "\r\n");            
        }
    }
    public class EarthInfo
    {
        //value define
        public Dictionary<string,double> dictOtherDiss = new Dictionary<string, double>();
        public string name;
        public int localx;
        public int localy;
        public int roil;
        public int speed;
        public int direct;          // 0~360
        public List<string> oldName = new List<string>();
        public void calcEarthNewRoil(int addroil)
        {
            this.roil = (int)Math.Sqrt(addroil * addroil + this.roil * this.roil);
        }
        public EarthInfo(int roil,int localx,int localy,int name,int direct)
        {
            this.roil = roil;
            this.localx = localx;
            this.localy = localy;
            this.name = name.ToString();
            this.oldName.Add(this.name);
            this.direct = direct;
            this.speed = 1;           
        }
    }
}
