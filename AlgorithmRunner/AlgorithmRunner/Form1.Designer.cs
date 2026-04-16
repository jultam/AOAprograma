namespace AlgorithmRunner
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle17 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle18 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle19 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle20 = new System.Windows.Forms.DataGridViewCellStyle();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.start = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.benchmark_function = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dimensions = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.knownOptimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.reachedOptimum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TimeElapsed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.download = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.alphaUpDown = new System.Windows.Forms.NumericUpDown();
            this.alphaLabel = new System.Windows.Forms.Label();
            this.muLabel = new System.Windows.Forms.Label();
            this.muUpDown = new System.Windows.Forms.NumericUpDown();
            this.epsilonLabel = new System.Windows.Forms.Label();
            this.epsilonUpDown = new System.Windows.Forms.NumericUpDown();
            this.PSUpDown = new System.Windows.Forms.NumericUpDown();
            this.MIterUpDown = new System.Windows.Forms.NumericUpDown();
            this.PSLabel = new System.Windows.Forms.Label();
            this.MIterLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.testUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBox3 = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBox4 = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.muUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.epsilonUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PSUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.MIterUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.testUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(160)))), ((int)(((byte)(237)))));
            this.button1.FlatAppearance.BorderColor = System.Drawing.SystemColors.Highlight;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.ForeColor = System.Drawing.SystemColors.Control;
            this.button1.Location = new System.Drawing.Point(25, 43);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(521, 55);
            this.button1.TabIndex = 0;
            this.button1.Text = "LOAD PARAMETERS";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(26, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(248, 18);
            this.label1.TabIndex = 1;
            this.label1.Text = "Want to load parameters from a file?";
            // 
            // start
            // 
            this.start.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(1)))), ((int)(((byte)(182)))), ((int)(((byte)(54)))));
            this.start.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.start.ForeColor = System.Drawing.SystemColors.Control;
            this.start.Location = new System.Drawing.Point(25, 747);
            this.start.Name = "start";
            this.start.Size = new System.Drawing.Size(521, 70);
            this.start.TabIndex = 3;
            this.start.Text = "START";
            this.start.UseVisualStyleBackColor = false;
            this.start.Click += new System.EventHandler(this.start_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.benchmark_function,
            this.dimensions,
            this.knownOptimum,
            this.reachedOptimum,
            this.TimeElapsed});
            this.dataGridView1.Location = new System.Drawing.Point(585, 118);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RowHeadersWidth = 51;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(901, 699);
            this.dataGridView1.TabIndex = 4;
            // 
            // ID
            // 
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ID.DefaultCellStyle = dataGridViewCellStyle16;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.Width = 35;
            // 
            // benchmark_function
            // 
            this.benchmark_function.HeaderText = "Benchmark Function";
            this.benchmark_function.MinimumWidth = 6;
            this.benchmark_function.Name = "benchmark_function";
            this.benchmark_function.Width = 125;
            // 
            // dimensions
            // 
            dataGridViewCellStyle17.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.dimensions.DefaultCellStyle = dataGridViewCellStyle17;
            this.dimensions.HeaderText = "Dimensions";
            this.dimensions.MinimumWidth = 6;
            this.dimensions.Name = "dimensions";
            this.dimensions.Width = 75;
            // 
            // knownOptimum
            // 
            dataGridViewCellStyle18.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle18.Format = "N6";
            dataGridViewCellStyle18.NullValue = null;
            this.knownOptimum.DefaultCellStyle = dataGridViewCellStyle18;
            this.knownOptimum.HeaderText = "Known Optimum";
            this.knownOptimum.MinimumWidth = 6;
            this.knownOptimum.Name = "knownOptimum";
            this.knownOptimum.Width = 125;
            // 
            // reachedOptimum
            // 
            dataGridViewCellStyle19.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle19.Format = "N6";
            dataGridViewCellStyle19.NullValue = null;
            this.reachedOptimum.DefaultCellStyle = dataGridViewCellStyle19;
            this.reachedOptimum.HeaderText = "Reached Optimum";
            this.reachedOptimum.MinimumWidth = 6;
            this.reachedOptimum.Name = "reachedOptimum";
            this.reachedOptimum.Width = 125;
            // 
            // TimeElapsed
            // 
            this.TimeElapsed.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            dataGridViewCellStyle20.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle20.Format = "N3";
            dataGridViewCellStyle20.NullValue = null;
            this.TimeElapsed.DefaultCellStyle = dataGridViewCellStyle20;
            this.TimeElapsed.HeaderText = "Time Elapsed, ms";
            this.TimeElapsed.MinimumWidth = 100;
            this.TimeElapsed.Name = "TimeElapsed";
            // 
            // download
            // 
            this.download.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(160)))), ((int)(((byte)(237)))));
            this.download.Enabled = false;
            this.download.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.download.ForeColor = System.Drawing.SystemColors.Control;
            this.download.Location = new System.Drawing.Point(1170, 43);
            this.download.Name = "download";
            this.download.Size = new System.Drawing.Size(316, 55);
            this.download.TabIndex = 5;
            this.download.Text = "DOWNLOAD";
            this.download.UseVisualStyleBackColor = false;
            this.download.Click += new System.EventHandler(this.download_Click);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox1.Location = new System.Drawing.Point(29, 298);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(517, 28);
            this.comboBox1.TabIndex = 6;
            // 
            // alphaUpDown
            // 
            this.alphaUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaUpDown.Location = new System.Drawing.Point(29, 217);
            this.alphaUpDown.Name = "alphaUpDown";
            this.alphaUpDown.Size = new System.Drawing.Size(141, 27);
            this.alphaUpDown.TabIndex = 7;
            this.alphaUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // alphaLabel
            // 
            this.alphaLabel.AutoSize = true;
            this.alphaLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.alphaLabel.Location = new System.Drawing.Point(25, 194);
            this.alphaLabel.Name = "alphaLabel";
            this.alphaLabel.Size = new System.Drawing.Size(18, 20);
            this.alphaLabel.TabIndex = 8;
            this.alphaLabel.Text = "α";
            // 
            // muLabel
            // 
            this.muLabel.AutoSize = true;
            this.muLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.muLabel.Location = new System.Drawing.Point(207, 194);
            this.muLabel.Name = "muLabel";
            this.muLabel.Size = new System.Drawing.Size(18, 20);
            this.muLabel.TabIndex = 10;
            this.muLabel.Text = "μ";
            // 
            // muUpDown
            // 
            this.muUpDown.DecimalPlaces = 1;
            this.muUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.muUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.muUpDown.Location = new System.Drawing.Point(211, 217);
            this.muUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.muUpDown.Name = "muUpDown";
            this.muUpDown.Size = new System.Drawing.Size(147, 27);
            this.muUpDown.TabIndex = 9;
            this.muUpDown.Value = new decimal(new int[] {
            4,
            0,
            0,
            65536});
            // 
            // epsilonLabel
            // 
            this.epsilonLabel.AutoSize = true;
            this.epsilonLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epsilonLabel.Location = new System.Drawing.Point(395, 194);
            this.epsilonLabel.Name = "epsilonLabel";
            this.epsilonLabel.Size = new System.Drawing.Size(18, 20);
            this.epsilonLabel.TabIndex = 12;
            this.epsilonLabel.Text = "ε";
            // 
            // epsilonUpDown
            // 
            this.epsilonUpDown.DecimalPlaces = 2;
            this.epsilonUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.epsilonUpDown.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.epsilonUpDown.Location = new System.Drawing.Point(399, 217);
            this.epsilonUpDown.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.epsilonUpDown.Name = "epsilonUpDown";
            this.epsilonUpDown.Size = new System.Drawing.Size(147, 27);
            this.epsilonUpDown.TabIndex = 11;
            this.epsilonUpDown.Value = new decimal(new int[] {
            5,
            0,
            0,
            131072});
            // 
            // PSUpDown
            // 
            this.PSUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PSUpDown.Location = new System.Drawing.Point(29, 141);
            this.PSUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.PSUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.PSUpDown.Name = "PSUpDown";
            this.PSUpDown.Size = new System.Drawing.Size(141, 27);
            this.PSUpDown.TabIndex = 13;
            this.PSUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // MIterUpDown
            // 
            this.MIterUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIterUpDown.Location = new System.Drawing.Point(211, 141);
            this.MIterUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.MIterUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.MIterUpDown.Name = "MIterUpDown";
            this.MIterUpDown.Size = new System.Drawing.Size(147, 27);
            this.MIterUpDown.TabIndex = 14;
            this.MIterUpDown.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // PSLabel
            // 
            this.PSLabel.AutoSize = true;
            this.PSLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PSLabel.Location = new System.Drawing.Point(25, 118);
            this.PSLabel.Name = "PSLabel";
            this.PSLabel.Size = new System.Drawing.Size(123, 20);
            this.PSLabel.TabIndex = 15;
            this.PSLabel.Text = "Population size";
            // 
            // MIterLabel
            // 
            this.MIterLabel.AutoSize = true;
            this.MIterLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MIterLabel.Location = new System.Drawing.Point(207, 118);
            this.MIterLabel.Name = "MIterLabel";
            this.MIterLabel.Size = new System.Drawing.Size(155, 20);
            this.MIterLabel.TabIndex = 16;
            this.MIterLabel.Text = "Maximum iterations";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(25, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 20);
            this.label2.TabIndex = 17;
            this.label2.Text = "Number generator:\r\n";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(395, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 20);
            this.label3.TabIndex = 19;
            this.label3.Text = "Amount of runs";
            // 
            // testUpDown
            // 
            this.testUpDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testUpDown.Location = new System.Drawing.Point(399, 141);
            this.testUpDown.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.testUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testUpDown.Name = "testUpDown";
            this.testUpDown.Size = new System.Drawing.Size(147, 27);
            this.testUpDown.TabIndex = 18;
            this.testUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(25, 354);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(127, 20);
            this.label4.TabIndex = 21;
            this.label4.Text = "MOA and MOP:\r\n";
            // 
            // comboBox2
            // 
            this.comboBox2.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox2.Location = new System.Drawing.Point(29, 377);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.Size = new System.Drawing.Size(517, 28);
            this.comboBox2.TabIndex = 20;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(25, 435);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(167, 20);
            this.label5.TabIndex = 23;
            this.label5.Text = "Solution initialization:\r\n";
            // 
            // comboBox3
            // 
            this.comboBox3.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox3.FormattingEnabled = true;
            this.comboBox3.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox3.Location = new System.Drawing.Point(29, 458);
            this.comboBox3.Name = "comboBox3";
            this.comboBox3.Size = new System.Drawing.Size(517, 28);
            this.comboBox3.TabIndex = 22;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(25, 515);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(133, 20);
            this.label6.TabIndex = 25;
            this.label6.Text = "Step calculation:\r\n";
            // 
            // comboBox4
            // 
            this.comboBox4.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.comboBox4.FormattingEnabled = true;
            this.comboBox4.Items.AddRange(new object[] {
            "test",
            "test2"});
            this.comboBox4.Location = new System.Drawing.Point(29, 538);
            this.comboBox4.Name = "comboBox4";
            this.comboBox4.Size = new System.Drawing.Size(517, 28);
            this.comboBox4.TabIndex = 24;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1516, 844);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBox4);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.testUpDown);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.MIterLabel);
            this.Controls.Add(this.PSLabel);
            this.Controls.Add(this.MIterUpDown);
            this.Controls.Add(this.PSUpDown);
            this.Controls.Add(this.epsilonLabel);
            this.Controls.Add(this.epsilonUpDown);
            this.Controls.Add(this.muLabel);
            this.Controls.Add(this.muUpDown);
            this.Controls.Add(this.alphaLabel);
            this.Controls.Add(this.alphaUpDown);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.download);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.start);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Arithmetic Optimization Algorithm Controller";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.alphaUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.muUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.epsilonUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PSUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.MIterUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.testUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button start;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button download;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.NumericUpDown alphaUpDown;
        private System.Windows.Forms.Label alphaLabel;
        private System.Windows.Forms.Label muLabel;
        private System.Windows.Forms.NumericUpDown muUpDown;
        private System.Windows.Forms.Label epsilonLabel;
        private System.Windows.Forms.NumericUpDown epsilonUpDown;
        private System.Windows.Forms.NumericUpDown PSUpDown;
        private System.Windows.Forms.NumericUpDown MIterUpDown;
        private System.Windows.Forms.Label PSLabel;
        private System.Windows.Forms.Label MIterLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown testUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBox4;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn benchmark_function;
        private System.Windows.Forms.DataGridViewTextBoxColumn dimensions;
        private System.Windows.Forms.DataGridViewTextBoxColumn knownOptimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn reachedOptimum;
        private System.Windows.Forms.DataGridViewTextBoxColumn TimeElapsed;
    }
}

