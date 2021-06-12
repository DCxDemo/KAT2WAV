﻿using System;
using System.IO;
using System.Security.Cryptography;

namespace LegacyThps.Containers
{
    public class KatEntry
    {
        public bool IsXsb = false;

        public int Index = 0;
        public int NumChannels = 0;
        public int Offset = 0;
        public int Size = 0;
        public int Frequency = 0;
        public int Loop = 0;
        public int Bits = 0;
        public int Unk = 0;

        public int BankIndex;
        public int SampleIndex;

        public byte[] Data;

        public byte channels = 0;
        public byte offset = 1;
        public byte size = 2;
        public byte freq = 3;
        public byte loop = 4;
        public byte bits = 5;
        public byte unk = 6;

        public int duped = -1;

        public int[] param;



        public KatEntry(BinaryReader br)
        {
            Read(br);
        }

        public static KatEntry FromReader(BinaryReader br)
        {
            return new KatEntry(br);
        }

        public virtual void Read(BinaryReader br)
        {
            NumChannels = br.ReadInt32();
            Offset = br.ReadInt32();
            Size = br.ReadInt32();
            Frequency = br.ReadInt32();
            Loop = br.ReadInt32();
            Bits = br.ReadInt32();
            Unk = br.ReadInt32();

            BankIndex = Unk / 1000;
            SampleIndex = Unk % 1000;

            br.BaseStream.Position += 16;

            //Name = (new string(br.ReadChars(16))).Split('\0')[0];
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

        public KatEntry(byte[] buf)
        {
            param = new int[buf.Length/4];
            Buffer.BlockCopy(buf, 0, param, 0, buf.Length);
        }


        public void SetData(BinaryReader b, bool xsb)
        {
            b.BaseStream.Position = param[offset];
            Data = b.ReadBytes(param[size]);

            //8 bit PCM should be unsigned, means 0x80 = 0 aka silence
            //xbox doesn't need that
            if (param[bits] == 8 && !xsb)
                for (int i = 0; i < Data.Length; i++)
                    Data[i] = (byte)((Data[i] + 0x80) % 256);

            //ADPCM conversion should be here
            if (param[bits] == 4)
            {
            }

        }

        public void GetSampleData(BinaryReader br)
        {
            br.BaseStream.Position = Offset;
            Data = br.ReadBytes(Size);

            //8 bit PCM should be unsigned, means 0x80 = 0 aka silence
            //xbox doesn't need that
            if (Bits == 8 && !IsXsb)
                for (int i = 0; i < Data.Length; i++)
                    Data[i] = (byte)((Data[i] + 0x80) % 256);

                    //data[i] = (byte)(data[i] << 4 & data[i] >> 4);
        }


        public void WriteWAV(string f)
        {
            using (BinaryWriter bw = new BinaryWriter(File.Open(f, FileMode.Create)))
            {
                bw.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));
                bw.Write(4 + 24 + 8 + Data.Length);
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
                bw.Write(Data.Length);
                bw.Write(Data);

                bw.Close();
            }
        }

        public virtual string GetName()
        {
            return $"{BankIndex}_{SampleIndex}.wav";
        }

        public void Save(string path)
        {
            string fullpath = Path.Combine(path, GetName());

            using (BinaryWriter bw = new BinaryWriter(File.Open(fullpath, FileMode.Create)))
            {
                bw.Write("RIFF".ToCharArray());
                bw.Write(4 + 24 + 8 + Data.Length);
                bw.Write("WAVE".ToCharArray());

                bw.Write("fmt ".ToCharArray());
                bw.Write((int)0x10);
                bw.Write((short)1);
                bw.Write((short)NumChannels);
                bw.Write(Frequency);
                bw.Write((Frequency * NumChannels * Bits) / 8);
                bw.Write((short)(Bits * NumChannels / 8));
                bw.Write((short)Bits);

                bw.Write("data".ToCharArray());
                bw.Write(Data.Length);
                bw.Write(Data);
            }
        }


        public override string ToString()
        {
            return
                $"Channels: {NumChannels} | " +
                $"Size: {Size} | " +
                $"Frequency: {Frequency} | " +
                $"Loop: {((Loop > 0) ? "Yes" : "No")} |" +
                $"Bits: {Bits} | " +
                $"Unk: {Unk}\r\n";
        }
    }
}
