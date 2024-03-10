namespace CulmativeWinAppCS12
{
    partial class Form1
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.listMsgs = new System.Windows.Forms.ListBox();
            this.chartData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.TempLbl = new System.Windows.Forms.Label();
            this.humLbl = new System.Windows.Forms.Label();
            this.lightLbl = new System.Windows.Forms.Label();
            this.moisLbl = new System.Windows.Forms.Label();
            this.tempBox = new System.Windows.Forms.TextBox();
            this.humBox = new System.Windows.Forms.TextBox();
            this.lightBox = new System.Windows.Forms.TextBox();
            this.moisBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).BeginInit();
            this.SuspendLayout();
            // 
            // listMsgs
            // 
            this.listMsgs.FormattingEnabled = true;
            this.listMsgs.Location = new System.Drawing.Point(27, 12);
            this.listMsgs.Name = "listMsgs";
            this.listMsgs.Size = new System.Drawing.Size(543, 82);
            this.listMsgs.TabIndex = 0;
            // 
            // chartData
            // 
            chartArea1.Name = "ChartArea1";
            this.chartData.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chartData.Legends.Add(legend1);
            this.chartData.Location = new System.Drawing.Point(576, 12);
            this.chartData.Name = "chartData";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chartData.Series.Add(series1);
            this.chartData.Size = new System.Drawing.Size(673, 713);
            this.chartData.TabIndex = 1;
            this.chartData.Text = "chart1";
            // 
            // TempLbl
            // 
            this.TempLbl.AutoSize = true;
            this.TempLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TempLbl.Location = new System.Drawing.Point(27, 125);
            this.TempLbl.Name = "TempLbl";
            this.TempLbl.Size = new System.Drawing.Size(217, 25);
            this.TempLbl.TabIndex = 2;
            this.TempLbl.Text = "Current Temperature:";
            // 
            // humLbl
            // 
            this.humLbl.AutoSize = true;
            this.humLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.humLbl.Location = new System.Drawing.Point(32, 172);
            this.humLbl.Name = "humLbl";
            this.humLbl.Size = new System.Drawing.Size(178, 25);
            this.humLbl.TabIndex = 3;
            this.humLbl.Text = "Current Humidity:";
            // 
            // lightLbl
            // 
            this.lightLbl.AutoSize = true;
            this.lightLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lightLbl.Location = new System.Drawing.Point(37, 223);
            this.lightLbl.Name = "lightLbl";
            this.lightLbl.Size = new System.Drawing.Size(222, 25);
            this.lightLbl.TabIndex = 4;
            this.lightLbl.Text = "Current Light Percent:";
            // 
            // moisLbl
            // 
            this.moisLbl.AutoSize = true;
            this.moisLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moisLbl.Location = new System.Drawing.Point(42, 273);
            this.moisLbl.Name = "moisLbl";
            this.moisLbl.Size = new System.Drawing.Size(335, 25);
            this.moisLbl.TabIndex = 5;
            this.moisLbl.Text = "Current Moisture (Water Percent):";
            // 
            // tempBox
            // 
            this.tempBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tempBox.Location = new System.Drawing.Point(260, 119);
            this.tempBox.Name = "tempBox";
            this.tempBox.ReadOnly = true;
            this.tempBox.Size = new System.Drawing.Size(100, 31);
            this.tempBox.TabIndex = 6;
            // 
            // humBox
            // 
            this.humBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.humBox.Location = new System.Drawing.Point(260, 178);
            this.humBox.Name = "humBox";
            this.humBox.ReadOnly = true;
            this.humBox.Size = new System.Drawing.Size(100, 31);
            this.humBox.TabIndex = 7;
            // 
            // lightBox
            // 
            this.lightBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lightBox.Location = new System.Drawing.Point(260, 227);
            this.lightBox.Name = "lightBox";
            this.lightBox.ReadOnly = true;
            this.lightBox.Size = new System.Drawing.Size(100, 31);
            this.lightBox.TabIndex = 8;
            // 
            // moisBox
            // 
            this.moisBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.moisBox.Location = new System.Drawing.Point(384, 277);
            this.moisBox.Name = "moisBox";
            this.moisBox.ReadOnly = true;
            this.moisBox.Size = new System.Drawing.Size(100, 31);
            this.moisBox.TabIndex = 9;
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(1247, 718);
            this.Controls.Add(this.moisBox);
            this.Controls.Add(this.lightBox);
            this.Controls.Add(this.humBox);
            this.Controls.Add(this.tempBox);
            this.Controls.Add(this.moisLbl);
            this.Controls.Add(this.lightLbl);
            this.Controls.Add(this.humLbl);
            this.Controls.Add(this.TempLbl);
            this.Controls.Add(this.chartData);
            this.Controls.Add(this.listMsgs);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listMsgs;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartData;
        private System.Windows.Forms.Label TempLbl;
        private System.Windows.Forms.Label humLbl;
        private System.Windows.Forms.Label lightLbl;
        private System.Windows.Forms.Label moisLbl;
        private System.Windows.Forms.TextBox tempBox;
        private System.Windows.Forms.TextBox humBox;
        private System.Windows.Forms.TextBox lightBox;
        private System.Windows.Forms.TextBox moisBox;
    }
}

