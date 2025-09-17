namespace wanderingbird.ModbusLib.Enums
{
    /// <summary>
    /// 规定存储区最小单位
    /// </summary>
    public enum AreaType
    {
        Byte = 1,
        Word = 2,
    }

    /// <summary>
    /// 定义了Modbus协议中常用的功能码。
    /// </summary>
    public enum FunctionCode : byte // 我们明确指定它的底层类型是 byte
    {
        /// <summary>
        /// 读取输出线圈
        /// </summary>
        ReadCoil = 0x01,

        /// <summary>
        /// 读取输入线圈
        /// </summary>
        ReadDiscreteInput = 0x02,

        /// <summary>
        /// 读取输出寄存器
        /// </summary>
        ReadHoldingRegister = 0x03,

        /// <summary>
        /// 读取输入寄存器
        /// </summary>
        ReadInputRegister = 0x04,

        /// <summary>
        /// 写入单个线圈
        /// </summary>
        WriteSingleCoil = 0x05,

        /// <summary>
        /// 写入单个寄存器
        /// </summary>
        WriteSingleRegister = 0x06,

        /// <summary>
        /// 写入多个线圈
        /// </summary>
        WriteMultipleCoils = 0x0F,

        /// <summary>
        /// 写入多个寄存器
        /// </summary>
        WriteMultipleRegisters = 0x10,
    }
}
