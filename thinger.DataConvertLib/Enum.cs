//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
//新阁教育，版权所有，关注微信公众号：dotNet工控上位机，下载更多学习软件和资料
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace thinger.DataConvertLib
{

    /// <summary>
    /// 字节序
    /// </summary>
    public enum DataFormat
    {
        /// <summary>
        /// 按照顺序排序
        /// </summary>
        ABCD = 0,
        /// <summary>
        /// 按照单字反转
        /// </summary>
        BADC = 1,
        /// <summary>
        /// 按照双字反转
        /// </summary>
        CDAB = 2,
        /// <summary>
        /// 按照倒序排序
        /// </summary>
        DCBA = 3,
    }

    /// <summary>
    /// 数据类型
    /// </summary>
    public enum DataType
    {
        /// <summary>
        /// 布尔
        /// </summary>
        Bool,
        /// <summary>
        /// 字节
        /// </summary>
        Byte,
        /// <summary>
        /// 字节
        /// </summary>
        SByte,
        /// <summary>
        /// 有符号16位整型
        /// </summary>
        Short,
        /// <summary>
        /// 无符号16位整型
        /// </summary>
        UShort,
        /// <summary>
        /// 有符号32位整型
        /// </summary>
        Int,
        /// <summary>
        /// 无符号32位整型
        /// </summary>
        UInt,
        /// <summary>
        /// 32位浮点型
        /// </summary>
        Float,
        /// <summary>
        /// 64位浮点型
        /// </summary>
        Double,
        /// <summary>
        /// 有符号64位整型
        /// </summary>
        Long,
        /// <summary>
        /// 无符号64位整型
        /// </summary>
        ULong,
        /// <summary>
        /// 字符串
        /// </summary>
        String,
        /// <summary>
        /// 超文本字符串
        /// </summary>
        WString,
        /// <summary>
        /// 结构体
        /// </summary>
        Struct,
        /// <summary>
        /// 布尔数组
        /// </summary>
        BoolArray,
        /// <summary>
        /// 字节数组
        /// </summary>
        ByteArray,
        /// <summary>
        /// 字节数组
        /// </summary>
        SByteArray,
        /// <summary>
        /// 有符号16位整型数组
        /// </summary>
        ShortArray,
        /// <summary>
        /// 无符号16位整型数组
        /// </summary>
        UShortArray,
        /// <summary>
        /// 有符号32位整型数组
        /// </summary>
        IntArray,
        /// <summary>
        /// 无符号32位整型数组
        /// </summary>
        UIntArray,
        /// <summary>
        /// 32位浮点数数组
        /// </summary>
        FloatArray,
        /// <summary>
        /// 64位浮点数数组
        /// </summary>
        DoubleArray,
        /// <summary>
        /// 64位有符号整型数组
        /// </summary>
        LongArray,
        /// <summary>
        /// 64位无符号整型数组
        /// </summary>
        ULongArray,
        /// <summary>
        /// 字符串数组
        /// </summary>
        StringArray,
        /// <summary>
        /// 超文本字符串数组
        /// </summary>
        WStringArray,
    }
}
