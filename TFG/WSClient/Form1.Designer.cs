namespace WSClient
{
    partial class Form1
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.trackBar1 = new System.Windows.Forms.TrackBar();
            this.tbConsumo = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.LBUsuarios = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.LBUbicaciones = new System.Windows.Forms.ListBox();
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).BeginInit();
            this.SuspendLayout();
            // 
            // trackBar1
            // 
            this.trackBar1.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.trackBar1.Location = new System.Drawing.Point(100, 214);
            this.trackBar1.Maximum = 1000;
            this.trackBar1.Name = "trackBar1";
            this.trackBar1.Size = new System.Drawing.Size(579, 45);
            this.trackBar1.TabIndex = 2;
            this.trackBar1.TickFrequency = 10;
            this.trackBar1.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // tbConsumo
            // 
            this.tbConsumo.Location = new System.Drawing.Point(204, 300);
            this.tbConsumo.Name = "tbConsumo";
            this.tbConsumo.Size = new System.Drawing.Size(100, 20);
            this.tbConsumo.TabIndex = 3;
            this.tbConsumo.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(488, 297);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(105, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = " CambiarConsumo";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // LBUsuarios
            // 
            this.LBUsuarios.FormattingEnabled = true;
            this.LBUsuarios.Location = new System.Drawing.Point(184, 84);
            this.LBUsuarios.Name = "LBUsuarios";
            this.LBUsuarios.Size = new System.Drawing.Size(120, 95);
            this.LBUsuarios.TabIndex = 5;
            this.LBUsuarios.SelectedIndexChanged += new System.EventHandler(this.LBUsuarios_SelectedIndexChanged);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 32);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(111, 23);
            this.button2.TabIndex = 6;
            this.button2.Text = "ActualizarUsuarios";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(498, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Ubicaciones";
            // 
            // LBUbicaciones
            // 
            this.LBUbicaciones.FormattingEnabled = true;
            this.LBUbicaciones.Location = new System.Drawing.Point(473, 84);
            this.LBUbicaciones.Name = "LBUbicaciones";
            this.LBUbicaciones.Size = new System.Drawing.Size(120, 95);
            this.LBUbicaciones.TabIndex = 8;
            this.LBUbicaciones.SelectedIndexChanged += new System.EventHandler(this.LBUbicaciones_SelectedIndexChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LBUbicaciones);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.LBUsuarios);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tbConsumo);
            this.Controls.Add(this.trackBar1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.trackBar1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TrackBar trackBar1;
        private System.Windows.Forms.TextBox tbConsumo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox LBUsuarios;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListBox LBUbicaciones;
    }
}

