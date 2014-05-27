using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Text;
using WiimoteLib;

namespace wiifit
{
    public partial class frmMain : Form
    {
        Wiimote wm = new Wiimote();
        StreamWriter sw = new StreamWriter(@"C:\Test.csv", true, Encoding.GetEncoding("Shift_JIS"));
        string output_result;
        public frmMain()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.wm.Connect();
            //各種メタデータを格納
            output_result = txtID.Text + "," + txtName.Text + "," + txtAge.Text + "," + cmbSex.Text + "," + txtHeight.Text + "," + cmbCondition.Text + "," + cmbPosition.Text;
            this.wm.WiimoteChanged += wm_WiimoteChanged;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.wm.Disconnect();
            sw.WriteLine(output_result);
            sw.Close();
        }

        void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            //WiimoteStateの値を取得
            WiimoteState ws = args.WiimoteState;

            //ピクチャーボックスへ描画
            this.DrawForms(ws);

            //ラベル

            //重さ(Kg)表示
            this.label1.Text = ws.BalanceBoardState.WeightKg + "kg";
            //重心のX座標表示
            this.label2.Text = "X:" +
                ((int)ws.BalanceBoardState.CenterOfGravity.X * (43.2 / 43)).ToString();
            //重心のY座標表示
            this.label3.Text = "Y:" +
                ((int)ws.BalanceBoardState.CenterOfGravity.Y * (23.7 / 24)).ToString();
            //結果出力用変数に格納
            output_result += ((int)ws.BalanceBoardState.CenterOfGravity.X * (43.2 / 43)).ToString() + "," + ((int)ws.BalanceBoardState.CenterOfGravity.Y * (23.7 / 24)).ToString() + "," + ws.BalanceBoardState.WeightKg + Environment.NewLine;
        }
        //フォーム描写関数
        public void DrawForms(WiimoteState ws)
        {
            //pictureBox1のグラフィックスを取得
            Graphics g = this.pictureBox1.CreateGraphics();
            g.Clear(Color.Black);     //画面を黒色にクリア

            //X、Y座標の計算
            float x =
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.X
                + 20.0f) * 10;    //表示位置(X座標)を求める
            float y =
                (wm.WiimoteState.BalanceBoardState.CenterOfGravity.Y
                + 12.0f) * 10;    //表示位置(Y座標)を求める

            //赤色でマーカを描写
            g.FillEllipse(Brushes.Red, x, y, 10, 10);

            g.Dispose();  //グラフィックスを開放
        }
    }
}
