namespace wanderingbird.CommunicationTest
{
    partial class FrmModbusRTU
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmModbusRTU));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_DevAddress = new System.Windows.Forms.MaskedTextBox();
            this.btn_DisConn = new System.Windows.Forms.Button();
            this.btn_Connect = new System.Windows.Forms.Button();
            this.chk_IsShortAddress = new System.Windows.Forms.CheckBox();
            this.txt_DataBits = new System.Windows.Forms.MaskedTextBox();
            this.cmb_Parity = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmb_DataFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmb_StopBits = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmb_BaudRate = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cmb_Port = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.lst_Info = new System.Windows.Forms.ListView();
            this.InfoTime = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Info = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.btn_Write = new System.Windows.Forms.Button();
            this.btn_Read = new System.Windows.Forms.Button();
            this.txt_SetValue = new System.Windows.Forms.MaskedTextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txt_Count = new System.Windows.Forms.MaskedTextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.cmb_DataType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txt_Variable = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_DevAddress);
            this.groupBox1.Controls.Add(this.btn_DisConn);
            this.groupBox1.Controls.Add(this.btn_Connect);
            this.groupBox1.Controls.Add(this.chk_IsShortAddress);
            this.groupBox1.Controls.Add(this.txt_DataBits);
            this.groupBox1.Controls.Add(this.cmb_Parity);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmb_DataFormat);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.cmb_StopBits);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmb_BaudRate);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cmb_Port);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(695, 207);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "通信参数";
            // 
            // txt_DevAddress
            // 
            this.txt_DevAddress.Location = new System.Drawing.Point(99, 150);
            this.txt_DevAddress.Name = "txt_DevAddress";
            this.txt_DevAddress.Size = new System.Drawing.Size(91, 26);
            this.txt_DevAddress.TabIndex = 28;
            // 
            // btn_DisConn
            // 
            this.btn_DisConn.Location = new System.Drawing.Point(580, 143);
            this.btn_DisConn.Name = "btn_DisConn";
            this.btn_DisConn.Size = new System.Drawing.Size(88, 36);
            this.btn_DisConn.TabIndex = 27;
            this.btn_DisConn.Text = "断开连接";
            this.btn_DisConn.UseVisualStyleBackColor = true;
            this.btn_DisConn.Click += new System.EventHandler(this.btn_DisConn_Click);
            // 
            // btn_Connect
            // 
            this.btn_Connect.Location = new System.Drawing.Point(450, 143);
            this.btn_Connect.Name = "btn_Connect";
            this.btn_Connect.Size = new System.Drawing.Size(88, 36);
            this.btn_Connect.TabIndex = 26;
            this.btn_Connect.Text = "建立连接";
            this.btn_Connect.UseVisualStyleBackColor = true;
            this.btn_Connect.Click += new System.EventHandler(this.btn_Connect_Click);
            // 
            // chk_IsShortAddress
            // 
            this.chk_IsShortAddress.AutoSize = true;
            this.chk_IsShortAddress.Location = new System.Drawing.Point(496, 98);
            this.chk_IsShortAddress.Name = "chk_IsShortAddress";
            this.chk_IsShortAddress.Size = new System.Drawing.Size(112, 23);
            this.chk_IsShortAddress.TabIndex = 25;
            this.chk_IsShortAddress.Text = "是否为短地址";
            this.chk_IsShortAddress.UseVisualStyleBackColor = true;
            // 
            // txt_DataBits
            // 
            this.txt_DataBits.Location = new System.Drawing.Point(99, 94);
            this.txt_DataBits.Name = "txt_DataBits";
            this.txt_DataBits.Size = new System.Drawing.Size(91, 26);
            this.txt_DataBits.TabIndex = 24;
            // 
            // cmb_Parity
            // 
            this.cmb_Parity.FormattingEnabled = true;
            this.cmb_Parity.Location = new System.Drawing.Point(545, 39);
            this.cmb_Parity.Name = "cmb_Parity";
            this.cmb_Parity.Size = new System.Drawing.Size(91, 27);
            this.cmb_Parity.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(473, 42);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 19);
            this.label9.TabIndex = 18;
            this.label9.Text = "校验位：";
            // 
            // cmb_DataFormat
            // 
            this.cmb_DataFormat.FormattingEnabled = true;
            this.cmb_DataFormat.Location = new System.Drawing.Point(319, 149);
            this.cmb_DataFormat.Name = "cmb_DataFormat";
            this.cmb_DataFormat.Size = new System.Drawing.Size(91, 27);
            this.cmb_DataFormat.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(247, 152);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 19);
            this.label4.TabIndex = 16;
            this.label4.Text = "大小端：";
            // 
            // cmb_StopBits
            // 
            this.cmb_StopBits.FormattingEnabled = true;
            this.cmb_StopBits.Location = new System.Drawing.Point(319, 94);
            this.cmb_StopBits.Name = "cmb_StopBits";
            this.cmb_StopBits.Size = new System.Drawing.Size(91, 27);
            this.cmb_StopBits.TabIndex = 15;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(247, 97);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 19);
            this.label5.TabIndex = 14;
            this.label5.Text = "停止位：";
            // 
            // cmb_BaudRate
            // 
            this.cmb_BaudRate.FormattingEnabled = true;
            this.cmb_BaudRate.Location = new System.Drawing.Point(319, 39);
            this.cmb_BaudRate.Name = "cmb_BaudRate";
            this.cmb_BaudRate.Size = new System.Drawing.Size(91, 27);
            this.cmb_BaudRate.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.Location = new System.Drawing.Point(247, 42);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 19);
            this.label6.TabIndex = 12;
            this.label6.Text = "波特率：";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(27, 152);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 19);
            this.label3.TabIndex = 10;
            this.label3.Text = "站地址：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(27, 97);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 19);
            this.label2.TabIndex = 8;
            this.label2.Text = "数据位：";
            // 
            // cmb_Port
            // 
            this.cmb_Port.FormattingEnabled = true;
            this.cmb_Port.Location = new System.Drawing.Point(99, 39);
            this.cmb_Port.Name = "cmb_Port";
            this.cmb_Port.Size = new System.Drawing.Size(91, 27);
            this.cmb_Port.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(27, 42);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "端口号：";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.lst_Info);
            this.groupBox2.Controls.Add(this.btn_Write);
            this.groupBox2.Controls.Add(this.btn_Read);
            this.groupBox2.Controls.Add(this.txt_SetValue);
            this.groupBox2.Controls.Add(this.label11);
            this.groupBox2.Controls.Add(this.txt_Count);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.cmb_DataType);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.txt_Variable);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox2.Location = new System.Drawing.Point(0, 207);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox2.Size = new System.Drawing.Size(695, 531);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "读写测试";
            // 
            // lst_Info
            // 
            this.lst_Info.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.InfoTime,
            this.Info});
            this.lst_Info.HideSelection = false;
            this.lst_Info.Location = new System.Drawing.Point(12, 241);
            this.lst_Info.Name = "lst_Info";
            this.lst_Info.Size = new System.Drawing.Size(671, 278);
            this.lst_Info.SmallImageList = this.imageList1;
            this.lst_Info.TabIndex = 35;
            this.lst_Info.UseCompatibleStateImageBehavior = false;
            this.lst_Info.View = System.Windows.Forms.View.Details;
            // 
            // InfoTime
            // 
            this.InfoTime.Text = "日志时间";
            this.InfoTime.Width = 200;
            // 
            // Info
            // 
            this.Info.Text = "日志信息";
            this.Info.Width = 300;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "info.png");
            this.imageList1.Images.SetKeyName(1, "warning.png");
            this.imageList1.Images.SetKeyName(2, "error.png");
            // 
            // btn_Write
            // 
            this.btn_Write.Location = new System.Drawing.Point(533, 185);
            this.btn_Write.Name = "btn_Write";
            this.btn_Write.Size = new System.Drawing.Size(88, 36);
            this.btn_Write.TabIndex = 34;
            this.btn_Write.Text = "写入";
            this.btn_Write.UseVisualStyleBackColor = true;
            this.btn_Write.Click += new System.EventHandler(this.btn_Write_Click);
            // 
            // btn_Read
            // 
            this.btn_Read.Location = new System.Drawing.Point(533, 119);
            this.btn_Read.Name = "btn_Read";
            this.btn_Read.Size = new System.Drawing.Size(88, 36);
            this.btn_Read.TabIndex = 33;
            this.btn_Read.Text = "读取";
            this.btn_Read.UseVisualStyleBackColor = true;
            this.btn_Read.Click += new System.EventHandler(this.btn_Read_Click);
            // 
            // txt_SetValue
            // 
            this.txt_SetValue.Location = new System.Drawing.Point(99, 191);
            this.txt_SetValue.Name = "txt_SetValue";
            this.txt_SetValue.Size = new System.Drawing.Size(292, 26);
            this.txt_SetValue.TabIndex = 32;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(13, 194);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 19);
            this.label11.TabIndex = 31;
            this.label11.Text = "写入数值：";
            // 
            // txt_Count
            // 
            this.txt_Count.Location = new System.Drawing.Point(99, 125);
            this.txt_Count.Name = "txt_Count";
            this.txt_Count.Size = new System.Drawing.Size(292, 26);
            this.txt_Count.TabIndex = 30;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(13, 128);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(79, 19);
            this.label10.TabIndex = 29;
            this.label10.Text = "读取数量：";
            // 
            // cmb_DataType
            // 
            this.cmb_DataType.FormattingEnabled = true;
            this.cmb_DataType.Location = new System.Drawing.Point(477, 59);
            this.cmb_DataType.Name = "cmb_DataType";
            this.cmb_DataType.Size = new System.Drawing.Size(144, 27);
            this.cmb_DataType.TabIndex = 28;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(391, 62);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(79, 19);
            this.label8.TabIndex = 27;
            this.label8.Text = "变量类型：";
            // 
            // txt_Variable
            // 
            this.txt_Variable.Location = new System.Drawing.Point(99, 59);
            this.txt_Variable.Name = "txt_Variable";
            this.txt_Variable.Size = new System.Drawing.Size(185, 26);
            this.txt_Variable.TabIndex = 26;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label7.Location = new System.Drawing.Point(13, 62);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(79, 19);
            this.label7.TabIndex = 25;
            this.label7.Text = "变量地址：";
            // 
            // FrmModbusRTU
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 19F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(695, 738);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "FrmModbusRTU";
            this.Text = "基于ModbusRTU协议实现串口通信";
            this.Load += new System.EventHandler(this.FrmModbusRTU_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox cmb_Port;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.MaskedTextBox txt_DataBits;
        private System.Windows.Forms.ComboBox cmb_Parity;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmb_DataFormat;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmb_StopBits;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmb_BaudRate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btn_DisConn;
        private System.Windows.Forms.Button btn_Connect;
        private System.Windows.Forms.CheckBox chk_IsShortAddress;
        private System.Windows.Forms.MaskedTextBox txt_SetValue;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MaskedTextBox txt_Count;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmb_DataType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox txt_Variable;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ListView lst_Info;
        private System.Windows.Forms.Button btn_Write;
        private System.Windows.Forms.Button btn_Read;
        private System.Windows.Forms.ColumnHeader InfoTime;
        private System.Windows.Forms.ColumnHeader Info;
        private System.Windows.Forms.MaskedTextBox txt_DevAddress;
        private System.Windows.Forms.ImageList imageList1;
    }
}

