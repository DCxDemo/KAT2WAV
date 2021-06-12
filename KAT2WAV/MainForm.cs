﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;
using System.Text;
using System.Linq;
using LegacyThps.Containers;

namespace KAT2WAV
{
    public partial class MainForm : Form
    {
        Kat kat;

        public MainForm()
        {
            InitializeComponent();
        }

        private void convertButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog op = new OpenFileDialog();
            op.Filter = "Treyarch THPS Soundbank (*.kat, *.xsb)|*.kat;*.xsb";
            op.Multiselect = true;

            if (op.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in op.FileNames)
                {
                    try
                    {
                        string bankname = Path.GetFileNameWithoutExtension(file);
                        string wavdir = Path.GetDirectoryName(file) + "\\" + bankname;
                        string katext = Path.GetExtension(file);

                        kat = Kat.FromFile(file);
                        kat.Extract(wavdir);
                    }
                    catch
                    {
                        MessageBox.Show("Can't open " + file);
                        return;
                    }
                }

                MessageBox.Show("Done.");
            }
        }

        private void ExtractWavs(string filename)
        { 


            /*

            using (BinaryReader br = new BinaryReader(File.OpenRead(katfn)))
            {
                int wavnum = br.ReadInt32();

                // MessageBox.Show(""+wavnum);

                DateTime ct = File.GetCreationTime(katfn);

                try
                {

                    switch (katext.ToLower())
                    {
                        case ".kat": for (int i = 0; i < wavnum; i++) wav.Add(new KatEntry(br.ReadBytes(11 * 4))); break;
                        case ".xsb":
                            for (int i = 0; i < wavnum; i++)
                            {
                                KatEntry t = new KatEntry();
                                t.ImportXSBHeader(br);
                                wav.Add(t);
                            }
                            break;
                    }

                    foreach (KatEntry k in wav) k.SetData(br, katext == ".xsb");
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

                    foreach (KatEntry k in wav) k.CalculateHash();

                    for (int i = 0; i < wav.Count; i++)
                        if (wav[i].duped == -1)
                            for (int j = i + 1; j < wav.Count; j++)
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

                    foreach (KatEntry k in wav)
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
 
            */
            }
    }
}
