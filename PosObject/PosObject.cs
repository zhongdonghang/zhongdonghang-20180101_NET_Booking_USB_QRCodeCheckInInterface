using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SeatManage.ClassModel;
using SeatManage.EnumType;
using SeatManage.InterfaceFactory;
using SeatManage.ISystemTerminal;
using Txq_csharp_sdk;

namespace PosObject
{
    public class PosObject : SeatManage.ISystemTerminal.IPOS.IPOSMethod
    {
        public event SeatManage.ISystemTerminal.IPOS.EventPosCardNo CardNoGeted;
        private SeatManage.ISystemTerminal.IPOS.ICardReader cardReader = null;
        private System.Timers.Timer timer = null;
        public PosObject()
        {
            try
            {
                if (objVbarapi.openDevice(1))
                {
                    objVbarapi.interval(300);
                    objVbarapi.backlight(true);
                }
                else
                {
                    SeatManage.SeatManageComm.WriteLog.Write("扫码器初始化失败");
                    throw new Exception("扫码器初始化失败");
                }
                    //cardReader = SeatManage.InterfaceFactory.AssemblyFactory.CreateAssembly("ICardReader") as SeatManage.ISystemTerminal.IPOS.ICardReader;
                    //if (!cardReader.Init())
                    //{
                    //    SeatManage.SeatManageComm.WriteLog.Write("读卡器初始化失败");
                    //    throw new Exception("读卡器初始化失败");

                    //}
                    //if (!cardReader.ConnectServer())
                    //{
                    //    SeatManage.SeatManageComm.WriteLog.Write("第三方初始化失败");
                    //    throw new Exception("初始化第三方失败");
                    //}
                    //cardReader.Beep();
                    timer = new System.Timers.Timer();
                timer.Interval = 500;
                timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        Vbarapi objVbarapi = new Vbarapi();

        public string Decoder()
        {
            byte[] result;
            string sResult = null;
            int size;
            if (objVbarapi.getResultStr(out result, out size))
            {
                string msg = System.Text.Encoding.Default.GetString(result);
                byte[] buffer = Encoding.UTF8.GetBytes(msg);
                sResult = Encoding.UTF8.GetString(buffer, 0, buffer.Length);
            }
            else
            {
                sResult = null;
            }
            return sResult;
        }

        void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            Stop();
            try
            {

                    string result = Decoder();
                    if (string.IsNullOrEmpty(result))
                    {

                    }
                    else
                    {
                        objVbarapi.beepControl(1);
                      

                        //string CardNo = AESAlgorithm.AESDecrypt(result);
                        string[] arr = result.Split('&');
                        string[] arr1 = arr[1].Split('=');
                        string cardNo = arr1[1];

                    //验证卡号
                  ReaderInfo reader =  new SeatManage.Bll.T_SM_Reader().GetReader(cardNo);
                        if ( !string.IsNullOrEmpty(reader.Name))
                        {
                            CardNoGeted(this, new SeatManage.ISystemTerminal.IPOS.CardEventArgs(cardNo, true, null));
                        }
                        else
                        {
                            SeatManage.SeatManageComm.WriteLog.Write("扫码结果:"+ result + "学号不存在");
                        }

                        //CardNoGeted(this, new SeatManage.ISystemTerminal.IPOS.CardEventArgs(cardNo, true, null));
                       // SeatManage.SeatManageComm.WriteLog.Write("扫码结果:" + result);
                    }
                //objVbarapi.disConnected();


                //string cardId = cardReader.GetCardId();
                ////获取到信息，读卡器响 
                //if (!string.IsNullOrEmpty(cardId))
                //{
                //    SeatManage.SeatManageComm.WriteLog.Write(cardId);
                //    string cardNo = GetCardNo(cardId.ToUpper());
                //    if (!string.IsNullOrEmpty(cardNo) && CardNoGeted != null)
                //    {
                //        CardNoGeted(this, new SeatManage.ISystemTerminal.IPOS.CardEventArgs(cardNo, true, null));
                //    }
                //    else
                //        if (string.IsNullOrEmpty(cardNo))
                //    {
                //        cardReader.Beep();
                //    }
                //}
            }
            catch (Exception ex)
            {
                SeatManage.SeatManageComm.WriteLog.Write(ex.ToString());
            }
            finally
            {
                Start();
            }
        }

        public void Reset()
        {
            //cardReader.Init();
            cardReader.Beep();
        }

        public void Start()
        {
            timer.Start();
        }

        public void Stop()
        {
            timer.Stop();
        }

        #region 私有方法
        /// <summary>
        /// 通过物理卡号获取学号
        /// </summary>
        /// <param name="cardId"></param>
        /// <returns></returns>
        private string GetCardNo(string cardId)
        {
            try
            {
                SeatManage.ClassModel.ReaderInfo reader = SeatManage.Bll.T_SM_Reader.GetReaderByCardId(cardId);
                if (reader != null)
                {
                    return reader.CardNo;
                }
                else
                {
                    return cardId;
                }
            }
            catch (Exception EX)
            {
                throw EX;
            }
        }
        /// <summary>
        /// 直接从卡片里获取卡号
        /// </summary>
        /// <returns></returns>
        private string GetCardNo()
        {
            string cardNo = cardReader.GetCardNo();// cardReader.GetCardId();
            return cardNo;
        }
        #endregion
    }
}
