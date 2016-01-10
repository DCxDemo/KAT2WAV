using System;
using System.IO;
using System.Security.Cryptography;

namespace KAT2WAV
{
    class KATWAV
    {
        public byte channels = 0;
        public byte offset = 1;
        public byte size = 2;
        public byte freq = 3;
        public byte loop = 4;
        public byte bits = 5;
        public byte unk = 6;

        public int duped = -1;

        public int[] param;
        public byte[] data;

        public string datahash = "";

        public void CalculateHash()
        {
            using (var md5 = MD5.Create())
            datahash = BitConverter.ToString(md5.ComputeHash(data)).Replace("-", "").ToLower();
        }

        public int SampleRate { get { return param[freq]; } }

        public KATWAV()
        {
        }

        public void ImportXSBHeader(BinaryReader b)
        {
            param = new int[11];

            param[size] = b.ReadInt32();
            param[offset] = b.ReadInt32();
            param[channels] = 1;
            param[freq] = b.ReadUInt16();
            param[unk] = b.ReadInt16(); //just to make unique

            int bitsvalue = b.ReadByte();
            if (bitsvalue > 0x80) bitsvalue -= 0x80;
            param[bits] = bitsvalue;

            b.ReadByte();
            b.ReadInt16();
            b.ReadInt16();
        }

        public KATWAV(byte[] buf)
        {
            param = new int[buf.Length/4];
            Buffer.BlockCopy(buf, 0, param, 0, buf.Length);
        }

        public override string ToString()
        {
            return
                "Ch:" + param[channels] + "|" +
                "Offset:" + param[offset] + "|" +
                "Size:" + param[size] + "|" +
                "freq:" + param[freq] + "|" +
                "Loop:" + ((param[loop] > 0) ? "Yes" : "No") + "|" +
                "Bits:" + param[bits] + "|" +
                "unk:" + param[unk];
        }

        public void SetData(BinaryReader b, bool xsb)
        {
            b.BaseStream.Position = param[offset];
            data = b.ReadBytes(param[size]);

            //8 bit PCM should be unsigned, means 0x80 = 0 aka silence
            //xbox doesn't need that
            if (param[bits] == 8 && !xsb)
                for (int i = 0; i < data.Length; i++)
                    data[i] = (byte)((data[i] + 0x80) % 256);

            //ADPCM conversion should be here
            if (param[bits] == 4)
            {
            }

        }

        public void WriteWAV(string f)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(f, FileMode.Create)))
            {
                bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                bw.Write(4 + 24 + 8 + data.Length);
                bw.Write(System.Text.Encoding.ASCII.GetBytes("WAVE"));

                bw.Write(System.Text.Encoding.ASCII.GetBytes("fmt "));
                bw.Write((int)0x10);
                bw.Write((short)1);
                bw.Write((short)param[channels]);
                bw.Write(param[freq]);
                bw.Write((param[freq] * param[channels] * param[bits]) / 8);
                bw.Write((short)(param[bits]* param[channels]/8));
                bw.Write((short)param[bits]);

                bw.Write(System.Text.Encoding.ASCII.GetBytes("data"));
                bw.Write(data.Length);
                bw.Write(data);

                bw.Close();
            }
        }

    }
}
