using lab3.Controllers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab3
{
    public partial class Сапёр : Form
    {
        
        public Сапёр()
        {
            InitializeComponent();

            MapController.Init(this);
        }        
    }
}