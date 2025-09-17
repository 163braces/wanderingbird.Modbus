using System;
using System.Collections.Generic;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Windows.Forms;
using thinger.DataConvertLib;
using wanderingbird.ModbusLib.Library;


namespace wanderingbird.CommunicationTest
{
    public partial class FrmModbusRTU : Form
    {

        // 声明通讯设备变量，将在连接时进行初始化
        private ModbusRTU device;
        // 连接状态
        private bool isConnected;

        public FrmModbusRTU()
        {
            InitializeComponent();
        }

        private void FrmModbusRTU_Load(object sender, EventArgs e)
        {
            this.lst_Info.Columns[1].Width = this.lst_Info.Width - this.lst_Info.Columns[0].Width - 30;

            // 串口配置
            this.cmb_Port.DataSource = SerialPort.GetPortNames();
            this.cmb_BaudRate.Items.AddRange(new string[] { "2400", "4800", "9600", "19200", "38400", "115200" });
            this.cmb_BaudRate.SelectedIndex = 2;
            this.cmb_Parity.DataSource = Enum.GetNames(typeof(Parity));
            this.cmb_Parity.SelectedIndex = 0;
            this.cmb_StopBits.DataSource = Enum.GetNames(typeof(StopBits));
            this.cmb_StopBits.SelectedIndex = 1;
            this.txt_DataBits.Text = "8";
            this.txt_DevAddress.Text = "1"; // 保持 TextBox 并设置默认值

            // 读写测试配置
            this.cmb_DataType.DataSource = Enum.GetNames(typeof(DataType)).Where(c => (DataType)Enum.Parse(typeof(DataType), c) <= DataType.String).ToList();
            this.cmb_DataType.SelectedIndex = 3; // 默认选择 Short

            // 为大小端下拉框绑定数据源
            this.cmb_DataFormat.DataSource = Enum.GetNames(typeof(DataFormat));
            this.cmb_DataFormat.SelectedIndex = 0; // 默认选择 ABCD
        }

        private void btn_Connect_Click(object sender, EventArgs e)
        {
            // --- 输入验证 ---
            byte stationAddress;
            if (!byte.TryParse(this.txt_DevAddress.Text, out stationAddress))
            {
                MessageBox.Show("站地址必须是一个有效的数字！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 中断连接过程
            }
            if (stationAddress < 1 || stationAddress > 247)
            {
                MessageBox.Show("站地址必须在 1 到 247 之间！", "输入错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return; // 中断连接过程
            }
            // --- 验证结束 ---

            // 在连接操作期间禁用按钮，防止重复点击
            btn_Connect.Enabled = false;
            btn_DisConn.Enabled = true;
            Application.DoEvents(); // 刷新界面

            try
            {
                // 1. 创建 ModbusRTU 对象
                this.device = new ModbusRTU(stationAddress, this.chk_IsShortAddress.Checked);

                // 2. 设置【大小端】格式
                device.DataFormat = (DataFormat)Enum.Parse(typeof(DataFormat), this.cmb_DataFormat.Text);

                // 3. 提取串口参数
                string portName = this.cmb_Port.Text;
                int baudRate = Convert.ToInt32(this.cmb_BaudRate.Text);
                Parity parity = (Parity)Enum.Parse(typeof(Parity), this.cmb_Parity.Text);
                int dataBits = Convert.ToInt32(this.txt_DataBits.Text);
                StopBits stopBits = (StopBits)Enum.Parse(typeof(StopBits), this.cmb_StopBits.Text);

                // 4. 调用连接方法
                var result = device.Connect(portName, baudRate, parity, dataBits, stopBits);

                if (result.IsSuccess)
                {
                    this.isConnected = true;
                    CommonMethods.AddLog(this.lst_Info, 0, "连接成功");
                }
                else
                {
                    this.isConnected = false;
                    btn_Connect.Enabled = true;
                    btn_DisConn.Enabled = false;
                    CommonMethods.AddLog(this.lst_Info, 2, "连接失败: " + result.Message);
                }
            }
            catch (Exception ex)
            {
                this.isConnected = false;
                btn_Connect.Enabled = true;
                btn_DisConn.Enabled = false;
                MessageBox.Show($"连接时发生错误: {ex.Message}");
                CommonMethods.AddLog(this.lst_Info, 2, "连接异常: " + ex.Message);
            }
        }

        private void btn_DisConn_Click(object sender, EventArgs e)
        {
            try
            {
                var result = device?.DisConnect();
                this.isConnected = false;
                btn_Connect.Enabled = true;
                btn_DisConn.Enabled = false;

                if (result != null && result.IsSuccess)
                {
                    CommonMethods.AddLog(this.lst_Info, 0, "断开连接成功");
                }
                else
                {
                    CommonMethods.AddLog(this.lst_Info, 1, "设备本未连接或断开时发生错误。");
                }
            }
            catch (Exception ex)
            {
                btn_Connect.Enabled = true;
                btn_DisConn.Enabled = false;
                MessageBox.Show($"断开连接时发生错误: {ex.Message}");
                CommonMethods.AddLog(this.lst_Info, 2, "断开异常: " + ex.Message);
            }
        }

        private void btn_Read_Click(object sender, EventArgs e)
        {
            if (!this.isConnected)
            {
                CommonMethods.AddLog(this.lst_Info, 1, "未建立连接，无法读取数据");
                return;
            }

            try
            {
                string address = this.txt_Variable.Text;
                ushort count = Convert.ToUInt16(this.txt_Count.Text);
                DataType dataType = (DataType)Enum.Parse(typeof(DataType), this.cmb_DataType.Text);

                switch (dataType)
                {
                    case DataType.Bool:
                        {
                            var result = device.ReadCommon<bool[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Byte:
                        {
                            var result = device.ReadCommon<byte[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.SByte:
                        {
                            var result = device.ReadCommon<sbyte[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Short:
                        {
                            var result = device.ReadCommon<short[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.UShort:
                        {
                            var result = device.ReadCommon<ushort[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Int:
                        {
                            var result = device.ReadCommon<int[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.UInt:
                        {
                            var result = device.ReadCommon<uint[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Float:
                        {
                            var result = device.ReadCommon<float[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Double:
                        {
                            var result = device.ReadCommon<double[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.Long:
                        {
                            var result = device.ReadCommon<long[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.ULong:
                        {
                            var result = device.ReadCommon<ulong[]>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + StringLib.GetStringFromValueArray(result.Content));
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    case DataType.String:
                        {
                            var result = device.ReadCommon<string>(address, count);
                            if (result.IsSuccess) CommonMethods.AddLog(lst_Info, 0, "读取成功: " + result.Content);
                            else CommonMethods.AddLog(lst_Info, 1, "读取失败: " + result.Message);
                            break;
                        }
                    default:
                        CommonMethods.AddLog(this.lst_Info, 1, "读取失败: 不支持的数据类型");
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取操作失败: {ex.Message}");
                CommonMethods.AddLog(this.lst_Info, 1, "读取失败: " + ex.Message);
            }
        }

        private void btn_Write_Click(object sender, EventArgs e)
        {
            if (!this.isConnected)
            {
                CommonMethods.AddLog(this.lst_Info, 1, "未建立连接，无法写入数据");
                return;
            }

            OperateResult result = null;

            try
            {
                string address = this.txt_Variable.Text;
                string textValue = this.txt_SetValue.Text.Trim();
                DataType dataType = (DataType)Enum.Parse(typeof(DataType), this.cmb_DataType.Text);

                switch (dataType)
                {
                    case DataType.Bool:
                        result = device.WriteCommon(address, ParseBoolArrayFromString(textValue));
                        break;
                    case DataType.Byte:
                    case DataType.SByte:
                        result = device.WriteCommon(address, ByteArrayLib.GetByteArrayFromHexString(textValue));
                        break;
                    case DataType.Short:
                        result = device.WriteCommon(address, ShortLib.GetShortArrayFromString(textValue));
                        break;
                    case DataType.UShort:
                        result = device.WriteCommon(address, UShortLib.GetUShortArrayFromString(textValue));
                        break;
                    case DataType.Int:
                        result = device.WriteCommon(address, IntLib.GetIntArrayFromString(textValue));
                        break;
                    case DataType.UInt:
                        result = device.WriteCommon(address, UIntLib.GetUIntArrayFromString(textValue));
                        break;
                    case DataType.Float:
                        result = device.WriteCommon(address, FloatLib.GetFloatArrayFromString(textValue));
                        break;
                    case DataType.Double:
                        result = device.WriteCommon(address, DoubleLib.GetDoubleArrayFromString(textValue));
                        break;
                    case DataType.Long:
                        result = device.WriteCommon(address, LongLib.GetLongArrayFromString(textValue));
                        break;
                    case DataType.ULong:
                        result = device.WriteCommon(address, ULongLib.GetULongArrayFromString(textValue));
                        break;
                    case DataType.String:
                        result = device.WriteCommon(address, textValue);
                        break;
                    default:
                        CommonMethods.AddLog(this.lst_Info, 1, "写入失败: 不支持的数据类型");
                        return;
                }

                if (result != null && result.IsSuccess)
                {
                    CommonMethods.AddLog(this.lst_Info, 0, "写入成功");
                }
                else
                {
                    string errorMsg = result?.Message ?? "未知错误";
                    CommonMethods.AddLog(this.lst_Info, 1, "写入失败: " + errorMsg);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"写入操作失败: {ex.Message}");
                CommonMethods.AddLog(this.lst_Info, 1, "写入失败: " + ex.Message);
            }
        }

        private bool[] ParseBoolArrayFromString(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return new bool[0];
            string[] parts = input.Split(new[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
            List<bool> bools = new List<bool>();
            foreach (string part in parts)
            {
                string lowerPart = part.ToLower();
                if (lowerPart == "true" || lowerPart == "1") bools.Add(true);
                else if (lowerPart == "false" || lowerPart == "0") bools.Add(false);
                else throw new FormatException($"无法将 '{part}' 转换为布尔值。");
            }
            return bools.ToArray();
        }
    }
}