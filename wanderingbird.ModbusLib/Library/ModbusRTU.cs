using System;
using System.Collections.Generic;
using System.Linq;
using thinger.DataConvertLib;
using wanderingbird.ModbusLib.Base;
using wanderingbird.ModbusLib.Enums;
using wanderingbird.ModbusLib.Helper;

namespace wanderingbird.ModbusLib.Library
{
    /// <summary>
    /// ModbusRTU协议库
    /// </summary>
    public class ModbusRTU : SerialDeviceBase
    {
        #region 属性
        /// <summary>
        /// 从站ID 默认值1
        /// </summary>
        public byte Station { get; private set; } = 1;

        /// <summary>
        /// 获取或设置是否使用短地址模式。
        /// 默认为 false，表示遵循标准地址约定 (40001 对应地址 0)。
        /// 设置为 true，通常表示使用基地址为0的约定 (40000 对应地址 0)。
        /// </summary>
        public bool IsShortAddress { get; private set; } = false;
        #endregion

        #region 构造函数
        /// <summary>
        /// 使用默认配置 (站号=1, 标准地址) 初始化一个新的 ModbusRTU 实例。
        /// </summary>
        public ModbusRTU() : this(1, false) { }

        /// <summary>
        /// 使用指定的站号和默认地址模式初始化一个新的 ModbusRTU 实例。
        /// </summary>
        /// <param name="station">设备的Modbus从站地址 (1-247)。</param>
        public ModbusRTU(byte station) : this(station, false) { }

        /// <summary>
        /// 使用指定的站号和地址模式初始化一个新的 ModbusRTU 实例。
        /// </summary>
        /// <param name="station">设备的Modbus从站地址 (1-247)。</param>
        /// <param name="isShortAddress">是否使用短地址模式。</param>
        public ModbusRTU(byte station, bool isShortAddress)
        {
            if (station == 0 || station > 247)
            {
                throw new ArgumentOutOfRangeException(nameof(station), "从站地址必须在 1 到 247 之间。");
            }

            this.Station = station;
            this.IsShortAddress = isShortAddress;

            // 设置协议的最小单位
            this.AreaType = AreaType.Word;
        }
        #endregion

        #region 生成读取寄存器报文帧
        /// <summary>
        /// 生成读取保持寄存器0x03或输入寄存器0x04的请求报文
        /// </summary>
        /// <param name="logicAddress">包含功能码和起始地址的逻辑地址</param>
        /// <param name="length">要读取寄存器的数量</param>
        /// <returns>一个包含完整ModbusRTU请求帧的OperateResult对象</returns>
        private OperateResult<byte[]> CreateReadRegistersFrame(string logicAddress, ushort length)
        {
            var analysisResult = ModbusUtility.AddressAnalysis(logicAddress, Station, IsShortAddress);
            if (!analysisResult.IsSuccess)
            {
                // 如果地址解析失败，直接将失败结果向上传递
                return OperateResult.CreateFailResult<byte[]>(analysisResult);
            }
            // 获取Modbus请求信息
            var modbusRequestInfo = analysisResult.Content;
            List<byte> readFrame = new List<byte>();
            readFrame.Add(modbusRequestInfo.Station);
            readFrame.Add(modbusRequestInfo.FunctionCode);
            // 添加起始地址 (2字节, 大端序：高位在前)
            readFrame.Add((byte)(modbusRequestInfo.Address >> 8));
            readFrame.Add((byte)(modbusRequestInfo.Address));
            // 添加寄存器数量(2字节, 大端序：高位在前)
            readFrame.Add((byte)(length >> 8));
            readFrame.Add((byte)length);
            var crc = ParityHelper.CalculateCRC(readFrame.ToArray(), readFrame.Count);
            readFrame.AddRange(crc);
            return OperateResult.CreateSuccessResult<byte[]>(readFrame.ToArray());
        }
        #endregion

        #region 解析读取寄存器响应报文
        /// <summary>
        /// 解析读取寄存器响应报文，返回寄存器数据
        /// </summary>
        /// <param name="response">响应报文</param>
        /// <param name="station">从站编号</param>
        /// <param name="functionCode">功能码</param>
        /// <param name="registerCount">读取寄存器数量</param>
        /// <returns>寄存器数据</returns>
        private OperateResult<byte[]> ParseReadRegistersFrame(byte[] response, byte station, byte functionCode, ushort registerCount)
        {
            if (!ParityHelper.CheckCRC(response))
            {
                return OperateResult.CreateFailResult<byte[]>("CRC校验失败");
            }
            if (response[0] != station)
            {
                return OperateResult.CreateFailResult<byte[]>("从站ID不一致");
            }
            if (response[1] == (functionCode + 0x80))
            {
                return OperateResult.CreateFailResult<byte[]>($"设备返回异常码: {response[2]:X2}");
            }
            if (response[1] != functionCode)
            {
                return OperateResult.CreateFailResult<byte[]>("功能码不一致");
            }
            if (response[2] != registerCount * 2)
            {
                return OperateResult.CreateFailResult<byte[]>("返回数据字节数与预期不符");
            }
            byte[] data = new byte[response[2]];
            Array.Copy(response, 3, data, 0, response[2]);
            return OperateResult.CreateSuccessResult<byte[]>(data);
        }
        #endregion

        #region 生成读取线圈报文帧
        /// <summary>
        /// 生成读取输出线圈0x01，输入线圈0x02的请求报文
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">线圈数量</param>
        /// <returns></returns>
        private OperateResult<byte[]> CreateReadCoilFrame(string logicAddress, ushort length)
        {
            var analysisResult = ModbusUtility.AddressAnalysis(logicAddress, Station, IsShortAddress);
            if (!analysisResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<byte[]>(analysisResult);
            }
            List<byte> readFrame = new List<byte>();
            var modbusRequestInfo = analysisResult.Content;
            readFrame.Add(modbusRequestInfo.Station);
            readFrame.Add(modbusRequestInfo.FunctionCode);
            readFrame.Add((byte)(modbusRequestInfo.Address >> 8));
            readFrame.Add((byte)(modbusRequestInfo.Address));
            readFrame.Add((byte)(length >> 8));
            readFrame.Add((byte)length);
            var crc = ParityHelper.CalculateCRC(readFrame.ToArray(), readFrame.Count);
            readFrame.AddRange(crc);
            return OperateResult.CreateSuccessResult<byte[]>(readFrame.ToArray());
        }
        #endregion

        #region 解析读取线圈响应报文
        private OperateResult<bool[]> ParseReadCoilFrame(byte[] response, byte station, byte functionCode, ushort length)
        {
            if (!ParityHelper.CheckCRC(response))
            {
                return OperateResult.CreateFailResult<bool[]>("CRC校验失败");
            }
            if (response[0] != station)
            {
                return OperateResult.CreateFailResult<bool[]>("从站ID不一致");
            }
            if (response[1] == (functionCode + 0x80))
            {
                return OperateResult.CreateFailResult<bool[]>($"设备返回异常码: {response[2]:X2}");
            }
            if (response[1] != functionCode)
            {
                return OperateResult.CreateFailResult<bool[]>("功能码不一致");
            }
            if (response[2] != (length + 7) / 8)
            {
                return OperateResult.CreateFailResult<bool[]>("返回数据字节数与预期不符");
            }
            //建立一个byte数组存储报文数据位
            var dataBytes = new byte[response[2]];
            //提取数据，将报文从数据位开始到结束复制到另一个数组
            Array.Copy(response, 3, dataBytes, 0, response[2]);
            //建立布尔数据存储转换后的数据
            var databools = new bool[length];
            int boolsCount = 0;
            for (int i = 0; i < dataBytes.Length; i++)
            {
                //按照顺序提取字节数据
                var temp = dataBytes[i];
                for (int j = 0; j < 8; j++)
                {
                    if (boolsCount >= length)
                    {
                        break; // 如果够了，就跳出内层循环
                    }
                    //将每个字节数据按位相与
                    bool bitState = (temp >> j & 1) == 1;
                    //将获取到的布尔数组存入数组
                    databools[boolsCount] = bitState;
                    boolsCount++;
                }
                if (boolsCount >= length)
                {
                    break; // 如果够了，也跳出外层循环
                }
            }
            return OperateResult.CreateSuccessResult<bool[]>(databools);
        }

        #endregion

        #region ReadBoolArray
        /// <summary>
        /// 读取指定数量线圈
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">线圈数量</param>
        /// <returns>返回以bool[]形式的线圈数值</returns>
        public override OperateResult<bool[]> ReadBoolArray(string logicAddress, ushort length)
        {
            //1、封包，将用户传入的逻辑地址转化为ModbusRTU协议数据帧
            var createFrameResult = CreateReadCoilFrame(logicAddress, length);
            if (!createFrameResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<bool[]>(createFrameResult.Message);
            }
            //2、发送并接收报文
            var responseResult = base.SendAndReceive(createFrameResult.Content);
            //检查通讯结果
            if (!responseResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<bool[]>(responseResult);
            }
            // 3、验证解析报文
            // 从请求报文中获取对应站号、功能码
            byte station = createFrameResult.Content[0];
            byte functionCode = createFrameResult.Content[1];
            return ParseReadCoilFrame(responseResult.Content, station, functionCode, length);
        }
        #endregion

        #region ReadByteArray
        /// <summary>
        /// 读取指定数量寄存器
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">寄存器数量</param>
        /// <returns>返回byte[]形式的寄存器数值</returns>
        public override OperateResult<byte[]> ReadByteArray(string logicAddress, ushort length)
        {
            // 1、封包，将用户传入的逻辑地址和所需读取寄存器数量转化为ModbusRTU协议数据帧
            var createFrameResult = CreateReadRegistersFrame(logicAddress, length);
            if (!createFrameResult.IsSuccess)
            {
                // 如果封包失败 (例如地址解析错误)，直接返回失败信息
                return createFrameResult;
            }

            // 2、发送并接收响应报文
            var responseResult = base.SendAndReceive(createFrameResult.Content);

            // 检查通信过程是否成功
            if (!responseResult.IsSuccess)
            {
                // 如果失败（比如超时），直接将底层的错误信息返回
                return responseResult;
            }

            // 3、验证报解析报文
            // 从请求报文中获取对应站号、功能码
            byte station = createFrameResult.Content[0];
            byte functionCode = createFrameResult.Content[1];
            return ParseReadRegistersFrame(responseResult.Content, station, functionCode, length);
        }
        #endregion

        #region WriteBoolArray
        /// <summary>
        /// 写入多个线圈
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="value">写入数据</param>
        /// <returns>返回写入结果</returns>
        public override OperateResult WriteBoolArray(string logicAddress, bool[] value)
        {
            //1、封包，将用户传入的逻辑地址、写入数据转化为ModbusRTU协议数据帧
            var createFrameResult = CreateWriteCoilFrame(logicAddress, value);
            if (!createFrameResult.IsSuccess)
            {
                return OperateResult.CreateFailResult(createFrameResult.Message);
            }
            //2、发送并接收响应报文
            var responseResult = base.SendAndReceive(createFrameResult.Content);
            if (!responseResult.IsSuccess)
            {
                return OperateResult.CreateFailResult(responseResult.Message);
            }
            // 3、验证解析报文
            byte station = createFrameResult.Content[0];
            byte functionCode = createFrameResult.Content[1];
            byte addressHigh = createFrameResult.Content[2];
            byte addressLow = createFrameResult.Content[3];
            return ParseWriteCoilFrame(responseResult.Content, station, functionCode, addressHigh, addressLow, (ushort)value.Length);
        }
        #endregion

        #region 解析写入线圈响应报文
        /// <summary>
        /// 解析写入线圈响应报文
        /// </summary>
        /// <param name="response">响应报文</param>
        /// <param name="station">从站ID</param>
        /// <param name="functionCode">功能码</param>
        /// <param name="addressHigh">起始地址高位</param>
        /// <param name="addressLow">起始地址低位</param>
        /// <param name="quantity">写入数量</param>
        /// <returns>返回响应报文验证结果</returns>
        private OperateResult ParseWriteCoilFrame(byte[] response, byte station, byte functionCode, byte addressHigh, byte addressLow, ushort quantity)
        {
            if (!ParityHelper.CheckCRC(response))
            {
                return OperateResult.CreateFailResult("CRC校验失败");
            }
            if (response[0] != station)
            {
                return OperateResult.CreateFailResult("从站ID不一致");
            }
            if (response[1] == (functionCode + 0x80))
            {
                return OperateResult.CreateFailResult($"设备返回异常码: {response[2]:X2}");
            }
            if (response[1] != functionCode)
            {
                return OperateResult.CreateFailResult("功能码不一致");
            }
            if (response[2] != addressHigh || response[3] != addressLow)
            {
                return OperateResult.CreateFailResult("起始地址不一致");
            }
            // 从响应报文中提取最后两个字节代表的 ushort 值
            ushort responseLastTwoBytes = (ushort)((response[4] << 8) | response[5]);
            if (functionCode == 0x05) // 如果是“写单个线圈”
            {
                // 对于写单个，最后两个字节代表的是【写入的值】 (0xFF00 或 0x0000)
                // 而我们期望的写入数量 quantity 必须是 1
                if (quantity != 1)
                {
                    return OperateResult.CreateFailResult($"写单个操作的数量必须为1，但请求数量为 {quantity}");
                }

                // 我们可以对写入的值做一个基本检查，它必须是 0xFF00 或 0x0000
                if (responseLastTwoBytes != 0xFF00 && responseLastTwoBytes != 0x0000)
                {
                    return OperateResult.CreateFailResult($"写单个响应返回了无效的值: {responseLastTwoBytes:X4}");
                }
            }
            else
            {
                //校验写入数量
                // 3. 从响应报文中提取【写入数量】
                ushort responseQuantity = (ushort)((response[4] << 8) | response[5]);
                // 比较写入数量
                if (responseQuantity != quantity)
                {
                    return OperateResult.CreateFailResult($"返回的写入数量不一致 (期望: {quantity}, 实际: {responseQuantity})");
                }
            }
            return OperateResult.CreateSuccessResult();
        }
        #endregion

        #region 生成写入线圈请求报文帧
        /// <summary>
        /// 生成写入线圈请求报文帧
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="value">写入数据</param>
        /// <returns>返回写入请求报文帧</returns>
        private OperateResult<byte[]> CreateWriteCoilFrame(string logicAddress, bool[] value)
        {
            //1、将逻辑地址转化为部分ModbusRTU协议帧信息
            var analysisResult = ModbusUtility.AddressAnalysis(logicAddress, Station, IsShortAddress, (ushort)value.Length);
            if (!analysisResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<byte[]>(analysisResult);
            }
            var modbusRequestInfo = analysisResult.Content;
            //2、将结果信息依次放入List<byte>集合中
            List<byte> writeFrame = new List<byte>();
            //添加从站ID
            writeFrame.Add(modbusRequestInfo.Station);
            //添加功能码
            writeFrame.Add(modbusRequestInfo.FunctionCode);
            //添加起始地址高位
            writeFrame.Add((byte)(modbusRequestInfo.Address >> 8));
            //添加起始地址低位
            writeFrame.Add((byte)(modbusRequestInfo.Address));
            if (modbusRequestInfo.FunctionCode == 0x05)
            {
                if (value[0] == true)
                {
                    writeFrame.Add(0xFF);
                    writeFrame.Add(0x00);
                }
                else
                {
                    writeFrame.Add(0x00);
                    writeFrame.Add(0x00);
                }
            }
            else
            {
                ushort quantity = (ushort)value.Length;
                writeFrame.Add((byte)(quantity >> 8));
                writeFrame.Add((byte)quantity);

                // 计算并添加字节数 (1字节)
                byte byteCount = (byte)((quantity + 7) / 8);
                writeFrame.Add(byteCount);

                //调用方法将原有布尔数组转化为字节形式
                byte[] packedData = PackBoolsToBytes(value);
                writeFrame.AddRange(packedData);
            }
            // 3. 计算并添加 CRC (通用步骤)
            var crc = ParityHelper.CalculateCRC(writeFrame.ToArray(), writeFrame.Count);
            writeFrame.AddRange(crc);

            return OperateResult.CreateSuccessResult(writeFrame.ToArray());
        }

        private byte[] PackBoolsToBytes(bool[] value)
        {
            // 1. 计算需要多少个字节来存放
            int byteCount = (value.Length + 7) / 8;
            byte[] packedData = new byte[byteCount];

            int boolIndex = 0;

            for (int i = 0; i < byteCount; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    if (boolIndex >= value.Length) break;

                    if (value[boolIndex] == true)
                    {
                        // 使用“按位或”和“左移”来设置字节中的特定比特位
                        packedData[i] = (byte)(packedData[i] | (1 << j));
                    }

                    boolIndex++;
                }
            }

            return packedData;
        }


        #endregion

        #region WriteByteArray
        /// <summary>
        /// 写入多个寄存器
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="value">写入值</param>
        /// <returns>返回写入结果</returns>
        public override OperateResult WriteByteArray(string logicAddress, byte[] value)
        {
            //对数据进行校验，不能为空
            if (value == null || value.Length == 0 || value.Length % 2 != 0)
            {
                return OperateResult.CreateFailResult("写入的数据不能为空，且字节长度必须为偶数。");
            }
            //1、封包
            var createFrameResult = CreateWriteRegFrame(logicAddress, value);
            //验证是否封包成功
            if (!createFrameResult.IsSuccess)
            {
                return createFrameResult;
            }
            //2、发送并接收报文
            var requestFream = SendAndReceive(createFrameResult.Content);
            //验证报文是否发送并成功接收响应报文
            if (!requestFream.IsSuccess)
            {
                return requestFream;
            }
            //3、解析报文
            return ParseWriteRegFrame(createFrameResult, requestFream, value);

        }

        /// <summary>
        /// 验证写入寄存器响应报文
        /// </summary>
        /// <param name="createFrameResult">请求报文</param>
        /// <param name="requestFream">响应报文</param>
        /// <param name="value">写入数据</param>
        /// <returns>响应报文是否正确</returns>
        private OperateResult ParseWriteRegFrame(OperateResult<byte[]> createFrameResult, OperateResult<byte[]> requestFream, byte[] value)
        {
            if (!ParityHelper.CheckCRC(requestFream.Content))
            {
                return OperateResult.CreateFailResult("CRC校验失败");
            }
            if (requestFream.Content[0] != createFrameResult.Content[0])
            {
                return OperateResult.CreateFailResult("从站ID不一致");
            }
            if (requestFream.Content[1] == (createFrameResult.Content[1] + 0x80))
            {
                return OperateResult.CreateFailResult($"设备返回异常码: {requestFream.Content[2]:X2}");
            }
            if (requestFream.Content[1] != createFrameResult.Content[1])
            {
                return OperateResult.CreateFailResult("功能码不一致");
            }
            if (requestFream.Content[2] != createFrameResult.Content[2] || requestFream.Content[3] != createFrameResult.Content[3])
            {
                return OperateResult.CreateFailResult("起始地址不一致");
            }
            //提取功能码
            byte functionCode = createFrameResult.Content[1];
            //判断其是写入单个寄存器还是多个寄存器
            if (functionCode == 0x06) // 如果是写单个
            {
                // 比较两个字节数组是否完全相等，需要用 SequenceEqual
                if (!createFrameResult.Content.SequenceEqual(requestFream.Content))
                {
                    return OperateResult.CreateFailResult("写单个响应与请求不一致");
                }
            }
            else // 如果是写多个
            {
                // 从响应报文中提取【寄存器数量】
                ushort responseQuantity = (ushort)((requestFream.Content[4] << 8) | requestFream.Content[5]);

                // 期望的寄存器数量
                ushort expectedQuantity = (ushort)(value.Length / 2);

                if (responseQuantity != expectedQuantity)
                {
                    return OperateResult.CreateFailResult($"写入多个寄存器，数量不一致 (期望: {expectedQuantity}, 实际: {responseQuantity})");
                }
            }
            // 成功时不返回Message
            return OperateResult.CreateSuccessResult();
        }
        #endregion

        #region 生成写入寄存器请求报文帧
        /// <summary>
        /// 生成写入寄存器报文帧
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="value">写入值</param>
        /// <returns>返回写入寄存器报文帧</returns>
        /// <exception cref="NotImplementedException"></exception>
        private OperateResult<byte[]> CreateWriteRegFrame(string logicAddress, byte[] value)
        {
            ushort registerCount = (ushort)(value.Length / 2);
            //解析逻辑地址，获取ModbusRequestInfo，即从站编号、功能码、起始地址
            var analysisResult = ModbusUtility.AddressAnalysis(logicAddress, Station, IsShortAddress, registerCount);
            if (!analysisResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<byte[]>(analysisResult);
            }
            //提取协议信息
            var modbusRequestInfo = analysisResult.Content;
            List<byte> writeFrame = new List<byte>();
            writeFrame.Add(modbusRequestInfo.Station);
            writeFrame.Add(modbusRequestInfo.FunctionCode);
            //加入高位地址
            writeFrame.Add((byte)(modbusRequestInfo.Address >> 8));
            //加入低位地址
            writeFrame.Add((byte)(modbusRequestInfo.Address));
            //判断功能码，判断是否为写入单个寄存器
            if (modbusRequestInfo.FunctionCode == 0x06)
            {
                writeFrame.Add(value[0]);
                writeFrame.Add(value[1]);
            }
            else
            {
                //写入多个寄存器下，写入寄存器数量的字节数占2个字节
                writeFrame.Add((byte)(registerCount >> 8));
                writeFrame.Add((byte)registerCount);
                // 添加数据占用字节数（字节计数）(1字节)
                writeFrame.Add((byte)value.Length);
                //添加数据
                writeFrame.AddRange(value);
            }
            // CRC 校验
            var crc = ParityHelper.CalculateCRC(writeFrame.ToArray(), writeFrame.Count);
            writeFrame.AddRange(crc);
            return OperateResult.CreateSuccessResult<byte[]>(writeFrame.ToArray());
        }
        #endregion
    }
}
