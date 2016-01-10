using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Linq;

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
            op.Filter = "Treyarch THPS Soundbank (*.kat, *.xsb)|*.kat;*.xsb";

            string bankname = "";
            string katfn = "";
            string wavdir = "";
            string katext = "";

            List<KATWAV> wav = new List<KATWAV>();

            if (op.ShowDialog() == DialogResult.OK)
            {
                katfn = op.FileName;
                bankname = Path.GetFileNameWithoutExtension(katfn);
                wavdir = Path.GetDirectoryName(katfn) + "\\" + bankname;
                katext = Path.GetExtension(katfn);

                try
                {
                    using (BinaryReader br = new BinaryReader(File.Open(katfn, FileMode.Open)))
                    {
                        int wavnum = br.ReadInt32();

                       // MessageBox.Show(""+wavnum);

                        DateTime ct = File.GetCreationTime(katfn);

                        try
                        {

                            switch (katext)
                            {
                                case ".kat": for (int i = 0; i < wavnum; i++) wav.Add(new KATWAV(br.ReadBytes(11 * 4))); break;
                                case ".xsb": for (int i = 0; i < wavnum; i++)
                                    {
                                        KATWAV t = new KATWAV();
                                        t.ImportXSBHeader(br);
                                        wav.Add(t);
                                    }
                                    break;
                            }

                            foreach (KATWAV k in wav) k.SetData(br, katext == ".xsb");
                          //  MessageBox.Show("headers done!");
                        }
                        catch
                        {
                            wav.Clear();
                            wavnum = 0;
                            MessageBox.Show("Not a KAT file!!!");
                        }


                        if (wavnum > 0)
                        {

                            foreach (KATWAV k in wav) k.CalculateHash();

                            for (int i = 0; i < wav.Count; i++)
                                if (wav[i].duped == -1)
                                    for (int j = i+1; j < wav.Count; j++)
                                        if (wav[i].datahash == wav[j].datahash)
                                            //if (wav[i].data.SequenceEqual(wav[j].data))
                                                wav[j].duped = i;
                            
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
                                string fn = wavdir + "\\" + bankname + "_" + p.ToString("000") + ".wav";
                                try
                                {
                                    if (k.duped > -1)
                                        fn += ".duped_" + k.duped.ToString("000") + ".wav";
                                    
                                    if (k.duped == -1)
                                    {
                                        k.WriteWAV(fn);
                                        File.SetCreationTime(fn, ct);
                                    }
                                }
                                catch
                                {
                                    MessageBox.Show("Can't write WAV " + fn);
                                }
                                sb.Append(Path.GetFileName(fn) + "\t" + k.ToString() + "\r\n");
                                p++;
                            }

                            //writes log
                           // File.WriteAllText(wavdir + "\\log.txt", sb.ToString());
                        }

                        br.Close();
                    }
                }
                catch
                {
                    MessageBox.Show("Can't open " + katfn);
                }

                MessageBox.Show("Done.");
            }
        }
    }
}
