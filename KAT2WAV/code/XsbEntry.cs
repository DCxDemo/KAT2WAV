using System.IO;

namespace LegacyThps.Containers
{
    class XsbEntry : KatEntry
    {
        public XsbEntry(BinaryReader br) : base(br)
        {
        }

        public override void Read(BinaryReader br)
        {
            IsXsb = true;

            Size = br.ReadInt32();
            Offset = br.ReadInt32();
            NumChannels = 1;
            Frequency = br.ReadUInt16();
            Index = br.ReadInt16();

            int bitsvalue = br.ReadByte();
            if (bitsvalue > 0x80) bitsvalue -= 0x80;
            Bits = bitsvalue;

            br.ReadByte();
            br.ReadInt16();
            br.ReadInt16();
        }

        public override string GetName()
        {
            return $"{((Sounds)Index).ToString()}.wav";
        }

        public new static XsbEntry FromReader(BinaryReader br)
        {
            return new XsbEntry(br);
        }
    }
}