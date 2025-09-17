namespace wanderingbird.ModbusLib.Models
{
    /// <summary>
    /// 存储Modbus请求信息
    /// </summary>
    public class ModbusRequestInfo
    {
        /// <summary>
        /// 从站编号
        /// </summary>
        public byte Station { get; set; }
        /// <summary>
        /// 功能码
        /// </summary>
        public byte FunctionCode { get; set; }
        /// <summary>
        /// 起始地址
        /// </summary>
        public ushort Address { get; set; }
    }
}
