using System;
using System.IO;
using System.IO.Ports;
using System.Threading;
using thinger.DataConvertLib;
using wanderingbird.ModbusLib.Lock;

namespace wanderingbird.ModbusLib.Base
{
    public abstract class SerialDeviceBase : ReadWriteBase
    {

        //串口变量
        private SerialPort serialPort = null;

        /// <summary>
        /// 读取超时时间
        /// </summary>
        public int ReadTimeOut { get; set; } = 2000;

        /// <summary>
        /// 写入超时时间
        /// </summary>
        public int WriteTimeOut { get; set; } = 2000;
        /// <summary>
        /// 接收超时时间
        /// </summary>
        public int ReceiveTimeout { get; set; } = 2000;

        /// <summary>
        /// 锁对象
        /// </summary>
        public SimpleHybirdLock simpleHybirdLock { get; set; } = new SimpleHybirdLock();
        /// <summary>
        /// 打开串口连接
        /// </summary>
        /// <param name="portName">端口号</param>
        /// <param name="baudRate">波特率</param>
        /// <param name="parity">校验位</param>
        /// <param name="dataBits">数据位</param>
        /// <param name="stopBits">停止位</param>
        /// <returns></returns>
        public OperateResult Connect(string portName, int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Close();
            }
            this.serialPort = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
            this.serialPort.ReadTimeout = this.ReadTimeOut;
            this.serialPort.WriteTimeout = this.WriteTimeOut;
            try
            {
                serialPort.Open();
                return OperateResult.CreateSuccessResult();
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult(ex.Message);
            }
        }

        /// <summary>
        /// 断开串口连接
        /// </summary>
        /// <returns></returns>
        public OperateResult DisConnect()
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    // 只有成功关闭了才返回成功
                    return OperateResult.CreateSuccessResult();
                }
                catch (Exception ex)
                {
                    // 如果关闭时发生异常，返回失败
                    return OperateResult.CreateFailResult(ex.Message);
                }
            }
            else
            {
                // 如果本来就是关闭的或null，也视为操作成功
                return OperateResult.CreateSuccessResult();
            }
        }



        /// <summary>
        /// 发送和接收
        /// </summary>
        /// <param name="request">请求</param>
        /// <returns></returns>
        protected OperateResult<byte[]> SendAndReceive(byte[] request)
        {
            //加锁
            simpleHybirdLock.Enter();
            if (serialPort == null || !serialPort.IsOpen)
            {
                return OperateResult.CreateFailResult<byte[]>("串口未打开或未初始化");
            }
            MemoryStream ms = null;
            try
            {
                ms = new MemoryStream();
                //丢弃串口接收缓冲区数据
                serialPort.DiscardInBuffer();
                serialPort.Write(request, 0, request.Length);
                //获取发送开始时间
                DateTime startTime = DateTime.Now;
                byte[] buffer = new byte[1024];
                while (true)
                {
                    Thread.Sleep(20);
                    //检查串口缓冲区是否有数据
                    if (serialPort.BytesToRead > 0)
                    {
                        int count = serialPort.Read(buffer, 0, buffer.Length);
                        ms.Write(buffer, 0, count);
                        startTime = DateTime.Now;
                    }
                    else
                    {
                        // ======== 改进的超时判断 ========
                        if (ms.Length > 0) // 如果已经收到过数据
                        {
                            // 判断“帧超时”
                            if ((DateTime.Now - startTime).TotalMilliseconds > 50) // 超过50ms没新数据了
                            {
                                break; // 认为接收完毕
                            }
                        }
                        else // 如果一个字节都没收到
                        {
                            // 判断“总超时”
                            if ((DateTime.Now - startTime).TotalMilliseconds > this.ReceiveTimeout)
                            {
                                // 直接返回超时错误
                                return OperateResult.CreateFailResult<byte[]>("Receive Timeout!");
                            }
                        }
                        // ===================================
                    }
                }
                return OperateResult.CreateSuccessResult<byte[]>(ms.ToArray());
            }
            catch (Exception ex)
            {
                return OperateResult.CreateFailResult<byte[]>(ex.Message);
            }
            finally
            {
                if (ms != null)
                {
                    ms.Dispose();
                }
                //解锁
                simpleHybirdLock.Leave();
            }
        }
    }
}
