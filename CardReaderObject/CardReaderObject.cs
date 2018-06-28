using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
namespace CardReaderObject
{
    public class CardReaderObject : SeatManage.ISystemTerminal.IPOS.ICardReader
    {
        int icdev = -1;
        public void Beep()
        {
            int i = CardReader.rf_beep((int)icdev, 20);
        }
        /// <summary>
        /// 初始化第三方服务器
        /// </summary>
        /// <returns></returns>
        public bool ConnectServer()
        {
            return true;
        }

        public string GetCardId()
        {
            int st = -1;//操作结果标识
            ulong TagType = 0x0004;//卡片类型M1卡 52428
            ulong tempcardId = 0;


            st = CardReader.rf_request(icdev, 1, out TagType);

            if (st != 0)
            {
                // throw new Exception("寻卡请求失败");
            }
            st = CardReader.rf_card(icdev, 1, ref tempcardId);
            if (st != 0)
            {
                return "";
            }
            else
            {
                return ConvertCardSn(tempcardId);
            }
        }
        /// <summary>
        /// 直接从卡片里面读取学号
        /// </summary>
        /// <returns></returns>
        public string GetCardNo()
        {
            throw new NotImplementedException();
        }
        string strCardSnType = ConfigurationManager.AppSettings["CardSnType"];
        string strCardSnLength = ConfigurationManager.AppSettings["cardSnLength"];
        /// <summary>
        /// 卡列号长度
        /// </summary>
        public string StrCardSnLength
        {
            get { return strCardSnLength; }
            set { strCardSnLength = value; }
        }
        //卡列号类型
        public string StrCardSnType
        {
            get { return strCardSnType; }
            set { strCardSnType = value; }
        }
        /// <summary>
        /// 卡列号转换
        /// </summary>
        /// <param name="cardNo"></param>
        /// <returns></returns>
        public string ConvertCardSn(ulong cardId)
        {

            switch (strCardSnType)
            {
                // 直接读取，返回首位不带0的十进制字符串；
                case "0":
                    if (strCardSnLength == "Full")
                    {
                       return cardId.ToString("D10");
                    }
                    else 
                    {
                        return cardId.ToString();
                    } 
                case "1":
                    if (strCardSnLength == "Full")
                    {
                        return cardId.ToString("X8");
                    }
                    else
                    {
                        return cardId.ToString("X");
                     }
                   
                case "2":
                    if (strCardSnLength == "Full")
                    {
                        return CardIdConvert(cardId.ToString("X8")).ToString("X8");
                    }
                    else
                    {
                        return CardIdConvert(cardId.ToString("X8")).ToString("X");
                    }
                    
                case "3":
                    if (strCardSnLength == "Full")
                    {
                        return CardIdConvert(cardId.ToString("X8")).ToString("D10");
                    }
                    else
                    {
                        return CardIdConvert(cardId.ToString("X8")).ToString();
                    }
                    
                default:
                    SeatManage.SeatManageComm.WriteLog.Write("没有配置读卡器返回卡列号的类型");
                    throw new Exception("没有配置读卡器返回卡列号的类型"); 
            }

        }

        /// <summary>
        /// 读到的ID转换为16进制调换12和78调换位置，34和56调换位置
        /// </summary>
        /// <param name="xNo">十六进制字符串</param>
        /// <returns></returns>
        public ulong CardIdConvert(string xNo)
        {

            char[] x16No = xNo.ToCharArray();
            StringBuilder temp = new StringBuilder();
            temp.Append(x16No[6].ToString());
            temp.Append(x16No[7].ToString());
            temp.Append(x16No[4].ToString());
            temp.Append(x16No[5].ToString());
            temp.Append(x16No[2].ToString());
            temp.Append(x16No[3].ToString());
            temp.Append(x16No[0].ToString());
            temp.Append(x16No[1].ToString());
            return Convert.ToUInt32(temp.ToString(), 16);//.ToString(); 
        }

        /// <summary>
        /// 初始化读卡器
        /// </summary>
        /// <returns></returns>
        public bool Init()
        {
            try
            {
                icdev = CardReader.rf_init(0, 115200);// 初始化串口
                if ((int)icdev < 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
