using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Test
{
    public partial class Form1 : Form
    {
        SeatManage.ISystemTerminal.IPOS.IPOSMethod POS = null;
        bool isReading = false;
        public Form1()
        {
            InitializeComponent();
            POS = new PosObject.PosObject();  //SeatManage.InterfaceFactory.AssemblyFactory.CreateAssembly("IPOSMethod") as SeatManage.ISystemTerminal.IPOS.IPOSMethod;
            POS.CardNoGeted += new SeatManage.ISystemTerminal.IPOS.EventPosCardNo(POS_CardNoGeted);

        }

        void POS_CardNoGeted(object sender, SeatManage.ISystemTerminal.IPOS.CardEventArgs e)
        {
            if (e.PosResult == true)
            {
                this.Invoke(new Action(() => {
                    this.label1.Text ="学号："+ e.CardNo;
                }));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (POS != null)
            {
                if (isReading)
                {
                    POS.Stop();
                    button1.Text = "自动读取学号";
                    isReading = false;
                }
                else
                {
                    POS.Start();
                    button1.Text = "停止自动读取学号";
                    isReading = true;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            CardReaderObject.CardReaderObject reader = new CardReaderObject.CardReaderObject();
            reader.Init();
            reader.StrCardSnType = "0";
            //reader.StrCardSnLength = "Part";
            this.label1.Text+= "直接读取的卡序列号：" + reader.GetCardId()+"\r";
            reader.StrCardSnType = "1";
            this.label1.Text += "直接转换十六进制：" + reader.GetCardId() + "\r";
            reader.StrCardSnType = "2";
            this.label1.Text += "高地位转换的16进制：" + reader.GetCardId() + "\r";
            reader.StrCardSnType = "3";
            this.label1.Text += "高地位转换的10进制：" + reader.GetCardId() + "\r";
        }

    }
}
