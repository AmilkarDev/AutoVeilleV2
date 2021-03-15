using AutoveilleBL;
using AutoveilleBL.Models;
using AutoveilleBL.Models.Enumerations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoveilleSegmentation
{
    public partial class frmMain : Form
    {
        private List<Concession> _concessions=new List<Concession>() ;
        public frmMain()
        {
            InitializeComponent();

            _concessions = Concessions.GetConcessionsActiveAutoveilleV2();

            liBDealer.DataSource = _concessions.OrderBy(x => x.NomCommerce).ToList();

            liBDealer.DisplayMember = "NomCommerce";
            liBDealer.ValueMember = "NoCommerce";

            //cmbDealer.SelectedIndex = -1;
            //foreach (var c in _concessions)
            //{
            //    cmbDealer.Items.Add(c.NoCommerce.ToString() + " - " + c.NomCommerce);
            //}
              

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (textBox1.Text.Trim() == "")
            {
                liBDealer.DataSource = _concessions.OrderBy(x => x.NomCommerceComplet).ToList();
            }
            else
            {
                liBDealer.DataSource = _concessions
                    .Where(x => x.NomCommerceComplet.ToLower().Contains(textBox1.Text.ToLower())).OrderBy(x => x.NomCommerceComplet).ToList();
            }
        }

        private void liBDealer_SelectedIndexChanged(object sender, EventArgs e)
        {
            
         if (liBDealer.SelectedIndex >= 0)
            {
                var dealers = (Concession)liBDealer.SelectedItem;
                lbNomCommerce.Text = dealers.NomCommerce;
                var ventes = Concessions.GetEvenements(dealers.NoCommerce, DateTime.Now);
                dgvEvents.DataSource = ventes;
            }
        }

        
    }
}
