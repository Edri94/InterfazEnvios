
namespace InterfazEnvios
{
    partial class frmParametros
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbTicket = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBic = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtOrigenDestino = new System.Windows.Forms.TextBox();
            this.txtBicOrigen = new System.Windows.Forms.TextBox();
            this.txtPswdUsr = new System.Windows.Forms.TextBox();
            this.txtLoginUsr = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombreServidor = new System.Windows.Forms.TextBox();
            this.tbAs400 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBibliotecaEnv = new System.Windows.Forms.TextBox();
            this.txtBibliotecaSaldos = new System.Windows.Forms.TextBox();
            this.txtBibliotecaEq = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.txtNombreSna = new System.Windows.Forms.TextBox();
            this.tbMqSeries = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtQueReprt = new System.Windows.Forms.TextBox();
            this.txtQueRecib = new System.Windows.Forms.TextBox();
            this.txtQueEnv = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.txtQueManager = new System.Windows.Forms.TextBox();
            this.tbInfo = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.chkEnvMsj = new System.Windows.Forms.CheckBox();
            this.chkEnvArchivos = new System.Windows.Forms.CheckBox();
            this.dtgvInfo = new System.Windows.Forms.DataGridView();
            this.tbPassword = new System.Windows.Forms.TabPage();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtConfirmPswd = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.txtNwPswd = new System.Windows.Forms.TextBox();
            this.txtPswdAct = new System.Windows.Forms.TextBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnSalir = new System.Windows.Forms.Button();
            this.btnCambiar = new System.Windows.Forms.Button();
            this.btnTest = new System.Windows.Forms.Button();
            this.btnGuardar = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tbTicket.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tbAs400.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbMqSeries.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tbInfo.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInfo)).BeginInit();
            this.tbPassword.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbTicket);
            this.tabControl1.Controls.Add(this.tbAs400);
            this.tabControl1.Controls.Add(this.tbMqSeries);
            this.tabControl1.Controls.Add(this.tbInfo);
            this.tabControl1.Controls.Add(this.tbPassword);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(938, 460);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // tbTicket
            // 
            this.tbTicket.Controls.Add(this.groupBox1);
            this.tbTicket.Location = new System.Drawing.Point(4, 29);
            this.tbTicket.Name = "tbTicket";
            this.tbTicket.Padding = new System.Windows.Forms.Padding(3);
            this.tbTicket.Size = new System.Drawing.Size(930, 427);
            this.tbTicket.TabIndex = 0;
            this.tbTicket.Text = "Ticket";
            this.tbTicket.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.cmbBic);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtOrigenDestino);
            this.groupBox1.Controls.Add(this.txtBicOrigen);
            this.groupBox1.Controls.Add(this.txtPswdUsr);
            this.groupBox1.Controls.Add(this.txtLoginUsr);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.txtNombreServidor);
            this.groupBox1.Location = new System.Drawing.Point(17, 21);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(886, 384);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos Conexion";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(467, 217);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(99, 20);
            this.label9.TabIndex = 3;
            this.label9.Text = "BIC Destino:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(463, 153);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(91, 20);
            this.label8.TabIndex = 3;
            this.label8.Text = "BIC Origen:";
            // 
            // cmbBic
            // 
            this.cmbBic.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBic.FormattingEnabled = true;
            this.cmbBic.Location = new System.Drawing.Point(573, 84);
            this.cmbBic.Name = "cmbBic";
            this.cmbBic.Size = new System.Drawing.Size(284, 28);
            this.cmbBic.TabIndex = 2;
            this.cmbBic.SelectedIndexChanged += new System.EventHandler(this.cmbBic_SelectedIndexChanged_1);
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(463, 86);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(44, 25);
            this.label7.TabIndex = 1;
            this.label7.Text = "BIC:";
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(26, 205);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(104, 45);
            this.label6.TabIndex = 1;
            this.label6.Text = "Password de Usuario:";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(26, 141);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 45);
            this.label2.TabIndex = 1;
            this.label2.Text = "Login de Usuario:";
            // 
            // txtOrigenDestino
            // 
            this.txtOrigenDestino.Location = new System.Drawing.Point(573, 214);
            this.txtOrigenDestino.Name = "txtOrigenDestino";
            this.txtOrigenDestino.ReadOnly = true;
            this.txtOrigenDestino.Size = new System.Drawing.Size(284, 26);
            this.txtOrigenDestino.TabIndex = 0;
            // 
            // txtBicOrigen
            // 
            this.txtBicOrigen.Location = new System.Drawing.Point(573, 150);
            this.txtBicOrigen.Name = "txtBicOrigen";
            this.txtBicOrigen.ReadOnly = true;
            this.txtBicOrigen.Size = new System.Drawing.Size(284, 26);
            this.txtBicOrigen.TabIndex = 0;
            // 
            // txtPswdUsr
            // 
            this.txtPswdUsr.Location = new System.Drawing.Point(136, 214);
            this.txtPswdUsr.Name = "txtPswdUsr";
            this.txtPswdUsr.ReadOnly = true;
            this.txtPswdUsr.Size = new System.Drawing.Size(284, 26);
            this.txtPswdUsr.TabIndex = 0;
            // 
            // txtLoginUsr
            // 
            this.txtLoginUsr.Location = new System.Drawing.Point(136, 150);
            this.txtLoginUsr.Name = "txtLoginUsr";
            this.txtLoginUsr.ReadOnly = true;
            this.txtLoginUsr.Size = new System.Drawing.Size(284, 26);
            this.txtLoginUsr.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(26, 76);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 45);
            this.label1.TabIndex = 1;
            this.label1.Text = "Nombre del Servidor:";
            // 
            // txtNombreServidor
            // 
            this.txtNombreServidor.Location = new System.Drawing.Point(136, 85);
            this.txtNombreServidor.Name = "txtNombreServidor";
            this.txtNombreServidor.ReadOnly = true;
            this.txtNombreServidor.Size = new System.Drawing.Size(284, 26);
            this.txtNombreServidor.TabIndex = 0;
            // 
            // tbAs400
            // 
            this.tbAs400.Controls.Add(this.groupBox2);
            this.tbAs400.Location = new System.Drawing.Point(4, 29);
            this.tbAs400.Name = "tbAs400";
            this.tbAs400.Padding = new System.Windows.Forms.Padding(3);
            this.tbAs400.Size = new System.Drawing.Size(930, 427);
            this.tbAs400.TabIndex = 1;
            this.tbAs400.Text = "AS/400";
            this.tbAs400.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label13);
            this.groupBox2.Controls.Add(this.label14);
            this.groupBox2.Controls.Add(this.txtBibliotecaEnv);
            this.groupBox2.Controls.Add(this.txtBibliotecaSaldos);
            this.groupBox2.Controls.Add(this.txtBibliotecaEq);
            this.groupBox2.Controls.Add(this.label18);
            this.groupBox2.Controls.Add(this.txtNombreSna);
            this.groupBox2.Location = new System.Drawing.Point(22, 21);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(886, 384);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos Conexion";
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(26, 270);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(99, 47);
            this.label10.TabIndex = 3;
            this.label10.Text = "Biblioteca de Envios:";
            // 
            // label13
            // 
            this.label13.Location = new System.Drawing.Point(26, 196);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(104, 45);
            this.label13.TabIndex = 1;
            this.label13.Text = "Biblioteca de Saldos:";
            // 
            // label14
            // 
            this.label14.Location = new System.Drawing.Point(26, 131);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(104, 45);
            this.label14.TabIndex = 1;
            this.label14.Text = "Biblioteca Equation:";
            // 
            // txtBibliotecaEnv
            // 
            this.txtBibliotecaEnv.Location = new System.Drawing.Point(136, 280);
            this.txtBibliotecaEnv.Name = "txtBibliotecaEnv";
            this.txtBibliotecaEnv.Size = new System.Drawing.Size(284, 26);
            this.txtBibliotecaEnv.TabIndex = 0;
            // 
            // txtBibliotecaSaldos
            // 
            this.txtBibliotecaSaldos.Location = new System.Drawing.Point(136, 205);
            this.txtBibliotecaSaldos.Name = "txtBibliotecaSaldos";
            this.txtBibliotecaSaldos.Size = new System.Drawing.Size(284, 26);
            this.txtBibliotecaSaldos.TabIndex = 0;
            // 
            // txtBibliotecaEq
            // 
            this.txtBibliotecaEq.Location = new System.Drawing.Point(136, 140);
            this.txtBibliotecaEq.Name = "txtBibliotecaEq";
            this.txtBibliotecaEq.Size = new System.Drawing.Size(284, 26);
            this.txtBibliotecaEq.TabIndex = 0;
            // 
            // label18
            // 
            this.label18.Location = new System.Drawing.Point(26, 69);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(104, 45);
            this.label18.TabIndex = 1;
            this.label18.Text = "Nombre del SNA:";
            // 
            // txtNombreSna
            // 
            this.txtNombreSna.Location = new System.Drawing.Point(136, 78);
            this.txtNombreSna.Name = "txtNombreSna";
            this.txtNombreSna.Size = new System.Drawing.Size(284, 26);
            this.txtNombreSna.TabIndex = 0;
            // 
            // tbMqSeries
            // 
            this.tbMqSeries.Controls.Add(this.groupBox3);
            this.tbMqSeries.Location = new System.Drawing.Point(4, 29);
            this.tbMqSeries.Name = "tbMqSeries";
            this.tbMqSeries.Padding = new System.Windows.Forms.Padding(3);
            this.tbMqSeries.Size = new System.Drawing.Size(930, 427);
            this.tbMqSeries.TabIndex = 2;
            this.tbMqSeries.Text = "MQ Series";
            this.tbMqSeries.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label15);
            this.groupBox3.Controls.Add(this.txtQueReprt);
            this.groupBox3.Controls.Add(this.txtQueRecib);
            this.groupBox3.Controls.Add(this.txtQueEnv);
            this.groupBox3.Controls.Add(this.label16);
            this.groupBox3.Controls.Add(this.txtQueManager);
            this.groupBox3.Location = new System.Drawing.Point(22, 21);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(886, 384);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos Conexion";
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(32, 264);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(99, 47);
            this.label11.TabIndex = 3;
            this.label11.Text = "Queue Reporte:";
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(32, 190);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 45);
            this.label12.TabIndex = 1;
            this.label12.Text = "Queue Recibir:";
            // 
            // label15
            // 
            this.label15.Location = new System.Drawing.Point(32, 125);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(104, 45);
            this.label15.TabIndex = 1;
            this.label15.Text = "Queue Enviar:";
            // 
            // txtQueReprt
            // 
            this.txtQueReprt.Location = new System.Drawing.Point(142, 274);
            this.txtQueReprt.Name = "txtQueReprt";
            this.txtQueReprt.Size = new System.Drawing.Size(284, 26);
            this.txtQueReprt.TabIndex = 0;
            this.txtQueReprt.TextChanged += new System.EventHandler(this.txtQueReprt_TextChanged);
            // 
            // txtQueRecib
            // 
            this.txtQueRecib.Location = new System.Drawing.Point(142, 199);
            this.txtQueRecib.Name = "txtQueRecib";
            this.txtQueRecib.Size = new System.Drawing.Size(284, 26);
            this.txtQueRecib.TabIndex = 0;
            this.txtQueRecib.TextChanged += new System.EventHandler(this.txtQueRecib_TextChanged);
            // 
            // txtQueEnv
            // 
            this.txtQueEnv.Location = new System.Drawing.Point(142, 134);
            this.txtQueEnv.Name = "txtQueEnv";
            this.txtQueEnv.Size = new System.Drawing.Size(284, 26);
            this.txtQueEnv.TabIndex = 0;
            this.txtQueEnv.TextChanged += new System.EventHandler(this.txtQueEnv_TextChanged);
            // 
            // label16
            // 
            this.label16.Location = new System.Drawing.Point(32, 64);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(104, 45);
            this.label16.TabIndex = 1;
            this.label16.Text = "Queue Manager:";
            // 
            // txtQueManager
            // 
            this.txtQueManager.Location = new System.Drawing.Point(142, 73);
            this.txtQueManager.Name = "txtQueManager";
            this.txtQueManager.Size = new System.Drawing.Size(284, 26);
            this.txtQueManager.TabIndex = 0;
            this.txtQueManager.TextChanged += new System.EventHandler(this.txtQueManager_TextChanged);
            // 
            // tbInfo
            // 
            this.tbInfo.Controls.Add(this.groupBox4);
            this.tbInfo.Location = new System.Drawing.Point(4, 29);
            this.tbInfo.Name = "tbInfo";
            this.tbInfo.Padding = new System.Windows.Forms.Padding(3);
            this.tbInfo.Size = new System.Drawing.Size(930, 427);
            this.tbInfo.TabIndex = 3;
            this.tbInfo.Text = "Informacion";
            this.tbInfo.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.chkEnvMsj);
            this.groupBox4.Controls.Add(this.chkEnvArchivos);
            this.groupBox4.Controls.Add(this.dtgvInfo);
            this.groupBox4.Location = new System.Drawing.Point(15, 32);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(892, 373);
            this.groupBox4.TabIndex = 0;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Generacion de Informacion";
            // 
            // chkEnvMsj
            // 
            this.chkEnvMsj.AutoSize = true;
            this.chkEnvMsj.Location = new System.Drawing.Point(19, 332);
            this.chkEnvMsj.Name = "chkEnvMsj";
            this.chkEnvMsj.Size = new System.Drawing.Size(418, 24);
            this.chkEnvMsj.TabIndex = 1;
            this.chkEnvMsj.Text = "Enviar mensajes Swift MT103 con slash de verificacion";
            this.chkEnvMsj.UseVisualStyleBackColor = true;
            this.chkEnvMsj.Click += new System.EventHandler(this.chkEnvMsj_Click);
            // 
            // chkEnvArchivos
            // 
            this.chkEnvArchivos.AutoSize = true;
            this.chkEnvArchivos.Location = new System.Drawing.Point(19, 305);
            this.chkEnvArchivos.Name = "chkEnvArchivos";
            this.chkEnvArchivos.Size = new System.Drawing.Size(294, 24);
            this.chkEnvArchivos.TabIndex = 1;
            this.chkEnvArchivos.Text = "Enviar archivos generados al AS/400";
            this.chkEnvArchivos.UseVisualStyleBackColor = true;
            this.chkEnvArchivos.Click += new System.EventHandler(this.chkEnvArchivos_Click);
            // 
            // dtgvInfo
            // 
            this.dtgvInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dtgvInfo.Location = new System.Drawing.Point(19, 37);
            this.dtgvInfo.Name = "dtgvInfo";
            this.dtgvInfo.ReadOnly = true;
            this.dtgvInfo.RowHeadersWidth = 62;
            this.dtgvInfo.RowTemplate.Height = 28;
            this.dtgvInfo.Size = new System.Drawing.Size(844, 251);
            this.dtgvInfo.TabIndex = 0;
            // 
            // tbPassword
            // 
            this.tbPassword.Controls.Add(this.groupBox5);
            this.tbPassword.Location = new System.Drawing.Point(4, 29);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Padding = new System.Windows.Forms.Padding(3);
            this.tbPassword.Size = new System.Drawing.Size(930, 427);
            this.tbPassword.TabIndex = 4;
            this.tbPassword.Text = "Password";
            this.tbPassword.UseVisualStyleBackColor = true;
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.label17);
            this.groupBox5.Controls.Add(this.label19);
            this.groupBox5.Controls.Add(this.txtConfirmPswd);
            this.groupBox5.Controls.Add(this.label20);
            this.groupBox5.Controls.Add(this.txtNwPswd);
            this.groupBox5.Controls.Add(this.txtPswdAct);
            this.groupBox5.Location = new System.Drawing.Point(25, 33);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(879, 350);
            this.groupBox5.TabIndex = 0;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Password de Configuración de la Interfaz";
            // 
            // label17
            // 
            this.label17.Location = new System.Drawing.Point(142, 204);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(161, 45);
            this.label17.TabIndex = 5;
            this.label17.Text = "Confirmar Nuevo Password:";
            // 
            // label19
            // 
            this.label19.Location = new System.Drawing.Point(142, 142);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(161, 45);
            this.label19.TabIndex = 6;
            this.label19.Text = "Nuevo Password:";
            // 
            // txtConfirmPswd
            // 
            this.txtConfirmPswd.Location = new System.Drawing.Point(340, 201);
            this.txtConfirmPswd.Name = "txtConfirmPswd";
            this.txtConfirmPswd.Size = new System.Drawing.Size(284, 26);
            this.txtConfirmPswd.TabIndex = 2;
            // 
            // label20
            // 
            this.label20.Location = new System.Drawing.Point(142, 80);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(161, 45);
            this.label20.TabIndex = 7;
            this.label20.Text = "Password Actual:";
            // 
            // txtNwPswd
            // 
            this.txtNwPswd.Location = new System.Drawing.Point(340, 139);
            this.txtNwPswd.Name = "txtNwPswd";
            this.txtNwPswd.Size = new System.Drawing.Size(284, 26);
            this.txtNwPswd.TabIndex = 3;
            // 
            // txtPswdAct
            // 
            this.txtPswdAct.Location = new System.Drawing.Point(340, 77);
            this.txtPswdAct.Name = "txtPswdAct";
            this.txtPswdAct.Size = new System.Drawing.Size(284, 26);
            this.txtPswdAct.TabIndex = 4;
            this.txtPswdAct.Leave += new System.EventHandler(this.txtPswdAct_Leave);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnSalir);
            this.panel1.Controls.Add(this.btnCambiar);
            this.panel1.Controls.Add(this.btnTest);
            this.panel1.Controls.Add(this.btnGuardar);
            this.panel1.Location = new System.Drawing.Point(956, 41);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(211, 426);
            this.panel1.TabIndex = 1;
            // 
            // btnSalir
            // 
            this.btnSalir.Location = new System.Drawing.Point(20, 364);
            this.btnSalir.Name = "btnSalir";
            this.btnSalir.Size = new System.Drawing.Size(170, 41);
            this.btnSalir.TabIndex = 0;
            this.btnSalir.Text = "Salir";
            this.btnSalir.UseVisualStyleBackColor = true;
            this.btnSalir.Click += new System.EventHandler(this.btnSalir_Click);
            // 
            // btnCambiar
            // 
            this.btnCambiar.Location = new System.Drawing.Point(20, 178);
            this.btnCambiar.Name = "btnCambiar";
            this.btnCambiar.Size = new System.Drawing.Size(170, 41);
            this.btnCambiar.TabIndex = 0;
            this.btnCambiar.Text = "Cambiar";
            this.btnCambiar.UseVisualStyleBackColor = true;
            this.btnCambiar.Click += new System.EventHandler(this.btnCambiar_Click);
            // 
            // btnTest
            // 
            this.btnTest.Location = new System.Drawing.Point(20, 240);
            this.btnTest.Name = "btnTest";
            this.btnTest.Size = new System.Drawing.Size(170, 41);
            this.btnTest.TabIndex = 0;
            this.btnTest.Text = "Test";
            this.btnTest.UseVisualStyleBackColor = true;
            this.btnTest.Click += new System.EventHandler(this.btnTest_Click);
            // 
            // btnGuardar
            // 
            this.btnGuardar.Location = new System.Drawing.Point(20, 302);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(170, 41);
            this.btnGuardar.TabIndex = 0;
            this.btnGuardar.Text = "Guardar";
            this.btnGuardar.UseVisualStyleBackColor = true;
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // frmParametros
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1179, 484);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmParametros";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Parametros";
            this.Activated += new System.EventHandler(this.frmParametros_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmParametros_FormClosing);
            this.Load += new System.EventHandler(this.frmParametros_Load);
            this.tabControl1.ResumeLayout(false);
            this.tbTicket.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tbAs400.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbMqSeries.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tbInfo.ResumeLayout(false);
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dtgvInfo)).EndInit();
            this.tbPassword.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbTicket;
        private System.Windows.Forms.TabPage tbAs400;
        private System.Windows.Forms.TabPage tbMqSeries;
        private System.Windows.Forms.TabPage tbInfo;
        private System.Windows.Forms.TabPage tbPassword;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnSalir;
        private System.Windows.Forms.Button btnCambiar;
        private System.Windows.Forms.Button btnTest;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbBic;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtOrigenDestino;
        private System.Windows.Forms.TextBox txtBicOrigen;
        private System.Windows.Forms.TextBox txtPswdUsr;
        private System.Windows.Forms.TextBox txtLoginUsr;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtNombreServidor;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox txtBibliotecaEnv;
        private System.Windows.Forms.TextBox txtBibliotecaSaldos;
        private System.Windows.Forms.TextBox txtBibliotecaEq;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtNombreSna;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtQueReprt;
        private System.Windows.Forms.TextBox txtQueRecib;
        private System.Windows.Forms.TextBox txtQueEnv;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox txtQueManager;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.DataGridView dtgvInfo;
        private System.Windows.Forms.CheckBox chkEnvMsj;
        private System.Windows.Forms.CheckBox chkEnvArchivos;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox txtConfirmPswd;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox txtNwPswd;
        private System.Windows.Forms.TextBox txtPswdAct;
    }
}