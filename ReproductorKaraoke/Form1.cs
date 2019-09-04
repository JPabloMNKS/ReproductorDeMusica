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
    public partial class Form1 : Form
    {
        // Drag Window Panel
        private bool draggable;
        private int mouseX;
        private int mouseY;
        // Variables
        bool mostrar = false;
        bool log = false;
        int aux = 0;

        private bool isPlaying = false;
        private string[] mp3Files;
        private string[] routeFiles;
        private string file;

        public delegate void Midelegado(String a); 


        public Form1()
        {
            InitializeComponent();
        }

        // Drag Window
        private void PanelTop_MouseDown(object sender, MouseEventArgs e)
        {
            draggable = true;
            mouseX = Cursor.Position.X - this.Left;
            mouseY = Cursor.Position.Y - this.Top;
        }
        private void PanelTop_MouseMove(object sender, MouseEventArgs e)
        {
            if (draggable)
            {
                this.Left = Cursor.Position.X - mouseX;
                this.Top = Cursor.Position.Y - mouseY;
            }
        }
        private void PanelTop_MouseUp(object sender, MouseEventArgs e)
        {
            draggable = false;
        }
        //Panel Buttons
        private void BtnClose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        private void BtnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void BtnAdmin_Click(object sender, EventArgs e)
        {
            Habilitar();
            aux++;
            if(aux%2==0)
                mostrar = false;
            else
                mostrar = true;
        }

        private void Button4_Click(object sender, EventArgs e)
        {
            Canciones canciones = new Canciones();
            canciones.Show();
        }

        private void Button9_Click(object sender, EventArgs e)
        {
            if (txtBoxUsuario.Text == "root" && txtBoxContrasena.Text == "toor")
            {
                log = true;
                mostrar = true;
                Habilitar();
            }
            else if (txtBoxUsuario.Text == "toor" && txtBoxContrasena.Text == "root")
            {
                log = false;
                mostrar = true;
                Habilitar();
            }

            else
            {
                txtBoxContrasena.Clear();
                txtBoxUsuario.Clear();
                mostrar = true;
                Habilitar();
                MessageBox.Show("WRONG!!!!");
            }
        }
        private void Habilitar()
        {
            if (mostrar == false)
            {
                btnLog.Visible = true;
                txtBoxContrasena.Visible = true;
                txtBoxUsuario.Visible = true;
            }
            if(mostrar == true)
            {
                btnLog.Visible = false;
                txtBoxContrasena.Visible = false;
                txtBoxUsuario.Visible = false;
            }
            if(log == true)
            {
                btnAgregar.Visible = true;
                btnMinimize.Visible = true;
                btnClose.Visible = true;
            }
            if(log == false)
            {
                btnAgregar.Visible = false;
                btnMinimize.Visible = false;
                btnClose.Visible = false;
            }
        }

        private void BtnAgregar_Click(object sender, EventArgs e)
        {
            musicFiles.Multiselect = true;
            musicFiles.Filter = "Music (.mp3)|*.mp3";
            if (musicFiles.ShowDialog() == DialogResult.OK)
            {
//                Canciones.cancioncitas = musicFiles.SafeFileNames;
                mp3Files = musicFiles.SafeFileNames;
                routeFiles = musicFiles.FileNames;
                foreach (var mp3File in mp3Files)
                {
                    songList.Items.Add(mp3File);
                }
            }
        }

        private void Button9_Click_1(object sender, EventArgs e)
        {

        }

        private void BtnPlay_Click(object sender, EventArgs e)
        {
            if (wmPlayer.URL == "")
            {
                wmPlayer.URL = routeFiles[0];
                songList.SelectedIndex = 0;
            }
            if (isPlaying == false)
            {
                wmPlayer.Ctlcontrols.play();
            }
            else if (isPlaying == true)
            {
                wmPlayer.Ctlcontrols.pause();
            }

        }

        private void SongList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try { file = routeFiles[songList.SelectedIndex]; }
            catch (Exception ex) { }

            btnNext.Enabled = true;
            btnPrevious.Enabled = true;
            if (songList.SelectedIndex <= 0)
                btnPrevious.Enabled = false;
            if (songList.SelectedIndex == songList.Items.Count - 1)
                btnNext.Enabled = false;
        }

        private void BtnPrevious_Click(object sender, EventArgs e)
        {
            file = routeFiles[--songList.SelectedIndex];
            wmPlayer.URL = file;
            wmPlayer.Ctlcontrols.play();

        }

        private void BtnNext_Click(object sender, EventArgs e)
        {
            file = routeFiles[++songList.SelectedIndex];
            wmPlayer.URL = file;
            wmPlayer.Ctlcontrols.play();
        }

        private void SongList_DoubleClick(object sender, EventArgs e)
        {
            wmPlayer.URL = file;
            wmPlayer.Ctlcontrols.play();
        }

        private void WmPlayer_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            switch (e.newState)
            {
                case 1:    // Stopped
                    songName.Text = "";
                    isPlaying = false;
                    time.Enabled = false;
                    break;
                case 2:    // Paused
                    btnPlay.Load("imagenes/Botones/013-play.png");
                    isPlaying = false;
                    time.Enabled = false;
                    break;

                case 3:    // Playing
                    songName.Text = wmPlayer.currentMedia.name;
                    txtDuration.Text = wmPlayer.currentMedia.durationString;
                    btnPlay.Load("imagenes/Botones/021-pause.png");
                    isPlaying = true;
                    time.Enabled = true;
                    int total = (int)wmPlayer.currentMedia.duration;
                    macTrackBar1.Maximum = total;
                    break;
            }
        }

        private void TbVolume_Scroll(object sender, EventArgs e)
        {
            wmPlayer.settings.volume = tbVolume.Value;
        }

        private void Time_Tick(object sender, EventArgs e)
        {
            macTrackBar1.Value = (int)wmPlayer.Ctlcontrols.currentPosition;
            txtActual.Text = wmPlayer.Ctlcontrols.currentPositionString;
        }

        private void MacTrackBar1_ValueChanged(object sender, decimal value)
        {
//            wmPlayer.Ctlcontrols.currentPosition = macTrackBar1.Value;
        }

        private void MacTrackBar1_MouseDown(object sender, MouseEventArgs e)
        {
            wmPlayer.Ctlcontrols.pause();
        }

        private void MacTrackBar1_MouseUp(object sender, MouseEventArgs e)
        {
            wmPlayer.Ctlcontrols.play();

        }
    }
}
