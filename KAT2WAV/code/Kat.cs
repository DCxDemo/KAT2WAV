using System;
using System.Collections.Generic;
using System.IO;

namespace LegacyThps.Containers
{
    public class Kat
    {
        public List<KatEntry> Entries = new List<KatEntry>();

        public Kat(BinaryReader br)
        {
            Read(br);
        }

        public void Read(BinaryReader br)
        {
            int numEntries = br.ReadInt32();

            for (int i = 0; i < numEntries; i++)
                Entries.Add(KatEntry.FromReader(br));

            for (int i = 0; i < numEntries; i++)
            {
                Entries[i].Index = i;
                Entries[i].GetSampleData(br, false);
            }
        }

        public static Kat FromFile(string filename)
        {
            using (BinaryReader br = new BinaryReader(File.OpenRead(filename)))
            {
                return new Kat(br);
            }
        }

        public void Extract(string path)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            foreach (var entry in Entries)
                entry.Save(path);
        }
    }
}