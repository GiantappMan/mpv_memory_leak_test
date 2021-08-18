using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MpvTest
{
    public partial class Form1 : Form
    {
        private Mpv.NET.Player.MpvPlayer _player;
        private string appDir;
        public Form1()
        {
            InitializeComponent();

            var assembly = Assembly.GetEntryAssembly();
            appDir = System.IO.Path.GetDirectoryName(assembly.Location);


            Task.Run(async () =>
            {

                try
                {
                    while (true)
                    {
                        Invoke(new Action(() =>
                        {
                            button1.PerformClick();
                        }));
                        await Task.Delay(300);
                        Invoke(new Action(() =>
                        {
                            button2.PerformClick();
                        }));
                        await Task.Delay(300);
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            });
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Play(Path.Combine(appDir, "videos\\1.mp4"));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Play(Path.Combine(appDir, "videos\\2.mp4"));
        }
        private void Play(string path)
        {
            //uncomment this line to reproduce memory leak
            _player?.Stop();


            if (_player == null)
            {

                string dllPath = $@"{appDir}\mpv-1.dll";
                _player = new Mpv.NET.Player.MpvPlayer(panel1.Handle, dllPath)
                {
                    Loop = true,
                    Volume = 0
                };

                _player.AutoPlay = true;
            }

            if (string.IsNullOrEmpty(path))
                return;

            _player?.API.SetPropertyString("hwdec", "auto");
            _player?.API.SetPropertyString("stop-screensaver", "no");

            _player?.Pause();
            _player?.Load(path);
            _player?.Resume();
        }
    }
}
