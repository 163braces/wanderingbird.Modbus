using System;
using System.Linq;
using System.Text;
using thinger.DataConvertLib;
using wanderingbird.ModbusLib.Enums;
using wanderingbird.ModbusLib.Interface;

namespace wanderingbird.ModbusLib.Base
{
    /// <summary>
    /// 读写基类，包含一些通用的读写方法
    /// </summary>
    public abstract class ReadWriteBase : IReadWrite
    {
        /// <summary>
        /// 大小端字序
        /// </summary>
        public DataFormat DataFormat { get; set; } = DataFormat.ABCD;

        /// <summary>
        /// 存储区最小单位
        /// </summary>
        public AreaType AreaType { get; set; } = AreaType.Word;

        /// <summary>
        /// 获取或设置在进行字符串读写时，默认使用的编码格式。
        /// 默认为 ASCII。
        /// </summary>
        public Encoding DefaultStringEncoding { get; set; } = Encoding.ASCII;

        #region 抽象方法由子类实现
        /// <summary>
        /// 读取一个布尔数组，抽象方法由子类实现
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public abstract OperateResult<bool[]> ReadBoolArray(string logicAddress, ushort length);
        /// <summary>
        /// 读取一个字节数组，抽象方法由子类实现
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public abstract OperateResult<byte[]> ReadByteArray(string logicAddress, ushort length);
        /// <summary>
        /// 写入一个布尔数组，抽象方法由子类实现
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract OperateResult WriteBoolArray(string logicAddress, bool[] value);
        /// <summary>
        /// 写入一个字节数组，抽象方法由子类实现
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public abstract OperateResult WriteByteArray(string logicAddress, byte[] value);
        #endregion



        #region ReadCommon
        /// <summary>
        ///  通用读取方法
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="address"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual OperateResult<T> ReadCommon<T>(string address, ushort length = 1)
        {
            string dataType = typeof(T).Name;

            OperateResult<T> result = new OperateResult<T>();

            switch (dataType)
            {
                case "Boolean":
                    result = OperateResult.CopyOperateResult<T, bool>(ReadBool(address));
                    break;
                case "Boolean[]":
                    result = OperateResult.CopyOperateResult<T, bool[]>(ReadBoolArray(address, length));
                    break;
                case "Byte":
                    result = OperateResult.CopyOperateResult<T, byte>(ReadByte(address));
                    break;
                case "Byte[]":
                    result = OperateResult.CopyOperateResult<T, byte[]>(ReadByteArray(address, length));
                    break;
                case "SByte":
                    result = OperateResult.CopyOperateResult<T, sbyte>(ReadSByte(address));
                    break;
                case "SByte[]":
                    result = OperateResult.CopyOperateResult<T, sbyte[]>(ReadSByteArray(address, length));
                    break;
                case "Int16":
                    result = OperateResult.CopyOperateResult<T, short>(ReadShort(address));
                    break;
                case "Int16[]":
                    result = OperateResult.CopyOperateResult<T, short[]>(ReadShortArray(address, length));
                    break;
                case "UInt16":
                    result = OperateResult.CopyOperateResult<T, ushort>(ReadUShort(address));
                    break;
                case "UInt16[]":
                    result = OperateResult.CopyOperateResult<T, ushort[]>(ReadUShortArray(address, length));
                    break;
                case "Int32":
                    result = OperateResult.CopyOperateResult<T, int>(ReadInt(address));
                    break;
                case "Int32[]":
                    result = OperateResult.CopyOperateResult<T, int[]>(ReadIntArray(address, length));
                    break;
                case "UInt32":
                    result = OperateResult.CopyOperateResult<T, uint>(ReadUInt(address));
                    break;
                case "UInt32[]":
                    result = OperateResult.CopyOperateResult<T, uint[]>(ReadUIntArray(address, length));
                    break;
                case "Int64":
                    result = OperateResult.CopyOperateResult<T, long>(ReadLong(address));
                    break;
                case "Int64[]":
                    result = OperateResult.CopyOperateResult<T, long[]>(ReadLongArray(address, length));
                    break;
                case "UInt64":
                    result = OperateResult.CopyOperateResult<T, ulong>(ReadULong(address));
                    break;
                case "UInt64[]":
                    result = OperateResult.CopyOperateResult<T, ulong[]>(ReadULongArray(address, length));
                    break;
                case "Single":
                    result = OperateResult.CopyOperateResult<T, float>(ReadFloat(address));
                    break;
                case "Single[]":
                    result = OperateResult.CopyOperateResult<T, float[]>(ReadFloatArray(address, length));
                    break;
                case "Double":
                    result = OperateResult.CopyOperateResult<T, double>(ReadDouble(address));
                    break;
                case "Double[]":
                    result = OperateResult.CopyOperateResult<T, double[]>(ReadDoubleArray(address, length));
                    break;
                case "String":
                    result = OperateResult.CopyOperateResult<T, string>(ReadString(address, length));
                    break;
                default:
                    break;
            }

            return result;
        }
        #endregion


        #region --- 通用写入方法 ---

        /// <summary>
        /// 根据传入值的类型，自动选择相应的写入方法。这是一个泛型版本。
        /// </summary>
        /// <typeparam name="T">要写入的数据类型。</typeparam>
        /// <param name="address">要写入的起始地址。</param>
        /// <param name="value">要写入的数据。</param>
        /// <returns>一个表示操作成功或失败的结果对象。</returns>
        public virtual OperateResult WriteCommon<T>(string address, T value)
        {
            if (value == null)
            {
                return OperateResult.CreateFailResult("写入的值不能为空 (null)。");
            }

            // 使用 C# 模式匹配来判断 T 的真实类型
            if (value is bool boolValue) return WriteBool(address, boolValue);
            if (value is bool[] boolArray) return WriteBoolArray(address, boolArray);

            if (value is short shortValue) return WriteShort(address, shortValue);
            if (value is short[] shortArray) return WriteShortArray(address, shortArray);

            if (value is ushort ushortValue) return WriteUShort(address, ushortValue);
            if (value is ushort[] ushortArray) return WriteUShortArray(address, ushortArray);

            if (value is int intValue) return WriteInt(address, intValue);
            if (value is int[] intArray) return WriteIntArray(address, intArray);

            if (value is uint uintValue) return WriteUInt(address, uintValue);
            if (value is uint[] uintArray) return WriteUIntArray(address, uintArray);

            if (value is long longValue) return WriteLong(address, longValue);
            if (value is long[] longArray) return WriteLongArray(address, longArray);

            if (value is ulong ulongValue) return WriteULong(address, ulongValue);
            if (value is ulong[] ulongArray) return WriteULongArray(address, ulongArray);

            if (value is float floatValue) return WriteFloat(address, floatValue);
            if (value is float[] floatArray) return WriteFloatArray(address, floatArray);

            if (value is double doubleValue) return WriteDouble(address, doubleValue);
            if (value is double[] doubleArray) return WriteDoubleArray(address, doubleArray);

            if (value is string stringValue) return WriteString(address, stringValue);

            if (value is byte[] byteArray) return WriteByteArray(address, byteArray);

            // 如果以上都不是，则返回不支持
            return OperateResult.CreateFailResult($"不支持的数据类型: {typeof(T).Name}");
        }

        #endregion

        /// <summary>
        /// 读取一个bool类型数据。
        /// </summary>
        /// <param name="logicAddress">要读取的线圈逻辑地址。</param>
        /// <returns>返回一个bool类型数据。</returns>
        public virtual OperateResult<bool> ReadBool(string logicAddress)
        {
            // 调用布尔数组读取的核心方法，请求一个长度为1的数组
            OperateResult<bool[]> arrayResult = ReadBoolArray(logicAddress, 1);
            if (arrayResult.IsSuccess)
            {
                // 如果成功，从返回的数组中取出第一个元素
                return OperateResult.CreateSuccessResult(arrayResult.Content[0]);
            }
            else
            {
                // 如果失败，将底层的错误信息包装成正确的泛型类型后传递上去
                return OperateResult.CreateFailResult<bool>(arrayResult);
            }
        }

        /// <summary>
        /// 写入一个bool类型数据。
        /// </summary>
        /// <param name="logicAddress">要写入的线圈逻辑地址。</param>
        /// <param name="value">要写入的布尔值。</param>
        /// <returns>返回写入结果。</returns>
        public virtual OperateResult WriteBool(string logicAddress, bool value)
        {
            // 将单个的布尔值，包装成一个长度为1的布尔数组
            bool[] boolArray = new bool[] { value };

            // 调用布尔数组写入的核心方法
            return WriteBoolArray(logicAddress, boolArray);
        }

        /// <summary>
        /// 读取单个字节 (byte)。
        /// 注意：在基于寄存器的协议(如Modbus)上，这仍会读取一个完整的寄存器(2字节)，但只返回第一个字节。
        /// </summary>
        /// <param name="address">要读取的地址。</param>
        /// <returns>一个包含byte值的操作结果对象。</returns>
        public virtual OperateResult<byte> ReadByte(string address)
        {
            // 1. 调用 ReadByteArray 读取【1个】最小协议单位 (对于Modbus是1个寄存器，即2字节)
            OperateResult<byte[]> byteResult = ReadByteArray(address, 1);

            // 2. 检查底层的字节读取操作是否成功
            if (byteResult.IsSuccess)
            {
                // 3. 检查返回的数组是否为空
                if (byteResult.Content != null && byteResult.Content.Length > 0)
                {
                    // 4. 【关键】只返回结果数组中的【第一个】字节
                    return OperateResult.CreateSuccessResult(byteResult.Content[0]);
                }
                else
                {
                    return OperateResult.CreateFailResult<byte>("读取成功，但返回的数据为空。");
                }
            }
            else
            {
                // 5. 如果底层读取失败，则将失败结果向上传递
                return OperateResult.CreateFailResult<byte>(byteResult);
            }
        }

        /// <summary>
        /// 读取有符号字节数组。
        /// 注意：为了与库的其他读取方法保持一致，此方法的 length 参数单位是【寄存器】。
        /// 例如，length = 1 将读取1个寄存器（2个字节），并返回一个包含2个元素的 sbyte 数组。
        /// </summary>
        /// <param name="address">要读取的起始地址。</param>
        /// <param name="length">要读取的【寄存器】数量。</param>
        /// <returns>一个包含 sbyte[] 值的操作结果对象。</returns>
        public virtual OperateResult<sbyte[]> ReadSByteArray(string address, ushort length)
        {
            // 1. 直接调用 ReadByteArray，因为 length 单位已经统一为寄存器数量。
            OperateResult<byte[]> byteResult = ReadByteArray(address, length);

            // 2. 检查底层操作是否成功
            if (byteResult.IsSuccess)
            {
                // 3. 检查返回的数据是否为空
                if (byteResult.Content != null)
                {
                    // 4. 【关键】将 ReadByteArray 返回的【所有】字节，全部转换为 sbyte。
                    sbyte[] finalSByteArray = byteResult.Content.Select(b => (sbyte)b).ToArray();

                    // 5. 返回最终结果
                    return OperateResult.CreateSuccessResult(finalSByteArray);
                }
                else
                {
                    return OperateResult.CreateFailResult<sbyte[]>("读取成功，但返回的数据为空。");
                }
            }
            else
            {
                // 6. 如果底层读取失败，则将失败结果向上传递
                return OperateResult.CreateFailResult<sbyte[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取单个有符号字节 (sbyte)。
        /// 注意：此方法会读取一个完整的寄存器（2个字节），但只返回其【第一个】字节转换后的 sbyte 值。
        /// </summary>
        /// <param name="address">要读取的地址。</param>
        /// <returns>一个包含sbyte值的操作结果对象。</returns>
        public virtual OperateResult<sbyte> ReadSByte(string address)
        {
            // 1. 调用 ReadByteArray 读取【1个】寄存器
            OperateResult<byte[]> byteResult = ReadByteArray(address, 1);

            // 2. 检查底层的字节读取操作是否成功
            if (byteResult.IsSuccess)
            {
                // 3. 检查返回的数组是否为空
                if (byteResult.Content != null && byteResult.Content.Length > 0)
                {
                    // 4. 【关键】将结果数组中的【第一个】字节，显式转换为 sbyte 后返回
                    return OperateResult.CreateSuccessResult((sbyte)byteResult.Content[0]);
                }
                else
                {
                    return OperateResult.CreateFailResult<sbyte>("读取成功，但返回的数据为空。");
                }
            }
            else
            {
                // 5. 如果底层读取失败，则将失败结果向上传递
                return OperateResult.CreateFailResult<sbyte>(byteResult);
            }
        }

        /// <summary>
        /// 读取多个short类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">读取数量</param>
        /// <returns>返回一个short数组</short></returns>
        public virtual OperateResult<short[]> ReadShortArray(string logicAddress, ushort length)
        {
            //根据存储区最小单位换算所需读取的单位数量
            var unitCount = (ushort)(length * 2 / (int)this.AreaType);
            //获取length个寄存器的数值
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                //将获取的数据转化为Short格式保存在数组中
                short[] shortResult = ShortLib.GetShortArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult<short[]>(shortResult);
            }
            else
            {
                return OperateResult.CreateFailResult<short[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个short类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <returns></returns>
        public virtual OperateResult<short> ReadShort(string logicAddress)
        {
            //一个short数值占用16位，刚好等于一个寄存器
            OperateResult<short[]> shortResult = ReadShortArray(logicAddress, 1);
            if (shortResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(shortResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<short>(shortResult);
            }
        }


        /// <summary>
        /// 写入一个或多个 short (Int16) 数据。
        /// </summary>
        public virtual OperateResult WriteShortArray(string address, short[] value)
        {
            // 使用数据转换库将 short[] 转换为 byte[]
            byte[] byteValue = ByteLib.GetByteArrayFromShortArray(value, this.DataFormat);
            // 调用底层的抽象方法进行写入
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 short (Int16) 数据。
        /// </summary>
        public virtual OperateResult WriteShort(string address, short value)
        {
            return WriteShortArray(address, new short[] { value });
        }


        /// <summary>
        /// 读取多个ushort类型数据
        /// </summary>
        /// <param name="logicAddress"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public virtual OperateResult<ushort[]> ReadUShortArray(string logicAddress, ushort length)
        {
            var unitCount = (ushort)(length * 2 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                ushort[] ushortResult = UShortLib.GetUShortArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult<ushort[]>(ushortResult);
            }
            else
            {
                return OperateResult.CreateFailResult<ushort[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个ushort类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <returns></returns>
        public virtual OperateResult<ushort> ReadUShort(string logicAddress)
        {
            //一个short数值占用16位，刚好等于一个寄存器
            OperateResult<ushort[]> ushortResult = ReadUShortArray(logicAddress, 1);
            if (ushortResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(ushortResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<ushort>(ushortResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 ushort (UInt16) 数据。
        /// </summary>
        public virtual OperateResult WriteUShortArray(string address, ushort[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromUShortArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 ushort (UInt16) 数据。
        /// </summary>
        public virtual OperateResult WriteUShort(string address, ushort value)
        {
            return WriteUShortArray(address, new ushort[] { value });
        }


        /// <summary>
        /// 读取多个int类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">读取数量</param>
        /// <returns>返回一个int数组</returns>
        public virtual OperateResult<int[]> ReadIntArray(string logicAddress, ushort length)
        {
            var unitCount = (ushort)(length * 4 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var intResult = IntLib.GetIntArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult<int[]>(intResult);
            }
            else
            {
                return OperateResult.CreateFailResult<int[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个int类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <returns>返回一个int类型数据</returns>
        public virtual OperateResult<int> ReadInt(string logicAddress)
        {
            //由于一个int类型占用4个字节，相当于两个寄存器
            OperateResult<int[]> intResult = ReadIntArray(logicAddress, 1);
            if (intResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult<int>(intResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<int>(intResult);
            }
        }

        // <summary>
        /// 写入一个或多个 int (Int32) 数据。
        /// </summary>
        public virtual OperateResult WriteIntArray(string address, int[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromIntArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 int (Int32) 数据。
        /// </summary>
        public virtual OperateResult WriteInt(string address, int value)
        {
            return WriteIntArray(address, new int[] { value });
        }



        /// <summary>
        /// 读取多个uint类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <param name="length">读取数量</param>
        /// <returns>返回一个uint数组</returns>
        public virtual OperateResult<uint[]> ReadUIntArray(string logicAddress, ushort length)
        {
            var unitCount = (ushort)(length * 4 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var uintResult = UIntLib.GetUIntArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult<uint[]>(uintResult);
            }
            else
            {
                return OperateResult.CreateFailResult<uint[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个uint类型数据
        /// </summary>
        /// <param name="logicAddress">逻辑地址</param>
        /// <returns>返回一个uint类型数据</returns>
        public virtual OperateResult<uint> ReadUInt(string logicAddress)
        {
            //由于一个int类型占用4个字节，相当于两个寄存器
            OperateResult<uint[]> uintResult = ReadUIntArray(logicAddress, 1);
            if (uintResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult<uint>(uintResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<uint>(uintResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 uint (UInt32) 数据。
        /// </summary>
        public virtual OperateResult WriteUIntArray(string address, uint[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromUIntArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 uint (UInt32) 数据。
        /// </summary>
        public virtual OperateResult WriteUInt(string address, uint value)
        {
            return WriteUIntArray(address, new uint[] { value });
        }

        /// <summary>
        /// 读取多个long (Int64) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <param name="length">要读取的long数据的数量。</param>
        /// <returns>返回一个long数组的操作结果对象。</returns>
        public virtual OperateResult<long[]> ReadLongArray(string logicAddress, ushort length)
        {
            // 1个 long = 8个字节
            var unitCount = (ushort)(length * 8 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var longResult = LongLib.GetLongArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult(longResult);
            }
            else
            {
                return OperateResult.CreateFailResult<long[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个long (Int64) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <returns>返回一个long类型数据的操作结果对象。</returns>
        public virtual OperateResult<long> ReadLong(string logicAddress)
        {
            var arrayResult = ReadLongArray(logicAddress, 1);
            if (arrayResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(arrayResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<long>(arrayResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 long (Int64) 数据。
        /// </summary>
        public virtual OperateResult WriteLongArray(string address, long[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromLongArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 long (Int64) 数据。
        /// </summary>
        public virtual OperateResult WriteLong(string address, long value)
        {
            return WriteLongArray(address, new long[] { value });
        }

        /// <summary>
        /// 读取多个ulong (UInt64) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <param name="length">要读取的ulong数据的数量。</param>
        /// <returns>返回一个ulong数组的操作结果对象。</returns>
        public virtual OperateResult<ulong[]> ReadULongArray(string logicAddress, ushort length)
        {
            // 1个 ulong = 8个字节
            var unitCount = (ushort)(length * 8 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var ulongResult = ULongLib.GetULongArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult(ulongResult);
            }
            else
            {
                return OperateResult.CreateFailResult<ulong[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个ulong (UInt64) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <returns>返回一个ulong类型数据的操作结果对象。</returns>
        public virtual OperateResult<ulong> ReadULong(string logicAddress)
        {
            var arrayResult = ReadULongArray(logicAddress, 1);
            if (arrayResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(arrayResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<ulong>(arrayResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 ulong (UInt64) 数据。
        /// </summary>
        public virtual OperateResult WriteULongArray(string address, ulong[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromULongArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 ulong (UInt64) 数据。
        /// </summary>
        public virtual OperateResult WriteULong(string address, ulong value)
        {
            return WriteULongArray(address, new ulong[] { value });
        }


        // --- 浮点数 ---

        /// <summary>
        /// 读取多个float (Single) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <param name="length">要读取的float数据的数量。</param>
        /// <returns>返回一个float数组的操作结果对象。</returns>
        public virtual OperateResult<float[]> ReadFloatArray(string logicAddress, ushort length)
        {
            // 1个 float = 4个字节
            var unitCount = (ushort)(length * 4 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var floatResult = FloatLib.GetFloatArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult(floatResult);
            }
            else
            {
                return OperateResult.CreateFailResult<float[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个float (Single) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <returns>返回一个float类型数据的操作结果对象。</returns>
        public virtual OperateResult<float> ReadFloat(string logicAddress)
        {
            var arrayResult = ReadFloatArray(logicAddress, 1);
            if (arrayResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(arrayResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<float>(arrayResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 float (Single) 数据。
        /// </summary>
        public virtual OperateResult WriteFloatArray(string address, float[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromFloatArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 float (Single) 数据。
        /// </summary>
        public virtual OperateResult WriteFloat(string logicAddress, float value)
        {
            return WriteFloatArray(logicAddress, new float[] { value });
        }


        /// <summary>
        /// 读取多个double (Double) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <param name="length">要读取的double数据的数量。</param>
        /// <returns>返回一个double数组的操作结果对象。</returns>
        public virtual OperateResult<double[]> ReadDoubleArray(string logicAddress, ushort length)
        {
            // 1个 double = 8个字节
            var unitCount = (ushort)(length * 8 / (int)this.AreaType);
            var byteResult = ReadByteArray(logicAddress, unitCount);
            if (byteResult.IsSuccess)
            {
                var doubleResult = DoubleLib.GetDoubleArrayFromByteArray(byteResult.Content, this.DataFormat);
                return OperateResult.CreateSuccessResult(doubleResult);
            }
            else
            {
                return OperateResult.CreateFailResult<double[]>(byteResult);
            }
        }

        /// <summary>
        /// 读取一个double (Double) 类型数据。
        /// </summary>
        /// <param name="logicAddress">逻辑地址。</param>
        /// <returns>返回一个double类型数据的操作结果对象。</returns>
        public virtual OperateResult<double> ReadDouble(string logicAddress)
        {
            var arrayResult = ReadDoubleArray(logicAddress, 1);
            if (arrayResult.IsSuccess)
            {
                return OperateResult.CreateSuccessResult(arrayResult.Content[0]);
            }
            else
            {
                return OperateResult.CreateFailResult<double>(arrayResult);
            }
        }

        /// <summary>
        /// 写入一个或多个 double (Double) 数据。
        /// </summary>
        public virtual OperateResult WriteDoubleArray(string address, double[] value)
        {
            byte[] byteValue = ByteLib.GetByteArrayFromDoubleArray(value, this.DataFormat);
            return WriteByteArray(address, byteValue);
        }

        /// <summary>
        /// 写入一个 double (Double) 数据。
        /// </summary>
        public virtual OperateResult WriteDouble(string address, double value)
        {
            return WriteDoubleArray(address, new double[] { value });
        }

        /// <summary>
        /// 读取指定长度的字符串，【使用默认编码】。
        /// 默认编码可以通过 DefaultStringEncoding 属性进行设置。
        /// </summary>
        /// <param name="logicAddress">要读取的起始地址。</param>
        /// <param name="length">要读取的【字节】数量。</param>
        /// <returns>一个包含字符串的操作结果对象。</returns>
        public virtual OperateResult<string> ReadString(string logicAddress, ushort length)
        {
            // 如果请求长度为0，直接返回空字符串
            if (length == 0)
            {
                return OperateResult.CreateSuccessResult(string.Empty);
            }

            // 1. 计算需要读取的协议单位数量
            var unitCount = (ushort)((length + (int)this.AreaType - 1) / (int)this.AreaType);

            // 2. 调用底层获取原始字节
            var responseResult = ReadByteArray(logicAddress, unitCount);
            if (!responseResult.IsSuccess)
            {
                return OperateResult.CreateFailResult<string>(responseResult);
            }

            // 3. 截取精确数量的字节
            byte[] byteResult = responseResult.Content.Take(length).ToArray();

            // 4. 【关键修正】使用我们新定义的【属性】来进行解码
            string finalString = this.DefaultStringEncoding.GetString(byteResult);

            // 5. 返回成功结果
            return OperateResult.CreateSuccessResult(finalString); // 可以省略<string>
        }

        /// <summary>
        /// 写入一个字符串，编码格式由 DefaultStringEncoding 属性决定。
        /// </summary>
        /// <param name="address">要写入的起始地址。</param>
        /// <param name="value">要写入的字符串。</-param>
        public virtual OperateResult WriteString(string address, string value)
        {
            // 1. 处理 null 输入
            if (value == null) value = string.Empty;

            // 2. 【关键】使用 DefaultStringEncoding 属性将字符串编码为字节
            byte[] byteValue = this.DefaultStringEncoding.GetBytes(value);
            byte[] finalBytesToWrite = byteValue;

            // 3. 智能的字节对齐
            if (this.AreaType == AreaType.Word && byteValue.Length % 2 != 0)
            {
                finalBytesToWrite = new byte[byteValue.Length + 1];
                Array.Copy(byteValue, finalBytesToWrite, byteValue.Length);
            }

            // 4. 调用底层写入
            return WriteByteArray(address, finalBytesToWrite);
        }

    }
}
