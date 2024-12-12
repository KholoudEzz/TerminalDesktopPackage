using static System.Net.Mime.MediaTypeNames;
using System.Windows.Forms;
using System.Xml.Linq;

namespace RashidConfiguration
{
    partial class Configuration
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            groupBox1 = new GroupBox();
            BothSilentRBtn = new RadioButton();
            BothRBtn = new RadioButton();
            PhyPrinterRBtn = new RadioButton();
            RPrinter_RBtn = new RadioButton();
            SaveSelectionBtn = new Button();
            groupBox3 = new GroupBox();
            label5 = new Label();
            PrintersCBox = new ComboBox();
            groupBox1.SuspendLayout();
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(BothSilentRBtn);
            groupBox1.Controls.Add(BothRBtn);
            groupBox1.Controls.Add(PhyPrinterRBtn);
            groupBox1.Controls.Add(RPrinter_RBtn);
            groupBox1.Location = new Point(39, 19);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(351, 215);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Printer Selection";
            // 
            // BothSilentRBtn
            // 
            BothSilentRBtn.AutoSize = true;
            BothSilentRBtn.Location = new Point(19, 176);
            BothSilentRBtn.Margin = new Padding(3, 4, 3, 4);
            BothSilentRBtn.Name = "BothSilentRBtn";
            BothSilentRBtn.Size = new Size(155, 24);
            BothSilentRBtn.TabIndex = 3;
            BothSilentRBtn.TabStop = true;
            BothSilentRBtn.Text = "Both Printers Silent";
            BothSilentRBtn.UseVisualStyleBackColor = true;
            // 
            // BothRBtn
            // 
            BothRBtn.AutoSize = true;
            BothRBtn.Location = new Point(19, 133);
            BothRBtn.Margin = new Padding(3, 4, 3, 4);
            BothRBtn.Name = "BothRBtn";
            BothRBtn.Size = new Size(114, 24);
            BothRBtn.TabIndex = 2;
            BothRBtn.TabStop = true;
            BothRBtn.Text = "Both Printers";
            BothRBtn.UseVisualStyleBackColor = true;
            // 
            // PhyPrinterRBtn
            // 
            PhyPrinterRBtn.AutoSize = true;
            PhyPrinterRBtn.Location = new Point(19, 91);
            PhyPrinterRBtn.Margin = new Padding(3, 4, 3, 4);
            PhyPrinterRBtn.Name = "PhyPrinterRBtn";
            PhyPrinterRBtn.Size = new Size(129, 24);
            PhyPrinterRBtn.TabIndex = 1;
            PhyPrinterRBtn.TabStop = true;
            PhyPrinterRBtn.Text = "Physical Printer";
            PhyPrinterRBtn.UseVisualStyleBackColor = true;
            // 
            // RPrinter_RBtn
            // 
            RPrinter_RBtn.AutoSize = true;
            RPrinter_RBtn.Location = new Point(19, 48);
            RPrinter_RBtn.Margin = new Padding(3, 4, 3, 4);
            RPrinter_RBtn.Name = "RPrinter_RBtn";
            RPrinter_RBtn.Size = new Size(121, 24);
            RPrinter_RBtn.TabIndex = 0;
            RPrinter_RBtn.TabStop = true;
            RPrinter_RBtn.Text = "Rashid Printer";
            RPrinter_RBtn.UseVisualStyleBackColor = true;
            RPrinter_RBtn.CheckedChanged += RPrinter_RBtn_CheckedChanged;
            // 
            // SaveSelectionBtn
            // 
            SaveSelectionBtn.Location = new Point(675, 242);
            SaveSelectionBtn.Margin = new Padding(3, 4, 3, 4);
            SaveSelectionBtn.Name = "SaveSelectionBtn";
            SaveSelectionBtn.Size = new Size(86, 31);
            SaveSelectionBtn.TabIndex = 1;
            SaveSelectionBtn.Text = "Save";
            SaveSelectionBtn.UseVisualStyleBackColor = true;
            SaveSelectionBtn.Click += SaveSelectionBtn_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(label5);
            groupBox3.Controls.Add(PrintersCBox);
            groupBox3.Location = new Point(432, 19);
            groupBox3.Margin = new Padding(3, 4, 3, 4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(3, 4, 3, 4);
            groupBox3.Size = new Size(329, 215);
            groupBox3.TabIndex = 4;
            groupBox3.TabStop = false;
            groupBox3.Text = "Physical Printer";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(17, 51);
            label5.Name = "label5";
            label5.Size = new Size(108, 20);
            label5.TabIndex = 2;
            label5.Text = "Default Printer:";
            // 
            // PrintersCBox
            // 
            PrintersCBox.FormattingEnabled = true;
            PrintersCBox.Location = new Point(65, 97);
            PrintersCBox.Margin = new Padding(3, 4, 3, 4);
            PrintersCBox.Name = "PrintersCBox";
            PrintersCBox.Size = new Size(242, 28);
            PrintersCBox.TabIndex = 3;
            PrintersCBox.SelectedIndexChanged += PrintersCBox_SelectedIndexChanged;
            // 
            // Configuration
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(819, 296);
            Controls.Add(groupBox3);
            Controls.Add(SaveSelectionBtn);
            Controls.Add(groupBox1);
            FormBorderStyle = FormBorderStyle.FixedToolWindow;
            Margin = new Padding(3, 4, 3, 4);
            Name = "Configuration";
            Text = "RashidConfiguration   -Version 1.0.3";
            Load += Configuration_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private GroupBox groupBox1;
        private RadioButton BothRBtn;
        private RadioButton PhyPrinterRBtn;
        private RadioButton RPrinter_RBtn;
        private Button SaveSelectionBtn;
        private GroupBox groupBox3;
        private Label label5;
        private ComboBox PrintersCBox;
        private RadioButton BothSilentRBtn;
    }
}
