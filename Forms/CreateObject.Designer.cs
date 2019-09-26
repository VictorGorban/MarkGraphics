namespace Game_Consoles
{
    partial class CreateObject
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
            this.AddObject = new System.Windows.Forms.Button();
            this.CoordinatesPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.ScaleZ = new System.Windows.Forms.NumericUpDown();
            this.ScaleY = new System.Windows.Forms.NumericUpDown();
            this.PositionZ = new System.Windows.Forms.NumericUpDown();
            this.ScaleX = new System.Windows.Forms.NumericUpDown();
            this.PositionY = new System.Windows.Forms.NumericUpDown();
            this.ScaleLabel = new System.Windows.Forms.Label();
            this.PositionX = new System.Windows.Forms.NumericUpDown();
            this.ZLabel = new System.Windows.Forms.Label();
            this.YLabel = new System.Windows.Forms.Label();
            this.XLabel = new System.Windows.Forms.Label();
            this.PositionLabel = new System.Windows.Forms.Label();
            this.RotateZ = new System.Windows.Forms.NumericUpDown();
            this.RotateY = new System.Windows.Forms.NumericUpDown();
            this.RotateX = new System.Windows.Forms.NumericUpDown();
            this.RotateLabel = new System.Windows.Forms.Label();
            this.ObjectNameLabel = new System.Windows.Forms.Label();
            this.ObjectName = new System.Windows.Forms.TextBox();
            this.CoordinatesPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).BeginInit();
            this.SuspendLayout();
            // 
            // AddObject
            // 
            this.AddObject.Location = new System.Drawing.Point(6, 195);
            this.AddObject.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.AddObject.Name = "AddObject";
            this.AddObject.Size = new System.Drawing.Size(349, 28);
            this.AddObject.TabIndex = 65;
            this.AddObject.Text = "Добавить";
            this.AddObject.UseVisualStyleBackColor = true;
            this.AddObject.Click += new System.EventHandler(this.AddObject_Click);
            // 
            // CoordinatesPanel
            // 
            this.CoordinatesPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CoordinatesPanel.Controls.Add(this.label1);
            this.CoordinatesPanel.Controls.Add(this.ScaleZ);
            this.CoordinatesPanel.Controls.Add(this.ScaleY);
            this.CoordinatesPanel.Controls.Add(this.PositionZ);
            this.CoordinatesPanel.Controls.Add(this.ScaleX);
            this.CoordinatesPanel.Controls.Add(this.PositionY);
            this.CoordinatesPanel.Controls.Add(this.ScaleLabel);
            this.CoordinatesPanel.Controls.Add(this.PositionX);
            this.CoordinatesPanel.Controls.Add(this.ZLabel);
            this.CoordinatesPanel.Controls.Add(this.YLabel);
            this.CoordinatesPanel.Controls.Add(this.XLabel);
            this.CoordinatesPanel.Controls.Add(this.PositionLabel);
            this.CoordinatesPanel.Controls.Add(this.RotateZ);
            this.CoordinatesPanel.Controls.Add(this.RotateY);
            this.CoordinatesPanel.Controls.Add(this.RotateX);
            this.CoordinatesPanel.Controls.Add(this.RotateLabel);
            this.CoordinatesPanel.Location = new System.Drawing.Point(9, 14);
            this.CoordinatesPanel.Margin = new System.Windows.Forms.Padding(0, 5, 0, 0);
            this.CoordinatesPanel.Name = "CoordinatesPanel";
            this.CoordinatesPanel.Padding = new System.Windows.Forms.Padding(5);
            this.CoordinatesPanel.Size = new System.Drawing.Size(346, 147);
            this.CoordinatesPanel.TabIndex = 66;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Location = new System.Drawing.Point(76, 4);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(201, 23);
            this.label1.TabIndex = 69;
            this.label1.Text = "Положение в пространстве";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ScaleZ
            // 
            this.ScaleZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleZ.DecimalPlaces = 1;
            this.ScaleZ.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleZ.Location = new System.Drawing.Point(265, 111);
            this.ScaleZ.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleZ.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleZ.Name = "ScaleZ";
            this.ScaleZ.Size = new System.Drawing.Size(70, 22);
            this.ScaleZ.TabIndex = 62;
            this.ScaleZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleZ.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ScaleY
            // 
            this.ScaleY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleY.DecimalPlaces = 1;
            this.ScaleY.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleY.Location = new System.Drawing.Point(189, 111);
            this.ScaleY.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleY.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleY.Name = "ScaleY";
            this.ScaleY.Size = new System.Drawing.Size(70, 22);
            this.ScaleY.TabIndex = 61;
            this.ScaleY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleY.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PositionZ
            // 
            this.PositionZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionZ.DecimalPlaces = 1;
            this.PositionZ.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionZ.Location = new System.Drawing.Point(265, 53);
            this.PositionZ.Name = "PositionZ";
            this.PositionZ.Size = new System.Drawing.Size(70, 22);
            this.PositionZ.TabIndex = 45;
            this.PositionZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ScaleX
            // 
            this.ScaleX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleX.DecimalPlaces = 1;
            this.ScaleX.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleX.Location = new System.Drawing.Point(113, 111);
            this.ScaleX.Maximum = new decimal(new int[] {
            100,
            0,
            0,
            65536});
            this.ScaleX.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.ScaleX.Name = "ScaleX";
            this.ScaleX.Size = new System.Drawing.Size(70, 22);
            this.ScaleX.TabIndex = 46;
            this.ScaleX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ScaleX.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // PositionY
            // 
            this.PositionY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionY.DecimalPlaces = 1;
            this.PositionY.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionY.Location = new System.Drawing.Point(189, 53);
            this.PositionY.Name = "PositionY";
            this.PositionY.Size = new System.Drawing.Size(70, 22);
            this.PositionY.TabIndex = 44;
            this.PositionY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ScaleLabel
            // 
            this.ScaleLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ScaleLabel.Location = new System.Drawing.Point(30, 117);
            this.ScaleLabel.Name = "ScaleLabel";
            this.ScaleLabel.Size = new System.Drawing.Size(70, 23);
            this.ScaleLabel.TabIndex = 60;
            this.ScaleLabel.Text = "Масштаб";
            this.ScaleLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // PositionX
            // 
            this.PositionX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionX.DecimalPlaces = 1;
            this.PositionX.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.PositionX.Location = new System.Drawing.Point(113, 53);
            this.PositionX.Name = "PositionX";
            this.PositionX.Size = new System.Drawing.Size(70, 22);
            this.PositionX.TabIndex = 43;
            this.PositionX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // ZLabel
            // 
            this.ZLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.ZLabel.ForeColor = System.Drawing.Color.Black;
            this.ZLabel.Location = new System.Drawing.Point(265, 27);
            this.ZLabel.Name = "ZLabel";
            this.ZLabel.Size = new System.Drawing.Size(70, 23);
            this.ZLabel.TabIndex = 42;
            this.ZLabel.Text = "Z";
            this.ZLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // YLabel
            // 
            this.YLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.YLabel.ForeColor = System.Drawing.Color.Black;
            this.YLabel.Location = new System.Drawing.Point(189, 27);
            this.YLabel.Name = "YLabel";
            this.YLabel.Size = new System.Drawing.Size(70, 23);
            this.YLabel.TabIndex = 41;
            this.YLabel.Text = "Y";
            this.YLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // XLabel
            // 
            this.XLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.XLabel.ForeColor = System.Drawing.Color.Black;
            this.XLabel.Location = new System.Drawing.Point(113, 27);
            this.XLabel.Name = "XLabel";
            this.XLabel.Size = new System.Drawing.Size(70, 23);
            this.XLabel.TabIndex = 40;
            this.XLabel.Text = "X";
            this.XLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // PositionLabel
            // 
            this.PositionLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.PositionLabel.Location = new System.Drawing.Point(2, 52);
            this.PositionLabel.Name = "PositionLabel";
            this.PositionLabel.Size = new System.Drawing.Size(98, 23);
            this.PositionLabel.TabIndex = 30;
            this.PositionLabel.Text = "Координаты";
            this.PositionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // RotateZ
            // 
            this.RotateZ.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RotateZ.Location = new System.Drawing.Point(265, 82);
            this.RotateZ.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateZ.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateZ.Name = "RotateZ";
            this.RotateZ.Size = new System.Drawing.Size(70, 22);
            this.RotateZ.TabIndex = 27;
            this.RotateZ.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateY
            // 
            this.RotateY.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RotateY.Location = new System.Drawing.Point(189, 82);
            this.RotateY.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateY.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateY.Name = "RotateY";
            this.RotateY.Size = new System.Drawing.Size(70, 22);
            this.RotateY.TabIndex = 25;
            this.RotateY.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateX
            // 
            this.RotateX.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RotateX.Location = new System.Drawing.Point(113, 82);
            this.RotateX.Maximum = new decimal(new int[] {
            180,
            0,
            0,
            0});
            this.RotateX.Minimum = new decimal(new int[] {
            180,
            0,
            0,
            -2147483648});
            this.RotateX.Name = "RotateX";
            this.RotateX.Size = new System.Drawing.Size(70, 22);
            this.RotateX.TabIndex = 24;
            this.RotateX.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // RotateLabel
            // 
            this.RotateLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.RotateLabel.Location = new System.Drawing.Point(30, 81);
            this.RotateLabel.Name = "RotateLabel";
            this.RotateLabel.Size = new System.Drawing.Size(70, 23);
            this.RotateLabel.TabIndex = 24;
            this.RotateLabel.Text = "Rotate:";
            this.RotateLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectNameLabel
            // 
            this.ObjectNameLabel.Location = new System.Drawing.Point(12, 164);
            this.ObjectNameLabel.Name = "ObjectNameLabel";
            this.ObjectNameLabel.Size = new System.Drawing.Size(81, 23);
            this.ObjectNameLabel.TabIndex = 67;
            this.ObjectNameLabel.Text = "Название";
            this.ObjectNameLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // ObjectName
            // 
            this.ObjectName.Location = new System.Drawing.Point(96, 167);
            this.ObjectName.Name = "ObjectName";
            this.ObjectName.Size = new System.Drawing.Size(259, 22);
            this.ObjectName.TabIndex = 68;
            this.ObjectName.TextChanged += new System.EventHandler(this.ObjectName_TextChanged);
            // 
            // CreateObject
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(365, 235);
            this.Controls.Add(this.ObjectNameLabel);
            this.Controls.Add(this.ObjectName);
            this.Controls.Add(this.CoordinatesPanel);
            this.Controls.Add(this.AddObject);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "CreateObject";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Создание объекта";
            this.CoordinatesPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ScaleZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ScaleX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PositionX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateZ)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RotateX)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button AddObject;
        private System.Windows.Forms.Panel CoordinatesPanel;
        private System.Windows.Forms.NumericUpDown ScaleZ;
        private System.Windows.Forms.NumericUpDown ScaleY;
        private System.Windows.Forms.NumericUpDown PositionZ;
        private System.Windows.Forms.NumericUpDown ScaleX;
        private System.Windows.Forms.NumericUpDown PositionY;
        private System.Windows.Forms.Label ScaleLabel;
        private System.Windows.Forms.NumericUpDown PositionX;
        private System.Windows.Forms.Label ZLabel;
        private System.Windows.Forms.Label YLabel;
        private System.Windows.Forms.Label XLabel;
        private System.Windows.Forms.Label PositionLabel;
        private System.Windows.Forms.NumericUpDown RotateZ;
        private System.Windows.Forms.NumericUpDown RotateY;
        private System.Windows.Forms.NumericUpDown RotateX;
        private System.Windows.Forms.Label RotateLabel;
        private System.Windows.Forms.Label ObjectNameLabel;
        private System.Windows.Forms.TextBox ObjectName;
        private System.Windows.Forms.Label label1;
    }
}