using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;

namespace KAT2WAV
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Dreamcast KAT Soundbank (*.kat)|*.kat";

            string bankname = "";
            string katfn = "";
            string wavdir = "";

            List<KATWAV> wav = new List<KATWAV>();

            if (op.ShowDialog() == DialogResult.OK)
            {
                katfn = op.FileName;
                bankname = Path.GetFileNameWithoutExtension(katfn);
                wavdir = Path.GetDirectoryName(katfn) + "\\" + bankname;

                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(katfn, FileMode.Open)))
                    {
                        int wavnum = br.ReadInt32();

                        DateTime ct = File.GetCreationTime(katfn);

                        try
                        {
                            for (int i = 0; i < wavnum; i++) wav.Add(new KATWAV(br.ReadBytes(11 * 4)));
                            foreach (KATWAV k in wav) k.SetData(br);
                        }
                        catch
                        {
                            wav.Clear();
                            wavnum = 0;
                            MessageBox.Show("Not a KAT file!!!");
                        }

                        if (wavnum > 0)
                        {
                            try
                            {
                                Directory.CreateDirectory(wavdir);
                            }
                            catch
                            {
                                MessageBox.Show("Cannot create folder " + wavdir);
                            }

                            int p = 0;
                            StringBuilder sb = new StringBuilder();

                            foreach (KATWAV k in wav)
                            {
                                string fn = wavdir + "\\" + bankname +"_"+ p.ToString("000") + ".wav";
                                try
                                {
                                    k.WriteWAV(fn);
                                    File.SetCreationTime(fn, ct);
                                }
                                catch
                                {
                                    MessageBox.Show("Can't write WAV " + fn);
                                }
                                sb.Append(Path.GetFileName(fn) + "\t" + k.ToString() + "\r\n");
                                p++;
                            }

                            //writes log
                            //File.WriteAllText(wavdir + "\\log.txt", sb.ToString());
                        }
                        br.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Can't open " + katfn);
                }
            }

            MessageBox.Show("Done.");
        }
    }
}
