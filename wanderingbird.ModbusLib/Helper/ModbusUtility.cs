using System;
using System.Linq;
using thinger.DataConvertLib;
using wanderingbird.ModbusLib.Models;

namespace wanderingbird.ModbusLib.Helper
{
    /// <summary>
    /// 提供与Modbus协议相关的辅助功能
    /// </summary>
    public static class ModbusUtility
    {
        /// <summary>
        /// 解析Modbus的逻辑地址字符串，将其转换为协议所需的具体地址对象。
        /// </summary>
        /// <param name="logicAddress">要解析的逻辑地址字符串。格式支持 "40001" 或 "s.40001" (s为站号)。</param>
        /// <param name="defaultStation">当逻辑地址中不包含站号时，使用的默认站号。</param>
        /// <param name="isShortAddress">是否使用短地址模式。为 true 时，地址将从0开始且长度被视为5位。</param>
        /// <returns>一个包含解析结果的操作对象。如果成功，Content属性为 ModbusAddress 对象。</returns>
        /// <remarks>
        /// 地址映射规则:
        /// - "0xxxx" -> 功能码 01 (读线圈)
        /// - "1xxxx" -> 功能码 02 (读离散输入)
        /// - "3xxxx" -> 功能码 04 (读输入寄存器)
        /// - "4x...x" -> 功能码 03 (读保持寄存器)
        /// 注意：地址编号会减1来作为协议的起始地址，例如 "40001" 对应协议地址 0。
        /// </remarks>
        public static OperateResult<ModbusRequestInfo> AddressAnalysis(string logicAddress, byte defaultStation, bool isShortAddress)
        {
            var result = new OperateResult<ModbusRequestInfo>();
            var modbusRequestInfo = new ModbusRequestInfo();
            string addressCode;
            try
            {
                //如果逻辑地址是点分格式,就将其分割为string字符数组，并将首位值作为从站ID，
                if (logicAddress.Contains('.'))
                {
                    var parts = logicAddress.Split('.');
                    modbusRequestInfo.Station = byte.Parse(parts[0]);
                    addressCode = parts[1];
                }
                else
                {
                    modbusRequestInfo.Station = defaultStation;
                    addressCode = logicAddress;
                }
                // 处理地址补齐 (长度固定为5位)
                // 这一步主要是为了健壮性，兼容 "1", "101" 等不完整输入
                addressCode = addressCode.PadLeft(5, '0');
                //获取起始地址
                ushort addressValue = ushort.Parse(addressCode.Substring(1));

                // 使用 isShortAddress 这【一个】开关来决定是否减1
                modbusRequestInfo.Address = isShortAddress ? addressValue : (ushort)(addressValue - 1);
                switch (addressCode[0])
                {
                    case '4':
                        modbusRequestInfo.FunctionCode = 0x03;
                        break;
                    case '3':
                        modbusRequestInfo.FunctionCode = 0x04;
                        break;
                    case '1':
                        modbusRequestInfo.FunctionCode = 0x02;
                        break;
                    case '0':
                        modbusRequestInfo.FunctionCode = 0x01;
                        break;
                    default:
                        // 如果首字母不是0,1,3,4，就抛出一个异常，让外层的catch去抓
                        throw new Exception("无法识别的存储区，地址必须以0,1,3,4开头。");
                }
                result.IsSuccess = true;
                result.Content = modbusRequestInfo;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "地址解析发生异常" + ex.Message;
                return result;
            }
        }

        public static OperateResult<ModbusRequestInfo> AddressAnalysis(string logicAddress, byte defaultStation, bool isShortAddress, ushort length)
        {
            var result = new OperateResult<ModbusRequestInfo>();
            var modbusRequestInfo = new ModbusRequestInfo();
            string addressCode;
            try
            {
                //如果逻辑地址是点分格式,就将其分割为string字符数组，并将首位值作为从站ID，
                if (logicAddress.Contains('.'))
                {
                    var parts = logicAddress.Split('.');
                    modbusRequestInfo.Station = byte.Parse(parts[0]);
                    addressCode = parts[1];
                }
                else
                {
                    modbusRequestInfo.Station = defaultStation;
                    addressCode = logicAddress;
                }
                // 处理地址补齐 (长度固定为5位)
                // 这一步主要是为了健壮性，兼容 "1", "101" 等不完整输入
                addressCode = addressCode.PadLeft(5, '0');
                //获取起始地址
                ushort addressValue = ushort.Parse(addressCode.Substring(1));

                // 使用 isShortAddress 这【一个】开关来决定是否减1
                modbusRequestInfo.Address = isShortAddress ? addressValue : (ushort)(addressValue - 1);
                switch (addressCode[0])
                {
                    case '4':
                        modbusRequestInfo.FunctionCode = (length > 1) ? (byte)0x10 : (byte)0x06;
                        break;
                    case '3':
                        throw new Exception("地址 " + logicAddress + " 是只读的，不支持写入。");
                    case '1':
                        throw new Exception("地址 " + logicAddress + " 是只读的，不支持写入。");
                    case '0':
                        modbusRequestInfo.FunctionCode = (length > 1) ? (byte)0x0F : (byte)0x05;
                        break;
                    default:
                        // 如果首字母不是0,1,3,4，就抛出一个异常，让外层的catch去抓
                        throw new Exception("无法识别的存储区，地址必须以0,1,3,4开头。");
                }
                result.IsSuccess = true;
                result.Content = modbusRequestInfo;
                return result;
            }
            catch (Exception ex)
            {
                result.IsSuccess = false;
                result.Message = "地址解析发生异常" + ex.Message;
                return result;
            }
        }
    }
}
