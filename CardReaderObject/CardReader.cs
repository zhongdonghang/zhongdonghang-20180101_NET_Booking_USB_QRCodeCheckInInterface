using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace CardReaderObject
{
   public class CardReader
    { 
        #region 读卡器接口
        /// <summary>
        /// 初始化串口
        /// </summary>
        /// <param name="port">串口号</param>
        /// <param name="baud">波特率</param>
        /// <returns>成功则返回串口标识符>0，失败返回负值</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_init", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_init(Int16 port, int baud);

        /// <summary>
        /// 寻卡请求
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="_Mode">寻卡模式mode_card</param>
        /// <param name="TagType">卡类型值，0x0004为M1卡，0x0010为ML卡</param>
        /// <returns>成功则返回 0</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_request", SetLastError = true, CharSet = CharSet.Ansi, ExactSpelling = true, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_request(int icdev, byte _Mode, out ulong TagType);

        /// <summary>
        /// 寻卡，能返回在工作区域内某张卡的序列号
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="_Mode">寻卡模式mode_card</param>
        /// <param name="_Snr">返回的卡序列号</param>
        /// <returns>成功则返回 0</returns>
        [DllImport("mwrf32.dll")]
        public static extern Int16 rf_card(int icdev, byte _Mode, ref UInt64 _Snr);

        /// <summary>
        /// 蜂鸣
        /// </summary>
        /// <param name="icdev">通讯设备标识符</param>
        /// <param name="_Msec">蜂鸣时间，单位是10毫秒</param>
        /// <returns>成功则返回 0</returns>
        [DllImport("mwrf32.dll", EntryPoint = "rf_beep", SetLastError = true, CharSet = CharSet.Auto, ExactSpelling = false, CallingConvention = CallingConvention.StdCall)]
        public static extern Int16 rf_beep(int icdev, int _Msec);
        #endregion
    }
}
