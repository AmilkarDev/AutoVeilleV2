namespace AutoveilleSegmentation
{
    partial class frmMain
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
            this.lbDealer = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.liBDealer = new System.Windows.Forms.ListBox();
            this.dgvEvents = new System.Windows.Forms.DataGridView();
            this.lbEvenements = new System.Windows.Forms.Label();
            this.lbNomCommerce = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).BeginInit();
            this.SuspendLayout();
            // 
            // lbDealer
            // 
            this.lbDealer.AutoSize = true;
            this.lbDealer.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbDealer.Location = new System.Drawing.Point(12, 16);
            this.lbDealer.Name = "lbDealer";
            this.lbDealer.Size = new System.Drawing.Size(164, 18);
            this.lbDealer.TabIndex = 1;
            this.lbDealer.Text = "Recherche commerce :";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(204, 16);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(490, 20);
            this.textBox1.TabIndex = 2;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // liBDealer
            // 
            this.liBDealer.FormattingEnabled = true;
            this.liBDealer.Location = new System.Drawing.Point(204, 42);
            this.liBDealer.Name = "liBDealer";
            this.liBDealer.Size = new System.Drawing.Size(490, 82);
            this.liBDealer.TabIndex = 3;
            this.liBDealer.SelectedIndexChanged += new System.EventHandler(this.liBDealer_SelectedIndexChanged);
            // 
            // dgvEvents
            // 
            this.dgvEvents.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEvents.Location = new System.Drawing.Point(27, 273);
            this.dgvEvents.Name = "dgvEvents";
            this.dgvEvents.Size = new System.Drawing.Size(667, 126);
            this.dgvEvents.TabIndex = 4;
            // 
            // lbEvenements
            // 
            this.lbEvenements.AutoSize = true;
            this.lbEvenements.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbEvenements.Location = new System.Drawing.Point(12, 218);
            this.lbEvenements.Name = "lbEvenements";
            this.lbEvenements.Size = new System.Drawing.Size(269, 18);
            this.lbEvenements.TabIndex = 5;
            this.lbEvenements.Text = "Voir les évènements d\'après cette date :";
            // 
            // lbNomCommerce
            // 
            this.lbNomCommerce.AutoSize = true;
            this.lbNomCommerce.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbNomCommerce.Location = new System.Drawing.Point(12, 168);
            this.lbNomCommerce.Name = "lbNomCommerce";
            this.lbNomCommerce.Size = new System.Drawing.Size(0, 18);
            this.lbNomCommerce.TabIndex = 6;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 449);
            this.Controls.Add(this.lbNomCommerce);
            this.Controls.Add(this.lbEvenements);
            this.Controls.Add(this.dgvEvents);
            this.Controls.Add(this.liBDealer);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.lbDealer);
            this.Name = "frmMain";
            this.Text = "frmMain";
            ((System.ComponentModel.ISupportInitialize)(this.dgvEvents)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbDealer;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ListBox liBDealer;
        private System.Windows.Forms.DataGridView dgvEvents;
        private System.Windows.Forms.Label lbEvenements;
        private System.Windows.Forms.Label lbNomCommerce;
    }
}