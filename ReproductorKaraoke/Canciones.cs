using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ReproductorKaraoke
{
    public partial class Canciones : Form
    {
        public static string[] cancioncitas;

        public Canciones()
        {
            InitializeComponent();
            foreach (var mp3File in cancioncitas)
            {
                listaCanciones.Items.Add(mp3File);
            }
        }

        private void Canciones_Load(object sender, EventArgs e)
        {

        }

        private void Button4_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
