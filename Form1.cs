using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;                               // 调用Dll文件
using System.IO;
using System.Management;
using System.Text.RegularExpressions;
using System.Threading;
using Modbus软件.Properties;
using System.Linq.Expressions;
using System.Data.OleDb;

namespace Modbus软件
{
    public partial class Form1 : Form
    {
        //private delegate void setTextBoxHandler(TextBox tb,string value);
        
        //private void setTextBox(TextBox tb, string value)
        //{
        //    if(tb.InvokeRequired)
        //    {
        //        setTextBoxHandler setvalue = new setTextBoxHandler(setTextBox);
        //        tb.Invoke(setvalue, new object[] { tb, value });
        //    }
        //    else
        //    {
        //        tb.Text = value;
        //    }
        //}

        public Form1()
        {
            InitializeComponent();
            Control.CheckForIllegalCrossThreadCalls = false;        // 充许子线程访问控件 但安全性会降低
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MyPara.Title = "Modbus";
            this.Text = MyPara.Title + DateTime.Now.ToString("  HH:mm:ss");

            this.toolTip1.SetToolTip(btn_ClearShow, "清空(Ctrl+D)");
            this.toolTip1.SetToolTip(button_Help, "帮助(F1)");
            this.toolTip1.SetToolTip(label_ShowHelp, "帮助提示\r\n" +
                                                     "清空文本：Crtl+D\r\n" +
                                                     "开启\\关闭提示：Ctrl+T\r\n" +
                                                     "开启\\关闭显示校验：Ctrl+Q\r\n" +
                                                     "开启\\关闭插入空格：Ctrl+S");
            this.toolTip1.SetToolTip(label_Space, "自动插入空格已开启(Ctrl+S)");
            this.toolTip1.SetToolTip(label_Tip, "自动提示已开启(Ctrl+T)");
            this.toolTip1.SetToolTip(label_Verity, "LRC/CRC校验显示已开启(Ctrl+Q)");
            this.toolTip1.SetToolTip(textBoxIn, "");
            this.toolTip1.SetToolTip(textBox_LRC, "LRC校验(双击复制)");
            this.toolTip1.SetToolTip(textBox_CRC, "CRC校验(双击复制)");
            this.toolTip1.SetToolTip(textBoxCon, "双击复制");
            this.toolTip1.SetToolTip(textBox_2AWrite, "");

            MyPara.Modbus.ReadData.SenorID = 1;
            MyPara.Modbus.ReadData.IsIDTtrue = false;
            MyPara.Modbus.ReadData.RtuCommand = new byte[8];
            MyPara.Modbus.ReadData.AsciiCommand = new byte[17];
            MyPara.Modbus.ReadData.Formula = new string[3];
            MyPara.Modbus.ReadData.sReceData = new string[4];

            MyPara.Modbus.Send.SensorID = 1;
            MyPara.Modbus.Send.Data = new uint[256];
            MyPara.Modbus.Send.sData = new string[256];
            MyPara.Modbus.Send.RtuCommand = new byte[256];
            MyPara.Modbus.Send.RecData = new byte[513];
            MyPara.Modbus.Send.sRtuCommandArray = new string[256];
            MyPara.Modbus.Send.MsgLen = new uint[256];
            MyPara.Modbus.Send.MsgStr = new string[256];
            MyPara.Modbus.Send.AsciiCommand = new byte[513];
            MyPara.Modbus.Send.sAsciiCommandArray = new string[513];

            MyPara.Modbus.Rece.Data = new uint[256];
            MyPara.Modbus.Rece.sData = new string[256];
            MyPara.Modbus.Rece.RtuCommand = new byte[256];
            MyPara.Modbus.Rece.RecData = new byte[513];
            MyPara.Modbus.Rece.sRtuCommandArray = new string[256];
            MyPara.Modbus.Rece.MsgStr = new string[256];
            MyPara.Modbus.Rece.AsciiCommand = new byte[513];
            MyPara.Modbus.Rece.sAsciiCommandArray = new string[513];

            MyPara.InOut.IsCtrlV = false;
            MyPara.InOut.IsAutoTip = false;
            MyPara.InOut.IsShowVerify = false;
            MyPara.InOut.IsAutoInsertSpace = false;
            MyPara.InOut.InsertSpacing = false;
            MyPara.InOut.TipNum = Para.tip.None;
            MyPara.InOut.InputData = new byte[513];
            MyPara.InOut.InputLen = 0;
            MyPara.InOut.InputStr = new string[513];
            MyPara.InOut.ConvData = new byte[513];
            MyPara.InOut.ConvStrArray = new string[513];

            MyPara.Colors.ReadColor = Color.Green;                  // 写颜色为蓝色
            MyPara.Colors.WriteColor = Color.Blue;                  // 读颜色为绿色
            MyPara.Colors.WarningColor = Color.Yellow;              // 警告色为黄色
            MyPara.Colors.ErrorColor = Color.Red;                   // 错误色为红色
            MyPara.Colors.DefaultBackColor = Control.DefaultBackColor;// 设置默认背景色
            MyPara.Colors.SendColor = Color.Black;                  // 发送字体颜色
            MyPara.Colors.RecColor = Color.Blue;                    // 接收字体颜色

            MyPara.RunState.Model = 0;                              // 运行模式初始值
            MyPara.RunState.InModel = (Para.inmodel)comboBoxInModel.SelectedIndex;  // 默认输入模式
            MyPara.RunState.ShowSendMask = 0x00000001;              // 显示发送掩码定义为第0位
            MyPara.RunState.ShowTimeMask = 0x00000002;              // 显示时间掩码定义为第1位
            MyPara.RunState.ShowAnalyzeMask = 0x00000004;           // 掩码定义为第2位(0：实时模式 1：仿真模式)
            MyPara.RunState.IsSerialUseMask = 0x40000000;           // 正在读取数据掩码定义为第30位(0：读写空闲状态 1：当前正在读写数据)
            MyPara.RunState.ReadWriteMast = 0x80000000;             // 读写掩码主义为第31位(0：读模式1：写模式)
            
            if (ShowTime.Checked)
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ShowTimeMask;
            else
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ShowTimeMask);

            if (ShowAnalyze.Checked)
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ShowAnalyzeMask;
            else
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ShowAnalyzeMask);

            MyPara.Modbus.Mark.IsMark = false;
            MyPara.Modbus.Mark.IsImport = false;
            MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFFF;
            MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFFF;

            // 按F1显示CHM帮助文档
            MyPara.HelpPath = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\Help\Modbus软件操作手册.pdf";
            helpProvider1.HelpNamespace = MyPara.HelpPath;
            helpProvider1.SetShowHelp(this, true);

            MyPara.DataTable = new DataTable();
            MyPara.DataTable.Columns.AddRange(new DataColumn[] { new DataColumn("序号"),
                                                                 new DataColumn("功能码(Hex)"),
                                                                 new DataColumn("寄存器地址(Hex)"),
                                                                 new DataColumn("描述"),
                                                                 new DataColumn("数据(Hex)"),
                                                                 new DataColumn("数据含义") });
            ParaTable.Columns.Clear();
            ParaTable.DataSource = MyPara.DataTable;
            for (int i = 0; i < ParaTable.Columns.Count; i++)       // 禁止列排序
            {
                ParaTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            ParaTable.Columns[0].Width = 50;                        // 表格宽度设置
            ParaTable.Columns[1].Width = 80;
            ParaTable.Columns[2].Width = 80;
            ParaTable.Columns[3].Width = 194;
            ParaTable.Columns[4].Width = 85;
            ParaTable.Columns[5].Width = 85;

            MyPara.SendTable = new DataTable();
            MyPara.SendTable.Columns.AddRange(new DataColumn[] { new DataColumn("字节数"),
                                                                 new DataColumn("描述"),
                                                                 new DataColumn("数据(Hex)"),
                                                                 new DataColumn("数据含义") });
            dataGridView2_SendTable.Columns.Clear();
            dataGridView2_SendTable.DataSource = MyPara.SendTable;
            for (int i = 0; i < dataGridView2_SendTable.Columns.Count; i++) // 禁止列排序
            {
                dataGridView2_SendTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView2_SendTable.Columns[0].Width = 60;
            dataGridView2_SendTable.Columns[1].Width = 150;
            dataGridView2_SendTable.Columns[2].Width = 85;
            dataGridView2_SendTable.Columns[3].Width = 245;
            dataGridView2_SendTable.RowTemplate.Height = 23;        // 行高23个像素

            MyPara.RecTable = new DataTable();
            MyPara.RecTable.Columns.AddRange(new DataColumn[] { new DataColumn("字节数"),
                                                                new DataColumn("描述"),
                                                                new DataColumn("数据(Hex)"),
                                                                new DataColumn("数据含义") });
            dataGridView3_RecTable.Columns.Clear();
            dataGridView3_RecTable.DataSource = MyPara.RecTable;
            for (int i = 0; i < dataGridView3_RecTable.Columns.Count; i++) // 禁止列排序
            {
                dataGridView3_RecTable.Columns[i].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            dataGridView3_RecTable.Columns[0].Width = 60;
            dataGridView3_RecTable.Columns[1].Width = 150;
            dataGridView3_RecTable.Columns[2].Width = 85;
            dataGridView3_RecTable.Columns[3].Width = 245;
            dataGridView3_RecTable.RowTemplate.Height = 23;         // 行高23个像素

            MyPara.Modbus.Send.SensorID = (uint)this.numericUpDown_SenorID.Value;
            MyPara.Modbus.Send.sSensorID = string.Format("{0:X2}", MyPara.Modbus.Send.SensorID);

            MyPara.Serial.IsOpen = false;                           // 串口默认为关闭状态
            MyPara.Serial.IsDataReceived = false;                   // 串口是否接收到数据
            MyPara.Serial.IsRefresh = false;
            RefreshSerial();                                        // 刷新串口

            comboBoxInModel.Text = "RTU2RTUAuto";                   // 输入模式默认设置
            ReadWrite.Text = "Read";                                // 传感器参数表默认为读
            Parity.Text = "NONE";                                   // 校验模式默认设置
            StopBit.Text = "1";                                     // 停止位默认设置
            Baud.Text = "9600";                                     // 奇偶校验默认设置

            RecoverySetting();                                      // 恢复历史设置
        }

        public Para MyPara = new Para();                            // 定义参数结构体

        public struct Para                                          // 结构体定义
        {
            public string Title;                                    // 标题栏名称
            public runstate RunState;                               // 运行状态
            public string HelpPath;                                 // 帮助文档路径
            public color Colors;                                    // 颜色定义
            public modbus Modbus;                                   // 代码参数结构体
            public inout InOut;                                     // 输入输出参数结构体
            public serial Serial;                                   // 串口参数结构体
            public DataTable DataTable;
            public DataTable SendTable;
            public DataTable RecTable;

            public struct runstate
            {
                public uint Model;                                  // 运行模式
                public inmodel InModel;                             // 输入模式
                public uint ShowSendMask;                           // 显示发送掩码           第0位     0：不显示发送     1：显示发送
                public uint ShowTimeMask;                           // 显示时间掩码           第1位     0：不显示时间戳   1：显示时间戳
                public uint ShowAnalyzeMask;                        // 模式掩码               第2位     0：不显示解析     1：显示解析
                public uint IsSerialUseMask;                        // 是否正在读写数据掩码   第30位    0：空闲状态       1：正在读写
                public uint ReadWriteMast;                          // 读写掩码               第31位    0：读模式         1：写模式
            }

            public enum inmodel
            {
                Normal = 0,                                         // 正常输入模式  转换不起作用
                RTU2ASCII,                                          // RTU 输入模式  转换会将RTU输入转换成ASCII
                ASCII2RTU,                                          // ASCII输入模式 转换会将ASCII输入转换成RTU
                RTU2RTUAuto,                                        // RTU 输入模式  该模式输入为RTU模式且不需要输入CRC校验码 转换会自动补全CRC校验码
                RTU2ASCIIAuto,                                      // RTU 输入模式  该模式输入与RTU模式且不需要输入CRC校验码 转换会自动将RTU转换成ASCII
            }

            public enum strerror
            {
                None = 0,
                CharError,
                LenError,
                TotalLenError,
            }

            public enum tip
            {
                None = 0,
                SenorID,
                SenorIDError,
                FunCode,
                FunCodeError,
                StartAddrH,
                StartAddrL,
                StartAddrNoInclude,
                RegNumH,
                RegNumL,
                RegNumError,
                ByteNum,
                ByteNumError,
                DataH,
                DataL,
                EquipmentInformation,
                DataNum2ALenError,
                CRCL,
                CRCH,
                CRCError,
                LenError,
                Finsh,
            }

            public struct color
            {
                public Color DefaultBackColor;
                public Color ReadColor;
                public Color WriteColor;
                public Color WarningColor;
                public Color ErrorColor;
                public Color SendColor;
                public Color RecColor;
            }

            public struct serial
            {
                public bool IsOpen;
                public bool IsRefresh;
                public bool IsDataReceived;
                public string[] Items;
                public string Name;
                public int Baud;
                public string Parity;
                public string Stop;
            }


            public struct inout
            {                
                public bool IsTrue;                                 // 输入数据是否正确
                public bool IsCtrlV;                                // 输入是否为粘贴
                public bool IsShowVerify;                           // 是否显示校验信息
                public bool InputFlag;                              // 输入字符串是否正确
                public bool IsAutoInsertSpace;                      // 是否自动插入空格
                public bool InsertSpacing;                          // 正在插入空格中
                public bool IsAutoTip;                              // 是否提示
                public uint DataNum;                                // 输入的数据编号
                public uint DataNum2A;                              // 2A写入数据编号
                public Para.tip TipNum;                             // 提示编号
                public byte[] InputData;                            // 输入数据
                public uint InputLen;                               // 输入长度
                public string[] InputStr;                           // 输入数据字符串

                public frameerror FrameError;                       // 帧错误码
                public string NormalStyle;                          // 正常模式为RTU或者ASCII模式
                public byte[] ConvData;                             // 转换字符串
                public uint ConvLen;                                // 转换数据长度
                public string ConvStr;                              // 转换字符串
                public string[] ConvStrArray;                       // 转换数据字符串
            }

            public enum frameerror
            {
                None = 0,                                           // 无错误
                SenorIDOverflow,                                    // 传感器地址溢出     正常范围是 0~247 
                FunCodeError,                                       // 功能码错误         仅支持 03/10 04 05 25 26/27 功能码
                StartAddrError,                                     // 起始ID错误         04功能码起始地址必须为偶数
                RegNumError,                                        // 寄存器数量错误     连读时
                ByteNumError,                                       // 字节数错误
                FrameHeaderError,                                   // 帧头错误
                FrameTailError,                                     // 帧尾错误
                FrameLenError,                                      // 帧长度错误
                CRCError,                                           // CRC校验错误
                LRCError,                                           // LRC校验错误
            }

            public struct modbus
            {
                public mark Mark;                                   // 导入参数
                public data Send;                                   // 传感器发送参数
                public data Rece;                                   // 传感器接收参数
                public string[,] SendStr;                           // 发送字符表
                public string[,] RecStr;                            // 接收字符表
                public readdata ReadData;                           // 双通道读数据参数

                public struct mark
                {
                    public bool IsImport;                           // 是否导入文件
                    public bool IsMark;                             // 是否标记
                    public uint StartMarkNum;                       // 标记起始序号
                    public uint EndMarkNum;                         // 标记结束序号
                    public uint StartFunCode;                       // 起始地址
                    public string sStartFunCode;                    // 起始地址字符串
                    public uint[] NewColorMark;                     // 颜色标记
                    public uint[] OldColorMark;                     // 颜色标记
                    public uint[,] FunCode;                         // 读取的功能码
                    public string[,] sFunCode;                      // 读取的功能码字符串
                    public uint[] MsgID;                            // 读取的消息ID
                    public string[] sMsgID;                         // 读取的消息ID字符串
                    public string[,] F04Formula;                    // 0x04功能码数据计算公式
                    public uint F04DefineCount;                     // 已定义公式个数
                }

                public struct readdata
                {
                    public bool IsIDTtrue;                          // ID是否正确
                    public bool FirstTime;
                    public bool IsCycleArrive;
                    public bool ReadFinsh;
                    public uint OverTimeCount;                      // 连续超时次数
                    public uint TimeOut;
                    public uint SenorID;
                    public uint MsgID;
                    public string RunModel;
                    public string sMsgID;
                    public byte[] RtuCommand;
                    public byte[] AsciiCommand;
                    public string[] Formula;                        // 数据计算条件及公式
                    public string[] sReceData;                      // 接收到的数据字符
                    public string Condition;                        // 转换后的条件
                    public string Formula1;
                    public string Formula2;
                }

                public struct data
                {
                    public uint SensorID;                           // 传感器ID
                    public uint FunCode;                            // 功能码
                    public uint StartAddr;                          // 起始地址
                    public uint RegNum;                             // 寄存器数量
                    public uint ByteNum;                            // 字节数
                    public uint[] Data;                             // 数据
                    public uint ErrorNum;                           // 错误编号
                    public uint CRC;                                // CRC校验
                    public uint LRC;                                // LRC校验

                    public string sSensorID;                        // 传感器ID
                    public string sFunCode;                         // 功能码
                    public string sStartAddr;                       // 起始地址
                    public string sRegNum;                          // 寄存器数量
                    public string sByteNum;                         // 字节数
                    public string[] sData;                          // 数据
                    public string sErrorNum;                        // 错误编号
                    public string sCRC;                             // CRC校验
                    public string sLRC;                             // LRC校验

                    public byte[] RtuCommand;                       // 命令数组
                    public uint RtuLen;                             // 命令数组长度
                    public string sRtuCommand;                      // 命令字符串 将一条指令合并成字符串 字节数据间以空格分开
                    public string[] sRtuCommandArray;               // 命令字符串数组 存储指令每字节数据字符

                    public byte[] AsciiCommand;
                    public uint AsciiLen;
                    public string sAsciiCommand;
                    public string[] sAsciiCommandArray;

                    public bool AnalyzeFinsh;                       // 解析完成
                    public uint[] MsgLen;                           // 信息长度
                    public string[] MsgStr;                         // 信息字符串      
                    public byte[] RecData;
                    public uint CorrectRecLen;                      // 正确回复长度
                    public uint RecLen;                             // 实际接收长度
                }
            }
        }

        public Para.strerror Str2Data()
        {
            string PATTERN = @"([^A-Fa-f0-9]|\s+?)+";

            char[] chs = { ' ' };
            string[] sData = textBoxIn.Text.Split(chs, options: StringSplitOptions.RemoveEmptyEntries);

            if (sData.Length == 0)
            {
                MyPara.InOut.InputLen = 0;
                return Para.strerror.None;
            }

            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
            {
                if (sData.Length > 256)                             // RTU输入支持最长256字节数据处理
                {
                    MyPara.InOut.InputLen = 0;
                    return Para.strerror.TotalLenError;
                }
            }

            if (MyPara.RunState.InModel == Para.inmodel.Normal)
            {
                if (sData.Length > 513)
                {
                    MyPara.InOut.InputLen = 0;
                    return Para.strerror.TotalLenError;
                }
                    
                if (sData.Length > 256)
                    MyPara.InOut.NormalStyle = "ASCII";
            }

            if (MyPara.RunState.InModel == Para.inmodel.RTU2RTUAuto ||
                MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
            {
                if (sData.Length > 254)                             // RTU不带校验输入支持最长254字节数据处理
                {
                    MyPara.InOut.InputLen = 0;
                    return Para.strerror.TotalLenError;
                }
            }

            if ((MyPara.RunState.InModel == Para.inmodel.ASCII2RTU) && (sData.Length > 513))
            {          
                MyPara.InOut.InputLen = 0;                          // 支持最长513个数据处理 超过长度不处理
                return Para.strerror.TotalLenError;
            }

            for (uint i = 0; i < sData.Length; i++)
            {
                if (sData[i].Length != 2)
                    return Para.strerror.LenError;

                if (System.Text.RegularExpressions.Regex.IsMatch(sData[i], PATTERN))    // 包含非法的16进制字符
                    return Para.strerror.CharError;
            }

            for (uint i = 0; i < sData.Length; i++)
                MyPara.InOut.InputData[i] = Convert.ToByte(sData[i], 16);
            MyPara.InOut.InputLen = (uint)sData.Length;

            return Para.strerror.None;
        }

        public void ModbusConv()                                    // 输入转换
        {
            switch (MyPara.RunState.InModel)
            {
                case Para.inmodel.Normal:
                    Array.Copy(MyPara.InOut.InputData, MyPara.InOut.ConvData, MyPara.InOut.InputLen);
                    MyPara.InOut.ConvLen = MyPara.InOut.InputLen;
                    break;

                case Para.inmodel.RTU2ASCII:
                    byte[] AsciiData = Modbus.Rtu2Ascii(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                    Array.Copy(AsciiData, MyPara.InOut.ConvData, AsciiData.Length);
                    MyPara.InOut.ConvLen = (uint)AsciiData.Length;
                    break;

                case Para.inmodel.ASCII2RTU:
                    if (MyPara.InOut.InputLen < 5)
                    {
                        MyPara.InOut.ConvLen = 0;
                        break;
                    }
                    else
                    {
                        byte[] Data = Modbus.Ascii2Rtu(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                        Array.Copy(Data, MyPara.InOut.ConvData, Data.Length);
                        MyPara.InOut.ConvLen = (uint)Data.Length;
                        break;
                    }

                case Para.inmodel.RTU2RTUAuto:
                    byte[] RtuData = Modbus.Rtu2RtuAuto(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                    Array.Copy(RtuData, MyPara.InOut.ConvData, RtuData.Length);
                    MyPara.InOut.ConvLen = (uint)RtuData.Length;
                    break;

                case Para.inmodel.RTU2ASCIIAuto:
                    AsciiData = Modbus.Rtu2AsciiAuto(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                    Array.Copy(AsciiData, MyPara.InOut.ConvData, AsciiData.Length);
                    MyPara.InOut.ConvLen = (uint)AsciiData.Length;
                    break;

                default:
                    Array.Copy(MyPara.InOut.InputData, MyPara.InOut.ConvData, MyPara.InOut.InputLen);
                    MyPara.InOut.ConvLen = MyPara.InOut.InputLen;
                    break;
            }
        }

        public void RecoverySetting()                               // 恢复历史设置
        {
            if (SerialCom.Items.Contains(Settings.Default.SerialCom))
                SerialCom.Text = Settings.Default.SerialCom;

            Baud.Text = Settings.Default.Baud;
            Parity.Text = Settings.Default.Parity;
            StopBit.Text = Settings.Default.StopBit;
            numericUpDown_SenorID.Value = Settings.Default.numericUpDown_SenorID;
            ReadWrite.Text = Settings.Default.ReadWrite;
            MsgID1.Text = Settings.Default.MsgID1;
            ReadCycle.Value = Settings.Default.ReadCycle;

            if (comboBoxInModel.Items.Contains(Settings.Default.comboBoxInModel))
                comboBoxInModel.Text = Settings.Default.comboBoxInModel;

            if (Settings.Default.label_Space)
            {
                MyPara.InOut.IsAutoInsertSpace = true;
                label_Space.Visible = true;
            }
            else
            {
                MyPara.InOut.IsAutoInsertSpace = false;
                label_Space.Visible = false;
            }
 
            if (Settings.Default.label_Tip)        
            {
                MyPara.InOut.IsAutoTip = true;
                label_Tip.Visible = true;
                GetInputTip();                              // 获取提示
            }    
            else
            {
                MyPara.InOut.IsAutoTip = false;
                label_Tip.Visible = false;
                GetInputTip();                              // 获取提示
            }

            if (Settings.Default.label_Verity)
            {
                MyPara.InOut.IsShowVerify = true;
                label_Verity.Visible = true;
                textBox_LRC.Visible = true;
                textBox_CRC.Visible = true;
            }
            else
            {
                MyPara.InOut.IsShowVerify = false;
            }

            if (Settings.Default.ShowSend)
            {
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ShowSendMask;
                ShowSend.Checked = true;
            }  
            else
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ShowSendMask);
                ShowSend.Checked = false;
            }

            if (Settings.Default.ShowTime)
            {
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ShowTimeMask;
                ShowTime.Checked = true;
            }
            else
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ShowTimeMask);
                ShowTime.Checked = false;
            }

            if (Settings.Default.ShowAnalyze)
            {
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ShowAnalyzeMask;
                ShowAnalyze.Checked = true;
            }
            else
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ShowAnalyzeMask);
                ShowAnalyze.Checked = false;
            }
        }

        public void GetSendAnalyzeData()
        {
            if (MyPara.Modbus.Send.FunCode == 0)
                return;

            if ((MyPara.RunState.Model & MyPara.RunState.ShowAnalyzeMask) != MyPara.RunState.ShowAnalyzeMask)
            {
                MyPara.SendTable.Rows.Clear();
                return;
            }

            MyPara.SendTable.Rows.Clear();
            if (MyPara.RunState.InModel == Para.inmodel.Normal)
            {
                if(MyPara.InOut.NormalStyle == "ASCII")
                    GetSendAsciiAnalyze();
                else
                    GetSendRtuAnalyze();
            }
                
            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
                GetSendAsciiAnalyze();
            if(MyPara.RunState.InModel == Para.inmodel.RTU2RTUAuto || MyPara.RunState.InModel == Para.inmodel.ASCII2RTU)
                GetSendRtuAnalyze();
        }

        public void GetSendAsciiAnalyze()
        {
            string RWFlag = "";
            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 ||
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
                RWFlag = "读";
            else
                RWFlag = "写";

            string[] Ascii = MyPara.Modbus.Send.sAsciiCommandArray;

            MyPara.SendTable.Rows.Add("001", "帧头", "3A", "ASCII模式帧起始符(冒号)");
            MyPara.SendTable.Rows.Add("002~003", "传感器地址", Ascii[1] + " " + Ascii[2], string.Format("传感器的地址为:{0:D}/0x{0:X2}", MyPara.Modbus.Send.SensorID));
            MyPara.SendTable.Rows.Add("004~005", "功能码", Ascii[3] + " " + Ascii[4], "0x" + MyPara.Modbus.Send.sFunCode + "," + GetFunCodeMeaning(MyPara.Modbus.Send.FunCode));

            if (MyPara.Modbus.Send.FunCode == 0x05 || MyPara.Modbus.Send.FunCode == 0x25 || MyPara.Modbus.Send.FunCode == 0x41)
            {
                MyPara.SendTable.Rows.Add("006~009", RWFlag + "寄存器地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Send.StartAddr) + GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("010~013", "写入寄存器数据", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("写入数据为:{0:D}/0x{0:X4}", MyPara.Modbus.Send.Data[0]));
                MyPara.SendTable.Rows.Add("014~015", "LRC校验数据", Ascii[13] + " " + Ascii[14], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Send.LRC));
                MyPara.SendTable.Rows.Add("016~017", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
            }

            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 ||
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
            {
                MyPara.SendTable.Rows.Add("006~009", RWFlag + "寄存器起始地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Send.StartAddr) + GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("010~013", "读寄存器数量", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("连续读取{0:D}/0x{0:X4}个寄存数据", MyPara.Modbus.Send.RegNum));
                MyPara.SendTable.Rows.Add("014~015", "LRC校验数据", Ascii[13] + " " + Ascii[14], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Send.LRC));
                MyPara.SendTable.Rows.Add("016~017", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
            }

            if (MyPara.Modbus.Send.FunCode == 0x10 || MyPara.Modbus.Send.FunCode == 0x27)
            {
                MyPara.SendTable.Rows.Add("006~009", RWFlag + "寄存器起始地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Send.StartAddr) + GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("010~013", "写寄存器数量", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("连续写入{0:D}/0x{0:X4}个寄存数据", MyPara.Modbus.Send.RegNum));
                MyPara.SendTable.Rows.Add("014~015", "写入数据字节数", Ascii[13] + " " + Ascii[14], string.Format("写入数据总字节数为:{0:D}/0x{0:X2}", MyPara.Modbus.Send.ByteNum));

                for (int i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                {
                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 16 + 4 * i, 17 + 4 * i),
                                              GetSendAddrMeaning((uint)(MyPara.Modbus.Send.StartAddr + i)),
                                              Ascii[15 + 4 * i] + " " + Ascii[16 + 4 * i] + " " + Ascii[17 + 4 * i] + " " + Ascii[18 + 4 * i],
                                              string.Format("写入数据为:{0:D}/0x{0:X4}", MyPara.Modbus.Send.Data[i]));
                }
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 16 + 4 * MyPara.Modbus.Send.RegNum, 17 + 4 * MyPara.Modbus.Send.RegNum),
                                          "LRC校验数据", Ascii[15 + 4 * MyPara.Modbus.Send.RegNum] + " " + Ascii[16 + 4 * MyPara.Modbus.Send.RegNum],
                                          string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Send.LRC));
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 18 + 4 * MyPara.Modbus.Send.RegNum, 19 + 4 * MyPara.Modbus.Send.RegNum),
                                          "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
            }

            if (MyPara.Modbus.Send.FunCode == 0x2A)
            {
                MyPara.SendTable.Rows.Add("006~009", RWFlag + "寄存器起始地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Send.StartAddr) + GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("010~013", "写寄存器数量", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("连续写入{0:D}/0x{0:X4}个寄存数据", MyPara.Modbus.Send.RegNum));

                uint Index = 13;
                for (uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                {
                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 1, Index + 2),
                                              string.Format("写入第{0:D}个信息字节数", i + 1),
                                              Ascii[Index] + " " + Ascii[Index + 1],
                                              string.Format("写入第{0:D}个信息字节数为{1:D}", i + 1, MyPara.Modbus.Send.MsgLen[i]));

                    string sMsgData = string.Format("{0:X2}", Ascii[Index + 2]);
                    for (uint j = 1; j < MyPara.Modbus.Send.MsgLen[i] * 2; j++)
                        sMsgData += " " + Ascii[Index + 2 + j];

                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 3, Index + 2 + MyPara.Modbus.Send.MsgLen[i]),
                                              string.Format("写入第{0:D}个信息内容", i + 1),
                                              sMsgData,
                                              string.Format("写入第{0:D}个信息内容为:", i + 1) + MyPara.Modbus.Send.MsgStr[i]);

                    Index = Index + 2 * MyPara.Modbus.Send.MsgLen[i] + 2;
                }
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 1, Index + 2),
                                          "LRC校验数据",
                                          Ascii[Index] + " " + Ascii[Index + 1],
                                          string.Format("LRC校验数据为:{0:D}/0x{1:X2}", MyPara.Modbus.Send.LRC, MyPara.Modbus.Send.LRC));
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 3, Index + 4),
                                          "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
            }
        }

        public void GetSendRtuAnalyze()
        {
            string RWFlag = "";
            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 || 
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
                RWFlag = "读";
            else
                RWFlag = "写";

            MyPara.SendTable.Rows.Add("001", "传感器地址", MyPara.Modbus.Send.sSensorID, string.Format("传感器的地址为:{0:D}", MyPara.Modbus.Send.SensorID));
            MyPara.SendTable.Rows.Add("002", "功能码", MyPara.Modbus.Send.sFunCode, GetFunCodeMeaning(MyPara.Modbus.Send.FunCode));

            if (MyPara.Modbus.Send.FunCode == 0x05 || MyPara.Modbus.Send.FunCode == 0x25 || MyPara.Modbus.Send.FunCode == 0x41)
            {
                MyPara.SendTable.Rows.Add("003~004", RWFlag + "寄存器地址", MyPara.Modbus.Send.sStartAddr, GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("005~006", "写入寄存器数据", MyPara.Modbus.Send.sData[0], "写入数据为:0x" + MyPara.Modbus.Send.sData[0]);
                MyPara.SendTable.Rows.Add("007~008", "CRC校验数据", MyPara.Modbus.Send.sCRC, "CRC校验数据低/高8位");
            }

            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 || 
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
            {
                MyPara.SendTable.Rows.Add("003~004", RWFlag + "寄存器起始地址", MyPara.Modbus.Send.sStartAddr, GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("005~006", "读寄存器数量", MyPara.Modbus.Send.sRegNum, string.Format("连续读取{0:D}个寄存数据", MyPara.Modbus.Send.RegNum));
                MyPara.SendTable.Rows.Add("007~008", "CRC校验数据", MyPara.Modbus.Send.sCRC, "CRC校验数据低/高8位");
            }

            if (MyPara.Modbus.Send.FunCode == 0x10 || MyPara.Modbus.Send.FunCode == 0x27)
            {
                MyPara.SendTable.Rows.Add("003~004", RWFlag + "寄存器起始地址", MyPara.Modbus.Send.sStartAddr, GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("005~006", "写寄存器数量", MyPara.Modbus.Send.sRegNum, string.Format("连续写入{0:D}个寄存数据", MyPara.Modbus.Send.RegNum));
                MyPara.SendTable.Rows.Add("007", "写入数据字节数", MyPara.Modbus.Send.sByteNum, string.Format("写入数据总字节数为:{0:D}", MyPara.Modbus.Send.ByteNum));

                for (int i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                {
                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 8 + 2 * i, 9 + 2 * i),
                                              GetSendAddrMeaning((uint)(MyPara.Modbus.Send.StartAddr + i)),
                                              MyPara.Modbus.Send.sData[i],
                                              string.Format("写入数据为:{0}", MyPara.Modbus.Send.Data[i]));
                }
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 8 + 2 * MyPara.Modbus.Send.RegNum, 9 + 2 * MyPara.Modbus.Send.RegNum),
                                          "CRC校验数据", MyPara.Modbus.Send.sCRC, "CRC校验数据低/高8位");
            }

            if (MyPara.Modbus.Send.FunCode == 0x2A)
            {
                MyPara.SendTable.Rows.Add("003~004", RWFlag + "寄存器起始地址", MyPara.Modbus.Send.sStartAddr, GetSendAddrMeaning(MyPara.Modbus.Send.StartAddr));
                MyPara.SendTable.Rows.Add("005~006", "写寄存器数量", MyPara.Modbus.Send.sRegNum, string.Format("连续写入{0:D}个寄存数据", MyPara.Modbus.Send.RegNum));

                uint Index = 6;
                for(uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                {
                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}", Index + 1),
                                              string.Format("写入第{0:D}个信息字节数", i + 1),
                                              string.Format("{0:X2}", MyPara.Modbus.Send.MsgLen[i]),
                                              string.Format("写入第{0:D}个信息字节数为:{1:D}", i + 1, MyPara.Modbus.Send.MsgLen[i]));

                    byte[] MsgByte = Encoding.Default.GetBytes(MyPara.Modbus.Send.MsgStr[i]);
                    string sMsgData = string.Format("{0:X2}", MsgByte[0]);
                    for (uint j = 1; j < MyPara.Modbus.Send.MsgLen[i]; j++)
                        sMsgData += string.Format(" {0:X2}", MsgByte[j]);

                    MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 2, Index + 1 + MyPara.Modbus.Send.MsgLen[i]),
                                              string.Format("写入第{0:D}个信息内容", i + 1),
                                              sMsgData,
                                              string.Format("写入第{0:D}个信息内容为:", i + 1) + MyPara.Modbus.Send.MsgStr[i]);
                    
                    Index = MyPara.InOut.ConvData[Index] + Index + 1;
                }
                MyPara.SendTable.Rows.Add(string.Format("{0:D3}~{1:D3}", MyPara.InOut.ConvLen - 1, MyPara.InOut.ConvLen),
                                          "CRC校验数据", MyPara.Modbus.Send.sCRC, "CRC校验数据低/高8位");
            }
        }

        public void GetReceAnalyzeData()
        {
            if (MyPara.Modbus.Rece.FunCode == 0)
                return;

            if ((MyPara.RunState.Model & MyPara.RunState.ShowAnalyzeMask) != MyPara.RunState.ShowAnalyzeMask)
            {
                MyPara.RecTable.Rows.Clear();
                return;
            }

            string RWFlag = "";
            if (MyPara.Modbus.Rece.FunCode == 0x03 || MyPara.Modbus.Rece.FunCode == 0x04 || 
                MyPara.Modbus.Rece.FunCode == 0x26 || MyPara.Modbus.Rece.FunCode == 0x2B)
                RWFlag = "读";
            else
                RWFlag = "写";

            MyPara.RecTable.Rows.Clear();
            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
            {
                string[] Ascii = MyPara.Modbus.Rece.sAsciiCommandArray;

                MyPara.RecTable.Rows.Add("001", "帧头", "3A", "ASCII模式帧起始符(冒号)");
                MyPara.RecTable.Rows.Add("002~003", "传感器地址", Ascii[1] + " " + Ascii[2], string.Format("传感器的地址为:{0:D}/0x{1:X2}", MyPara.Modbus.Rece.SensorID, MyPara.Modbus.Rece.SensorID));
                MyPara.RecTable.Rows.Add("004~005", "功能码", Ascii[3] + " " + Ascii[4], "0x" + MyPara.Modbus.Rece.sFunCode + "," + GetFunCodeMeaning(MyPara.Modbus.Rece.FunCode));

                if (MyPara.Modbus.Rece.FunCode > 0x80)
                {
                    MyPara.RecTable.Rows.Add("006~007", "错误码", Ascii[5] + " " + Ascii[6], string.Format("{0:D}/0x{0:X2},", MyPara.Modbus.Rece.ErrorNum) + GetErrorMsg(MyPara.Modbus.Rece.ErrorNum));
                    MyPara.RecTable.Rows.Add("008~009", "LRC校验数据", Ascii[7] + " " + Ascii[8], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.LRC));
                    MyPara.RecTable.Rows.Add("010~011", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x05 || MyPara.Modbus.Rece.FunCode == 0x25)
                {
                    MyPara.RecTable.Rows.Add("006~009", RWFlag + "寄存器地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Rece.StartAddr) + GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("010~013", "写入寄存器数据", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("写入数据为:{0:D}/0x{0:X4}", MyPara.Modbus.Rece.Data[0]));
                    MyPara.RecTable.Rows.Add("014~015", "LRC校验数据", Ascii[13] + " " + Ascii[14], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.LRC));
                    MyPara.RecTable.Rows.Add("016~017", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x03 || MyPara.Modbus.Rece.FunCode == 0x04 || MyPara.Modbus.Rece.FunCode == 0x26)
                {
                    MyPara.RecTable.Rows.Add("006~007", "数据总字节数", Ascii[5] + " " + Ascii[6], string.Format("读取数据总字节数为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.ByteNum));

                    int i;
                    for(i = 0; i < MyPara.Modbus.Rece.ByteNum / 2; i++)
                    {
                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 8 + 4 * i, 11 + 4 * i),
                                                 GetReceAddrMeaning((uint)(MyPara.Modbus.Rece.StartAddr + i)),
                                                 Ascii[7 + 4 * i] + " " + Ascii[8 + 4 * i] + " " + Ascii[9 + 4 * i] + " " + Ascii[10 + 4 * i],
                                                 string.Format("读取数据为:{0:D}/0x{0:X4}", MyPara.Modbus.Rece.Data[i]));
                    }
                    MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 8 + 4 * i, 9 + 4 * i), "LRC校验数据", Ascii[7 + 4 * i] + " " + Ascii[8 + 4 * i], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.LRC));
                    MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 10 + 4 * i, 11 + 4 * i), "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x10 || MyPara.Modbus.Rece.FunCode == 0x27 || MyPara.Modbus.Rece.FunCode == 0x2A)
                {
                    MyPara.RecTable.Rows.Add("006~009", RWFlag + "寄存器起始地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], string.Format("0x{0:X4},", MyPara.Modbus.Rece.StartAddr) + GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("010~013", "写入寄存器数量", Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12], string.Format("连续写入{0:D}/0x{0:X4}个寄存器", MyPara.Modbus.Rece.RegNum));
                    MyPara.RecTable.Rows.Add("014~015", "LRC校验数据", Ascii[13] + " " + Ascii[14], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.LRC));
                    MyPara.RecTable.Rows.Add("016~017", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x2B)
                {
                    MyPara.RecTable.Rows.Add("006~009", "读寄存器数量", MyPara.Modbus.Rece.sRegNum, string.Format("连续读取{0:D}个寄存数据", MyPara.Modbus.Rece.RegNum));

                    uint Index = 9;
                    for (uint i = 0; i < MyPara.Modbus.Rece.RegNum; i++)
                    {
                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 1, Index + 2),
                                                 string.Format("读取第{0:D}个信息字节数", i + 1),
                                                 string.Format("{0:X2}", MyPara.Modbus.Rece.MsgLen[i]),
                                                 string.Format("读取第{0:D}个信息字节数为{1:D}", i + 1, MyPara.Modbus.Rece.MsgLen[i]));

                        byte[] MsgByte = Encoding.Default.GetBytes(MyPara.Modbus.Rece.MsgStr[i]);
                        string sMsgData = string.Format("{0:X2}", MsgByte[0]);
                        for (uint j = 1; j < MyPara.Modbus.Rece.MsgLen[i]; j++)
                            sMsgData += string.Format(" {0:X2}", MsgByte[j]);

                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 3, Index + 1 + 2 * MyPara.Modbus.Rece.MsgLen[i]),
                                                 string.Format("读取第{0:D}个信息内容", i + 1),
                                                 sMsgData,
                                                 string.Format("读取第{0:D}个信息内容为:", i + 1) + MyPara.Modbus.Rece.MsgStr[i]);

                        Index = Index + MyPara.Modbus.Rece.MsgLen[i] * 2 + 2;
                    }
                    MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 1, Index + 2),
                                             "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x41)
                {
                    MyPara.RecTable.Rows.Add("006~009", RWFlag + "寄存器地址", Ascii[5] + " " + Ascii[6] + " " + Ascii[7] + " " + Ascii[8], 
                                             "0x" + MyPara.Modbus.Rece.sStartAddr + "," + GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("010~013", "数据字节数", 
                                             Ascii[9] + " " + Ascii[10] + " " + Ascii[11] + " " + Ascii[12],
                                             string.Format("数据字节数为:{0:D}/0x{0:X4}", MyPara.Modbus.Rece.ByteNum));
                    MyPara.RecTable.Rows.Add("014~015", "数据内容", Ascii[13] + " " + Ascii[14], string.Format("{0:D}/0x{0:X2},", MyPara.Modbus.Rece.Data[0]) + GetF41DataMeaning(MyPara.Modbus.Rece.Data[0]));
                    MyPara.RecTable.Rows.Add("016~017", "LRC校验数据", Ascii[15] + " " + Ascii[16], string.Format("LRC校验数据为:{0:D}/0x{0:X2}", MyPara.Modbus.Rece.LRC));
                    MyPara.RecTable.Rows.Add("018~019", "帧尾", "0D 0A", "ASCII模式帧结束符(换行)");
                }
            }
            else
            {
                MyPara.RecTable.Rows.Add("001", "传感器地址", MyPara.Modbus.Rece.sSensorID, string.Format("传感器的地址为:{0:D}", MyPara.Modbus.Rece.SensorID));
                MyPara.RecTable.Rows.Add("002", "功能码", MyPara.Modbus.Rece.sFunCode, GetFunCodeMeaning(MyPara.Modbus.Rece.FunCode));

                if (MyPara.Modbus.Rece.FunCode > 0x80)
                {
                    MyPara.RecTable.Rows.Add("003", "错误码", MyPara.Modbus.Rece.sErrorNum, GetErrorMsg(MyPara.Modbus.Rece.ErrorNum));
                    MyPara.RecTable.Rows.Add("004~005", "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x05 || MyPara.Modbus.Rece.FunCode == 0x25)
                {
                    MyPara.RecTable.Rows.Add("003~004", RWFlag + "寄存器地址", MyPara.Modbus.Rece.sStartAddr, GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("005~006", "写入寄存器数据", MyPara.Modbus.Rece.sData[0], "写入数据为:0x" + MyPara.Modbus.Rece.sData[0]);
                    MyPara.RecTable.Rows.Add("007~008", "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x03 || MyPara.Modbus.Rece.FunCode == 0x04 || MyPara.Modbus.Rece.FunCode == 0x26)
                {
                    MyPara.RecTable.Rows.Add("003", "数据总字节数", MyPara.Modbus.Rece.sByteNum, string.Format("读取数据总字节数为:{0:D}", MyPara.Modbus.Rece.ByteNum));

                    int i;
                    for (i = 0; i < MyPara.Modbus.Rece.ByteNum / 2; i++)
                    {
                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 4 + 2 * i, 5 + 2 * i),
                                                 GetReceAddrMeaning((uint)(MyPara.Modbus.Rece.StartAddr + i)),
                                                 MyPara.Modbus.Rece.sData[i],
                                                 string.Format("读取数据为:{0:D}", MyPara.Modbus.Rece.Data[i]));
                    }
                    MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", 4 + 2 * i, 5 + 2 * i), "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x10 || MyPara.Modbus.Rece.FunCode == 0x27 || MyPara.Modbus.Rece.FunCode == 0x2A)
                {
                    MyPara.RecTable.Rows.Add("003~004", RWFlag + "寄存器起始地址", MyPara.Modbus.Rece.sStartAddr, GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("005~006", "写寄存器数量", MyPara.Modbus.Rece.sRegNum, string.Format("连续写入{0:D}个寄存数据", MyPara.Modbus.Rece.RegNum));
                    MyPara.RecTable.Rows.Add("007~008", "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x2B)
                {
                    MyPara.RecTable.Rows.Add("003~004", "读寄存器数量", MyPara.Modbus.Rece.sRegNum, string.Format("连续读取{0:D}个寄存数据", MyPara.Modbus.Rece.RegNum));

                    uint Index = 4;
                    for (uint i = 0; i < MyPara.Modbus.Rece.RegNum; i++)
                    {
                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}", Index + 1),
                                                 string.Format("读取第{0:D}个信息字节数", i + 1),
                                                 string.Format("{0:X2}", MyPara.Modbus.Rece.MsgLen[i]),
                                                 string.Format("读取第{0:D}个信息字节数为{1:D}", i + 1, MyPara.Modbus.Rece.MsgLen[i]));

                        byte[] MsgByte = Encoding.Default.GetBytes(MyPara.Modbus.Rece.MsgStr[i]);
                        string sMsgData = string.Format("{0:X2}", MsgByte[0]);
                        for (uint j = 1; j < MyPara.Modbus.Rece.MsgLen[i]; j++)
                            sMsgData += string.Format(" {0:X2}", MsgByte[j]);

                        MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", Index + 2, Index + 1 + MyPara.Modbus.Rece.MsgLen[i]),
                                                 string.Format("读取第{0:D}个信息内容", i + 1),
                                                 sMsgData,
                                                 string.Format("读取第{0:D}个信息内容为:", i + 1) + MyPara.Modbus.Rece.MsgStr[i]);

                        Index = MyPara.InOut.ConvData[Index] + Index + 1;
                    }
                    MyPara.RecTable.Rows.Add(string.Format("{0:D3}~{1:D3}", MyPara.InOut.ConvLen - 1, MyPara.InOut.ConvLen),
                                             "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }

                if (MyPara.Modbus.Rece.FunCode == 0x41)
                {
                    MyPara.RecTable.Rows.Add("003~004", RWFlag + "寄存器地址", MyPara.Modbus.Rece.sStartAddr, GetReceAddrMeaning(MyPara.Modbus.Rece.StartAddr));
                    MyPara.RecTable.Rows.Add("005~006", "数据字节数",
                                             string.Format("{0:X2} {1:X2}", MyPara.Modbus.Rece.ByteNum / 256, MyPara.Modbus.Rece.ByteNum % 256),
                                             string.Format("数据字节数为:{0:D}", MyPara.Modbus.Rece.ByteNum)); 
                    MyPara.RecTable.Rows.Add("007", "数据内容", MyPara.Modbus.Rece.sData[0], GetF41DataMeaning(MyPara.Modbus.Rece.Data[0]));
                    MyPara.RecTable.Rows.Add("008~009", "CRC校验数据", MyPara.Modbus.Rece.sCRC, "CRC校验数据低/高8位");
                }
            }
        }

        public string GetF41DataMeaning(uint Data)
        {
            string str = "";

            switch (Data)
            {
                case 0:
                    str = "接收成功";
                    break;
                case 1:
                    str = "接收失败";
                    break;
                case 2:
                    str = "校验码错误";
                    break;
                default:
                    str = "";
                    break;
            }
            return str;
        }

        public string GetFunCodeMeaning(uint FunCode)
        {
            string FunCodeMeaning;

            switch (FunCode)
            {
                case 0x03:
                    FunCodeMeaning = "读保持寄存器";
                    break;
                case 0x04:
                    FunCodeMeaning = "读输入寄存器";
                    break;
                case 0x05:
                    FunCodeMeaning = "写单线圈寄存器";
                    break;
                case 0x10:
                    FunCodeMeaning = "写保持寄存器";
                    break;
                case 0x25:
                    FunCodeMeaning = "内部写单线圈寄存器";
                    break;
                case 0x26:
                    FunCodeMeaning = "内部读保持寄存器";
                    break;
                case 0x27:
                    FunCodeMeaning = "内部写保持寄存器";
                    break;
                case 0x2A:
                    FunCodeMeaning = "写设备信息";
                    break;
                case 0x2B:
                    FunCodeMeaning = "读设备信息";
                    break;
                case 0x41:
                    FunCodeMeaning = "升级APP专用";
                    break;
                default:
                    FunCodeMeaning = "读写错误";
                    break;
            }

            return FunCodeMeaning;
        }

        public string GetSendAddrMeaning(uint Addr)
        {
            string RWFlag = "";
            string AddrMeaming = "";            

            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 || 
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
                RWFlag = "读";
            else
                RWFlag = "写";

            if (MyPara.Modbus.Mark.IsImport)
            {
                int Index = -1;
                for (int i = 0; i < MyPara.Modbus.Mark.MsgID.Length; i++)
                {
                    if((MyPara.Modbus.Mark.MsgID[i] == Addr) && 
                      ((MyPara.Modbus.Mark.FunCode[i, 0] == MyPara.Modbus.Send.FunCode) || 
                       (MyPara.Modbus.Mark.FunCode[i, 1] == MyPara.Modbus.Send.FunCode)))
                    {
                        Index = i;
                        break;
                    }
                }

                if (Index != -1)
                    AddrMeaming = RWFlag + "\"" + ParaTable.Rows[Index].Cells[3].Value.ToString() + "\"寄存器";
                else
                    AddrMeaming = RWFlag + string.Format("0x{0:X4}寄存器", Addr);
            }
            else
                AddrMeaming = RWFlag + string.Format("0x{0:X4}寄存器", Addr);

            return AddrMeaming;
        }

        public string GetErrorMsg(uint FunCode)
        {
            string ErrorMsg = "";

            switch (FunCode)
            {
                case 0x01:
                    ErrorMsg = "非支持的功能码";
                    break;
                case 0x02:
                    ErrorMsg = "寄存器地址不正确";
                    break;
                case 0x03:
                    ErrorMsg = "寄存器值超出范围";
                    break;
                default:
                    ErrorMsg = "";
                    break;
            }

            return ErrorMsg;
        }

        public string GetReceAddrMeaning(uint Addr)
        {
            string RWFlag = "";
            string AddrMeaming = "";

            if (MyPara.Modbus.Rece.FunCode == 0x03 || MyPara.Modbus.Rece.FunCode == 0x04 || 
                MyPara.Modbus.Rece.FunCode == 0x26 || MyPara.Modbus.Rece.FunCode == 0x2B)
                RWFlag = "读";
            else
                RWFlag = "写";

            if (MyPara.Modbus.Mark.IsImport)
            {
                int Index = -1;
                for (int i = 0; i < MyPara.Modbus.Mark.MsgID.Length; i++)
                {
                    if ((MyPara.Modbus.Mark.MsgID[i] == Addr) &&
                      ((MyPara.Modbus.Mark.FunCode[i, 0] == MyPara.Modbus.Rece.FunCode) ||
                       (MyPara.Modbus.Mark.FunCode[i, 1] == MyPara.Modbus.Rece.FunCode)))
                    {
                        Index = i;
                        break;
                    }
                }

                if (Index != -1)
                    AddrMeaming = RWFlag + "\"" + ParaTable.Rows[Index].Cells[3].Value.ToString() + "\"寄存器";
                else
                    AddrMeaming = RWFlag + string.Format("0x{0:X4}寄存器", Addr);
            }
            else
                AddrMeaming = RWFlag + string.Format("0x{0:X4}寄存器", Addr);

            return AddrMeaming;
        }

        public void GetInputTip()                                   // 获取输入提示
        {
            if (MyPara.InOut.IsAutoTip)                             // 自动提示打开
            {
                if ((MyPara.RunState.InModel == Para.inmodel.RTU2ASCII) ||
                    (MyPara.RunState.InModel == Para.inmodel.RTU2RTUAuto) ||
                    (MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto))
                {                                                   // 仅此三种模式提示 即输入为RTU模式时提供提示
                    switch (MyPara.InOut.InputLen)
                    {
                        case 0:
                            MyPara.InOut.TipNum = Para.tip.SenorID;
                            break;
                        case 1:
                            MyPara.InOut.TipNum = Para.tip.FunCode;
                            break;
                        case 2:
                            MyPara.InOut.TipNum = Para.tip.StartAddrH;
                            break;
                        case 3:
                            MyPara.InOut.TipNum = Para.tip.StartAddrL;
                            break;
                        default:
                            break;
                    }

                    if (MyPara.InOut.InputLen >= 4)
                    {
                        if (!IsSupportFunCode(MyPara.InOut.InputData[1]))
                            MyPara.InOut.TipNum = Para.tip.FunCodeError;
                        else
                        {
                            if ((MyPara.InOut.InputData[1] == 0x03) || (MyPara.InOut.InputData[1] == 0x04) ||
                                (MyPara.InOut.InputData[1] == 0x05) || (MyPara.InOut.InputData[1] == 0x25) ||
                                (MyPara.InOut.InputData[1] == 0x26) || (MyPara.InOut.InputData[1] == 0x2B))
                            {
                                switch (MyPara.InOut.InputLen)
                                {
                                    case 4:
                                        if ((MyPara.InOut.InputData[1] == 0x05) || (MyPara.InOut.InputData[1] == 0x25))
                                            MyPara.InOut.TipNum = Para.tip.DataH;
                                        else
                                            MyPara.InOut.TipNum = Para.tip.RegNumH;
                                        break;
                                    case 5:
                                        if ((MyPara.InOut.InputData[1] == 0x05) || (MyPara.InOut.InputData[1] == 0x25))
                                            MyPara.InOut.TipNum = Para.tip.DataL;
                                        else
                                            MyPara.InOut.TipNum = Para.tip.RegNumL;
                                        break;
                                }

                                if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                {
                                    if (MyPara.InOut.InputLen == 6)
                                        MyPara.InOut.TipNum = Para.tip.CRCL;

                                    if (MyPara.InOut.InputLen == 7)
                                        MyPara.InOut.TipNum = Para.tip.CRCH;

                                    if (MyPara.InOut.InputLen == 8)
                                        MyPara.InOut.TipNum = Para.tip.Finsh;

                                    if (MyPara.InOut.InputLen > 8)
                                        MyPara.InOut.TipNum = Para.tip.LenError;
                                }
                                else
                                {
                                    if (MyPara.InOut.InputLen == 6)
                                        MyPara.InOut.TipNum = Para.tip.Finsh;

                                    if (MyPara.InOut.InputLen > 6)
                                        MyPara.InOut.TipNum = Para.tip.LenError;
                                }
                            }

                            if ((MyPara.InOut.InputData[1] == 0x10) || (MyPara.InOut.InputData[1] == 0x27))
                            {
                                switch (MyPara.InOut.InputLen)
                                {
                                    case 4:
                                        MyPara.InOut.TipNum = Para.tip.RegNumH;
                                        break;
                                    case 5:
                                        MyPara.InOut.TipNum = Para.tip.RegNumL;
                                        break;
                                    case 6:
                                        MyPara.InOut.TipNum = Para.tip.ByteNum;
                                        break;
                                }

                                if (MyPara.InOut.InputLen >= 7)
                                {
                                    uint TotalByte = (uint)(9 + 2 * (MyPara.InOut.InputData[4] * 256 + MyPara.InOut.InputData[5]));
                                    if ((MyPara.InOut.InputLen >= 7) && (MyPara.InOut.InputLen < TotalByte - 2))
                                    {
                                        MyPara.InOut.DataNum = (MyPara.InOut.InputLen - 5) / 2;
                                        if((MyPara.InOut.InputLen - 5) % 2 == 0)
                                            MyPara.InOut.TipNum = Para.tip.DataH;
                                        else
                                            MyPara.InOut.TipNum = Para.tip.DataL;
                                    }

                                    if (MyPara.InOut.InputLen >= TotalByte - 2)
                                    {
                                        if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                        {
                                            if (MyPara.InOut.InputLen == TotalByte - 2)
                                                MyPara.InOut.TipNum = Para.tip.CRCL;

                                            if (MyPara.InOut.InputLen == TotalByte - 1)
                                                MyPara.InOut.TipNum = Para.tip.CRCH;

                                            if (MyPara.InOut.InputLen == TotalByte)
                                                MyPara.InOut.TipNum = Para.tip.Finsh;

                                            if (MyPara.InOut.InputLen > TotalByte)
                                                MyPara.InOut.TipNum = Para.tip.LenError;
                                        }
                                        else
                                        {
                                            if (MyPara.InOut.InputLen == TotalByte - 2)
                                                MyPara.InOut.TipNum = Para.tip.Finsh;

                                            if (MyPara.InOut.InputLen > TotalByte - 2)
                                                MyPara.InOut.TipNum = Para.tip.LenError;
                                        }     
                                    }
                                }
                            }

                            if (MyPara.InOut.InputData[1] == 0x2A)
                            {
                                if (MyPara.InOut.InputLen == 4)
                                    MyPara.InOut.TipNum = Para.tip.RegNumH;

                                if (MyPara.InOut.InputLen == 5)
                                    MyPara.InOut.TipNum = Para.tip.RegNumL;

                                if(MyPara.InOut.InputLen == 6)
                                {
                                    if ((MyPara.InOut.InputData[4] * 256 + MyPara.InOut.InputData[5]) == 0)
                                        MyPara.InOut.TipNum = Para.tip.RegNumError;
                                    else
                                    {
                                        MyPara.InOut.DataNum2A = 1;
                                        MyPara.InOut.TipNum = Para.tip.EquipmentInformation;
                                        textBox_2AWrite.Visible = true;
                                        textBox_2AWrite.Focus();    // 将光标定位在textBox_2AWrite上
                                    }
                                }

                                if(MyPara.InOut.InputLen > 6)
                                {
                                    MyPara.InOut.DataNum2A = 1;
                                    int RegNum = MyPara.InOut.InputData[4] * 256 + MyPara.InOut.InputData[5];

                                    if(RegNum == 0)
                                        MyPara.InOut.TipNum = Para.tip.RegNumError;
                                    else
                                    {
                                        uint Index = 6;
                                        while (true)
                                        {
                                            if(MyPara.InOut.DataNum2A < (RegNum + 1))
                                            {
                                                if (MyPara.InOut.InputData[Index] == 0)
                                                    MyPara.InOut.TipNum = Para.tip.DataNum2ALenError;
                                                else
                                                {
                                                    if (MyPara.InOut.InputLen < (MyPara.InOut.InputData[Index] + Index + 1))
                                                    {
                                                            MyPara.InOut.TipNum = Para.tip.EquipmentInformation;
                                                            break;
                                                    }

                                                    if (MyPara.InOut.InputLen == (MyPara.InOut.InputData[Index] + Index + 1))
                                                    { 
                                                        if (MyPara.InOut.DataNum2A < RegNum)
                                                        {
                                                            Index = MyPara.InOut.InputData[Index] + Index + 1;
                                                            MyPara.InOut.DataNum2A++;
                                                            MyPara.InOut.TipNum = Para.tip.EquipmentInformation;
                                                            textBox_2AWrite.Visible = true;
                                                            textBox_2AWrite.Focus();    // 将光标定位在textBox_2AWrite上
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            if (!MyPara.Modbus.Mark.IsMark)
                                                                textBoxIn.Focus();    // 将光标定位在textBox_2AWrite上

                                                            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                                                MyPara.InOut.TipNum = Para.tip.CRCH;
                                                            else
                                                                MyPara.InOut.TipNum = Para.tip.Finsh;
                                                            break;
                                                        }
                                                    }

                                                    if (MyPara.InOut.InputLen > (MyPara.InOut.InputData[Index] + Index + 1))
                                                    {
                                                        Index = MyPara.InOut.InputData[Index] + Index + 1;
                                                        MyPara.InOut.DataNum2A++;
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                switch (MyPara.InOut.InputLen - Index)
                                                {
                                                    case 0:
                                                        if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                                            MyPara.InOut.TipNum = Para.tip.CRCH;
                                                        else
                                                            MyPara.InOut.TipNum = Para.tip.LenError;
                                                        break;
                                                    case 1:
                                                        if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                                            MyPara.InOut.TipNum = Para.tip.CRCL;
                                                        else
                                                            MyPara.InOut.TipNum = Para.tip.LenError;
                                                        break;

                                                    case 2:
                                                        if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                                            MyPara.InOut.TipNum = Para.tip.Finsh;
                                                        else
                                                            MyPara.InOut.TipNum = Para.tip.LenError;
                                                        break;
                                                    default:
                                                        MyPara.InOut.TipNum = Para.tip.LenError;
                                                        break;
                                                }

                                                textBox_2AWrite.Visible = false;
                                                textBox_2AWrite.Text = "";
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (MyPara.InOut.InputData[1] == 0x41)
                            {
                                if (MyPara.InOut.InputLen == 4)
                                    MyPara.InOut.TipNum = Para.tip.DataH;

                                if (MyPara.InOut.InputLen == 5)
                                    MyPara.InOut.TipNum = Para.tip.DataL;

                                if (MyPara.InOut.InputLen == 6)
                                {
                                    if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                        MyPara.InOut.TipNum = Para.tip.CRCH;
                                    else
                                        MyPara.InOut.TipNum = Para.tip.Finsh;
                                }

                                if (MyPara.InOut.InputLen == 7)
                                {
                                    if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                        MyPara.InOut.TipNum = Para.tip.CRCL;
                                    else
                                        MyPara.InOut.TipNum = Para.tip.LenError;
                                }

                                if (MyPara.InOut.InputLen == 8)
                                {
                                    if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII)
                                        MyPara.InOut.TipNum = Para.tip.Finsh;
                                    else
                                        MyPara.InOut.TipNum = Para.tip.LenError;
                                }

                                if (MyPara.InOut.InputLen > 8)
                                    MyPara.InOut.TipNum = Para.tip.LenError;
                            }
                        }
                    }

                    GetInputTipMsg();
                }
                else
                    this.toolTip1.SetToolTip(textBoxIn, "");
            }
            else
                this.toolTip1.SetToolTip(textBoxIn, "");
        }

        public void GetInputTipMsg()
        {
            string TipMsg = "";

            switch (MyPara.InOut.TipNum)
            {
                case Para.tip.None:
                    TipMsg = "";
                    break;
                case Para.tip.SenorID:
                    TipMsg = "请输入传感器地址(取值1~247)";
                    break;
                case Para.tip.FunCode:
                    TipMsg = "请输入功能码：\r\n03/10：读写保持寄存器" +
                                            "\r\n04：读输入寄存器" +
                                            "\r\n05：写单线圈寄存器" +
                                            "\r\n25：内部写单线圈寄存器" +
                                            "\r\n26/27：内部读写保持寄存器" +
                                            "\r\n2B/2A：读写设备信息" +
                                            "\r\n41：升级APP专用";
                    break;
                case Para.tip.FunCodeError:
                    TipMsg = "非支持的功能码,无法解析";
                    break;
                case Para.tip.StartAddrH:
                    TipMsg = "请输入寄存器地址高字节";
                    break;
                case Para.tip.StartAddrL:
                    TipMsg = "请输入寄存器地址低字节";
                    break;
                case Para.tip.RegNumH:
                    TipMsg = "请输入寄存器数量高字节";
                    break;
                case Para.tip.RegNumL:
                    TipMsg = "请输入寄存器数量低字节";
                    break;
                case Para.tip.RegNumError:
                    TipMsg = "寄存器数量为0错误(必须是正整数)";
                    break;
                case Para.tip.ByteNum:
                    TipMsg = "请输入写入数据总字节数(寄存器数量的2倍)";
                    break;
                case Para.tip.DataH:
                    if (MyPara.InOut.InputData[1] == 0x41)
                        TipMsg = "请输入写入数据高字节，默认为0x00";
                    else
                        TipMsg = string.Format("请输入写入第{0:D}个数据高字节", MyPara.InOut.DataNum);
                    break;
                case Para.tip.DataL:
                    if (MyPara.InOut.InputData[1] == 0x41)
                        TipMsg = "请输入写入数据低字节，默认为0x00";
                    else
                        TipMsg = string.Format("请输入写入第{0:D}个数据低字节", MyPara.InOut.DataNum);
                    break;
                case Para.tip.EquipmentInformation:
                    if (MyPara.Modbus.Mark.IsImport)
                    {
                        string sMsgID = string.Format("{0:X4}", MyPara.InOut.InputData[2] * 256 + MyPara.InOut.InputData[3] + MyPara.InOut.DataNum2A - 1);
                        string sFunCode = string.Format("{0:X2}", MyPara.InOut.InputData[1]);

                        int Index = -1;
                        for (uint i = 0; i < MyPara.Modbus.Mark.sMsgID.Length; i++)
                        {
                            if ((MyPara.Modbus.Mark.sFunCode[i, 0] == sFunCode) || (MyPara.Modbus.Mark.sFunCode[i, 1] == sFunCode) && 
                                (MyPara.Modbus.Mark.sMsgID[i] == sMsgID))
                            {
                                Index = (int)i;
                                break;
                            }
                        }

                        if (Index >= 0)
                        {
                            TipMsg = "请输入写入\"" + ParaTable.Rows[Index].Cells[3].Value.ToString() + "\"的信息内容";
                            this.toolTip1.SetToolTip(textBox_2AWrite, "请输入写入\"" + ParaTable.Rows[Index].Cells[3].Value.ToString() + "\"的信息内容(不超过64字节，回车结束)");
                        }
                        else
                        {
                            TipMsg = string.Format("请输入写入0x{0:X4}的信息内容(不超过64字节)", MyPara.InOut.InputData[2] * 256 + MyPara.InOut.InputData[3] + MyPara.InOut.DataNum2A - 1);
                            this.toolTip1.SetToolTip(textBox_2AWrite, string.Format("请输入写入0x{0:X4}的信息内容(不超过64字节，回车结束)", MyPara.InOut.InputData[2] * 256 + MyPara.InOut.InputData[3] + MyPara.InOut.DataNum2A - 1));
                        }
                    }
                    else
                    {
                        TipMsg = string.Format("请输入写入0x{0:X4}的信息内容(不超过64字节)", MyPara.InOut.InputData[2] * 256 + MyPara.InOut.InputData[3] + MyPara.InOut.DataNum2A - 1);
                        this.toolTip1.SetToolTip(textBox_2AWrite, string.Format("请输入写入0x{0:X4}的信息内容(不超过64字节，回车结束)", MyPara.InOut.InputData[2] * 256 + MyPara.InOut.InputData[3] + MyPara.InOut.DataNum2A - 1));
                    }
                    
                    break;
                case Para.tip.DataNum2ALenError:
                    TipMsg = "信息长度为0错误(长度必须是正整数)";
                    break;   
                case Para.tip.CRCL:
                    TipMsg = "请输入CRC校验低字节";
                    break;
                case Para.tip.CRCH:
                    TipMsg = "请输入CRC校验高字节";
                    break;
                case Para.tip.Finsh:
                    TipMsg = "指令输入完成";
                    break;
                case Para.tip.LenError:
                    TipMsg = "输入指令长度过长,请删减";
                    break;
                default:
                    TipMsg = "";
                    break;
            }

            this.toolTip1.SetToolTip(textBoxIn, TipMsg);
        }

        public class Modbus
        {
            public static uint GetLRC(byte[] Data, uint DataLen)
            {
                uint LRC = 0;

                for (int i = 0; i < DataLen; i++)
                    LRC += Data[i];

                LRC = (256 - (LRC % 256)) % 256;
                return LRC;
            }

            public static uint GetCRC(byte[] Data, uint DataLen)
            {
                uint CRCLo = 0xFF;                                  // 高CRC字节初始化
                uint CRCHi = 0xFF;                                  // 低CRC字节初始化 
                uint uIndex = 0;                                    // CRC循环中的索引 

                for (uint i = 0; i < DataLen; i++)                  // 传输消息缓冲区
                {
                    uIndex = CRCLo ^ Data[i];                       // 计算CRC 
                    CRCLo = CRCHi ^ auchCRCLo[uIndex];
                    CRCHi = auchCRCHi[uIndex];
                }

                return (CRCHi << 8) | CRCLo;
            }

            public static byte[] Rtu2Ascii(byte[] RtuData, uint DataLen)
            {
                return Rtu2AsciiAuto(RtuData, DataLen - 2);
            }

            public static byte[] Ascii2Rtu(byte[] AsciiData, uint AsciiLen)
            {
                uint RtuLen = (AsciiLen - 1) / 2;
                byte[] RtuData = new byte[RtuLen];

                for(int i = 1; i < AsciiLen - 4; i++)
                {
                    if (AsciiData[i] >= '0' && AsciiData[i] <= '9')
                        AsciiData[i] = (byte)(AsciiData[i] - '0');
                    else
                    {
                        if (AsciiData[i] >= 'A' && AsciiData[i] <= 'F')
                            AsciiData[i] = (byte)(AsciiData[i] - 'A' + 10);
                    }
                }

                for (int i = 0; i < RtuLen - 2; i++)
                {
                    RtuData[i] = (byte)((AsciiData[(i << 1) + 1] << 4) + AsciiData[(i << 1) + 2]);
                }

                uint CRC = GetCRC(RtuData, RtuLen - 2);
                RtuData[RtuLen - 2] = (byte)CRC;
                RtuData[RtuLen - 1] = (byte)(CRC >> 8);

                return RtuData;
            }

            public static byte[] Rtu2RtuAuto(byte[] RtuData, uint DataLen)
            {
                byte[] NewRtuData = new byte[DataLen + 2];
                
                for(int i = 0; i < DataLen; i++)
                {
                    NewRtuData[i] = RtuData[i];
                }

                uint CRC = GetCRC(RtuData, DataLen);
                NewRtuData[DataLen] = (byte)CRC;
                NewRtuData[DataLen + 1] = (byte)(CRC >> 8);

                return NewRtuData;
            }

            public static byte[] Rtu2AsciiAuto(byte[] RtuData, uint DataLen)
            {
                byte[] AsciiData = new byte[2 * DataLen + 5];

                AsciiData[0] = 0x3A;

                for(int i = 0; i < DataLen; i++)
                {
                    AsciiData[(i << 1) + 1] = (byte)(RtuData[i] >> 4);
                    AsciiData[(i << 1) + 2] = (byte)(RtuData[i] & 0x0F);
                }
                uint LRC = GetLRC(RtuData, DataLen);
                AsciiData[2 * DataLen + 1] = (byte)(LRC >> 4);
                AsciiData[2 * DataLen + 2] = (byte)(LRC & 0x0F);

                for (int i = 1; i <= 2 * DataLen + 2; i++) 
                {
                    if (AsciiData[i] <= 9)
                        AsciiData[i] = (byte)(AsciiData[i] + 48);
                    else
                        AsciiData[i] = (byte)(AsciiData[i] - 10 + 65);
                }

                AsciiData[2 * DataLen + 3] = 0x0D;                  // ASCII模式结束符
                AsciiData[2 * DataLen + 4] = 0x0A;
                
                return AsciiData;
            }

            public static byte[] Ascii2RtuData(byte[] AsciiData, uint AsciiLen) // 除帧头帧尾以外的RTU数据
            {
                uint RtuLen = (AsciiLen - 3) / 2;
                byte[] RtuData = new byte[RtuLen];

                for (int i = 1; i < AsciiLen - 2; i++)
                {
                    if (AsciiData[i] >= '0' && AsciiData[i] <= '9')
                        AsciiData[i] = (byte)(AsciiData[i] - '0');
                    else
                    {
                        if (AsciiData[i] >= 'A' && AsciiData[i] <= 'F')
                            AsciiData[i] = (byte)(AsciiData[i] - 'A' + 10);
                    }
                }

                for (int i = 0; i < RtuLen; i++)
                {
                    RtuData[i] = (byte)((AsciiData[(i << 1) + 1] << 4) + AsciiData[(i << 1) + 2]);
                }

                return RtuData;
            }

            static byte[] auchCRCLo = { 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
                                        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
                                        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40, 0x00, 0xC1, 0x81, 0x40,
                                        0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0, 0x80, 0x41, 0x00, 0xC1,
                                        0x81, 0x40, 0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41,
                                        0x00, 0xC1, 0x81, 0x40, 0x01, 0xC0, 0x80, 0x41, 0x01, 0xC0,
                                        0x80, 0x41, 0x00, 0xC1, 0x81, 0x40 };

            /* CRC高位字节值表*/
            static byte[] auchCRCHi = { 0x00, 0xC0, 0xC1, 0x01, 0xC3, 0x03, 0x02, 0xC2, 0xC6, 0x06,
                                        0x07, 0xC7, 0x05, 0xC5, 0xC4, 0x04, 0xCC, 0x0C, 0x0D, 0xCD,
                                        0x0F, 0xCF, 0xCE, 0x0E, 0x0A, 0xCA, 0xCB, 0x0B, 0xC9, 0x09,
                                        0x08, 0xC8, 0xD8, 0x18, 0x19, 0xD9, 0x1B, 0xDB, 0xDA, 0x1A,
                                        0x1E, 0xDE, 0xDF, 0x1F, 0xDD, 0x1D, 0x1C, 0xDC, 0x14, 0xD4,
                                        0xD5, 0x15, 0xD7, 0x17, 0x16, 0xD6, 0xD2, 0x12, 0x13, 0xD3,
                                        0x11, 0xD1, 0xD0, 0x10, 0xF0, 0x30, 0x31, 0xF1, 0x33, 0xF3,
                                        0xF2, 0x32, 0x36, 0xF6, 0xF7, 0x37, 0xF5, 0x35, 0x34, 0xF4,
                                        0x3C, 0xFC, 0xFD, 0x3D, 0xFF, 0x3F, 0x3E, 0xFE, 0xFA, 0x3A,
                                        0x3B, 0xFB, 0x39, 0xF9, 0xF8, 0x38, 0x28, 0xE8, 0xE9, 0x29,
                                        0xEB, 0x2B, 0x2A, 0xEA, 0xEE, 0x2E, 0x2F, 0xEF, 0x2D, 0xED,
                                        0xEC, 0x2C, 0xE4, 0x24, 0x25, 0xE5, 0x27, 0xE7, 0xE6, 0x26,
                                        0x22, 0xE2, 0xE3, 0x23, 0xE1, 0x21, 0x20, 0xE0, 0xA0, 0x60,
                                        0x61, 0xA1, 0x63, 0xA3, 0xA2, 0x62, 0x66, 0xA6, 0xA7, 0x67,
                                        0xA5, 0x65, 0x64, 0xA4, 0x6C, 0xAC, 0xAD, 0x6D, 0xAF, 0x6F,
                                        0x6E, 0xAE, 0xAA, 0x6A, 0x6B, 0xAB, 0x69, 0xA9, 0xA8, 0x68,
                                        0x78, 0xB8, 0xB9, 0x79, 0xBB, 0x7B, 0x7A, 0xBA, 0xBE, 0x7E,
                                        0x7F, 0xBF, 0x7D, 0xBD, 0xBC, 0x7C, 0xB4, 0x74, 0x75, 0xB5,
                                        0x77, 0xB7, 0xB6, 0x76, 0x72, 0xB2, 0xB3, 0x73, 0xB1, 0x71,
                                        0x70, 0xB0, 0x50, 0x90, 0x91, 0x51, 0x93, 0x53, 0x52, 0x92,
                                        0x96, 0x56, 0x57, 0x97, 0x55, 0x95, 0x94, 0x54, 0x9C, 0x5C,
                                        0x5D, 0x9D, 0x5F, 0x9F, 0x9E, 0x5E, 0x5A, 0x9A, 0x9B, 0x5B,
                                        0x99, 0x59, 0x58, 0x98, 0x88, 0x48, 0x49, 0x89, 0x4B, 0x8B,
                                        0x8A, 0x4A, 0x4E, 0x8E, 0x8F, 0x4F, 0x8D, 0x4D, 0x4C, 0x8C,
                                        0x44, 0x84, 0x85, 0x45, 0x87, 0x47, 0x46, 0x86, 0x82, 0x42,
                                        0x43, 0x83, 0x41, 0x81, 0x80, 0x40 };
        }

        private void ShowSend_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ShowSend.Checked)                              // 显示发送对应位置1(第0位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model | this.MyPara.RunState.ShowSendMask;
            }
            else                                                    // 显示发送对应位清0(第0位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model & (~this.MyPara.RunState.ShowSendMask);
            }
        }

        private void ShowTime_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ShowTime.Checked)                              // 显示时间对应位置1(第1位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model | this.MyPara.RunState.ShowTimeMask;
            }
            else                                                    // 显示时间对应位清0(第1位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model & (~this.MyPara.RunState.ShowTimeMask);
            }
        }

        private void Simulation_CheckedChanged(object sender, EventArgs e)
        {
            if (this.ShowAnalyze.Checked)                            // 显示发送对应位置1(第2位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model | this.MyPara.RunState.ShowAnalyzeMask;
                GetSendAnalyzeData();
                GetReceAnalyzeData();
            }
            else                                                    // 实时模式对应位清0(第2位)
            {
                this.MyPara.RunState.Model = this.MyPara.RunState.Model & (~this.MyPara.RunState.ShowAnalyzeMask);
                MyPara.SendTable.Rows.Clear();
                MyPara.RecTable.Rows.Clear();
            }
        }

        private void comboBoxInModel_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.MyPara.RunState.InModel = (Para.inmodel)this.comboBoxInModel.SelectedIndex;                 // 设置输入模式

            if (MyPara.Modbus.Mark.IsMark)                           // 如果标记选中 按模式重新转换
            {
                textBoxIn.Text = "";
                TextBoxInShow();
            }
            else
            {
                string InputStr = textBoxIn.Text;                   // 若不做此三步 转换不一定成功
                textBoxIn.Text = "";
                textBoxIn.Text = InputStr;
                TextBoxInConv();
            }
        }

        private void OpenFile_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Excel|*.xls;*.xlsx";          // 文件筛选器设定
            openFileDialog1.Title = "传感器参数导入";

            string Path = System.IO.Directory.GetParent(System.Environment.CurrentDirectory).Parent.FullName + @"\SenorLibrary";
            openFileDialog1.InitialDirectory = Path;                // 设置相对路径

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string FileName = openFileDialog1.FileName;
                string StrConn = "Provider = Microsoft.Ace.OLEDB.12.0; Data Source = " +
                                  FileName + "; Extended Properties = 'Excel 12.0; HDR=No; IMEX=1;'";

                DataSet dataset = new DataSet();

                OleDbDataAdapter DataBase = new OleDbDataAdapter();
                try
                {
                    DataBase = new OleDbDataAdapter("SELECT * FROM [$A1:R65536]", StrConn);  // dt.Rows[0][2].ToString().Trim() 为第个Excel表里第一个Sheet的名字
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "错误");
                    return;
                }
                DataBase.Fill(dataset, "Sheet1");                   // Sheet1 并非导入Excel表格名称 是自定义数表的名称
               
                if (dataset.Tables["Sheet1"].Columns.Count != 6)
                {
                    MessageBox.Show("参数表列数错误", "错误");
                    return;
                }

                SenorModel.Text = dataset.Tables["Sheet1"].Rows[0][1].ToString();
                VersionTime.Text = dataset.Tables["Sheet1"].Rows[0][3].ToString();
                VersionID.Text = dataset.Tables["Sheet1"].Rows[0][5].ToString();

                MyPara.DataTable.Rows.Clear();
                for (int i = 3; i < dataset.Tables["Sheet1"].Rows.Count; i++)
                    MyPara.DataTable.Rows.Add(dataset.Tables["Sheet1"].Rows[i].ItemArray);
                ParaTable.DataSource = MyPara.DataTable;

                int ParaRowCount = MyPara.DataTable.Rows.Count;
                MyPara.Modbus.Mark.OldColorMark = new uint[ParaRowCount];
                MyPara.Modbus.Mark.NewColorMark = new uint[ParaRowCount];
                MyPara.Modbus.Mark.FunCode = new uint[ParaRowCount, 2];
                MyPara.Modbus.Mark.sFunCode = new string[ParaRowCount, 2];
                MyPara.Modbus.Mark.MsgID = new uint[ParaRowCount];
                MyPara.Modbus.Mark.sMsgID = new string[ParaRowCount];
                
                for (int i = 0; i < ParaRowCount; i++)              // 1行表格描述 1行空行 1行表头 结尾1行空白  因此减4行
                {
                    string Row1 = ParaTable.Rows[i].Cells[1].Value.ToString();
                    string[] row = Row1.Split('/');
                    if (row.Length == 2)
                    {
                        if ((Row1.Length != 5) || (Row1.Substring(2, 1) != "/"))
                        {
                            MessageBox.Show(string.Format("Excel表格第{0:D}行,第B列内容错误,请检查", i + 4), "错误");
                            return;
                        }

                        MyPara.Modbus.Mark.sFunCode[i, 0] = row[0];
                        MyPara.Modbus.Mark.sFunCode[i, 1] = row[1];
                        try
                        {
                            MyPara.Modbus.Mark.FunCode[i, 0] = Convert.ToUInt32(row[0], 16);
                            MyPara.Modbus.Mark.FunCode[i, 1] = Convert.ToUInt32(row[1], 16);
                        }
                        catch(Exception ex)
                        {
                            MessageBox.Show(string.Format("Excel表格第{0:D}行,第B列内容错误,请检查\r\n" + ex.Message, i + 4), "错误");
                            return;
                        }
                    }
                    else
                    {
                        if ((Row1.Length != 2))
                        {
                            MessageBox.Show(string.Format("Excel表格第{0:D}行,第B列内容错误,请检查!", i + 4), "错误");
                            return;
                        }

                        MyPara.Modbus.Mark.sFunCode[i, 0] = row[0];
                        MyPara.Modbus.Mark.sFunCode[i, 1] = "00";
                        try
                        {
                            MyPara.Modbus.Mark.FunCode[i, 0] = Convert.ToUInt32(row[0], 16);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(string.Format("Excel表格第{0:D}行,第B列内容错误,请检查!\r\n" + ex.Message, i + 4), "错误");
                            return;
                        }
                        MyPara.Modbus.Mark.FunCode[i, 1] = 0;
                    }

                    MyPara.Modbus.Mark.sMsgID[i] = ParaTable.Rows[i].Cells[2].Value.ToString();

                    if (MyPara.Modbus.Mark.sMsgID[i].Length != 4)
                    {
                        MessageBox.Show(string.Format("Excel表格第{0:D}行,第C列内容长度为不4错误,请检查!", i + 4), "错误");
                        return;
                    }
                    try
                    {
                        MyPara.Modbus.Mark.MsgID[i] = Convert.ToUInt32(MyPara.Modbus.Mark.sMsgID[i], 16);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(string.Format("Excel表格第{0:D}行,第C列内容错误,请检查!\r\n" + ex.Message, i + 4), "错误");
                        return;
                    }
                }

                uint F04Len = 0;
                for (int i = 0; i < ParaRowCount; i++)
                {
                    if (MyPara.Modbus.Mark.sFunCode[i, 0] == "04")
                        F04Len++;
                }
                MyPara.Modbus.Mark.F04Formula = new string[F04Len / 2, 4];

                F04Len = 0;
                bool IsFindF04 = false;
                for (int i = 0; i < ParaRowCount; i++)
                {
                    if (MyPara.Modbus.Mark.sFunCode[i, 0] == "04")
                    {
                        IsFindF04 = true;
                        string CellValue = ParaTable.Rows[i].Cells[4].Value.ToString();
                        if ((CellValue.Length != 0) && ((MyPara.Modbus.Mark.MsgID[i] % 2) == 0))
                        {
                            MyPara.Modbus.Mark.F04Formula[F04Len, 0] = MyPara.Modbus.Mark.sMsgID[i];
                            MyPara.Modbus.Mark.F04Formula[F04Len, 1] = CellValue;
                            string[] Formula = ParaTable.Rows[i].Cells[5].Value.ToString().Split('\\');
                            MyPara.Modbus.Mark.F04Formula[F04Len, 2] = Formula[0];
                            if(Formula.Length == 2)
                                MyPara.Modbus.Mark.F04Formula[F04Len, 3] = Formula[1];
                            F04Len++;
                        }
                    }
                    else
                    {
                        if (IsFindF04)
                            break;
                    }
                }
                MyPara.Modbus.Mark.F04DefineCount = F04Len;

                MyPara.Modbus.Mark.IsImport = true;
            }
        }

        private void ReadData_Click(object sender, EventArgs e)
        {
            if (this.ReadData.Text == "读数")
            {
                if (MyPara.Modbus.ReadData.IsIDTtrue)
                {
                    if (MyPara.Modbus.ReadData.SenorID == 0)
                    {
                        this.ReadData.BackColor = this.MyPara.Colors.ErrorColor;
                        MessageBox.Show("传感器地址不能为0", "提示");
                    }
                    else
                    {
                        if (!MyPara.Serial.IsOpen)
                        {
                            MessageBox.Show("串口未打开", "提示");
                            return;
                        }

                        GetReadDataMsg();

                        // 输入模式为 RTU2ASCII或RTU2ASCIIAuto模式时 读数据为ASCII模式 其它输入模式时读数据为RTU模式
                        if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
                            MyPara.Modbus.ReadData.RunModel = "ASCII";
                        else
                            MyPara.Modbus.ReadData.RunModel = "RTU";

                        this.ReadData.Text = "停止";
                        this.ReadData.BackColor = this.MyPara.Colors.ReadColor;
                        MyPara.Modbus.ReadData.FirstTime = true;
                        timer_ReadData.Interval = 10;               // 立即执行读操作
                        timer_ReadData.Enabled = true;
                    }
                }
                else
                {
                    this.ReadData.BackColor = this.MyPara.Colors.ErrorColor;
                    MessageBox.Show("寄存器地址未输入", "提示");
                }
            }
            else
                CloseReadData();
        }

        public void GetReadDataMsg()
        {
            MyPara.Modbus.ReadData.SenorID = Convert.ToUInt32(numericUpDown_SenorID.Value);
            MyPara.Modbus.ReadData.RtuCommand[0] = (byte)MyPara.Modbus.ReadData.SenorID;
            MyPara.Modbus.ReadData.RtuCommand[1] = 0x04;
            MyPara.Modbus.ReadData.RtuCommand[2] = (byte)(MyPara.Modbus.ReadData.MsgID >> 8);
            MyPara.Modbus.ReadData.RtuCommand[3] = (byte)(MyPara.Modbus.ReadData.MsgID);
            MyPara.Modbus.ReadData.RtuCommand[4] = 0;
            MyPara.Modbus.ReadData.RtuCommand[5] = 2;
            uint CRC = Modbus.GetCRC(MyPara.Modbus.ReadData.RtuCommand, 6);
            MyPara.Modbus.ReadData.RtuCommand[6] = (byte)CRC;
            MyPara.Modbus.ReadData.RtuCommand[7] = (byte)(CRC >> 8);
            MyPara.Modbus.ReadData.OverTimeCount = 0;

            MyPara.Modbus.ReadData.AsciiCommand = Modbus.Rtu2Ascii(MyPara.Modbus.ReadData.RtuCommand, 8);

            GetFormula();                                           // 获取数据计算公式
        }

        public void GetFormula()
        {
            for (int i = 0; i < MyPara.Modbus.Mark.F04DefineCount; i++)
            {
                if(MyPara.Modbus.ReadData.sMsgID == MyPara.Modbus.Mark.F04Formula[i, 0])
                {
                    MyPara.Modbus.ReadData.Formula[0] = MyPara.Modbus.Mark.F04Formula[i, 1];
                    MyPara.Modbus.ReadData.Formula[1] = MyPara.Modbus.Mark.F04Formula[i, 2];
                    MyPara.Modbus.ReadData.Formula[2] = MyPara.Modbus.Mark.F04Formula[i, 3];
                    return;
                }
            }

            MyPara.Modbus.ReadData.Formula[0] = "";
            MyPara.Modbus.ReadData.Formula[1] = "";
            MyPara.Modbus.ReadData.Formula[2] = "";
        }

        public void CloseReadData()
        {
            this.ReadData.Text = "读数";
            this.ReadData.BackColor = this.MyPara.Colors.DefaultBackColor;
            timer_ReadData.Enabled = false;
            MyPara.Modbus.ReadData.IsCycleArrive = false;
        }

        private void textBoxIn_TextChanged(object sender, EventArgs e)
        {
            TextBoxInConv();
        }

        public void TextBoxInConv()                                 // 输入内容转换
        {
            if (MyPara.InOut.InsertSpacing)                         // 自动插入空格不作处理 
            {
                MyPara.InOut.InsertSpacing = false;
                return;
            }

            if (MyPara.InOut.IsCtrlV)                               // 复制内容则需要将小写转换成大写
            {
                MyPara.InOut.IsCtrlV = false;
                textBoxIn.Text = textBoxIn.Text.ToUpper();
                textBoxIn.Focus();                                  // 获取焦点
                textBoxIn.Select(textBoxIn.TextLength, 0);          // 光标定位到文本最后
                textBoxIn.ScrollToCaret();                          // 滚动到光标处
            }

            byte[] InputText = Encoding.UTF8.GetBytes(textBoxIn.Text);
            MyPara.SendTable.Rows.Clear();

            if (InputText.Length == 0)                              // 输入为空 功能码赋值为0 转换内容清空
            {
                MyPara.Modbus.Send.FunCode = 0;
                MyPara.Modbus.Send.RtuLen = 0;                      // 发送数据长度为0
                MyPara.Modbus.Send.AsciiLen = 0;
                MyPara.InOut.InputLen = 0;
                textBoxCon.Text = "";
                GetInputTip();                                      // 获取提示
                ShowVerity(null, 0);
                return;
            }

            MyPara.InOut.InputFlag = true;
            MyPara.InOut.FrameError = Para.frameerror.None;

            if (MyPara.RunState.InModel == Para.inmodel.Normal)
            {
                Para.strerror err = Str2Data();
                if (err != 0)
                {
                    MyPara.Modbus.Send.FunCode = 0;
                    MyPara.InOut.InputFlag = false;
                    MyPara.InOut.InputLen = 0;

                    return;
                }
                else
                {
                    if (MyPara.InOut.IsShowVerify)
                        ShowVerity(MyPara.InOut.InputData, MyPara.InOut.InputLen);

                    MyPara.InOut.ConvLen = MyPara.InOut.InputLen;
                    for (int i = 0; i < MyPara.InOut.ConvLen; i++)
                    {
                        MyPara.InOut.ConvData[i] = MyPara.InOut.InputData[i];
                        MyPara.InOut.ConvStrArray[i] = string.Format("{0:X2}", MyPara.InOut.ConvData[i]);
                    }
                    MyPara.InOut.ConvStr = MyPara.InOut.ConvStrArray[0];
                    for (int i = 1; i < MyPara.InOut.InputLen; i++)
                        MyPara.InOut.ConvStr += " " + MyPara.InOut.ConvStrArray[i];

                    if ((MyPara.InOut.InputLen >= 3) &&
                        (MyPara.InOut.InputData[0] == 0x3A) && 
                        (MyPara.InOut.InputData[MyPara.InOut.InputLen - 2] == 0x0D) &&
                        (MyPara.InOut.InputData[MyPara.InOut.InputLen - 1] == 0x0A))
                    {
                        MyPara.InOut.NormalStyle = "ASCII";

                        if ((MyPara.InOut.InputLen % 2) != 1)
                            MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                        else
                        {
                            byte[] RtuData = Modbus.Ascii2Rtu(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                            GetInputRtuAnalyze(RtuData, (uint)RtuData.Length);
                        }
                    }
                    else
                    {
                        if (MyPara.InOut.InputLen <= 256)
                        {
                            MyPara.InOut.NormalStyle = "RTU";
                            GetInputRtuAnalyze(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                        } 
                    }
                }
            }
            else
            {
                Para.strerror err = Str2Data();

                if (err != 0 || MyPara.InOut.InputLen < 2)
                {
                    MyPara.Modbus.Send.FunCode = 0;
                    MyPara.InOut.InputFlag = false;

                    if (err == 0)
                    {
                        GetInputTip();                              // 获取提示
                        textBoxCon.Text = "";

                        if (MyPara.InOut.IsShowVerify)
                            ShowVerity(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                    }
                    return;
                }

                if (MyPara.InOut.IsShowVerify)
                    ShowVerity(MyPara.InOut.InputData, MyPara.InOut.InputLen);

                ModbusConv();
                MyPara.InOut.ConvData[MyPara.InOut.ConvLen] = (byte)0;

                MyPara.InOut.ConvStr = string.Format("{0:X2}", MyPara.InOut.ConvData[0]);
                for (uint i = 1; i < MyPara.InOut.ConvLen; i++)
                {
                    MyPara.InOut.ConvStrArray[i] = string.Format("{0:X2}", MyPara.InOut.ConvData[i]);
                    MyPara.InOut.ConvStr += " " + MyPara.InOut.ConvStrArray[i];
                }

                if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
                {
                    byte[] RtuData = Modbus.Ascii2Rtu(MyPara.InOut.ConvData, MyPara.InOut.ConvLen);
                    GetInputRtuAnalyze(RtuData, (uint)RtuData.Length);
                }
                else
                    GetInputRtuAnalyze(MyPara.InOut.ConvData, MyPara.InOut.ConvLen);

            }

            GetInputTip();                                          // 获取提示
            GetCommandArray();                                      // 获取命令数组

            textBoxCon.Text = MyPara.InOut.ConvStr;
            if (MyPara.InOut.FrameError == Para.frameerror.None) 
                GetSendAnalyzeData();
        }

        public void ShowVerity(byte[] Data, uint DataLen)
        {
            if (DataLen == 0)
            {
                textBox_LRC.Text = "";
                textBox_CRC.Text = "";
                return;
            }

            uint LRC = Modbus.GetLRC(Data, DataLen);
            textBox_LRC.Text = string.Format("{0:X2}", LRC);
            uint CRC = Modbus.GetCRC(Data, DataLen);
            textBox_CRC.Text = string.Format("{0:X2} {1:X2}", CRC % 256, CRC / 256);
        }

        public void GetInputRtuAnalyze(byte[] RtuData, uint DataLen)
        {
            MyPara.Modbus.Send.RtuLen = DataLen;

            MyPara.Modbus.Send.SensorID = RtuData[0];
            MyPara.Modbus.Send.sSensorID = string.Format("{0:X2}", MyPara.Modbus.Send.SensorID);

            if (MyPara.Modbus.Send.SensorID > 247)
                MyPara.InOut.FrameError = Para.frameerror.SenorIDOverflow;

            MyPara.Modbus.Send.FunCode = RtuData[1];
            MyPara.Modbus.Send.sFunCode = string.Format("{0:X2}", MyPara.Modbus.Send.FunCode);

            if (IsSupportFunCode(MyPara.Modbus.Send.FunCode) == false)
            {
                MyPara.InOut.FrameError = Para.frameerror.FunCodeError;
                return;
            }

            if (MyPara.Modbus.Send.FunCode == 0x05 || MyPara.Modbus.Send.FunCode == 0x25 || MyPara.Modbus.Send.FunCode == 0x41)
            {
                if (DataLen != 8)
                    MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                else
                {
                    MyPara.Modbus.Send.StartAddr = (uint)((RtuData[2] << 8) + RtuData[3]);
                    MyPara.Modbus.Send.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Send.StartAddr);
                    MyPara.Modbus.Send.Data[0] = (uint)((RtuData[4] << 8) + RtuData[5]);
                    MyPara.Modbus.Send.sData[0] = string.Format("{0:X4}", MyPara.Modbus.Send.Data[0]);
                    MyPara.Modbus.Send.CRC = (uint)((RtuData[6] << 8) + RtuData[7]);
                    MyPara.Modbus.Send.sCRC = string.Format("{0:X4}", MyPara.Modbus.Send.CRC);
                }
            }

            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 || 
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
            {
                if (DataLen != 8)
                    MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                else
                {
                    MyPara.Modbus.Send.StartAddr = (uint)((RtuData[2] << 8) + RtuData[3]);
                    MyPara.Modbus.Send.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Send.StartAddr);
                    MyPara.Modbus.Send.RegNum = (uint)((RtuData[4] << 8) + RtuData[5]);
                    MyPara.Modbus.Send.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Send.RegNum);
                    MyPara.Modbus.Send.CRC = (uint)((RtuData[6] << 8) + RtuData[7]);
                    MyPara.Modbus.Send.sCRC = string.Format("{0:X4}", MyPara.Modbus.Send.CRC);
                }
            }

            if (MyPara.Modbus.Send.FunCode == 0x10 || MyPara.Modbus.Send.FunCode == 0x27)
            {
                if (DataLen < 11)
                    MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                else
                {
                    MyPara.Modbus.Send.StartAddr = (uint)((RtuData[2] << 8) + RtuData[3]);
                    MyPara.Modbus.Send.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Send.StartAddr);
                    MyPara.Modbus.Send.RegNum = (uint)((RtuData[4] << 8) + RtuData[5]);
                    MyPara.Modbus.Send.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Send.RegNum);
                    MyPara.Modbus.Send.ByteNum = RtuData[6];
                    MyPara.Modbus.Send.sByteNum = string.Format("{0:X2}", MyPara.Modbus.Send.ByteNum);

                    if (DataLen != 2 * MyPara.Modbus.Send.RegNum + 9)
                    {
                        MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                    }
                    else
                    {
                        for (int i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                        {
                            MyPara.Modbus.Send.Data[i] = (uint)((RtuData[7 + 2 * i] << 8) + RtuData[8 + 2 * i]);
                            MyPara.Modbus.Send.sData[i] = string.Format("{0:X4}", MyPara.Modbus.Send.Data[i]);
                        }
                        MyPara.Modbus.Send.CRC = (uint)((RtuData[6] << 8) + RtuData[7]);
                        MyPara.Modbus.Send.sCRC = string.Format("{0:X4}", MyPara.Modbus.Send.CRC);
                    }
                }
            }

            if (MyPara.Modbus.Send.FunCode == 0x2A)
            {
                if (DataLen < 8)
                    MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                else
                {
                    MyPara.Modbus.Send.StartAddr = (uint)((RtuData[2] << 8) + RtuData[3]);
                    MyPara.Modbus.Send.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Send.StartAddr);
                    MyPara.Modbus.Send.RegNum = (uint)((RtuData[4] << 8) + RtuData[5]);
                    MyPara.Modbus.Send.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Send.RegNum);

                    uint Index = 6;
                    Array.Clear(MyPara.Modbus.Send.MsgLen, 0, MyPara.Modbus.Send.MsgLen.Length);
                    for (uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                    {
                        if((RtuData[Index] + Index + 3) > DataLen)
                        {
                            MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                            return;
                        }
                        else
                        {
                            byte[] MsgByte = new byte[RtuData[Index]];
                            MyPara.Modbus.Send.MsgLen[i] = RtuData[Index];
                            Array.Copy(RtuData, Index + 1, MsgByte, 0, RtuData[Index]);
                            MyPara.Modbus.Send.MsgStr[i] = Encoding.Default.GetString(MsgByte);
                            Index = RtuData[Index] + Index + 1;
                        }
                    }

                    if ((Index + 2) != DataLen)
                    {
                        MyPara.InOut.FrameError = Para.frameerror.FrameLenError;
                        return;
                    }
                    else
                    {
                        MyPara.Modbus.Send.CRC = (uint)((RtuData[Index] << 8) + RtuData[Index + 1]);
                        MyPara.Modbus.Send.sCRC = string.Format("{0:X4}", MyPara.Modbus.Send.CRC);
                    }
                }
            }
        }

        public bool IsSupportFunCode(uint FunCode)
        {
            if ((FunCode == 0x03) || (FunCode == 0x10) || (FunCode == 0x04) || (FunCode == 0x05) ||
                (FunCode == 0x25) || (FunCode == 0x26) || (FunCode == 0x27) || (FunCode == 0x2B) ||
                (FunCode == 0x2A) || (FunCode == 0x41))
                return true;
            else
                return false;
        }

        private void numericUpDown_SenorID_ValueChanged(object sender, EventArgs e)
        {
            MyPara.Modbus.Send.SensorID = (uint)this.numericUpDown_SenorID.Value;
            MyPara.Modbus.Send.sSensorID = string.Format("{0:X2}", MyPara.Modbus.Send.SensorID);
            MyPara.Modbus.ReadData.SenorID = MyPara.Modbus.Send.SensorID;

            GetReadDataMsg();                                       // 更新读数信息

            if (MyPara.Modbus.Mark.IsMark)
            {
                GetCommandArray();                                  // 更新选中信息
                TextBoxInShow();                                    // 重新生成输入数据
            }
        }

        private void ParaTable_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MyPara.Modbus.Mark.IsImport == false)
                return;

            if (e.RowIndex == -1 || e.RowIndex >= ParaTable.Rows.Count - 1) // 单击行头或行尾取消标记
            {
                MyPara.SendTable.Rows.Clear();

                ParaTable.DefaultCellStyle.SelectionBackColor = Color.White;
                ParaTable.DefaultCellStyle.SelectionForeColor = Color.Black;

                Array.Clear(MyPara.Modbus.Mark.NewColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);
                for (int i = 0; i < MyPara.Modbus.Mark.NewColorMark.Length; i++)
                {
                    if (MyPara.Modbus.Mark.NewColorMark[i] != MyPara.Modbus.Mark.OldColorMark[i])
                    {
                        ParaTable.Rows[i].DefaultCellStyle.BackColor = Color.White;
                        ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                    }
                }
                Array.Clear(MyPara.Modbus.Mark.OldColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);

                MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFFF;
                MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFFF;
                textBoxIn.Text = "";
                MyPara.Modbus.Mark.IsMark = false;
                ReadWrite.Enabled = true;
                return;
            }

            MyPara.Modbus.Mark.IsMark = true;                            // 标记
            ReadWrite.Enabled = false;
            if (MyPara.Modbus.Mark.StartMarkNum >= 0x10000)              // 开始标记地址没标记 即第一次
            {
                if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04)    // 04功能码 每次单击 需要标记两个连续的寄存器
                {
                    if ((MyPara.Modbus.Mark.MsgID[e.RowIndex] % 2) == 0)
                    {
                        MyPara.Modbus.Mark.StartMarkNum = (uint)e.RowIndex;
                        MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex + 1;
                    }
                    else
                    {
                        MyPara.Modbus.Mark.StartMarkNum = (uint)e.RowIndex - 1;
                        MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex;
                    }
                }
                else
                {
                    MyPara.Modbus.Mark.StartMarkNum = (uint)e.RowIndex;
                    MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex;
                }
            }
            else
            {
                if ((uint)(e.RowIndex) < MyPara.Modbus.Mark.StartMarkNum)
                {
                    if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04 &&
                        MyPara.Modbus.Mark.MsgID[e.RowIndex] % 2 == 1)   // 04功能码 每次单击 需要标记两个连续的寄存器
                        MyPara.Modbus.Mark.StartMarkNum = (uint)e.RowIndex - 1;
                    else
                        MyPara.Modbus.Mark.StartMarkNum = (uint)e.RowIndex;
                }
                else
                {
                    if ((uint)e.RowIndex == MyPara.Modbus.Mark.StartMarkNum)
                    {
                        MyPara.Modbus.Mark.IsMark = false;
                        ReadWrite.Enabled = true;
                        MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFF;
                        MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFF;
                        MyPara.Modbus.Send.sFunCode = "00";
                    }
                    else
                    {
                        if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04 &&
                          (uint)e.RowIndex == MyPara.Modbus.Mark.StartMarkNum + 1)
                        {
                            MyPara.Modbus.Mark.IsMark = false;
                            ReadWrite.Enabled = true;
                            MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFF;
                            MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFF;
                            MyPara.Modbus.Send.sFunCode = "00";
                        }
                        else
                        {
                            if ((uint)e.RowIndex == MyPara.Modbus.Mark.EndMarkNum - 1)
                            {
                                if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04 && MyPara.Modbus.Mark.MsgID[e.RowIndex] % 2 == 0)
                                    MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex - 1;
                                else
                                    MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex;
                            }
                            else
                            {
                                if ((MyPara.Modbus.Mark.EndMarkNum == (uint)e.RowIndex))
                                {
                                    if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04 && MyPara.Modbus.Mark.MsgID[e.RowIndex] % 2 == 1)
                                        MyPara.Modbus.Mark.EndMarkNum = (uint)(e.RowIndex - 2);
                                    else
                                        MyPara.Modbus.Mark.EndMarkNum = (uint)(e.RowIndex - 1);
                                }
                                else
                                {
                                    if (MyPara.Modbus.Mark.FunCode[e.RowIndex, 0] == 0x04 && MyPara.Modbus.Mark.MsgID[e.RowIndex] % 2 == 0)
                                        MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex + 1;
                                    else
                                        MyPara.Modbus.Mark.EndMarkNum = (uint)e.RowIndex;
                                }
                            }
                        }
                    }
                }
            }

            if (MyPara.Modbus.Mark.IsMark == false)
            {
                ReadWrite.Enabled = true;
                MyPara.Modbus.Mark.StartFunCode = 0;
                MyPara.Modbus.Mark.sStartFunCode = "00";
                MyPara.SendTable.Rows.Clear();
            }

            Array.Clear(MyPara.Modbus.Mark.NewColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);
            if (MyPara.Modbus.Mark.IsMark)
            {
                if ((MyPara.RunState.Model & MyPara.RunState.ReadWriteMast) == MyPara.RunState.ReadWriteMast)    // 写模式
                {
                    if (MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0] == "04")
                    {
                        MyPara.Modbus.Mark.StartFunCode = 0;
                        MyPara.Modbus.Mark.sStartFunCode = "00";
                        MyPara.Modbus.Mark.NewColorMark[MyPara.Modbus.Mark.StartMarkNum] = 3;
                    }
                    else
                    {
                        MyPara.Modbus.Mark.NewColorMark[MyPara.Modbus.Mark.StartMarkNum] = 2;
                        if (MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0] == "05" ||
                            MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0] == "25" ||
                            MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0] == "41")
                            MyPara.Modbus.Mark.sStartFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0];
                        else
                            MyPara.Modbus.Mark.sStartFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 1];
                    }
                }
                else
                {
                    MyPara.Modbus.Mark.sStartFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0];
                    MyPara.Modbus.Mark.NewColorMark[MyPara.Modbus.Mark.StartMarkNum] = 1;
                }

                if (MyPara.Modbus.Mark.sStartFunCode == "05" || MyPara.Modbus.Mark.sStartFunCode == "25" || MyPara.Modbus.Mark.sStartFunCode == "41")
                {
                    if ((MyPara.RunState.Model & MyPara.RunState.ReadWriteMast) == MyPara.RunState.ReadWriteMast)    // 写模式
                    {
                        for (uint i = MyPara.Modbus.Mark.StartMarkNum + 1; i <= MyPara.Modbus.Mark.EndMarkNum; i++)
                        {
                            MyPara.Modbus.Mark.NewColorMark[i] = 3;             // 错误标记
                        }
                    }
                    else
                    {
                        for (uint i = MyPara.Modbus.Mark.StartMarkNum; i <= MyPara.Modbus.Mark.EndMarkNum; i++)
                        {
                            MyPara.Modbus.Mark.NewColorMark[i] = 3;             // 错误标记
                        }
                    }
                }
                else
                {
                    if (MyPara.Modbus.Mark.EndMarkNum < (uint)0x10000)
                    {
                        if ((MyPara.RunState.Model & MyPara.RunState.ReadWriteMast) != MyPara.RunState.ReadWriteMast)   // 读模式
                        {
                            for (uint i = MyPara.Modbus.Mark.StartMarkNum; i <= MyPara.Modbus.Mark.EndMarkNum; i++)
                            {
                                if (((MyPara.Modbus.Mark.MsgID[MyPara.Modbus.Mark.StartMarkNum] + i - MyPara.Modbus.Mark.StartMarkNum) != MyPara.Modbus.Mark.MsgID[i]) ||
                                    (MyPara.Modbus.Mark.sStartFunCode != MyPara.Modbus.Mark.sFunCode[i, 0]))
                                {
                                    MyPara.Modbus.Mark.NewColorMark[i] = 3;     // 错误标记
                                }
                                else
                                    MyPara.Modbus.Mark.NewColorMark[i] = 1;     // 读标记色            
                            }
                        }
                        else
                        {
                            for (uint i = MyPara.Modbus.Mark.StartMarkNum; i <= MyPara.Modbus.Mark.EndMarkNum; i++)
                            {
                                if (MyPara.Modbus.Mark.sStartFunCode == "00")
                                {
                                    MyPara.Modbus.Mark.NewColorMark[i] = 3;     // 读标记色  
                                }
                                else
                                {
                                    if (((MyPara.Modbus.Mark.MsgID[MyPara.Modbus.Mark.StartMarkNum] + i - MyPara.Modbus.Mark.StartMarkNum) != MyPara.Modbus.Mark.MsgID[i]) ||
                                    (MyPara.Modbus.Mark.sStartFunCode != MyPara.Modbus.Mark.sFunCode[i, 1]))
                                    {
                                        MyPara.Modbus.Mark.NewColorMark[i] = 3; // 错误标记
                                    }
                                    else
                                        MyPara.Modbus.Mark.NewColorMark[i] = 2; // 读标记色 
                                }
                            }
                        }
                    }
                }
            }

            for (int i = 0; i < MyPara.Modbus.Mark.sMsgID.Length; i++)
            {
                if (MyPara.Modbus.Mark.NewColorMark[i] != MyPara.Modbus.Mark.OldColorMark[i])
                {
                    switch (MyPara.Modbus.Mark.NewColorMark[i])
                    {
                        case 0:                                     // 无标记
                            ParaTable.Rows[i].DefaultCellStyle.BackColor = Color.White;
                            ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                            break;
                        case 1:                                     // 读标记
                            ParaTable.Rows[i].DefaultCellStyle.BackColor = MyPara.Colors.ReadColor;
                            ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                        case 2:                                     // 写标记
                            ParaTable.Rows[i].DefaultCellStyle.BackColor = MyPara.Colors.WriteColor;
                            ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                        case 3:                                     // 错误标记
                            ParaTable.Rows[i].DefaultCellStyle.BackColor = MyPara.Colors.ErrorColor;
                            ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.White;
                            break;
                        default:
                            break;
                    }
                    MyPara.Modbus.Mark.OldColorMark[i] = MyPara.Modbus.Mark.NewColorMark[i];
                }
            }

            // 设置当前选中的单元格与此行单元格同色 否则选中单元格会以默认色显示
            ParaTable.DefaultCellStyle.SelectionForeColor= Color.Black;
            ParaTable.DefaultCellStyle.SelectionBackColor = ParaTable.Rows[e.RowIndex].DefaultCellStyle.BackColor;

            if (MyPara.Modbus.Mark.StartMarkNum < 0x10000)
            {
                if (MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0] == "04")
                    MsgID1.Text = MyPara.Modbus.Mark.sMsgID[MyPara.Modbus.Mark.StartMarkNum];
            }

            if (MyPara.Modbus.Mark.IsMark)
            {
                MyPara.Modbus.Send.SensorID = (uint)this.numericUpDown_SenorID.Value;
                MyPara.Modbus.Send.sSensorID = string.Format("{0:X2}", MyPara.Modbus.Send.SensorID);

                if ((MyPara.RunState.Model & MyPara.RunState.ReadWriteMast) == MyPara.RunState.ReadWriteMast)   // 写模式
                {
                    if (MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 1] == "00")
                        MyPara.Modbus.Mark.sStartFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0];
                    else
                        MyPara.Modbus.Mark.sStartFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 1];
                }
                else
                    MyPara.Modbus.Send.sFunCode = MyPara.Modbus.Mark.sFunCode[MyPara.Modbus.Mark.StartMarkNum, 0];

                MyPara.Modbus.Mark.StartFunCode = Convert.ToUInt32(MyPara.Modbus.Mark.sStartFunCode, 16);

                MyPara.Modbus.Send.sFunCode = MyPara.Modbus.Mark.sStartFunCode;
                MyPara.Modbus.Send.FunCode = MyPara.Modbus.Mark.StartFunCode;
                MyPara.Modbus.Send.sStartAddr = MyPara.Modbus.Mark.sMsgID[MyPara.Modbus.Mark.StartMarkNum];
                MyPara.Modbus.Send.StartAddr = Convert.ToUInt32(MyPara.Modbus.Send.sStartAddr, 16);
                MyPara.Modbus.Send.RegNum = (MyPara.Modbus.Mark.EndMarkNum - MyPara.Modbus.Mark.StartMarkNum) + 1;
                MyPara.Modbus.Send.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Send.RegNum);
                
                if (MyPara.Modbus.Send.FunCode == 0x05 && MyPara.Modbus.Send.FunCode == 0x25)
                {
                    MyPara.Modbus.Send.Data[0] = 0xFF00;
                    MyPara.Modbus.Send.sData[0] = string.Format("{0:X4}", MyPara.Modbus.Send.Data[0]);
                }

                if (MyPara.Modbus.Send.FunCode == 0x10 ||
                   MyPara.Modbus.Send.FunCode == 0x27)
                {
                    MyPara.Modbus.Send.ByteNum = 2 * MyPara.Modbus.Send.RegNum;
                    MyPara.Modbus.Send.sByteNum = string.Format("{0:X4}", MyPara.Modbus.Send.ByteNum);

                    for (int i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                    {
                        MyPara.Modbus.Send.Data[i] = (uint)(i + 1);
                        MyPara.Modbus.Send.sData[i] = string.Format("{0:X4}", MyPara.Modbus.Send.Data[i]);
                    }
                }

                if (MyPara.Modbus.Send.FunCode == 0x2A)
                {
                    for (uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)                        // 消息默认为"Msg1"、"Msg2"...等
                    {
                        MyPara.Modbus.Send.MsgStr[i] = string.Format("Msg{0:D}", i + 1);
                        MyPara.Modbus.Send.MsgLen[i] = (uint)MyPara.Modbus.Send.MsgStr[i].Length;
                    }
                }

                if (MyPara.Modbus.Send.FunCode == 0x41)
                    MyPara.Modbus.Send.ByteNum = 0x0000;

                GetCommandArray();

                TextBoxInShow();                                    // 在输入框生成数据
            }
            else
                textBoxIn.Text = "";
        }

        public void TextBoxInShow()
        {
            switch (MyPara.RunState.InModel)
            {
                case Para.inmodel.Normal:
                    textBoxIn.Text = MyPara.Modbus.Send.sRtuCommand;
                    break;

                case Para.inmodel.RTU2ASCII:
                    textBoxIn.Text = MyPara.Modbus.Send.sRtuCommand;
                    break;

                case Para.inmodel.ASCII2RTU:
                    textBoxIn.Text = MyPara.Modbus.Send.sAsciiCommand;
                    break;

                case Para.inmodel.RTU2RTUAuto:
                    textBoxIn.Text = MyPara.Modbus.Send.sRtuCommand.Substring(0, MyPara.Modbus.Send.sRtuCommand.Length - 6);
                    break;

                case Para.inmodel.RTU2ASCIIAuto:
                    textBoxIn.Text = MyPara.Modbus.Send.sRtuCommand.Substring(0, MyPara.Modbus.Send.sRtuCommand.Length - 6);
                    break;

                default:
                    textBoxIn.Text = MyPara.Modbus.Send.sRtuCommand;
                    break;
            }
        }

        public void GetCommandArray()
        {            
            MyPara.Modbus.Send.RtuCommand[0] = (byte)MyPara.Modbus.Send.SensorID;               // 第1字节为传感器ID
            MyPara.Modbus.Send.RtuCommand[1] = (byte)MyPara.Modbus.Send.FunCode;                // 第2字节为功能码
            MyPara.Modbus.Send.RtuCommand[2] = (byte)(MyPara.Modbus.Send.StartAddr >> 8);       // 第3字节为起始地址高8位
            MyPara.Modbus.Send.RtuCommand[3] = (byte)MyPara.Modbus.Send.StartAddr;              // 第4字节为起始地址低8位

            if (MyPara.Modbus.Send.FunCode == 0x05 || MyPara.Modbus.Send.FunCode == 0x25)
            {
                MyPara.Modbus.Send.RtuCommand[4] = (byte)(MyPara.Modbus.Send.Data[0] / 256);    // 第5字节为数据高8位 此处默认为FF
                MyPara.Modbus.Send.RtuCommand[5] = 0x00;                                        // 第6字节为数据低8位 此处默认为00
                MyPara.Modbus.Send.RtuLen = 6;                                                  // 命令长度
            }

            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x04 || 
                MyPara.Modbus.Send.FunCode == 0x26 || MyPara.Modbus.Send.FunCode == 0x2B)
            {
                MyPara.Modbus.Send.RtuCommand[4] = (byte)(MyPara.Modbus.Send.RegNum >> 8);      // 第5字节为数据数量高8位
                MyPara.Modbus.Send.RtuCommand[5] = (byte)MyPara.Modbus.Send.RegNum;             // 第6字节为数据数量低8位
                MyPara.Modbus.Send.RtuLen = 6;                                                  // 命令长度
            }

            if (MyPara.Modbus.Send.FunCode == 0x10 || MyPara.Modbus.Send.FunCode == 0x27)
            {
                MyPara.Modbus.Send.RtuCommand[4] = (byte)(MyPara.Modbus.Send.RegNum >> 8);      // 第5字节为数据数量高8位
                MyPara.Modbus.Send.RtuCommand[5] = (byte)MyPara.Modbus.Send.RegNum;             // 第6字节为数据数量低8位
                MyPara.Modbus.Send.RtuCommand[6] = (byte)MyPara.Modbus.Send.ByteNum;            // 第7字节为数据字节数

                for (int i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                {
                    MyPara.Modbus.Send.RtuCommand[7 + 2 * i] = (byte)(MyPara.Modbus.Send.Data[i] / 256);
                    MyPara.Modbus.Send.RtuCommand[8 + 2 * i] = (byte)(MyPara.Modbus.Send.Data[i] % 256);
                }

                MyPara.Modbus.Send.RtuLen = 7 + 2 * MyPara.Modbus.Send.RegNum;                  // 命令长度
            }

            if (MyPara.Modbus.Send.FunCode == 0x2A)
            {
                MyPara.Modbus.Send.RtuCommand[4] = (byte)(MyPara.Modbus.Send.RegNum >> 8);      // 第5字节为数据数量高8位
                MyPara.Modbus.Send.RtuCommand[5] = (byte)MyPara.Modbus.Send.RegNum;             // 第6字节为数据数量低8位

                if (MyPara.Modbus.Mark.IsMark)
                {
                    uint Index = 6;
                    for (uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)                        // 消息默认为"Msg1"、"Msg2"...等
                    {
                        MyPara.Modbus.Send.RtuCommand[Index] = (byte)MyPara.Modbus.Send.MsgStr[i].Length;
                        MyPara.Modbus.Send.RtuCommand[Index + 1] = 0x4D;                        // 此处0x4D 0x73 0x67表示"Msg"的编码
                        MyPara.Modbus.Send.RtuCommand[Index + 2] = 0x73;
                        MyPara.Modbus.Send.RtuCommand[Index + 3] = 0x67;
                        MyPara.Modbus.Send.RtuCommand[Index] = (byte)MyPara.Modbus.Send.MsgStr[i].Length;   // 信息长度

                        string StrNum = string.Format("{0:D}", i + 1);
                        byte[] StrNumByte = Encoding.Default.GetBytes(StrNum);
                        Array.Copy(StrNumByte, 0, MyPara.Modbus.Send.RtuCommand, Index + 4, StrNumByte.Length);
                        
                        Index = MyPara.Modbus.Send.RtuCommand[Index] + Index + 1;
                    }

                    MyPara.Modbus.Send.RtuLen = Index;                                          // 命令长度
                }
                else
                {
                    Array.Copy(MyPara.InOut.InputData, MyPara.Modbus.Send.RtuCommand, MyPara.InOut.InputLen);
                    MyPara.InOut.ConvLen = MyPara.InOut.InputLen;
                    MyPara.Modbus.Send.RtuLen = MyPara.InOut.InputLen;                          // 命令长度

                    uint Index = 6;
                    if (Index <= MyPara.InOut.InputLen)
                    {
                        for (uint i = 0; i < MyPara.Modbus.Send.RegNum; i++)
                        {
                            MyPara.Modbus.Send.MsgLen[i] = MyPara.Modbus.Send.RtuCommand[Index];
                            if ((MyPara.Modbus.Send.RtuCommand[Index] + Index + 1) <= MyPara.InOut.InputLen)
                            {
                                byte[] MsgByte = new byte[MyPara.Modbus.Send.RtuCommand[Index]];
                                Array.Copy(MyPara.Modbus.Send.RtuCommand, Index + 1, MsgByte, 0, MyPara.Modbus.Send.RtuCommand[Index]);
                                MyPara.Modbus.Send.MsgStr[i] = Encoding.Default.GetString(MsgByte);
                                Index = Index + 1 + MyPara.Modbus.Send.MsgLen[i];
                            }
                            else
                                break;
                        }
                    }
                }                
            }

            if (MyPara.Modbus.Send.FunCode == 0x41)
            {
                MyPara.Modbus.Send.RtuCommand[4] = (byte)(MyPara.Modbus.Send.ByteNum / 256);    // 第5字节为字节数高8位
                MyPara.Modbus.Send.RtuCommand[5] = (byte)(MyPara.Modbus.Send.ByteNum % 256);    // 第6字节为字节数8位

                uint i = 0;
                for (i = 0; i < MyPara.Modbus.Send.ByteNum; i++)
                {
                    MyPara.Modbus.Send.Data[i] = i + 1;
                    MyPara.Modbus.Send.sData[i] = string.Format("{0:X4}", MyPara.Modbus.Send.Data[i]);
                    MyPara.Modbus.Send.RtuCommand[2 * i + 6] = (byte)(MyPara.Modbus.Send.Data[i] / 256);
                    MyPara.Modbus.Send.RtuCommand[2 * i + 7] = (byte)(MyPara.Modbus.Send.Data[i] % 256);
                }
                MyPara.Modbus.Send.RtuLen = 2 * i + 6;                                          // 命令长度
            }

            if (MyPara.Modbus.Send.FunCode == 0x04)
                MyPara.Modbus.Send.CorrectRecLen = 5 + 4 * MyPara.Modbus.Send.RegNum;           // 正确回复长度
            if (MyPara.Modbus.Send.FunCode == 0x03 || MyPara.Modbus.Send.FunCode == 0x26)
                MyPara.Modbus.Send.CorrectRecLen = 5 + 2 * MyPara.Modbus.Send.RegNum;           // 正确回复长度
            if (MyPara.Modbus.Send.FunCode == 0x05 || MyPara.Modbus.Send.FunCode == 0x25 ||
                MyPara.Modbus.Send.FunCode == 0x10 || MyPara.Modbus.Send.FunCode == 0x27 || 
                MyPara.Modbus.Send.FunCode == 0x2A)
                MyPara.Modbus.Send.CorrectRecLen = 8;                                           // 正确回复长度
            if (MyPara.Modbus.Send.FunCode == 0x2B)
                MyPara.Modbus.Send.CorrectRecLen = 4 + 10 * MyPara.Modbus.Send.RegNum;          // 回复长度估算值 每一个信息长度加内容估算为10
            if (MyPara.Modbus.Send.FunCode == 0x41)
                MyPara.Modbus.Send.CorrectRecLen = 9;                                           // 正确回复长度

            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
                MyPara.Modbus.Send.CorrectRecLen = 2 * MyPara.Modbus.Send.CorrectRecLen + 1;    // 正确回复长度

            uint LRC = Modbus.GetLRC(MyPara.Modbus.Send.RtuCommand, MyPara.Modbus.Send.RtuLen);
            MyPara.Modbus.Send.LRC = LRC;
            MyPara.Modbus.Send.sLRC = string.Format("{0:X2}", LRC);

            uint CRC = Modbus.GetCRC(MyPara.Modbus.Send.RtuCommand, MyPara.Modbus.Send.RtuLen);
            MyPara.Modbus.Send.RtuCommand[MyPara.Modbus.Send.RtuLen] = (byte)CRC;
            MyPara.Modbus.Send.RtuCommand[MyPara.Modbus.Send.RtuLen + 1] = (byte)(CRC >> 8);
            MyPara.Modbus.Send.RtuLen = MyPara.Modbus.Send.RtuLen + 2;
            MyPara.Modbus.Send.CRC = (CRC >> 8) + ((CRC & 0x00FF) << 8);
            MyPara.Modbus.Send.sCRC = string.Format("{0:X4}", MyPara.Modbus.Send.CRC);

            byte[] AsciiData = Modbus.Rtu2Ascii(MyPara.Modbus.Send.RtuCommand, MyPara.Modbus.Send.RtuLen);
            for (int i = 0; i < AsciiData.Length; i++)
            {
                MyPara.Modbus.Send.AsciiCommand[i] = AsciiData[i];
            }
            MyPara.Modbus.Send.AsciiLen = (uint)AsciiData.Length;

            MyPara.Modbus.Send.sRtuCommandArray[0] = string.Format("{0:X2}", MyPara.Modbus.Send.RtuCommand[0]);
            MyPara.Modbus.Send.sRtuCommand = MyPara.Modbus.Send.sRtuCommandArray[0];
            for (int i = 1; i < MyPara.Modbus.Send.RtuLen; i++)
            {
                MyPara.Modbus.Send.sRtuCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Send.RtuCommand[i]);
                MyPara.Modbus.Send.sRtuCommand += " " + MyPara.Modbus.Send.sRtuCommandArray[i];
            }

            MyPara.Modbus.Send.sAsciiCommandArray[0] = string.Format("{0:X2}", MyPara.Modbus.Send.AsciiCommand[0]);
            MyPara.Modbus.Send.sAsciiCommand = MyPara.Modbus.Send.sAsciiCommandArray[0];
            for (int i = 1; i < MyPara.Modbus.Send.AsciiLen; i++)
            {
                MyPara.Modbus.Send.sAsciiCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Send.AsciiCommand[i]);
                MyPara.Modbus.Send.sAsciiCommand += " " + MyPara.Modbus.Send.sAsciiCommandArray[i];
            }
        }

        private void ParaTable_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (MyPara.Modbus.Mark.IsImport == false)
                return;

            Array.Clear(MyPara.Modbus.Mark.NewColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);
            for (int i = 0; i < MyPara.Modbus.Mark.NewColorMark.Length; i++)
            {
                if (MyPara.Modbus.Mark.NewColorMark[i] != MyPara.Modbus.Mark.OldColorMark[i])
                {
                    ParaTable.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.Black;

                    MyPara.Modbus.Mark.OldColorMark[i] = MyPara.Modbus.Mark.NewColorMark[i];
                }
            }

            ParaTable.DefaultCellStyle.SelectionBackColor = Color.White;
            ParaTable.DefaultCellStyle.SelectionForeColor = Color.Black;

            MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFFF;
            MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFFF;
            textBoxIn.Text = "";
            MyPara.Modbus.Mark.IsMark = false;
            ReadWrite.Enabled = true;
            MyPara.SendTable.Rows.Clear();
        }

        private void MsgID1_KeyPress(object sender, KeyPressEventArgs e)
        {
            uint KeyCharNum = (uint)e.KeyChar;
            if (!(KeyCharNum >= 48 && KeyCharNum <= 57) &&          // 数字
                !(KeyCharNum >= 65 && KeyCharNum <= 70) &&          // 大写字母A~F
                !(KeyCharNum >= 97 && KeyCharNum <= 102) &&         // 小写字母a~f
                !(KeyCharNum == 8))                                 // 退格键
            {
                e.Handled = true;
            }
            else
            {
                if (KeyCharNum >= 97 && KeyCharNum <= 102)
                    e.KeyChar = Char.ToUpper(e.KeyChar);            // 小写字母转大写字母

                Uncheck();                                          // 取消选中
            }     
        }

        private void MsgID1_TextChanged(object sender, EventArgs e)
        {
            if (MsgID1.Text.Length > 0)
            {
                MyPara.Modbus.ReadData.IsIDTtrue = true;
                MyPara.Modbus.ReadData.MsgID = Convert.ToUInt32(MsgID1.Text, 16);
                if (MyPara.Modbus.ReadData.MsgID % 2 == 1)
                    MyPara.Modbus.ReadData.MsgID--;
                MyPara.Modbus.ReadData.sMsgID = string.Format("{0:X4}", MyPara.Modbus.ReadData.MsgID);
            }
            else
            {
                MyPara.Modbus.ReadData.IsIDTtrue = false;
                MyPara.Modbus.ReadData.MsgID = 0;
                MyPara.Modbus.ReadData.sMsgID = "0000";
            }

            GetReadDataMsg();
        }

        private void timer1_Time_Tick(object sender, EventArgs e)
        {
            this.Text = MyPara.Title + DateTime.Now.ToString("  HH:mm:ss");
        }

        private void ReadCycle_ValueChanged(object sender, EventArgs e)
        {
            timer_ReadData.Interval = Convert.ToInt32(ReadCycle.Value);         // 设定读数定时器的周期值
        }

        private void ReadWrite_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReadWrite.Text == "Read")
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.ReadWriteMast);
            else
                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.ReadWriteMast;
        }

        private void button1_Clear_Click(object sender, EventArgs e)
        {
            richTextBox1.Text = "";
        }

        private void SendStr_Click(object sender, EventArgs e)
        {
            if (MyPara.Modbus.Send.RtuLen == 0)
                return;

            timer_ShowReceData.Enabled = true;                                  // 启动定时器 接收解析显示在定时器中完成
            Thread thread = new Thread(new ThreadStart(SendReceMethod));        // 创建接收发线程       
            thread.IsBackground = true;                                         // 后台线程 不设置关闭程序后线程依然会运行
            thread.Start();                                                     // 启动线程
        }

        public void SendReceMethod()
        {
            bool SerialState = IsSerialCanUse();

            if (SerialState == false)
                return;

            MyPara.RunState.Model |= MyPara.RunState.IsSerialUseMask;                           // 标记串口在使用中

            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
                SendReceData(MyPara.Modbus.Send.AsciiCommand, MyPara.Modbus.Send.AsciiLen);     // 发送
            else
            {
                if ((MyPara.RunState.InModel == Para.inmodel.Normal) && (MyPara.InOut.NormalStyle == "ASCII"))
                    SendReceData(MyPara.Modbus.Send.AsciiCommand, MyPara.Modbus.Send.AsciiLen); // 发送
                else
                    SendReceData(MyPara.Modbus.Send.RtuCommand, MyPara.Modbus.Send.RtuLen);     // 发送
            }
                
        }

        public void SendReceData(byte[] SendData, uint DataLen)
        {
            int TimeOut = (int)(100 + MyPara.Modbus.ReadData.TimeOut * (DataLen + 8) / 1000);   // 计算响应超时时间(ms) MyPara.Modbus.ReadData.TimeOut * (DataLen + 8) / 1000为数据传输时间 100为下位机响应时间
            int TimeOutCount = (TimeOut + 1) / 2;

            if (serialPort1.IsOpen == false)
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                return;
            }
            if (serialPort1.BytesToRead > 0)                                                    // 发送指令前检测接收缓存中是否有数据 有则清除
                serialPort1.ReadExisting();

            MyPara.Serial.IsDataReceived = false;
            serialPort1.Write(SendData, 0, (int)DataLen);                                       // 串口发送数据

            string SendStr = string.Format("{0:X2}", SendData[0]);
            for (int i = 1; i < DataLen; i++)
                SendStr += " " + string.Format("{0:X2}", SendData[i]);

            RichTextBoxShow(SendStr, false);

            for (int i = 0; i < TimeOutCount; i++)
            {
                if (MyPara.Serial.IsDataReceived == false)                                      // 等待接收第一个字节数据
                    Thread.Sleep(2);
                else
                    break;
            }

            if (serialPort1.IsOpen == false)
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                return;
            }
            int RecLen = serialPort1.BytesToRead;
            if (RecLen != MyPara.Modbus.Send.CorrectRecLen)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (serialPort1.IsOpen == false)
                    {
                        MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                        return;
                    }
                    int NewRecLen = serialPort1.BytesToRead;
                    if (NewRecLen != MyPara.Modbus.Send.CorrectRecLen)
                    {
                        if (NewRecLen != RecLen)
                        {
                            i = -1;
                            RecLen = NewRecLen;
                        }
                        Thread.Sleep(2);
                    }
                    else
                        break;
                }
            }

            if(MyPara.Serial.IsDataReceived == false)
            {
                MyPara.RunState.Model &= (~MyPara.RunState.IsSerialUseMask);            // 标记串口使用结束
                return;
            }

            if (serialPort1.IsOpen == false)
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                return;
            }
            byte[] RecData = new byte[serialPort1.BytesToRead];
            serialPort1.Read(RecData, 0, RecData.Length);
            MyPara.Modbus.Rece.RecLen = (uint)RecData.Length;

            MyPara.RunState.Model &= (~MyPara.RunState.IsSerialUseMask);                // 标记串口使用结束

            string ReceStr = string.Format("{0:X2}", RecData[0]);
            for(int i = 1; i < MyPara.Modbus.Rece.RecLen; i++)
                ReceStr += " " + string.Format("{0:X2}", RecData[i]);

            RichTextBoxShow(ReceStr, true);

            if (MyPara.RunState.InModel == Para.inmodel.RTU2ASCII || MyPara.RunState.InModel == Para.inmodel.RTU2ASCIIAuto)
            {
                for(int i = 0; i < MyPara.Modbus.Rece.RecLen; i++)
                {
                    MyPara.Modbus.Rece.AsciiCommand[i] = RecData[i];
                    MyPara.Modbus.Rece.sAsciiCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Rece.AsciiCommand[i]);
                }
                MyPara.Modbus.Rece.AsciiLen = MyPara.Modbus.Rece.RecLen;

                byte[] RtuData = Modbus.Ascii2Rtu(MyPara.Modbus.Rece.AsciiCommand, MyPara.Modbus.Rece.AsciiLen);
                for (int i = 0; i < RtuData.Length; i++)
                {
                    MyPara.Modbus.Rece.RtuCommand[i] = RtuData[i];
                    MyPara.Modbus.Rece.sRtuCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Rece.RtuCommand[i]);
                }
                MyPara.Modbus.Rece.RtuLen = (uint)RtuData.Length;
            }
            else
            {
                for (int i = 0; i < MyPara.Modbus.Rece.RecLen; i++)
                {
                    MyPara.Modbus.Rece.RtuCommand[i] = RecData[i];
                    MyPara.Modbus.Rece.sRtuCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Rece.RtuCommand[i]);
                }
                MyPara.Modbus.Rece.RtuLen = MyPara.Modbus.Rece.RecLen;

                byte[] AsciiData = Modbus.Rtu2Ascii(MyPara.Modbus.Rece.AsciiCommand, MyPara.Modbus.Rece.RtuLen);
                for (int i = 0; i < AsciiData.Length; i++)
                {
                    MyPara.Modbus.Rece.AsciiCommand[i] = AsciiData[i];
                    MyPara.Modbus.Rece.sAsciiCommandArray[i] = string.Format("{0:X2}", MyPara.Modbus.Rece.AsciiCommand[i]);
                }
                MyPara.Modbus.Rece.AsciiLen = (uint)AsciiData.Length;
            }

            MyPara.Modbus.Rece.SensorID = MyPara.Modbus.Rece.RtuCommand[0];
            MyPara.Modbus.Rece.sSensorID = string.Format("{0:X2}", MyPara.Modbus.Rece.SensorID);
            MyPara.Modbus.Rece.FunCode = MyPara.Modbus.Rece.RtuCommand[1];
            MyPara.Modbus.Rece.sFunCode = string.Format("{0:X2}", MyPara.Modbus.Rece.FunCode);

            if(MyPara.Modbus.Rece.FunCode >= 0x80)
            {
                MyPara.Modbus.Rece.FunCode = MyPara.Modbus.Rece.RtuCommand[1];
                MyPara.Modbus.Rece.sFunCode = string.Format("{0:X2}", MyPara.Modbus.Rece.FunCode);
                MyPara.Modbus.Rece.ErrorNum = MyPara.Modbus.Rece.RtuCommand[2];
                MyPara.Modbus.Rece.sErrorNum = string.Format("{0:X2}", MyPara.Modbus.Rece.ErrorNum);
            }

            if(MyPara.Modbus.Rece.FunCode == 0x05 || MyPara.Modbus.Rece.FunCode == 0x25)
            {
                MyPara.Modbus.Rece.StartAddr = (uint)((MyPara.Modbus.Rece.RtuCommand[2] << 8) + MyPara.Modbus.Rece.RtuCommand[3]);
                MyPara.Modbus.Rece.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Rece.StartAddr);
                MyPara.Modbus.Rece.Data[0] = (uint)((MyPara.Modbus.Rece.RtuCommand[4] << 8) + MyPara.Modbus.Rece.RtuCommand[5]);
                MyPara.Modbus.Rece.sData[0] = string.Format("{0:X4}", MyPara.Modbus.Rece.Data[0]);
            }

            if (MyPara.Modbus.Rece.FunCode == 0x03 || MyPara.Modbus.Rece.FunCode == 0x04 || MyPara.Modbus.Rece.FunCode == 0x26)
            {
                MyPara.Modbus.Rece.StartAddr = MyPara.Modbus.Send.StartAddr;
                MyPara.Modbus.Rece.sStartAddr = MyPara.Modbus.Send.sStartAddr;
                MyPara.Modbus.Rece.ByteNum = MyPara.Modbus.Rece.RtuCommand[2];
                MyPara.Modbus.Rece.sByteNum = string.Format("{0:X2}", MyPara.Modbus.Rece.ByteNum);

                for(int i = 0; i < MyPara.Modbus.Rece.ByteNum / 2; i++)
                {
                    MyPara.Modbus.Rece.Data[i] = (uint)((MyPara.Modbus.Rece.RtuCommand[3 + 2 * i] << 8) + MyPara.Modbus.Rece.RtuCommand[4 + 2 * i]);
                    MyPara.Modbus.Rece.sData[i] = string.Format("{0:X4}", MyPara.Modbus.Rece.Data[i]);
                }
            }

            if (MyPara.Modbus.Rece.FunCode == 0x10 || MyPara.Modbus.Rece.FunCode == 0x27 || MyPara.Modbus.Rece.FunCode == 0x2A)
            {
                MyPara.Modbus.Rece.StartAddr = (uint)((MyPara.Modbus.Rece.RtuCommand[2] << 8) + MyPara.Modbus.Rece.RtuCommand[3]);
                MyPara.Modbus.Rece.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Rece.StartAddr);
                MyPara.Modbus.Rece.RegNum = (uint)((MyPara.Modbus.Rece.RtuCommand[4] << 8) + MyPara.Modbus.Rece.RtuCommand[5]);
                MyPara.Modbus.Rece.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Rece.RegNum);
            }

            if (MyPara.Modbus.Rece.FunCode == 0x2B)
            {
                MyPara.Modbus.Rece.RegNum = (uint)((MyPara.Modbus.Rece.RtuCommand[2] << 8) + MyPara.Modbus.Rece.RtuCommand[3]);
                MyPara.Modbus.Rece.sRegNum = string.Format("{0:X4}", MyPara.Modbus.Rece.RegNum);

                uint Index = 4;
                for (uint i = 0; i < MyPara.Modbus.Rece.RegNum; i++)
                {
                    MyPara.Modbus.Rece.MsgLen[i] = MyPara.Modbus.Rece.RtuCommand[Index];
                    byte[] MsgByte = new byte[MyPara.Modbus.Rece.RtuCommand[Index]];
                    Array.Copy(MyPara.Modbus.Rece.RtuCommand, Index + 1, MsgByte, 0, MsgByte.Length);
                    MyPara.Modbus.Rece.MsgStr[i] = Encoding.Default.GetString(MsgByte);
                }
            }

            if (MyPara.Modbus.Rece.FunCode == 0x41)
            {
                MyPara.Modbus.Rece.StartAddr = (uint)((MyPara.Modbus.Rece.RtuCommand[2] << 8) + MyPara.Modbus.Rece.RtuCommand[3]);
                MyPara.Modbus.Rece.sStartAddr = string.Format("{0:X4}", MyPara.Modbus.Rece.StartAddr);
                MyPara.Modbus.Rece.ByteNum = (uint)((MyPara.Modbus.Rece.RtuCommand[4] << 8) + MyPara.Modbus.Rece.RtuCommand[5]);
                MyPara.Modbus.Rece.sByteNum = string.Format("{0:X4}", MyPara.Modbus.Rece.ByteNum);
                MyPara.Modbus.Rece.Data[0] = (uint)MyPara.Modbus.Rece.RtuCommand[6];
                MyPara.Modbus.Rece.sData[0] = string.Format("{0:X2}", MyPara.Modbus.Rece.Data[0]);
            }


            MyPara.Modbus.Rece.CRC = (uint)((MyPara.Modbus.Rece.RtuCommand[MyPara.Modbus.Rece.RtuLen - 2] << 8) + MyPara.Modbus.Rece.RtuCommand[MyPara.Modbus.Rece.RtuLen - 1]);
            MyPara.Modbus.Rece.sCRC = string.Format("{0:X4}", MyPara.Modbus.Rece.CRC);

            MyPara.Modbus.Rece.LRC = Modbus.GetLRC(MyPara.Modbus.Rece.RtuCommand, MyPara.Modbus.Rece.RtuLen - 2);
            MyPara.Modbus.Rece.sLRC = string.Format("{0:X2}", MyPara.Modbus.Rece.LRC);

            MyPara.Modbus.Rece.AnalyzeFinsh = true;     // 解析完成 子线程接收数据在datagridview显示时会出问题 暂时解决方法在timer_ShowReceData定时器中显示
        }

        public void RichTextBoxShow(string Str, bool IsRecData)
        {
            if ((IsRecData == false) && ((MyPara.RunState.Model & MyPara.RunState.ShowSendMask) != MyPara.RunState.ShowSendMask))
                return;                                                         // 数据为发送数据且未勾选显示发送直接返回

            if (Str.Length != 0)
            {
                if (richTextBox1.Text.Length != 0)
                    richTextBox1.AppendText("\r\n");

                if (IsRecData)
                    richTextBox1.SelectionColor = MyPara.Colors.RecColor;
                else
                    richTextBox1.SelectionColor = MyPara.Colors.SendColor;

                if ((MyPara.RunState.Model & MyPara.RunState.ShowTimeMask) == MyPara.RunState.ShowTimeMask)
                    richTextBox1.AppendText(DateTime.Now.ToString("[HH:mm:ss:fff]"));

                richTextBox1.AppendText(Str);
                richTextBox1.ScrollToCaret();
            }
        }

        private void btn_SerialOpen_Click(object sender, EventArgs e)
        {
            if (btn_SerialOpen.Text == "打开")
                SerialOpen();
            else
                SerialClose();
        }

        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x0219)                                    // usb串口
            {
                if (m.WParam.ToInt32() == 0x8000 ||                 // 串口接入
                    m.WParam.ToInt32() == 0x8004)                   // 串口拔出
                {
                    timer_RefreshSerialDelay.Enabled = true;        // 串口接入或拔出后定时器延时10ms再执行刷新操作 软件延时有产生问题
                }
            }
            base.WndProc(ref m);
        }

        public void RefreshSerial()
        {
            string[] SerialNames;
            SerialNames = MulGetHardwareInfo(HardwareEnum.Win32_PnPEntity, "Name"); // 调用方式通过WMI获取COM端口

            MyPara.Serial.IsRefresh = true;                                         // 刷新开始 标记

            if (SerialNames.Length == 0)
            {
                SerialClose();

                SerialCom.Items.Clear();
                SerialCom.Items.Add("Default(COM1)");
                SerialCom.SelectedIndex = 0;
                MyPara.Serial.IsRefresh = false;                                    // 刷新结束 标记

                return;
            }

            string[] NewSerialNames = new string[SerialNames.Length];               // 截取前面6个字符信息 + "..." + 端口号
            for (int i = 0; i < SerialNames.Length; i++)
            {
                if (SerialNames[i].Length >= 12 && (SerialNames[i].Substring(0, 8).Contains("(") == false))
                    NewSerialNames[i] = SerialNames[i].Substring(0, 8) + ".(" + Regex.Replace(SerialNames[i], @"(.*\()(.*)(\).*)", "$2") + ")";
                else
                    NewSerialNames[i] = SerialNames[i];
            }

            string OldIItemStr = SerialCom.Text;
            int OldItemsCount = SerialCom.Items.Count;
            int Count = SerialNames.Length > OldItemsCount ? SerialNames.Length : OldItemsCount;

            for (int i = 0; i < Count; i++)
            {
                if (i < OldItemsCount)
                {
                    if (i < SerialNames.Length)
                        SerialCom.Items[i] = NewSerialNames[i];
                    else
                        SerialCom.Items.RemoveAt(SerialCom.Items.Count - 1);
                }
                else
                    SerialCom.Items.Add(NewSerialNames[i]);
            }
            MyPara.Serial.IsRefresh = false;                        // 刷新结束 标记

            if (Array.IndexOf(NewSerialNames, OldIItemStr) >= 0)
                SerialCom.Text = OldIItemStr;
            else
            {
                SerialClose();
                SerialCom.Text = SerialCom.Items[0].ToString();
            }
        }

        public enum HardwareEnum
        {
            Win32_SerialPort = 10,                                  // 串口
            Win32_PnPEntity = 49,                                   // 所有
        }

        public static string[] MulGetHardwareInfo(HardwareEnum hardType, string propKey)
        {
            List<string> strs = new List<string>();
            try
            {
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher("select * from " + hardType))
                {
                    var hardInfos = searcher.Get(); foreach (var hardInfo in hardInfos)
                    {
                        string pattern = @"COM\d{1,3}";
                        Regex rx = new Regex(pattern);

                        if (hardInfo.Properties[propKey].Value != null && rx.IsMatch(hardInfo.Properties[propKey].Value.ToString()))
                        {
                            strs.Add(hardInfo.Properties[propKey].Value.ToString());
                        }
                    }
                    searcher.Dispose();
                }
                return strs.ToArray();
            }
            catch { return null; }
            finally { strs = null; }
        }

        private void SerialID_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (MyPara.Serial.IsRefresh)
                return;
            else
            {
                if (SerialCom.Items.Count == 0)
                {
                    if (serialPort1.IsOpen)
                        SerialClose();

                    serialPort1.PortName = "COM1";
                    SerialCom.Text = "";
                    return;
                }

                if (Regex.Replace(SerialCom.Text, @"(.*\()(.*)(\).*)", "$2") != serialPort1.PortName)
                {
                    if (serialPort1.IsOpen)
                    {
                        SerialClose();
                        serialPort1.PortName = Regex.Replace(SerialCom.Text, @"(.*\()(.*)(\).*)", "$2");    // 提取文本中小括号内的内容
                        SerialOpen();
                    }
                    else
                        serialPort1.PortName = Regex.Replace(SerialCom.Text, @"(.*\()(.*)(\).*)", "$2");
                }
            }
        }

        public void SerialOpen()
        {
            try
            {
                if(Baud.SelectedIndex == -1)
                {
                    try
                    {
                        MyPara.Serial.Baud = Convert.ToInt32(Baud.Text);
                        serialPort1.BaudRate = MyPara.Serial.Baud;
                    }
                    catch
                    {
                        MessageBox.Show("请先设置波特率", "提示");
                        return;
                    }
                }
                serialPort1.Open();
                if (serialPort1.IsOpen)
                {
                    MyPara.Serial.IsOpen = true;
                    btn_SerialOpen.BackColor = Color.Green;
                    btn_SerialOpen.Text = "关闭";
                }
            }
            catch (Exception)
            {
                MyPara.Serial.IsOpen = false;
                btn_SerialOpen.BackColor = MyPara.Colors.ErrorColor;
            }
        }

        public void SerialClose()
        {
            if (serialPort1.IsOpen)
                serialPort1.Close();
            btn_SerialOpen.Text = "打开";
            MyPara.Serial.IsOpen = false;

            if (btn_SerialOpen.BackColor != MyPara.Colors.DefaultBackColor)
                btn_SerialOpen.BackColor = MyPara.Colors.DefaultBackColor;
        }

        // 波特率自定义设置需要修改combox的DropDownStyle属性，该属性值进行切换设置时会导致软件死机，因此
        // 使用了2个波特率combox，1个是可更改Text内容，1个不可更改Text内容，2个来回切换实现波特率自定义
        private void Baud_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Baud.SelectedIndex == 7)
            {
                Baud.Visible = false;
                Baud1.Visible = true;
                Baud1.SelectedIndex = 7;
                Baud1.Focus();
                return;
            }
            else
            {
                int baud = Convert.ToInt32(Baud.Text);
                MyPara.Serial.Baud = baud;
                serialPort1.BaudRate = baud;
                MyPara.Modbus.ReadData.TimeOut = (uint)(11000000 / baud + 1);
            }
        }

        private void Baud1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Baud1.SelectedIndex == -1)
                return;

            if (Baud1.SelectedIndex == 7)
            {
                Baud1.SelectedIndex = -1;
                return;
            }
            else
            {
                Baud1.Visible = false;
                Baud.Visible = true;
                Baud.Focus();
                Baud.SelectedIndex = Baud1.SelectedIndex;
            }
        }

        private void Baud1_TextChanged(object sender, EventArgs e)
        {
            if (Baud1.SelectedIndex == -1)
            {
                if (Baud1.Text.Length > 0)
                {
                    try
                    {
                        MyPara.Serial.Baud = Convert.ToInt32(Baud1.Text);
                        serialPort1.BaudRate = MyPara.Serial.Baud;
                    }
                    catch
                    {
                    }
                }
            }
        }

        private void Baud1_KeyPress(object sender, KeyPressEventArgs e)
        {
            uint KeyCharNum = e.KeyChar;

            if (KeyCharNum == 13)                                               // 回车键
            {
                if (!Baud.Items.Contains(Baud1.Text))
                {
                    Baud.Items.Add(Baud1.Text);
                    Baud1.Items.Add(Baud1.Text);
                }    
            }

            if (!(KeyCharNum >= 48 && KeyCharNum <= 57) && !(KeyCharNum == 8))  // 数字与退格键                                                                    
            {
                e.Handled = true;
                return;
            }
        }

        private void Parity_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (Parity.Text)
            {
                case "NONE":
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    break;
                case "ODD":
                    serialPort1.Parity = System.IO.Ports.Parity.Odd;
                    break;
                case "EVEN":
                    serialPort1.Parity = System.IO.Ports.Parity.Even;
                    break;
                default:
                    serialPort1.Parity = System.IO.Ports.Parity.None;
                    break;
            }
            MyPara.Serial.Parity = Parity.Text;
        }

        private void StopBit_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (StopBit.Text)
            {
                case "1":
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    break;
                case "1.5":
                    try
                    {
                        serialPort1.StopBits = System.IO.Ports.StopBits.OnePointFive;
                    }
                    catch 
                    {
                        serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    }
                    break;
                case "2":
                    serialPort1.StopBits = System.IO.Ports.StopBits.Two;
                    break;
                default:
                    serialPort1.StopBits = System.IO.Ports.StopBits.One;
                    break;
            }
            MyPara.Serial.Stop = StopBit.Text;
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            MyPara.Serial.IsDataReceived = true;
        }

        private void button_Help_Click(object sender, EventArgs e)
        {
            // 点击按钮，显示帮助文档
            if (File.Exists(MyPara.HelpPath) == false)              // 文件不存在就提示
                MessageBox.Show("帮助文档丢失!!!", "提示");
            else
                System.Diagnostics.Process.Start(MyPara.HelpPath);  // 文件存在就打开 
        }

        private void timer_RefreshSerialDelay_Tick(object sender, EventArgs e)
        {
            timer_RefreshSerialDelay.Enabled = false;
            RefreshSerial();
        }

        private void timer_ReadData_Tick(object sender, EventArgs e)
        {
            if (MyPara.Modbus.ReadData.FirstTime)
            {
                MyPara.Modbus.ReadData.FirstTime = false;
                timer_ReadData.Interval = (int)ReadCycle.Value;
                MyPara.Modbus.ReadData.IsCycleArrive = true;
                MyPara.Modbus.ReadData.ReadFinsh = false;
                CreatReadDataThread();
            }
            else
            {
                MyPara.Modbus.ReadData.IsCycleArrive = true;
                if (MyPara.Modbus.ReadData.ReadFinsh)
                {
                    MyPara.Modbus.ReadData.ReadFinsh = false;
                    CreatReadDataThread();
                }        
            }
        }
        
        public void CreatReadDataThread()
        {
            Thread thread = new Thread(new ThreadStart(ReadDataThreadMethod));  // 创建线程       
            thread.IsBackground = true;                                         // 后台线程 不设置关闭程序后线程依然会运行
            thread.Start();                                                     // 启动线程
        }

        public void ReadDataThreadMethod()
        {
            while (MyPara.Modbus.ReadData.IsCycleArrive)
            {
                MyPara.Modbus.ReadData.IsCycleArrive = false;

                if (IsSerialCanUse() == false)
                {
                    CloseReadData();
                    return;
                }

                MyPara.RunState.Model = MyPara.RunState.Model | MyPara.RunState.IsSerialUseMask;    // 标记Serial在使用中

                if (MyPara.Modbus.ReadData.RunModel == "RTU")
                {
                    SerialSendData(MyPara.Modbus.ReadData.RtuCommand, 8);
                }
                else
                {
                    SerialSendData(MyPara.Modbus.ReadData.AsciiCommand, 17);
                }

                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束

                Thread.Sleep(10);                                                                   // 此处延时防止周期超时时发送数据依然能正常发送 延时要大于5ms
                
                if (MyPara.Modbus.ReadData.IsCycleArrive == false)
                    MyPara.Modbus.ReadData.ReadFinsh = true;
            }
        }

        public bool IsSerialCanUse()
        {
            if (MyPara.Serial.IsOpen == false)
            {
                this.Invoke((MethodInvoker)delegate
                {
                    MessageBox.Show("串口未打开", "提示");

                });
                return false;
            }

            int Count = 0;
            while ((MyPara.RunState.Model & MyPara.RunState.IsSerialUseMask) == MyPara.RunState.IsSerialUseMask)
            {
                Thread.Sleep(5);
                if (++Count == 100)                                                         // 在连续0.5内若串口一直被占用则停止读数
                {
                    this.Invoke((MethodInvoker)delegate
                    {
                        MessageBox.Show("串口被占用,数据访问超时", "提示");

                    });
                    return false;
                }
            }

            return true;
        }

        public void SerialSendData(byte[] SendData, uint DataLen)
        {
            int TimeOut = (int)(100 + MyPara.Modbus.ReadData.TimeOut * (DataLen + 8) / 1000);   // 计算响应超时时间(ms) MyPara.Modbus.ReadData.OverTimeCount * 16 / 1000为数据传输时间 100为下位机响应时间
            int TimeOutCount = (TimeOut + 1) / 2;

            MyPara.Modbus.ReadData.OverTimeCount = 0;                                       // 超时次数清零
            while (true)
            {
                MyPara.Serial.IsDataReceived = false;

                if (serialPort1.IsOpen == false)
                {
                    MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                    CloseReadData();
                    return;
                }

                if (serialPort1.BytesToRead > 0)                                            // 发送指令前检测接收缓存中是否有数据 有则清除
                    serialPort1.ReadExisting();

                serialPort1.Write(SendData, 0, (int)DataLen);                               // 串口发送数据
                MyPara.Modbus.ReadData.OverTimeCount++;

                int i;
                for (i = 0; i < TimeOutCount; i++)
                {
                    if (MyPara.Serial.IsDataReceived == false)                              // 等待接收第一个字节数据
                        Thread.Sleep(2);
                    else
                        break;
                }

                if (i != TimeOutCount)
                    break;
                else
                {
                    if (MyPara.Modbus.ReadData.OverTimeCount == 3)                          // 连续3次数据访问超时则停止读取数据
                    {
                        MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                        CloseReadData();
                        this.Invoke((MethodInvoker)delegate
                        {
                            MessageBox.Show("连续3次读取数据无响应, 数据读取停止", "提示");

                        });
                        return;
                    }
                }
            }

            int RecLen = serialPort1.BytesToRead;
            int Response;
            if (MyPara.Modbus.ReadData.RunModel == "RTU")
                Response = 9;
            else
                Response = 19;

            if (RecLen != Response)
            {
                for (int i = 0; i < 10; i++)
                {
                    if (serialPort1.IsOpen == false)
                    {
                        MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                        CloseReadData();
                        return;
                    }

                    int NewRecLen = serialPort1.BytesToRead;
                    if (NewRecLen != Response)
                    {
                        if (NewRecLen != RecLen)
                        {
                            i = -1;
                            RecLen = NewRecLen;
                        }
                        Thread.Sleep(2);
                    }
                    else
                        break;
                }
            }

            if (serialPort1.IsOpen == false)
            {
                MyPara.RunState.Model = MyPara.RunState.Model & (~MyPara.RunState.IsSerialUseMask); // 标记Serial使用结束
                CloseReadData();
                return;
            }
            byte[] RecData = new byte[serialPort1.BytesToRead];
            serialPort1.Read(RecData, 0, RecData.Length);
            RecLen = RecData.Length;
            if (MyPara.Modbus.ReadData.RunModel == "ASCII")
                RecData = Modbus.Ascii2RtuData(RecData, (uint)RecData.Length);

            if ((RecData[0] != MyPara.Modbus.ReadData.SenorID) ||                           // 地址错误
                (RecData[1] != 0x04) ||                                                     // 功能码错误
                (RecData[2] != 4) ||                                                        // 字节数错误
                (RecLen != Response))                                                       // 数据长度错误
            {
                RecData1.Text = "Error";
            }
            else
            {
                if ((MyPara.Modbus.ReadData.RunModel == "RTU" && (Modbus.GetCRC(RecData, (uint)RecData.Length) != 0)) || // CRC校验错误
                    (MyPara.Modbus.ReadData.RunModel == "ASCII" && (Modbus.GetLRC(RecData, (uint)RecData.Length) != 0)))
                {
                    RecData1.Text = "Error";
                }
                else
                {
                    if(MyPara.Modbus.ReadData.Formula[0] == "")
                    {
                        uint data = (uint)((RecData[3] << 24) + (RecData[4] << 16) + (RecData[5] << 8) + RecData[6]);
                        RecData1.Text = string.Format("0x{0:X8}", data);
                    }
                    else
                    {
                        MyPara.Modbus.ReadData.sReceData[0] = string.Format("{0:D}", RecData[6]);
                        MyPara.Modbus.ReadData.sReceData[1] = string.Format("{0:D}", RecData[5]);
                        MyPara.Modbus.ReadData.sReceData[2] = string.Format("{0:D}", RecData[4]);
                        MyPara.Modbus.ReadData.sReceData[3] = string.Format("{0:D}", RecData[3]);

                        MyPara.Modbus.ReadData.Condition = MyPara.Modbus.ReadData.Formula[0];
                        MyPara.Modbus.ReadData.Condition = MyPara.Modbus.ReadData.Condition.Replace("d0", MyPara.Modbus.ReadData.sReceData[0]);
                        MyPara.Modbus.ReadData.Condition = MyPara.Modbus.ReadData.Condition.Replace("d1", MyPara.Modbus.ReadData.sReceData[1]);
                        MyPara.Modbus.ReadData.Condition = MyPara.Modbus.ReadData.Condition.Replace("d2", MyPara.Modbus.ReadData.sReceData[2]);
                        MyPara.Modbus.ReadData.Condition = MyPara.Modbus.ReadData.Condition.Replace("d3", MyPara.Modbus.ReadData.sReceData[3]);

                        object Condition = null;
                        try
                        {
                            Condition = new DataTable().Compute(MyPara.Modbus.ReadData.Condition, null);
                        }
                        catch(Exception ex)
                        {
                            this.Invoke((MethodInvoker)delegate
                            {
                                MessageBox.Show("自定义条件错误,请检查!\r\n" + ex.Message, "错误");
                            });
                            CloseReadData();
                            return;
                        }

                        if(Condition.ToString() == "True")
                        {
                            MyPara.Modbus.ReadData.Formula1 = MyPara.Modbus.ReadData.Formula[1];
                            MyPara.Modbus.ReadData.Formula1 = MyPara.Modbus.ReadData.Formula1.Replace("d0", MyPara.Modbus.ReadData.sReceData[0]);
                            MyPara.Modbus.ReadData.Formula1 = MyPara.Modbus.ReadData.Formula1.Replace("d1", MyPara.Modbus.ReadData.sReceData[1]);
                            MyPara.Modbus.ReadData.Formula1 = MyPara.Modbus.ReadData.Formula1.Replace("d2", MyPara.Modbus.ReadData.sReceData[2]);
                            MyPara.Modbus.ReadData.Formula1 = MyPara.Modbus.ReadData.Formula1.Replace("d3", MyPara.Modbus.ReadData.sReceData[3]);

                            object Result = new DataTable().Compute(MyPara.Modbus.ReadData.Formula1, null);
                            RecData1.Text = Result.ToString();
                        }
                        else
                        {
                            MyPara.Modbus.ReadData.Formula2 = MyPara.Modbus.ReadData.Formula[2];
                            MyPara.Modbus.ReadData.Formula2 = MyPara.Modbus.ReadData.Formula2.Replace("d0", MyPara.Modbus.ReadData.sReceData[0]);
                            MyPara.Modbus.ReadData.Formula2 = MyPara.Modbus.ReadData.Formula2.Replace("d1", MyPara.Modbus.ReadData.sReceData[1]);
                            MyPara.Modbus.ReadData.Formula2 = MyPara.Modbus.ReadData.Formula2.Replace("d2", MyPara.Modbus.ReadData.sReceData[2]);
                            MyPara.Modbus.ReadData.Formula2 = MyPara.Modbus.ReadData.Formula2.Replace("d3", MyPara.Modbus.ReadData.sReceData[3]);

                            try
                            {
                                object Result = new DataTable().Compute(MyPara.Modbus.ReadData.Formula2, null);
                                RecData1.Text = Result.ToString();
                            }
                            catch(Exception ex)
                            {
                                this.Invoke((MethodInvoker)delegate
                                {
                                    MessageBox.Show(ex.Message, "错误");
                                });
                                CloseReadData();
                            } 
                        } 
                    }
                }
            }
        }

        private void timer_ShowReceData_Tick(object sender, EventArgs e)
        {
            if (MyPara.Modbus.Rece.AnalyzeFinsh)
            {
                timer_ShowReceData.Enabled = false;
                MyPara.Modbus.Rece.AnalyzeFinsh = false;
                if ((MyPara.RunState.Model & MyPara.RunState.ShowAnalyzeMask) == MyPara.RunState.ShowAnalyzeMask)
                    GetReceAnalyzeData();
                else
                    MyPara.RecTable.Rows.Clear();
            }
        }

        private void textBoxIn_KeyPress(object sender, KeyPressEventArgs e)
        {
            uint KeyCharNum = (uint)e.KeyChar;

            if (KeyCharNum == 17)                                   // Ctrl+Q 是否显示LRC/CRC校验
            {
                if (MyPara.InOut.IsShowVerify)
                {
                    MyPara.InOut.IsShowVerify = false;
                    label_Verity.Visible = false;
                    textBox_LRC.Visible = false;
                    textBox_CRC.Visible = false;
                }
                else
                {
                    MyPara.InOut.IsShowVerify = true;
                    label_Verity.Visible = true;
                    textBox_LRC.Visible = true;
                    textBox_CRC.Visible = true;
                    ShowVerity(MyPara.InOut.InputData, MyPara.InOut.InputLen);
                }

                return;
            }

            if (KeyCharNum == 4)                                    // Ctrl+D快捷键 清空输入文本内容
            {
                textBoxIn.Text = "";
                return;
            }

            if (KeyCharNum == 20)                                   // Ctrl+T快捷键 提示打开/提示关闭
            {
                if (MyPara.InOut.IsAutoTip)
                {
                    MyPara.InOut.IsAutoTip = false;
                    label_Tip.Visible = false;
                    this.toolTip1.SetToolTip(textBoxIn, "");
                }
                else
                {
                    MyPara.InOut.IsAutoTip = true;
                    label_Tip.Visible = true;
                    GetInputTip();                                  // 获取提示
                }

                return;
            }

            if (KeyCharNum == 19)                                   // Ctrl+S快捷键 自动插入空格/取消自动插入空格
            {
                if (MyPara.InOut.IsAutoInsertSpace)
                {
                    MyPara.InOut.IsAutoInsertSpace = false;
                    label_Space.Visible = false;
                }
                else
                {
                    MyPara.InOut.IsAutoInsertSpace = true;
                    label_Space.Visible = true;
                }

                return;
            }

            if (!(KeyCharNum >= 48 && KeyCharNum <= 57) &&          // 数字
                !(KeyCharNum >= 65 && KeyCharNum <= 70) &&          // 大写字母A~F
                !(KeyCharNum >= 97 && KeyCharNum <= 102) &&         // 小写字母a~f
                !(KeyCharNum == 1) &&                               // 全选
                !(KeyCharNum == 24) &&                              // 剪切
                !(KeyCharNum == 3) &&                               // 复制
                !(KeyCharNum == 22) &&                              // 帖贴
                !(KeyCharNum == 26) &&                              // 撤销
                !(KeyCharNum == 32) &&                              // 空格
                !(KeyCharNum == 8))                                 // 退格键
            {
                e.Handled = true;
                return;
            }

            if (KeyCharNum == 22)
            {
                MyPara.InOut.IsCtrlV = false;
                string PATTERN = @"([^A-Fa-f0-9]|\s+?)+";           // 正则表达式 16进制字符以外的字符
                string NewStr = System.Windows.Forms.Clipboard.GetText().Replace(" ", "");
                if (System.Text.RegularExpressions.Regex.IsMatch(NewStr, PATTERN))
                {
                    e.Handled = true;
                    return;
                }
                else
                    MyPara.InOut.IsCtrlV = true;
            }

            if (KeyCharNum == 32)
                MyPara.InOut.InsertSpacing = true;

            if ((KeyCharNum >= 48 && KeyCharNum <= 57) ||           // 数字
                (KeyCharNum >= 65 && KeyCharNum <= 70) ||           // 大写字母A~F
                (KeyCharNum >= 97 && KeyCharNum <= 102))            // 小写字母a~f
            {
                if (MyPara.InOut.IsAutoInsertSpace)
                {
                    if(textBoxIn.SelectionStart == textBoxIn.Text.Length)   // 光标在最未尾时才自动插入空格
                        AutoInsertSpace();                          // 自动插入空格
                }   
            }

            if (KeyCharNum >= 97 && KeyCharNum <= 102)
                e.KeyChar = Char.ToUpper(e.KeyChar);                // 小写字母转大写字母

            Uncheck();                                              // 取消选中
        }

        public void AutoInsertSpace()                               // 自动插入空格
        {
            if (textBoxIn.Text.Length >= 2)
            {
                char[] InputChar = textBoxIn.Text.Substring(textBoxIn.Text.Length - 2, 2).ToCharArray();
                if ((InputChar[0] != ' ') && (InputChar[1] != ' '))
                {
                    MyPara.InOut.InsertSpacing = true;
                    textBoxIn.AppendText(" ");
                }
            }
        }

        public void Uncheck()                                       // 取消选中
        {
            if (MyPara.Modbus.Mark.IsImport == false || MyPara.Modbus.Mark.IsMark == false)
                return;

            ParaTable.DefaultCellStyle.SelectionBackColor = Color.White;
            ParaTable.DefaultCellStyle.SelectionForeColor = Color.Black;

            Array.Clear(MyPara.Modbus.Mark.NewColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);
            for (int i = 0; i < MyPara.Modbus.Mark.NewColorMark.Length; i++)
            {
                if (MyPara.Modbus.Mark.NewColorMark[i] != MyPara.Modbus.Mark.OldColorMark[i])
                {
                    ParaTable.Rows[i].DefaultCellStyle.BackColor = Color.White;
                    ParaTable.Rows[i].DefaultCellStyle.ForeColor = Color.Black;
                }
            }
            Array.Clear(MyPara.Modbus.Mark.OldColorMark, 0, MyPara.Modbus.Mark.NewColorMark.Length);

            MyPara.Modbus.Mark.StartMarkNum = 0xFFFFFFFF;
            MyPara.Modbus.Mark.EndMarkNum = 0xFFFFFFFF;
            MyPara.Modbus.Mark.IsMark = false;
            ReadWrite.Enabled = true;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 4)                                     // Ctrl+D快捷键 清空输入文本内容
                richTextBox1.Text = "";
        }

        private void textBox_LRC_DoubleClick(object sender, EventArgs e)
        {
            if(textBox_LRC.Text.Length != 0)
                System.Windows.Forms.Clipboard.SetText(textBox_LRC.Text);
        }

        private void textBox_CRC_DoubleClick(object sender, EventArgs e)
        {
            if (textBox_CRC.Text.Length != 0)
                System.Windows.Forms.Clipboard.SetText(textBox_CRC.Text);
        }

        private void textBoxIn_Click(object sender, EventArgs e)
        {
            textBox_2AWrite.Visible = false;
            textBox_2AWrite.Text = "";
        }

        private void textBox_2AWrite_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(((uint)e.KeyChar) == 13)                             // 按下回车键 则将文本框内容转换后添加到textBoxIn的未尾
            {
                if (textBox_2AWrite.Text.Length > 0)
                {
                    string ConvStr = "";

                    byte[] textBox_2AWriteByte = Encoding.Default.GetBytes(textBox_2AWrite.Text);

                    ConvStr += string.Format(" {0:X2}", textBox_2AWriteByte.Length);
                    for (int i = 0; i < textBox_2AWriteByte.Length; i++)
                        ConvStr += string.Format(" {0:X2}", textBox_2AWriteByte[i]);

                    textBox_2AWrite.Visible = false;
                    textBox_2AWrite.Text = "";
                    textBoxIn.AppendText(ConvStr);
                }
            }
        }

        private void Baud_TextChanged(object sender, EventArgs e)
        {
            if (Baud.Text.Length > 0)
            {
                try
                {
                    MyPara.Serial.Baud = Convert.ToInt32(Baud.Text);
                    serialPort1.BaudRate = MyPara.Serial.Baud;
                }
                catch
                {
                }
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            SaveSettings();                                         // 保存配置
        }

        public void SaveSettings()
        {
            Settings.Default.SerialCom = SerialCom.Text;
            Settings.Default.Baud = Baud.Text;
            Settings.Default.Parity = Parity.Text;
            Settings.Default.StopBit = StopBit.Text;
            Settings.Default.numericUpDown_SenorID = (int)numericUpDown_SenorID.Value;
            Settings.Default.ReadWrite = ReadWrite.Text;
            Settings.Default.MsgID1 = MsgID1.Text;
            Settings.Default.ReadCycle = (int)ReadCycle.Value;
            Settings.Default.comboBoxInModel = comboBoxInModel.Text;
            Settings.Default.label_Space = MyPara.InOut.IsAutoInsertSpace;
            Settings.Default.label_Tip = MyPara.InOut.IsAutoTip;
            Settings.Default.label_Verity = MyPara.InOut.IsShowVerify;

            if ((MyPara.RunState.Model & MyPara.RunState.ShowSendMask) == MyPara.RunState.ShowSendMask)
                Settings.Default.ShowSend = true;
            else
                Settings.Default.ShowSend = false;

            if ((MyPara.RunState.Model & MyPara.RunState.ShowTimeMask) == MyPara.RunState.ShowTimeMask)
                Settings.Default.ShowTime = true;
            else
                Settings.Default.ShowTime = false;

            if ((MyPara.RunState.Model & MyPara.RunState.ShowAnalyzeMask) == MyPara.RunState.ShowAnalyzeMask)
                Settings.Default.ShowAnalyze = true;
            else
                Settings.Default.ShowAnalyze = false;

            Settings.Default.Save();
        }

        private void textBoxCon_DoubleClick(object sender, EventArgs e)
        {
            if (textBoxCon.Text.Length > 0)
                System.Windows.Forms.Clipboard.SetText(textBoxCon.Text);
        }
    }
}

