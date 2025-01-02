namespace PrinterSettingsTool
{
    partial class PrinterTool
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            SaveSelectionBtn = new Button();
            groupBox2 = new GroupBox();
            textBoxPathJSON = new TextBox();
            buttonBrowseJSON = new Button();
            label3 = new Label();
            textBoxPathXPS = new TextBox();
            buttonBrowseXPS = new Button();
            label2 = new Label();
            textBoxPathXML = new TextBox();
            buttonBrowseXML = new Button();
            labelPath = new Label();
            groupBox4 = new GroupBox();
            panel2 = new Panel();
            radioBFRNo = new RadioButton();
            radioBFRYes = new RadioButton();
            panel1 = new Panel();
            RB_XML = new RadioButton();
            RB_XPS = new RadioButton();
            label5 = new Label();
            TBPrinterOutPath = new TextBox();
            PrinterOutPathBtn = new Button();
            label4 = new Label();
            ExecutablePathBtn = new Button();
            TBExecutablePath = new TextBox();
            label6 = new Label();
            label1 = new Label();
            groupBox1 = new GroupBox();
            DBBrowse = new Button();
            TBDatabaseFile = new TextBox();
            label8 = new Label();
            ReSetBtn = new Button();
            groupBox3 = new GroupBox();
            groupBox2.SuspendLayout();
            groupBox4.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // SaveSelectionBtn
            // 
            SaveSelectionBtn.Location = new Point(766, 423);
            SaveSelectionBtn.Margin = new Padding(3, 4, 3, 4);
            SaveSelectionBtn.Name = "SaveSelectionBtn";
            SaveSelectionBtn.Size = new Size(86, 31);
            SaveSelectionBtn.TabIndex = 1;
            SaveSelectionBtn.Text = "Save";
            SaveSelectionBtn.UseVisualStyleBackColor = true;
            SaveSelectionBtn.Click += SaveSelectionBtn_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(textBoxPathJSON);
            groupBox2.Controls.Add(buttonBrowseJSON);
            groupBox2.Controls.Add(label3);
            groupBox2.Controls.Add(textBoxPathXPS);
            groupBox2.Controls.Add(buttonBrowseXPS);
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(textBoxPathXML);
            groupBox2.Controls.Add(buttonBrowseXML);
            groupBox2.Controls.Add(labelPath);
            groupBox2.Location = new Point(39, 482);
            groupBox2.Margin = new Padding(3, 4, 3, 4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(3, 4, 3, 4);
            groupBox2.Size = new Size(703, 229);
            groupBox2.TabIndex = 2;
            groupBox2.TabStop = false;
            groupBox2.Text = "Paths Configuration";
            groupBox2.Visible = false;
            // 
            // textBoxPathJSON
            // 
            textBoxPathJSON.Location = new Point(102, 163);
            textBoxPathJSON.Margin = new Padding(3, 4, 3, 4);
            textBoxPathJSON.Name = "textBoxPathJSON";
            textBoxPathJSON.Size = new Size(459, 27);
            textBoxPathJSON.TabIndex = 7;
            // 
            // buttonBrowseJSON
            // 
            buttonBrowseJSON.Location = new Point(591, 167);
            buttonBrowseJSON.Margin = new Padding(3, 4, 3, 4);
            buttonBrowseJSON.Name = "buttonBrowseJSON";
            buttonBrowseJSON.Size = new Size(70, 31);
            buttonBrowseJSON.TabIndex = 8;
            buttonBrowseJSON.Text = "Browse";
            buttonBrowseJSON.UseVisualStyleBackColor = true;
            buttonBrowseJSON.Click += buttonBrowseJSON_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(11, 167);
            label3.Name = "label3";
            label3.Size = new Size(79, 20);
            label3.TabIndex = 6;
            label3.Text = "JSON Path:";
            // 
            // textBoxPathXPS
            // 
            textBoxPathXPS.Location = new Point(102, 100);
            textBoxPathXPS.Margin = new Padding(3, 4, 3, 4);
            textBoxPathXPS.Name = "textBoxPathXPS";
            textBoxPathXPS.Size = new Size(459, 27);
            textBoxPathXPS.TabIndex = 4;
            // 
            // buttonBrowseXPS
            // 
            buttonBrowseXPS.Location = new Point(591, 104);
            buttonBrowseXPS.Margin = new Padding(3, 4, 3, 4);
            buttonBrowseXPS.Name = "buttonBrowseXPS";
            buttonBrowseXPS.Size = new Size(70, 31);
            buttonBrowseXPS.TabIndex = 5;
            buttonBrowseXPS.Text = "Browse";
            buttonBrowseXPS.UseVisualStyleBackColor = true;
            buttonBrowseXPS.Click += buttonBrowseXPS_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(11, 104);
            label2.Name = "label2";
            label2.Size = new Size(69, 20);
            label2.TabIndex = 3;
            label2.Text = "XPS Path:";
            // 
            // textBoxPathXML
            // 
            textBoxPathXML.Location = new Point(102, 35);
            textBoxPathXML.Margin = new Padding(3, 4, 3, 4);
            textBoxPathXML.Name = "textBoxPathXML";
            textBoxPathXML.Size = new Size(459, 27);
            textBoxPathXML.TabIndex = 1;
            // 
            // buttonBrowseXML
            // 
            buttonBrowseXML.Location = new Point(591, 39);
            buttonBrowseXML.Margin = new Padding(3, 4, 3, 4);
            buttonBrowseXML.Name = "buttonBrowseXML";
            buttonBrowseXML.Size = new Size(70, 31);
            buttonBrowseXML.TabIndex = 2;
            buttonBrowseXML.Text = "Browse";
            buttonBrowseXML.UseVisualStyleBackColor = true;
            buttonBrowseXML.Click += buttonBrowseXML_Click;
            // 
            // labelPath
            // 
            labelPath.AutoSize = true;
            labelPath.Location = new Point(11, 39);
            labelPath.Name = "labelPath";
            labelPath.Size = new Size(73, 20);
            labelPath.TabIndex = 0;
            labelPath.Text = "XML Path:";
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(panel2);
            groupBox4.Controls.Add(panel1);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(TBPrinterOutPath);
            groupBox4.Controls.Add(PrinterOutPathBtn);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(ExecutablePathBtn);
            groupBox4.Controls.Add(TBExecutablePath);
            groupBox4.Controls.Add(label6);
            groupBox4.Controls.Add(label1);
            groupBox4.Location = new Point(39, 13);
            groupBox4.Margin = new Padding(3, 4, 3, 4);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(3, 4, 3, 4);
            groupBox4.Size = new Size(703, 210);
            groupBox4.TabIndex = 5;
            groupBox4.TabStop = false;
            groupBox4.Text = "Rasheed Printer";
            // 
            // panel2
            // 
            panel2.Controls.Add(radioBFRNo);
            panel2.Controls.Add(radioBFRYes);
            panel2.Location = new Point(591, 15);
            panel2.Name = "panel2";
            panel2.Size = new Size(268, 61);
            panel2.TabIndex = 15;
            panel2.Visible = false;
            // 
            // radioBFRNo
            // 
            radioBFRNo.AutoSize = true;
            radioBFRNo.Location = new Point(152, 20);
            radioBFRNo.Margin = new Padding(3, 4, 3, 4);
            radioBFRNo.Name = "radioBFRNo";
            radioBFRNo.Size = new Size(50, 24);
            radioBFRNo.TabIndex = 12;
            radioBFRNo.Text = "No";
            radioBFRNo.UseVisualStyleBackColor = true;
            radioBFRNo.CheckedChanged += radioBFRNo_CheckedChanged;
            // 
            // radioBFRYes
            // 
            radioBFRYes.AutoSize = true;
            radioBFRYes.Checked = true;
            radioBFRYes.Location = new Point(18, 20);
            radioBFRYes.Margin = new Padding(3, 4, 3, 4);
            radioBFRYes.Name = "radioBFRYes";
            radioBFRYes.Size = new Size(51, 24);
            radioBFRYes.TabIndex = 13;
            radioBFRYes.TabStop = true;
            radioBFRYes.Text = "Yes";
            radioBFRYes.UseVisualStyleBackColor = true;
            radioBFRYes.CheckedChanged += radioBFRYes_CheckedChanged;
            // 
            // panel1
            // 
            panel1.Controls.Add(RB_XML);
            panel1.Controls.Add(RB_XPS);
            panel1.Location = new Point(136, 28);
            panel1.Name = "panel1";
            panel1.Size = new Size(268, 48);
            panel1.TabIndex = 14;
            // 
            // RB_XML
            // 
            RB_XML.AutoSize = true;
            RB_XML.Location = new Point(152, 9);
            RB_XML.Margin = new Padding(3, 4, 3, 4);
            RB_XML.Name = "RB_XML";
            RB_XML.Size = new Size(59, 24);
            RB_XML.TabIndex = 3;
            RB_XML.Text = "XML";
            RB_XML.UseVisualStyleBackColor = true;
            RB_XML.CheckedChanged += RB_XML_CheckedChanged;
            // 
            // RB_XPS
            // 
            RB_XPS.AutoSize = true;
            RB_XPS.Location = new Point(18, 9);
            RB_XPS.Margin = new Padding(3, 4, 3, 4);
            RB_XPS.Name = "RB_XPS";
            RB_XPS.Size = new Size(55, 24);
            RB_XPS.TabIndex = 4;
            RB_XPS.Text = "XPS";
            RB_XPS.UseVisualStyleBackColor = true;
            RB_XPS.CheckedChanged += RB_XPS_CheckedChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(435, 35);
            label5.Name = "label5";
            label5.Size = new Size(88, 20);
            label5.TabIndex = 11;
            label5.Text = "Rasheed FR:";
            label5.Visible = false;
            // 
            // TBPrinterOutPath
            // 
            TBPrinterOutPath.Location = new Point(154, 103);
            TBPrinterOutPath.Margin = new Padding(3, 4, 3, 4);
            TBPrinterOutPath.Name = "TBPrinterOutPath";
            TBPrinterOutPath.Size = new Size(443, 27);
            TBPrinterOutPath.TabIndex = 9;
            // 
            // PrinterOutPathBtn
            // 
            PrinterOutPathBtn.Location = new Point(625, 102);
            PrinterOutPathBtn.Margin = new Padding(3, 4, 3, 4);
            PrinterOutPathBtn.Name = "PrinterOutPathBtn";
            PrinterOutPathBtn.Size = new Size(70, 31);
            PrinterOutPathBtn.TabIndex = 10;
            PrinterOutPathBtn.Text = "Browse";
            PrinterOutPathBtn.UseVisualStyleBackColor = true;
            PrinterOutPathBtn.Click += PrinterOutPathBtn_Click;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(19, 107);
            label4.Name = "label4";
            label4.Size = new Size(90, 20);
            label4.TabIndex = 8;
            label4.Text = "Output Path:";
            // 
            // ExecutablePathBtn
            // 
            ExecutablePathBtn.Location = new Point(625, 153);
            ExecutablePathBtn.Margin = new Padding(3, 4, 3, 4);
            ExecutablePathBtn.Name = "ExecutablePathBtn";
            ExecutablePathBtn.Size = new Size(70, 31);
            ExecutablePathBtn.TabIndex = 7;
            ExecutablePathBtn.Text = "Browse";
            ExecutablePathBtn.UseVisualStyleBackColor = true;
            ExecutablePathBtn.Click += ExecutablePathBtn_Click;
            // 
            // TBExecutablePath
            // 
            TBExecutablePath.Location = new Point(154, 153);
            TBExecutablePath.Margin = new Padding(3, 4, 3, 4);
            TBExecutablePath.Name = "TBExecutablePath";
            TBExecutablePath.Size = new Size(443, 27);
            TBExecutablePath.TabIndex = 6;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 159);
            label6.Name = "label6";
            label6.Size = new Size(84, 20);
            label6.TabIndex = 5;
            label6.Text = "Executable:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(15, 41);
            label1.Name = "label1";
            label1.Size = new Size(93, 20);
            label1.TabIndex = 2;
            label1.Text = "Output Type:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(DBBrowse);
            groupBox1.Controls.Add(TBDatabaseFile);
            groupBox1.Controls.Add(label8);
            groupBox1.Location = new Point(39, 237);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(703, 99);
            groupBox1.TabIndex = 9;
            groupBox1.TabStop = false;
            groupBox1.Text = "DataBase Configuration";
            // 
            // DBBrowse
            // 
            DBBrowse.Location = new Point(625, 35);
            DBBrowse.Margin = new Padding(3, 4, 3, 4);
            DBBrowse.Name = "DBBrowse";
            DBBrowse.Size = new Size(71, 31);
            DBBrowse.TabIndex = 9;
            DBBrowse.Text = "Browse";
            DBBrowse.UseVisualStyleBackColor = true;
            DBBrowse.Click += DBBrowse_Click;
            // 
            // TBDatabaseFile
            // 
            TBDatabaseFile.ForeColor = Color.Gray;
            TBDatabaseFile.Location = new Point(154, 35);
            TBDatabaseFile.Margin = new Padding(3, 4, 3, 4);
            TBDatabaseFile.Name = "TBDatabaseFile";
            TBDatabaseFile.Size = new Size(443, 27);
            TBDatabaseFile.TabIndex = 1;
            TBDatabaseFile.Text = "Enter File path like  c:\\dbfile.db";
            TBDatabaseFile.GotFocus += TBDatabaseFile_GotFocus;
            TBDatabaseFile.LostFocus += TBDatabaseFile_LostFocus;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(11, 39);
            label8.Name = "label8";
            label8.Size = new Size(134, 20);
            label8.TabIndex = 0;
            label8.Text = "Database File Path:";
            // 
            // ReSetBtn
            // 
            ReSetBtn.Location = new Point(214, 43);
            ReSetBtn.Margin = new Padding(3, 4, 3, 4);
            ReSetBtn.Name = "ReSetBtn";
            ReSetBtn.Size = new Size(224, 31);
            ReSetBtn.TabIndex = 10;
            ReSetBtn.Text = "Reset Rasheed Printer";
            ReSetBtn.UseVisualStyleBackColor = true;
            ReSetBtn.Click += ReSetBtn_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(ReSetBtn);
            groupBox3.Location = new Point(39, 357);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(703, 99);
            groupBox3.TabIndex = 11;
            groupBox3.TabStop = false;
            groupBox3.Text = "Rasheed Printer Reset";
            // 
            // PrinterTool
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(883, 496);
            Controls.Add(groupBox3);
            Controls.Add(groupBox1);
            Controls.Add(groupBox4);
            Controls.Add(groupBox2);
            Controls.Add(SaveSelectionBtn);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 4, 3, 4);
            Name = "PrinterTool";
            Text = "Print Settings Tool -Version 1.0.4";
            Load += PrinterTool_Load;
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private Button SaveSelectionBtn;
        private GroupBox groupBox2;

        private Label labelPath;

        private TextBox textBoxPathXML;
        private Button buttonBrowseXML;
        private TextBox textBoxPathJSON;
        private Button buttonBrowseJSON;
        private Label label3;
        private TextBox textBoxPathXPS;
        private Button buttonBrowseXPS;
        private Label label2;
        private GroupBox groupBox4;
        private Label label1;
        private RadioButton RB_XPS;
        private RadioButton RB_XML;
        private Button ExecutablePathBtn;
        private TextBox TBExecutablePath;
        private Label label6;
        private TextBox TBPrinterOutPath;
        private Button PrinterOutPathBtn;
        private Label label4;
        private GroupBox groupBox1;
        private TextBox textBox2;
        private Button button2;
        private Label label7;
        private TextBox TBDatabaseFile;
        private Button button3;
        private Label label8;
        private Button DBBrowse;
        private Button ReSetBtn;
        private GroupBox groupBox3;
        private RadioButton radioBFRYes;
        private RadioButton radioBFRNo;
        private Label label5;
        private Panel panel1;
        private Panel panel2;
    }
}
