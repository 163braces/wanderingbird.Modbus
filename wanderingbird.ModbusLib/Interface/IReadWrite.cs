using thinger.DataConvertLib;

namespace wanderingbird.ModbusLib.Interface
{
    /// <summary>
    /// 提供设备进行数据读写的核心接口。
    /// </summary>
    public interface IReadWrite
    {
        /// <summary>
        /// 读取布尔数组
        /// </summary>
        /// <param name="logicAddress">起始地址</param>
        /// <param name="length">读取数量</param>
        /// <returns></returns>
        OperateResult<bool[]> ReadBoolArray(string logicAddress, ushort length);

        /// <summary>
        /// 读取字节数组
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        OperateResult<byte[]> ReadByteArray(string logicAddress, ushort length);

        /// <summary>
        /// 写入布尔数组
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        OperateResult WriteBoolArray(string logicAddress, bool[] value);

        /// <summary>
        /// 写入字节数组
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        OperateResult WriteByteArray(string logicAddress, byte[] value);
    }
}
