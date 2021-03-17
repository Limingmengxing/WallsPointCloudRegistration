namespace _2020プログラム
{
    partial class Form1
    {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent()
        {
            this.button1 = new System.Windows.Forms.Button();
            this.傾き位置合わせ = new System.Windows.Forms.Button();
            this.tabcontrol = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.歪み検出 = new System.Windows.Forms.Button();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.button3 = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.差分の差 = new System.Windows.Forms.Button();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.メッシュの分割 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.位置合わせ = new System.Windows.Forms.Button();
            this.tabcontrol.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(48, 162);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 28);
            this.button1.TabIndex = 4;
            this.button1.Text = "クラスタリング";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.クラスタリング_Click);
            // 
            // 傾き位置合わせ
            // 
            this.傾き位置合わせ.Location = new System.Drawing.Point(36, 144);
            this.傾き位置合わせ.Name = "傾き位置合わせ";
            this.傾き位置合わせ.Size = new System.Drawing.Size(93, 23);
            this.傾き位置合わせ.TabIndex = 5;
            this.傾き位置合わせ.Text = "傾き位置合わせ";
            this.傾き位置合わせ.UseVisualStyleBackColor = true;
            this.傾き位置合わせ.Click += new System.EventHandler(this.傾き位置合わせ_Click);
            // 
            // tabcontrol
            // 
            this.tabcontrol.Controls.Add(this.tabPage1);
            this.tabcontrol.Controls.Add(this.tabPage5);
            this.tabcontrol.Controls.Add(this.tabPage4);
            this.tabcontrol.Controls.Add(this.tabPage2);
            this.tabcontrol.Controls.Add(this.tabPage3);
            this.tabcontrol.Location = new System.Drawing.Point(0, -2);
            this.tabcontrol.Name = "tabcontrol";
            this.tabcontrol.SelectedIndex = 0;
            this.tabcontrol.Size = new System.Drawing.Size(193, 332);
            this.tabcontrol.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.傾き位置合わせ);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(185, 306);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "傾き";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.歪み検出);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(185, 306);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "歪み検出";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // 歪み検出
            // 
            this.歪み検出.Location = new System.Drawing.Point(55, 142);
            this.歪み検出.Name = "歪み検出";
            this.歪み検出.Size = new System.Drawing.Size(75, 23);
            this.歪み検出.TabIndex = 8;
            this.歪み検出.Text = "歪み検出";
            this.歪み検出.UseVisualStyleBackColor = true;
            this.歪み検出.Click += new System.EventHandler(this.歪み検出_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.button3);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(185, 306);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "表面性状";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(53, 158);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(75, 23);
            this.button3.TabIndex = 1;
            this.button3.Text = "中央値で位置合わせ";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.中央値で位置合わせ);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.差分の差);
            this.tabPage2.Controls.Add(this.button1);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(185, 306);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "余計";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // 差分の差
            // 
            this.差分の差.Location = new System.Drawing.Point(48, 55);
            this.差分の差.Name = "差分の差";
            this.差分の差.Size = new System.Drawing.Size(75, 23);
            this.差分の差.TabIndex = 8;
            this.差分の差.Text = "差分の差";
            this.差分の差.UseVisualStyleBackColor = true;
            this.差分の差.Click += new System.EventHandler(this.差分の差_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.メッシュの分割);
            this.tabPage3.Controls.Add(this.button4);
            this.tabPage3.Controls.Add(this.位置合わせ);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(185, 306);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "2019研究";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // メッシュの分割
            // 
            this.メッシュの分割.Location = new System.Drawing.Point(35, 60);
            this.メッシュの分割.Name = "メッシュの分割";
            this.メッシュの分割.Size = new System.Drawing.Size(86, 23);
            this.メッシュの分割.TabIndex = 6;
            this.メッシュの分割.Text = "メッシュの分割";
            this.メッシュの分割.UseVisualStyleBackColor = true;
            this.メッシュの分割.Click += new System.EventHandler(this.メッシュの分割_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(35, 179);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(86, 23);
            this.button4.TabIndex = 5;
            this.button4.Text = "差分計算";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.差分計算_Click);
            // 
            // 位置合わせ
            // 
            this.位置合わせ.Location = new System.Drawing.Point(35, 116);
            this.位置合わせ.Name = "位置合わせ";
            this.位置合わせ.Size = new System.Drawing.Size(86, 23);
            this.位置合わせ.TabIndex = 4;
            this.位置合わせ.Text = "位置合わせ";
            this.位置合わせ.UseVisualStyleBackColor = true;
            this.位置合わせ.Click += new System.EventHandler(this.位置合わせ_Click);
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(195, 329);
            this.Controls.Add(this.tabcontrol);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tabcontrol.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button 傾き位置合わせ;
        private System.Windows.Forms.TabControl tabcontrol;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Button 位置合わせ;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button メッシュの分割;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.Button 差分の差;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.Button 歪み検出;
        private System.Windows.Forms.Button button3;
    }
}

