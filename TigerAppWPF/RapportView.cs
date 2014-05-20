using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TigerAppWPF
{
    public partial class RapportView : Form
    {
        public RapportView()
        {
            InitializeComponent();
        }

        private void RapportView_Load(object sender, EventArgs e)
        {
            EquityBindingSource.DataSource = Engine.getEngine().Portfolio;
            this.reportViewer1.RefreshReport();
        }
    }
}
