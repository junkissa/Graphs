namespace Graph_try_OpenGL_Csh
{
    partial class Form1
    {
        /// <summary>
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Обязательный метод для поддержки конструктора - не изменяйте
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
			this.AnT = new Tao.Platform.Windows.SimpleOpenGlControl();
			this.toolStrip1 = new System.Windows.Forms.ToolStrip();
			this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.пошукВШиринуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.пошукВГлибинуToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.крускалToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.прімаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.беллманаФордаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.дейкстриToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.флойдаВоршеллаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.джонсонаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.фордаФалкерсонаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.алгоритмЕдмондсаКарпаToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.мишкоюToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.згенеруватиToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.checkBox1 = new System.Windows.Forms.CheckBox();
			this.checkBox2 = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.toolStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// AnT
			// 
			this.AnT.AccumBits = ((byte)(0));
			this.AnT.AutoCheckErrors = false;
			this.AnT.AutoFinish = false;
			this.AnT.AutoMakeCurrent = true;
			this.AnT.AutoSwapBuffers = true;
			this.AnT.BackColor = System.Drawing.Color.Black;
			this.AnT.ColorBits = ((byte)(32));
			this.AnT.DepthBits = ((byte)(16));
			this.AnT.Location = new System.Drawing.Point(0, 35);
			this.AnT.Name = "AnT";
			this.AnT.Size = new System.Drawing.Size(733, 459);
			this.AnT.StencilBits = ((byte)(0));
			this.AnT.TabIndex = 0;
			this.AnT.Tag = "";
			this.AnT.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.AnT_MouseDoubleClick);
			this.AnT.MouseDown += new System.Windows.Forms.MouseEventHandler(this.AnT_MouseDown);
			this.AnT.MouseMove += new System.Windows.Forms.MouseEventHandler(this.AnT_MouseMove);
			this.AnT.MouseUp += new System.Windows.Forms.MouseEventHandler(this.AnT_MouseUp);
			// 
			// toolStrip1
			// 
			this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripDropDownButton1,
            this.toolStripDropDownButton2});
			this.toolStrip1.Location = new System.Drawing.Point(0, 0);
			this.toolStrip1.Name = "toolStrip1";
			this.toolStrip1.Size = new System.Drawing.Size(934, 25);
			this.toolStrip1.TabIndex = 1;
			this.toolStrip1.Text = "toolStrip1";
			// 
			// toolStripDropDownButton1
			// 
			this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.пошукВШиринуToolStripMenuItem,
            this.пошукВГлибинуToolStripMenuItem,
            this.крускалToolStripMenuItem,
            this.прімаToolStripMenuItem,
            this.беллманаФордаToolStripMenuItem,
            this.дейкстриToolStripMenuItem,
            this.флойдаВоршеллаToolStripMenuItem,
            this.джонсонаToolStripMenuItem,
            this.фордаФалкерсонаToolStripMenuItem,
            this.алгоритмЕдмондсаКарпаToolStripMenuItem});
			this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
			this.toolStripDropDownButton1.Size = new System.Drawing.Size(55, 22);
			this.toolStripDropDownButton1.Text = "Метод";
			this.toolStripDropDownButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextAboveImage;
			this.toolStripDropDownButton1.ToolTipText = "Метод";
			// 
			// пошукВШиринуToolStripMenuItem
			// 
			this.пошукВШиринуToolStripMenuItem.Name = "пошукВШиринуToolStripMenuItem";
			this.пошукВШиринуToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.пошукВШиринуToolStripMenuItem.Text = "Пошук в ширину";
			this.пошукВШиринуToolStripMenuItem.Click += new System.EventHandler(this.пошукВШиринуToolStripMenuItem_Click);
			// 
			// пошукВГлибинуToolStripMenuItem
			// 
			this.пошукВГлибинуToolStripMenuItem.Name = "пошукВГлибинуToolStripMenuItem";
			this.пошукВГлибинуToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.пошукВГлибинуToolStripMenuItem.Text = "Пошук в глибину";
			this.пошукВГлибинуToolStripMenuItem.Click += new System.EventHandler(this.пошукВГлибинуToolStripMenuItem_Click);
			// 
			// крускалToolStripMenuItem
			// 
			this.крускалToolStripMenuItem.Name = "крускалToolStripMenuItem";
			this.крускалToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.крускалToolStripMenuItem.Text = "Крускала";
			this.крускалToolStripMenuItem.Click += new System.EventHandler(this.крускалToolStripMenuItem_Click);
			// 
			// прімаToolStripMenuItem
			// 
			this.прімаToolStripMenuItem.Name = "прімаToolStripMenuItem";
			this.прімаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.прімаToolStripMenuItem.Text = "Пріма";
			this.прімаToolStripMenuItem.Click += new System.EventHandler(this.прімаToolStripMenuItem_Click);
			// 
			// беллманаФордаToolStripMenuItem
			// 
			this.беллманаФордаToolStripMenuItem.Name = "беллманаФордаToolStripMenuItem";
			this.беллманаФордаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.беллманаФордаToolStripMenuItem.Text = "Беллмана-Форда";
			this.беллманаФордаToolStripMenuItem.Click += new System.EventHandler(this.беллманаФордаToolStripMenuItem_Click);
			// 
			// дейкстриToolStripMenuItem
			// 
			this.дейкстриToolStripMenuItem.Name = "дейкстриToolStripMenuItem";
			this.дейкстриToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.дейкстриToolStripMenuItem.Text = "Дейкстри";
			this.дейкстриToolStripMenuItem.Click += new System.EventHandler(this.дейкстриToolStripMenuItem_Click);
			// 
			// флойдаВоршеллаToolStripMenuItem
			// 
			this.флойдаВоршеллаToolStripMenuItem.Name = "флойдаВоршеллаToolStripMenuItem";
			this.флойдаВоршеллаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.флойдаВоршеллаToolStripMenuItem.Text = " Флойда-Воршелла";
			this.флойдаВоршеллаToolStripMenuItem.Click += new System.EventHandler(this.флойдаВоршеллаToolStripMenuItem_Click);
			// 
			// джонсонаToolStripMenuItem
			// 
			this.джонсонаToolStripMenuItem.Name = "джонсонаToolStripMenuItem";
			this.джонсонаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.джонсонаToolStripMenuItem.Text = "Джонсона";
			this.джонсонаToolStripMenuItem.Click += new System.EventHandler(this.джонсонаToolStripMenuItem_Click);
			// 
			// фордаФалкерсонаToolStripMenuItem
			// 
			this.фордаФалкерсонаToolStripMenuItem.Name = "фордаФалкерсонаToolStripMenuItem";
			this.фордаФалкерсонаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.фордаФалкерсонаToolStripMenuItem.Text = "Форда-Фалкерсона";
			this.фордаФалкерсонаToolStripMenuItem.Click += new System.EventHandler(this.фордаФалкерсонаToolStripMenuItem_Click);
			// 
			// алгоритмЕдмондсаКарпаToolStripMenuItem
			// 
			this.алгоритмЕдмондсаКарпаToolStripMenuItem.Name = "алгоритмЕдмондсаКарпаToolStripMenuItem";
			this.алгоритмЕдмондсаКарпаToolStripMenuItem.Size = new System.Drawing.Size(182, 22);
			this.алгоритмЕдмондсаКарпаToolStripMenuItem.Text = "Едмондса-Карпа";
			this.алгоритмЕдмондсаКарпаToolStripMenuItem.Click += new System.EventHandler(this.алгоритмЕдмондсаКарпаToolStripMenuItem_Click);
			// 
			// toolStripDropDownButton2
			// 
			this.toolStripDropDownButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
			this.toolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.мишкоюToolStripMenuItem,
            this.згенеруватиToolStripMenuItem});
			this.toolStripDropDownButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton2.Image")));
			this.toolStripDropDownButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.toolStripDropDownButton2.Name = "toolStripDropDownButton2";
			this.toolStripDropDownButton2.Size = new System.Drawing.Size(91, 22);
			this.toolStripDropDownButton2.Text = "Намалювати";
			// 
			// мишкоюToolStripMenuItem
			// 
			this.мишкоюToolStripMenuItem.Name = "мишкоюToolStripMenuItem";
			this.мишкоюToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.мишкоюToolStripMenuItem.Text = "Мишкою";
			this.мишкоюToolStripMenuItem.Click += new System.EventHandler(this.мишкоюToolStripMenuItem_Click);
			// 
			// згенеруватиToolStripMenuItem
			// 
			this.згенеруватиToolStripMenuItem.Name = "згенеруватиToolStripMenuItem";
			this.згенеруватиToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
			this.згенеруватиToolStripMenuItem.Text = "Стерти";
			this.згенеруватиToolStripMenuItem.Click += new System.EventHandler(this.згенеруватиToolStripMenuItem_Click);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(750, 75);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(149, 20);
			this.textBox1.TabIndex = 2;
			this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(747, 50);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(0, 13);
			this.label1.TabIndex = 5;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(747, 59);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(89, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "поле вводу ваги";
			// 
			// checkBox1
			// 
			this.checkBox1.Location = new System.Drawing.Point(750, 117);
			this.checkBox1.Name = "checkBox1";
			this.checkBox1.Size = new System.Drawing.Size(149, 23);
			this.checkBox1.TabIndex = 7;
			this.checkBox1.Text = "Орієнтований";
			this.checkBox1.UseVisualStyleBackColor = true;
			this.checkBox1.CheckedChanged += new System.EventHandler(this.Орієнтований_CheckedChanged);
			// 
			// checkBox2
			// 
			this.checkBox2.Location = new System.Drawing.Point(750, 146);
			this.checkBox2.Name = "checkBox2";
			this.checkBox2.Size = new System.Drawing.Size(104, 24);
			this.checkBox2.TabIndex = 8;
			this.checkBox2.Text = "Зважений";
			this.checkBox2.UseVisualStyleBackColor = true;
			this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(739, 187);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(195, 32);
			this.label3.TabIndex = 9;
			this.label3.Text = "Для вибору ведучої вершни - двічі лівою кнопкою миші";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(739, 219);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(183, 38);
			this.label4.TabIndex = 10;
			this.label4.Text = "Для вибору стоку у методах на максимальний потік - двічі правою кнопкою";
			// 
			// Form1
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(934, 496);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.checkBox2);
			this.Controls.Add(this.checkBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.toolStrip1);
			this.Controls.Add(this.AnT);
			this.Name = "Form1";
			this.Text = "Form1";
			this.Load += new System.EventHandler(this.Form1_Load);
			this.toolStrip1.ResumeLayout(false);
			this.toolStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private Tao.Platform.Windows.SimpleOpenGlControl AnT;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton2;
        private System.Windows.Forms.ToolStripMenuItem пошукВШиринуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem пошукВГлибинуToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem мишкоюToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem згенеруватиToolStripMenuItem;
		private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ToolStripMenuItem крускалToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem прімаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem беллманаФордаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem дейкстриToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem флойдаВоршеллаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem джонсонаToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem фордаФалкерсонаToolStripMenuItem;
		private System.Windows.Forms.CheckBox checkBox1;
		private System.Windows.Forms.CheckBox checkBox2;
		private System.Windows.Forms.ToolStripMenuItem алгоритмЕдмондсаКарпаToolStripMenuItem;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
    }
}

