
namespace InterfazEnvios
{
    partial class frmInformacion
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabRutas = new System.Windows.Forms.TabPage();
            this.dtgRutas = new System.Windows.Forms.DataGridView();
            this.tabArchivos = new System.Windows.Forms.TabPage();
            this.dtgArchivos = new System.Windows.Forms.DataGridView();
            this.btnReRapida = new System.Windows.Forms.Button();
            this.btnSalir = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabRutas.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgRutas)).BeginInit();
            this.tabArchivos.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgArchivos)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(12, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(889, 423);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Parametros Generales de Operacion";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabRutas);
            this.tabControl1.Controls.Add(this.tabArchivos);
            this.tabControl1.Location = new System.Drawing.Point(20, 39);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(854, 378);
            this.tabControl1.TabIndex = 0;
            // 
            // tabRutas
            // 
            this.tabRutas.Controls.Add(this.dtgRutas);
            this.tabRutas.Location = new System.Drawing.Point(4, 29);
            this.tabRutas.Name = "tabRutas";
            this.tabRutas.Padding = new System.Windows.Forms.Padding(3);
            this.tabRutas.Size = new System.Drawing.Size(846, 345);
            this.tabRutas.TabIndex = 0;
            this.tabRutas.Text = "Rutas Carpetas de Trabajo";
            this.tabRutas.UseVisualStyleBackColor = true;
            // 
            // dtgRutas
            // 
            this.dtgRutas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgRutas.Location = new System.Drawing.Point(6, 6);
            this.dtgRutas.Name = "dtgRutas";
            this.dtgRutas.RowHeadersWidth = 62;
            this.dtgRutas.RowTemplate.Height = 28;
            this.dtgRutas.Size = new System.Drawing.Size(834, 282);
            this.dtgRutas.TabIndex = 0;
            // 
            // tabArchivos
            // 
            this.tabArchivos.Controls.Add(this.dtgArchivos);
            this.tabArchivos.Location = new System.Drawing.Point(4, 29);
            this.tabArchivos.Name = "tabArchivos";
            this.tabArchivos.Padding = new System.Windows.Forms.Padding(3);
            this.tabArchivos.Size = new System.Drawing.Size(846, 345);
            this.tabArchivos.TabIndex = 1;
            this.tabArchivos.Text = "Archivos a la Fecha";
            this.tabArchivos.UseVisualStyleBackColor = true;
            // 
            // dtgArchivos
            // 
            this.dtgArchivos.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgArchivos.Location = new System.Drawing.Point(6, 6);
            this.dtgArchivos.Name = "dtgArchivos";
            this.dtgArchivos.RowHeadersWidth = 62;
            this.dtgArchivos.RowTemplate.Height = 28;
            this.dtgArchivos.Size = new System.Drawing.Size(834, 333);
            this.dtgArchivos.TabIndex = 1;
            // 
            // btnReRapida
            // 
            this.btnReRapida.Location = new System.Drawing.Point(578, 465);
            this.btnReRapida.Name = "btnReRapida";
            this.btnReRapida.Size = new System.Drawing.Size(153, 34);
            this.btnReRapida.TabIndex = 1;
            this.btnReRapida.Text = "Referencia Rapida";
            this.btnReRapida.UseVisualStyleBackColor = true;
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(748, 465);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(153, 34);
            this.btnSalir.TabIndex = 1;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // frmInformacion
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 511);
            this.Controls.Add(this.btnSalir);
            this.Controls.Add(this.btnReRapida);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmInformacion";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmInformacion";
            this.Load += new System.EventHandler(this.frmInformacion_Load);
            this.groupBox1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabRutas.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgRutas)).EndInit();
            this.tabArchivos.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dtgArchivos)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRutas;
        private System.Windows.Forms.DataGridView dtgRutas;
        private System.Windows.Forms.TabPage tabArchivos;
        private System.Windows.Forms.Button btnReRapida;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.DataGridView dtgArchivos;
    }
}