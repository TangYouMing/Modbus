namespace Modbus软件
{
    partial class Form1
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.Baud1 = new System.Windows.Forms.ComboBox();
            this.btn_SerialOpen = new System.Windows.Forms.Button();
            this.StopBit = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Parity = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Baud = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SerialCom = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ParaTable = new System.Windows.Forms.DataGridView();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ReadWrite = new System.Windows.Forms.ComboBox();
            this.SenorModel = new System.Windows.Forms.TextBox();
            this.OpenFile = new System.Windows.Forms.Button();
            this.VersionID = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.VersionTime = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBoxIn = new System.Windows.Forms.TextBox();
            this.textBoxCon = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.comboBoxInModel = new System.Windows.Forms.ComboBox();
            this.ShowSend = new System.Windows.Forms.CheckBox();
            this.ShowTime = new System.Windows.Forms.CheckBox();
            this.SendStr = new System.Windows.Forms.Button();
            this.RecData1 = new System.Windows.Forms.TextBox();
            this.groupBox_MsgID = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBox_2AWrite = new System.Windows.Forms.TextBox();
            this.label_Verity = new System.Windows.Forms.Label();
            this.textBox_CRC = new System.Windows.Forms.TextBox();
            this.textBox_LRC = new System.Windows.Forms.TextBox();
            this.label_Tip = new System.Windows.Forms.Label();
            this.label_Space = new System.Windows.Forms.Label();
            this.label_ShowHelp = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.ShowAnalyze = new System.Windows.Forms.CheckBox();
            this.btn_ClearShow = new System.Windows.Forms.Button();
            this.numericUpDown_SenorID = new System.Windows.Forms.NumericUpDown();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.ReadData = new System.Windows.Forms.Button();
            this.MsgID1 = new System.Windows.Forms.TextBox();
            this.ReadCycle = new System.Windows.Forms.NumericUpDown();
            this.SenorID = new System.Windows.Forms.Label();
            this.timer1_Time = new System.Windows.Forms.Timer(this.components);
            this.dataGridView2_SendTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.serialPort1 = new System.IO.Ports.SerialPort(this.components);
            this.dataGridView3_RecTable = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button_Help = new System.Windows.Forms.Button();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            this.timer_RefreshSerialDelay = new System.Windows.Forms.Timer(this.components);
            this.timer_ReadData = new System.Windows.Forms.Timer(this.components);
            this.timer_ShowReceData = new System.Windows.Forms.Timer(this.components);
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParaTable)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.groupBox_MsgID.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SenorID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCycle)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2_SendTable)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3_RecTable)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.Baud1);
            this.groupBox1.Controls.Add(this.btn_SerialOpen);
            this.groupBox1.Controls.Add(this.StopBit);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.Parity);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.Baud);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.SerialCom);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 8);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox1.Size = new System.Drawing.Size(594, 61);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "串口设置";
            // 
            // Baud1
            // 
            this.Baud1.FormattingEnabled = true;
            this.Baud1.ItemHeight = 14;
            this.Baud1.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "自定义"});
            this.Baud1.Location = new System.Drawing.Point(241, 24);
            this.Baud1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Baud1.MaxLength = 9;
            this.Baud1.Name = "Baud1";
            this.Baud1.Size = new System.Drawing.Size(65, 22);
            this.Baud1.TabIndex = 7;
            this.Baud1.Visible = false;
            this.Baud1.SelectedIndexChanged += new System.EventHandler(this.Baud1_SelectedIndexChanged);
            this.Baud1.TextChanged += new System.EventHandler(this.Baud1_TextChanged);
            this.Baud1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Baud1_KeyPress);
            // 
            // btn_SerialOpen
            // 
            this.btn_SerialOpen.Location = new System.Drawing.Point(528, 24);
            this.btn_SerialOpen.Name = "btn_SerialOpen";
            this.btn_SerialOpen.Size = new System.Drawing.Size(60, 25);
            this.btn_SerialOpen.TabIndex = 5;
            this.btn_SerialOpen.Text = "打开";
            this.btn_SerialOpen.UseVisualStyleBackColor = true;
            this.btn_SerialOpen.Click += new System.EventHandler(this.btn_SerialOpen_Click);
            // 
            // StopBit
            // 
            this.StopBit.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.StopBit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.StopBit.FormattingEnabled = true;
            this.StopBit.Items.AddRange(new object[] {
            "1",
            "2"});
            this.StopBit.Location = new System.Drawing.Point(470, 25);
            this.StopBit.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.StopBit.Name = "StopBit";
            this.StopBit.Size = new System.Drawing.Size(50, 22);
            this.StopBit.TabIndex = 4;
            this.StopBit.SelectedIndexChanged += new System.EventHandler(this.StopBit_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(421, 29);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 14);
            this.label4.TabIndex = 6;
            this.label4.Text = "停止位";
            // 
            // Parity
            // 
            this.Parity.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Parity.FormattingEnabled = true;
            this.Parity.Items.AddRange(new object[] {
            "NONE",
            "EVEN",
            "ODD"});
            this.Parity.Location = new System.Drawing.Point(361, 25);
            this.Parity.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Parity.Name = "Parity";
            this.Parity.Size = new System.Drawing.Size(55, 22);
            this.Parity.TabIndex = 3;
            this.Parity.SelectedIndexChanged += new System.EventHandler(this.Parity_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(312, 29);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 14);
            this.label3.TabIndex = 4;
            this.label3.Text = "校验位";
            // 
            // Baud
            // 
            this.Baud.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Baud.FormattingEnabled = true;
            this.Baud.ItemHeight = 14;
            this.Baud.Items.AddRange(new object[] {
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200",
            "自定义"});
            this.Baud.Location = new System.Drawing.Point(242, 25);
            this.Baud.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Baud.MaxLength = 9;
            this.Baud.Name = "Baud";
            this.Baud.Size = new System.Drawing.Size(65, 22);
            this.Baud.TabIndex = 2;
            this.Baud.SelectedIndexChanged += new System.EventHandler(this.Baud_SelectedIndexChanged);
            this.Baud.TextChanged += new System.EventHandler(this.Baud_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(193, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(49, 14);
            this.label2.TabIndex = 2;
            this.label2.Text = "波特率";
            // 
            // SerialCom
            // 
            this.SerialCom.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SerialCom.FormattingEnabled = true;
            this.SerialCom.ItemHeight = 14;
            this.SerialCom.Location = new System.Drawing.Point(55, 25);
            this.SerialCom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.SerialCom.Name = "SerialCom";
            this.SerialCom.Size = new System.Drawing.Size(133, 22);
            this.SerialCom.TabIndex = 1;
            this.SerialCom.SelectedIndexChanged += new System.EventHandler(this.SerialID_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 14);
            this.label1.TabIndex = 1;
            this.label1.Text = "串口号";
            // 
            // ParaTable
            // 
            this.ParaTable.AllowUserToResizeColumns = false;
            this.ParaTable.AllowUserToResizeRows = false;
            this.ParaTable.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.ParaTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.ParaTable.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ParaTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.ParaTable.ColumnHeadersHeight = 36;
            this.ParaTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.ParaTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column6,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ParaTable.DefaultCellStyle = dataGridViewCellStyle8;
            this.ParaTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.ParaTable.Location = new System.Drawing.Point(14, 143);
            this.ParaTable.Name = "ParaTable";
            this.ParaTable.ReadOnly = true;
            this.ParaTable.RowHeadersVisible = false;
            this.ParaTable.RowHeadersWidth = 51;
            this.ParaTable.RowTemplate.Height = 27;
            this.ParaTable.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.ParaTable.Size = new System.Drawing.Size(594, 462);
            this.ParaTable.TabIndex = 11;
            this.ParaTable.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParaTable_CellClick);
            this.ParaTable.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.ParaTable_CellDoubleClick);
            // 
            // Column6
            // 
            this.Column6.HeaderText = "序号";
            this.Column6.Name = "Column6";
            this.Column6.ReadOnly = true;
            this.Column6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column6.Width = 60;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "功能码(Hex)";
            this.Column1.MinimumWidth = 6;
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 86;
            // 
            // Column2
            // 
            this.Column2.HeaderText = "寄存器地址(Hex)";
            this.Column2.MinimumWidth = 6;
            this.Column2.Name = "Column2";
            this.Column2.ReadOnly = true;
            this.Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column2.Width = 86;
            // 
            // Column3
            // 
            this.Column3.HeaderText = "描述";
            this.Column3.MinimumWidth = 6;
            this.Column3.Name = "Column3";
            this.Column3.ReadOnly = true;
            this.Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column3.Width = 170;
            // 
            // Column4
            // 
            this.Column4.HeaderText = "数据(Hex)";
            this.Column4.Name = "Column4";
            this.Column4.ReadOnly = true;
            this.Column4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column4.Width = 86;
            // 
            // Column5
            // 
            this.Column5.HeaderText = "数据含义";
            this.Column5.Name = "Column5";
            this.Column5.ReadOnly = true;
            this.Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column5.Width = 86;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ReadWrite);
            this.groupBox2.Controls.Add(this.SenorModel);
            this.groupBox2.Controls.Add(this.OpenFile);
            this.groupBox2.Controls.Add(this.VersionID);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.VersionTime);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Location = new System.Drawing.Point(14, 73);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(594, 64);
            this.groupBox2.TabIndex = 11;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "传感器参数";
            // 
            // ReadWrite
            // 
            this.ReadWrite.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.ReadWrite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ReadWrite.FormattingEnabled = true;
            this.ReadWrite.Items.AddRange(new object[] {
            "Read",
            "Write"});
            this.ReadWrite.Location = new System.Drawing.Point(460, 23);
            this.ReadWrite.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.ReadWrite.Name = "ReadWrite";
            this.ReadWrite.Size = new System.Drawing.Size(60, 22);
            this.ReadWrite.TabIndex = 10;
            this.ReadWrite.SelectedIndexChanged += new System.EventHandler(this.ReadWrite_SelectedIndexChanged);
            // 
            // SenorModel
            // 
            this.SenorModel.Location = new System.Drawing.Point(83, 23);
            this.SenorModel.Name = "SenorModel";
            this.SenorModel.ReadOnly = true;
            this.SenorModel.Size = new System.Drawing.Size(75, 23);
            this.SenorModel.TabIndex = 6;
            this.SenorModel.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // OpenFile
            // 
            this.OpenFile.Location = new System.Drawing.Point(528, 22);
            this.OpenFile.Name = "OpenFile";
            this.OpenFile.Size = new System.Drawing.Size(60, 25);
            this.OpenFile.TabIndex = 9;
            this.OpenFile.Text = "导入";
            this.OpenFile.UseVisualStyleBackColor = true;
            this.OpenFile.Click += new System.EventHandler(this.OpenFile_Click);
            // 
            // VersionID
            // 
            this.VersionID.Location = new System.Drawing.Point(361, 23);
            this.VersionID.Name = "VersionID";
            this.VersionID.ReadOnly = true;
            this.VersionID.Size = new System.Drawing.Size(75, 23);
            this.VersionID.TabIndex = 8;
            this.VersionID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(312, 27);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(49, 14);
            this.label7.TabIndex = 14;
            this.label7.Text = "版本号";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(6, 27);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(77, 14);
            this.label5.TabIndex = 8;
            this.label5.Text = "传感器型号";
            // 
            // VersionTime
            // 
            this.VersionTime.Location = new System.Drawing.Point(229, 23);
            this.VersionTime.Name = "VersionTime";
            this.VersionTime.ReadOnly = true;
            this.VersionTime.Size = new System.Drawing.Size(75, 23);
            this.VersionTime.TabIndex = 7;
            this.VersionTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(166, 27);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(63, 14);
            this.label6.TabIndex = 13;
            this.label6.Text = "版本日期";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.InitialDirectory = "SenorLibrary";
            // 
            // textBoxIn
            // 
            this.textBoxIn.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxIn.Location = new System.Drawing.Point(6, 56);
            this.textBoxIn.Multiline = true;
            this.textBoxIn.Name = "textBoxIn";
            this.textBoxIn.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIn.Size = new System.Drawing.Size(431, 64);
            this.textBoxIn.TabIndex = 17;
            this.textBoxIn.Click += new System.EventHandler(this.textBoxIn_Click);
            this.textBoxIn.TextChanged += new System.EventHandler(this.textBoxIn_TextChanged);
            this.textBoxIn.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBoxIn_KeyPress);
            // 
            // textBoxCon
            // 
            this.textBoxCon.Location = new System.Drawing.Point(6, 123);
            this.textBoxCon.Multiline = true;
            this.textBoxCon.Name = "textBoxCon";
            this.textBoxCon.ReadOnly = true;
            this.textBoxCon.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxCon.Size = new System.Drawing.Size(431, 64);
            this.textBoxCon.TabIndex = 21;
            this.textBoxCon.DoubleClick += new System.EventHandler(this.textBoxCon_DoubleClick);
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(-73, 4);
            this.textBox6.Name = "textBox6";
            this.textBox6.Size = new System.Drawing.Size(67, 23);
            this.textBox6.TabIndex = 20;
            // 
            // comboBoxInModel
            // 
            this.comboBoxInModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxInModel.FormattingEnabled = true;
            this.comboBoxInModel.Items.AddRange(new object[] {
            "Normal",
            "RTU2ASCII",
            "ASCII2RTU",
            "RTU2RTUAuto",
            "RTU2ASCIIAuto"});
            this.comboBoxInModel.Location = new System.Drawing.Point(443, 56);
            this.comboBoxInModel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.comboBoxInModel.Name = "comboBoxInModel";
            this.comboBoxInModel.Size = new System.Drawing.Size(109, 22);
            this.comboBoxInModel.TabIndex = 19;
            this.comboBoxInModel.SelectedIndexChanged += new System.EventHandler(this.comboBoxInModel_SelectedIndexChanged);
            // 
            // ShowSend
            // 
            this.ShowSend.AutoSize = true;
            this.ShowSend.Location = new System.Drawing.Point(13, 13);
            this.ShowSend.Name = "ShowSend";
            this.ShowSend.Size = new System.Drawing.Size(82, 18);
            this.ShowSend.TabIndex = 22;
            this.ShowSend.Text = "显示发送";
            this.ShowSend.UseVisualStyleBackColor = true;
            this.ShowSend.CheckedChanged += new System.EventHandler(this.ShowSend_CheckedChanged);
            // 
            // ShowTime
            // 
            this.ShowTime.AutoSize = true;
            this.ShowTime.Location = new System.Drawing.Point(13, 33);
            this.ShowTime.Name = "ShowTime";
            this.ShowTime.Size = new System.Drawing.Size(82, 18);
            this.ShowTime.TabIndex = 23;
            this.ShowTime.Text = "显示时间";
            this.ShowTime.UseVisualStyleBackColor = true;
            this.ShowTime.CheckedChanged += new System.EventHandler(this.ShowTime_CheckedChanged);
            // 
            // SendStr
            // 
            this.SendStr.Location = new System.Drawing.Point(443, 87);
            this.SendStr.Name = "SendStr";
            this.SendStr.Size = new System.Drawing.Size(109, 25);
            this.SendStr.TabIndex = 20;
            this.SendStr.Text = "发  送";
            this.SendStr.UseVisualStyleBackColor = true;
            this.SendStr.Click += new System.EventHandler(this.SendStr_Click);
            // 
            // RecData1
            // 
            this.RecData1.Location = new System.Drawing.Point(278, 25);
            this.RecData1.Name = "RecData1";
            this.RecData1.ReadOnly = true;
            this.RecData1.Size = new System.Drawing.Size(110, 23);
            this.RecData1.TabIndex = 14;
            this.RecData1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // groupBox_MsgID
            // 
            this.groupBox_MsgID.Controls.Add(this.label9);
            this.groupBox_MsgID.Controls.Add(this.textBox_2AWrite);
            this.groupBox_MsgID.Controls.Add(this.label_Verity);
            this.groupBox_MsgID.Controls.Add(this.textBox_CRC);
            this.groupBox_MsgID.Controls.Add(this.textBox_LRC);
            this.groupBox_MsgID.Controls.Add(this.label_Tip);
            this.groupBox_MsgID.Controls.Add(this.label_Space);
            this.groupBox_MsgID.Controls.Add(this.label_ShowHelp);
            this.groupBox_MsgID.Controls.Add(this.label8);
            this.groupBox_MsgID.Controls.Add(this.groupBox4);
            this.groupBox_MsgID.Controls.Add(this.btn_ClearShow);
            this.groupBox_MsgID.Controls.Add(this.numericUpDown_SenorID);
            this.groupBox_MsgID.Controls.Add(this.richTextBox1);
            this.groupBox_MsgID.Controls.Add(this.ReadData);
            this.groupBox_MsgID.Controls.Add(this.MsgID1);
            this.groupBox_MsgID.Controls.Add(this.ReadCycle);
            this.groupBox_MsgID.Controls.Add(this.RecData1);
            this.groupBox_MsgID.Controls.Add(this.SenorID);
            this.groupBox_MsgID.Controls.Add(this.SendStr);
            this.groupBox_MsgID.Controls.Add(this.comboBoxInModel);
            this.groupBox_MsgID.Controls.Add(this.textBox6);
            this.groupBox_MsgID.Controls.Add(this.textBoxCon);
            this.groupBox_MsgID.Controls.Add(this.textBoxIn);
            this.groupBox_MsgID.Location = new System.Drawing.Point(614, 8);
            this.groupBox_MsgID.Name = "groupBox_MsgID";
            this.groupBox_MsgID.Size = new System.Drawing.Size(560, 306);
            this.groupBox_MsgID.TabIndex = 12;
            this.groupBox_MsgID.TabStop = false;
            this.groupBox_MsgID.Text = "输入输出";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(533, 29);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(21, 14);
            this.label9.TabIndex = 53;
            this.label9.Text = "ms";
            // 
            // textBox_2AWrite
            // 
            this.textBox_2AWrite.Location = new System.Drawing.Point(275, 97);
            this.textBox_2AWrite.MaxLength = 64;
            this.textBox_2AWrite.Name = "textBox_2AWrite";
            this.textBox_2AWrite.Size = new System.Drawing.Size(144, 23);
            this.textBox_2AWrite.TabIndex = 52;
            this.textBox_2AWrite.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_2AWrite.Visible = false;
            this.textBox_2AWrite.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.textBox_2AWrite_KeyPress);
            // 
            // label_Verity
            // 
            this.label_Verity.AutoSize = true;
            this.label_Verity.BackColor = System.Drawing.Color.Black;
            this.label_Verity.Font = new System.Drawing.Font("宋体", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Verity.Location = new System.Drawing.Point(380, 51);
            this.label_Verity.Name = "label_Verity";
            this.label_Verity.Size = new System.Drawing.Size(6, 5);
            this.label_Verity.TabIndex = 51;
            this.label_Verity.Text = " ";
            this.label_Verity.Visible = false;
            // 
            // textBox_CRC
            // 
            this.textBox_CRC.Location = new System.Drawing.Point(369, 164);
            this.textBox_CRC.MaxLength = 4;
            this.textBox_CRC.Name = "textBox_CRC";
            this.textBox_CRC.ReadOnly = true;
            this.textBox_CRC.Size = new System.Drawing.Size(50, 23);
            this.textBox_CRC.TabIndex = 50;
            this.textBox_CRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_CRC.Visible = false;
            this.textBox_CRC.DoubleClick += new System.EventHandler(this.textBox_CRC_DoubleClick);
            // 
            // textBox_LRC
            // 
            this.textBox_LRC.Location = new System.Drawing.Point(320, 164);
            this.textBox_LRC.MaxLength = 4;
            this.textBox_LRC.Name = "textBox_LRC";
            this.textBox_LRC.ReadOnly = true;
            this.textBox_LRC.Size = new System.Drawing.Size(50, 23);
            this.textBox_LRC.TabIndex = 49;
            this.textBox_LRC.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.textBox_LRC.Visible = false;
            this.textBox_LRC.DoubleClick += new System.EventHandler(this.textBox_LRC_DoubleClick);
            // 
            // label_Tip
            // 
            this.label_Tip.AutoSize = true;
            this.label_Tip.BackColor = System.Drawing.Color.Red;
            this.label_Tip.Font = new System.Drawing.Font("宋体", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Tip.Location = new System.Drawing.Point(391, 51);
            this.label_Tip.Name = "label_Tip";
            this.label_Tip.Size = new System.Drawing.Size(6, 5);
            this.label_Tip.TabIndex = 48;
            this.label_Tip.Text = " ";
            this.label_Tip.Visible = false;
            // 
            // label_Space
            // 
            this.label_Space.AutoSize = true;
            this.label_Space.BackColor = System.Drawing.Color.Blue;
            this.label_Space.Font = new System.Drawing.Font("宋体", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_Space.Location = new System.Drawing.Point(402, 51);
            this.label_Space.Name = "label_Space";
            this.label_Space.Size = new System.Drawing.Size(6, 5);
            this.label_Space.TabIndex = 47;
            this.label_Space.Text = " ";
            this.label_Space.Visible = false;
            // 
            // label_ShowHelp
            // 
            this.label_ShowHelp.AutoSize = true;
            this.label_ShowHelp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.label_ShowHelp.Font = new System.Drawing.Font("宋体", 3.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label_ShowHelp.Location = new System.Drawing.Point(413, 51);
            this.label_ShowHelp.Name = "label_ShowHelp";
            this.label_ShowHelp.Size = new System.Drawing.Size(6, 5);
            this.label_ShowHelp.TabIndex = 46;
            this.label_ShowHelp.Text = " ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(141, 29);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 14);
            this.label8.TabIndex = 44;
            this.label8.Text = "寄存器地址";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.ShowSend);
            this.groupBox4.Controls.Add(this.ShowTime);
            this.groupBox4.Controls.Add(this.ShowAnalyze);
            this.groupBox4.Location = new System.Drawing.Point(443, 112);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(109, 75);
            this.groupBox4.TabIndex = 16;
            this.groupBox4.TabStop = false;
            // 
            // ShowAnalyze
            // 
            this.ShowAnalyze.AutoSize = true;
            this.ShowAnalyze.Location = new System.Drawing.Point(13, 53);
            this.ShowAnalyze.Name = "ShowAnalyze";
            this.ShowAnalyze.Size = new System.Drawing.Size(82, 18);
            this.ShowAnalyze.TabIndex = 24;
            this.ShowAnalyze.Text = "显示解析";
            this.ShowAnalyze.UseVisualStyleBackColor = true;
            this.ShowAnalyze.CheckedChanged += new System.EventHandler(this.Simulation_CheckedChanged);
            // 
            // btn_ClearShow
            // 
            this.btn_ClearShow.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btn_ClearShow.BackgroundImage")));
            this.btn_ClearShow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btn_ClearShow.Location = new System.Drawing.Point(514, 284);
            this.btn_ClearShow.Name = "btn_ClearShow";
            this.btn_ClearShow.Size = new System.Drawing.Size(20, 20);
            this.btn_ClearShow.TabIndex = 25;
            this.btn_ClearShow.UseVisualStyleBackColor = true;
            this.btn_ClearShow.Click += new System.EventHandler(this.button1_Clear_Click);
            // 
            // numericUpDown_SenorID
            // 
            this.numericUpDown_SenorID.Location = new System.Drawing.Point(83, 25);
            this.numericUpDown_SenorID.Maximum = new decimal(new int[] {
            247,
            0,
            0,
            0});
            this.numericUpDown_SenorID.Name = "numericUpDown_SenorID";
            this.numericUpDown_SenorID.Size = new System.Drawing.Size(54, 23);
            this.numericUpDown_SenorID.TabIndex = 12;
            this.numericUpDown_SenorID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.numericUpDown_SenorID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown_SenorID.ValueChanged += new System.EventHandler(this.numericUpDown_SenorID_ValueChanged);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(6, 190);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.ReadOnly = true;
            this.richTextBox1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
            this.richTextBox1.Size = new System.Drawing.Size(546, 114);
            this.richTextBox1.TabIndex = 25;
            this.richTextBox1.Text = "";
            this.richTextBox1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.richTextBox1_KeyPress);
            // 
            // ReadData
            // 
            this.ReadData.BackColor = System.Drawing.SystemColors.Control;
            this.ReadData.Location = new System.Drawing.Point(397, 24);
            this.ReadData.Name = "ReadData";
            this.ReadData.Size = new System.Drawing.Size(70, 25);
            this.ReadData.TabIndex = 15;
            this.ReadData.Text = "读数";
            this.ReadData.UseVisualStyleBackColor = false;
            this.ReadData.Click += new System.EventHandler(this.ReadData_Click);
            // 
            // MsgID1
            // 
            this.MsgID1.Location = new System.Drawing.Point(219, 25);
            this.MsgID1.MaxLength = 4;
            this.MsgID1.Name = "MsgID1";
            this.MsgID1.Size = new System.Drawing.Size(50, 23);
            this.MsgID1.TabIndex = 13;
            this.MsgID1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.MsgID1.TextChanged += new System.EventHandler(this.MsgID1_TextChanged);
            this.MsgID1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.MsgID1_KeyPress);
            // 
            // ReadCycle
            // 
            this.ReadCycle.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReadCycle.Location = new System.Drawing.Point(477, 25);
            this.ReadCycle.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.ReadCycle.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReadCycle.Name = "ReadCycle";
            this.ReadCycle.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.ReadCycle.Size = new System.Drawing.Size(55, 23);
            this.ReadCycle.TabIndex = 16;
            this.ReadCycle.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ReadCycle.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.ReadCycle.ValueChanged += new System.EventHandler(this.ReadCycle_ValueChanged);
            // 
            // SenorID
            // 
            this.SenorID.AutoSize = true;
            this.SenorID.Location = new System.Drawing.Point(6, 29);
            this.SenorID.Name = "SenorID";
            this.SenorID.Size = new System.Drawing.Size(77, 14);
            this.SenorID.TabIndex = 28;
            this.SenorID.Text = "传感器地址";
            // 
            // timer1_Time
            // 
            this.timer1_Time.Enabled = true;
            this.timer1_Time.Interval = 1000;
            this.timer1_Time.Tick += new System.EventHandler(this.timer1_Time_Tick);
            // 
            // dataGridView2_SendTable
            // 
            this.dataGridView2_SendTable.AllowUserToResizeColumns = false;
            this.dataGridView2_SendTable.AllowUserToResizeRows = false;
            this.dataGridView2_SendTable.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView2_SendTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView2_SendTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridView2_SendTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView2_SendTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5});
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView2_SendTable.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridView2_SendTable.Location = new System.Drawing.Point(614, 318);
            this.dataGridView2_SendTable.Name = "dataGridView2_SendTable";
            this.dataGridView2_SendTable.ReadOnly = true;
            this.dataGridView2_SendTable.RowHeadersVisible = false;
            this.dataGridView2_SendTable.RowHeadersWidth = 24;
            this.dataGridView2_SendTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView2_SendTable.RowTemplate.Height = 27;
            this.dataGridView2_SendTable.Size = new System.Drawing.Size(560, 142);
            this.dataGridView2_SendTable.TabIndex = 27;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "字节数";
            this.dataGridViewTextBoxColumn1.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 60;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "描述";
            this.dataGridViewTextBoxColumn3.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 150;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "数据(Hex)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 85;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "数据含义";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 245;
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 50;
            this.toolTip1.ReshowDelay = 100;
            // 
            // serialPort1
            // 
            this.serialPort1.ReceivedBytesThreshold = 5;
            this.serialPort1.DataReceived += new System.IO.Ports.SerialDataReceivedEventHandler(this.serialPort1_DataReceived);
            // 
            // dataGridView3_RecTable
            // 
            this.dataGridView3_RecTable.AllowUserToResizeColumns = false;
            this.dataGridView3_RecTable.AllowUserToResizeRows = false;
            this.dataGridView3_RecTable.BackgroundColor = System.Drawing.SystemColors.ButtonFace;
            this.dataGridView3_RecTable.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView3_RecTable.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridView3_RecTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dataGridView3_RecTable.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView3_RecTable.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridView3_RecTable.Location = new System.Drawing.Point(614, 463);
            this.dataGridView3_RecTable.Name = "dataGridView3_RecTable";
            this.dataGridView3_RecTable.ReadOnly = true;
            this.dataGridView3_RecTable.RowHeadersVisible = false;
            this.dataGridView3_RecTable.RowHeadersWidth = 24;
            this.dataGridView3_RecTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dataGridView3_RecTable.RowTemplate.Height = 27;
            this.dataGridView3_RecTable.Size = new System.Drawing.Size(560, 142);
            this.dataGridView3_RecTable.TabIndex = 28;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "字节数";
            this.dataGridViewTextBoxColumn2.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            this.dataGridViewTextBoxColumn2.Width = 60;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "描述";
            this.dataGridViewTextBoxColumn6.MinimumWidth = 6;
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 150;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "数据(Hex)";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 85;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "数据含义";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 245;
            // 
            // button_Help
            // 
            this.button_Help.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("button_Help.BackgroundImage")));
            this.button_Help.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.button_Help.Location = new System.Drawing.Point(1103, -1);
            this.button_Help.Name = "button_Help";
            this.button_Help.Size = new System.Drawing.Size(20, 18);
            this.button_Help.TabIndex = 29;
            this.button_Help.UseVisualStyleBackColor = true;
            this.button_Help.Click += new System.EventHandler(this.button_Help_Click);
            // 
            // timer_RefreshSerialDelay
            // 
            this.timer_RefreshSerialDelay.Interval = 20;
            this.timer_RefreshSerialDelay.Tick += new System.EventHandler(this.timer_RefreshSerialDelay_Tick);
            // 
            // timer_ReadData
            // 
            this.timer_ReadData.Tick += new System.EventHandler(this.timer_ReadData_Tick);
            // 
            // timer_ShowReceData
            // 
            this.timer_ShowReceData.Interval = 5;
            this.timer_ShowReceData.Tick += new System.EventHandler(this.timer_ShowReceData_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(1184, 611);
            this.Controls.Add(this.button_Help);
            this.Controls.Add(this.dataGridView3_RecTable);
            this.Controls.Add(this.dataGridView2_SendTable);
            this.Controls.Add(this.groupBox_MsgID);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.ParaTable);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.HelpButton = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1200, 650);
            this.MinimumSize = new System.Drawing.Size(1200, 650);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Modbus";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ParaTable)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox_MsgID.ResumeLayout(false);
            this.groupBox_MsgID.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown_SenorID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ReadCycle)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2_SendTable)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView3_RecTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox Parity;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox Baud;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox SerialCom;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox StopBit;
        private System.Windows.Forms.DataGridView ParaTable;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox VersionID;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox VersionTime;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button OpenFile;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBoxIn;
        private System.Windows.Forms.TextBox textBoxCon;
        private System.Windows.Forms.TextBox textBox6;
        private System.Windows.Forms.ComboBox comboBoxInModel;
        private System.Windows.Forms.CheckBox ShowSend;
        private System.Windows.Forms.CheckBox ShowTime;
        private System.Windows.Forms.Button SendStr;
        private System.Windows.Forms.TextBox RecData1;
        private System.Windows.Forms.GroupBox groupBox_MsgID;
        private System.Windows.Forms.NumericUpDown ReadCycle;
        private System.Windows.Forms.Label SenorID;
        private System.Windows.Forms.Button ReadData;
        private System.Windows.Forms.TextBox MsgID1;
        private System.Windows.Forms.DataGridView dataGridView2_SendTable;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox ShowAnalyze;
        private System.Windows.Forms.NumericUpDown numericUpDown_SenorID;
        private System.Windows.Forms.TextBox SenorModel;
        private System.Windows.Forms.ComboBox ReadWrite;
        private System.Windows.Forms.Button btn_ClearShow;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.IO.Ports.SerialPort serialPort1;
        private System.Windows.Forms.Button btn_SerialOpen;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.DataGridView dataGridView3_RecTable;
        private System.Windows.Forms.Button button_Help;
        private System.Windows.Forms.HelpProvider helpProvider1;
        private System.Windows.Forms.Timer timer_RefreshSerialDelay;
        private System.Windows.Forms.Timer timer1_Time;
        private System.Windows.Forms.Timer timer_ReadData;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Timer timer_ShowReceData;
        private System.Windows.Forms.Label label_ShowHelp;
        private System.Windows.Forms.Label label_Tip;
        private System.Windows.Forms.Label label_Space;
        private System.Windows.Forms.TextBox textBox_CRC;
        private System.Windows.Forms.TextBox textBox_LRC;
        private System.Windows.Forms.Label label_Verity;
        private System.Windows.Forms.TextBox textBox_2AWrite;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox Baud1;
    }
}

