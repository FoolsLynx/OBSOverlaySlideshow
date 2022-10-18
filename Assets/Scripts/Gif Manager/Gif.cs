using UnityEngine;
using System.IO;
using System.Text;
using System;

public class Gif
{
    public int Width;
    public int Height;

    public byte ColourResolution;
    public int SortFlags;

    public byte PixelAspect;

    public Color32 BackgroundColour;

    public Gif(string file)
    {
        Load(file);
    }

    public void Load(string file)
    {
        using Stream stream = File.Open(file, FileMode.Open);
        using GifReader reader = new(stream);

        // Header Block
        string header = reader.ReadString(6);
        Debug.Log(header);

        // Logical Screen Descriptior
        Width = reader.ReadInt16();
        Height = reader.ReadInt16();

        Debug.Log(Width);
        Debug.Log(Height);

        byte packed = reader.ReadByte();
        
        bool colourTableFlag = ((packed & 0x80) >> 7) == 1;
        ColourResolution = (byte)((packed & 0x60) >> 5);
        SortFlags = ((byte)(packed & 0x10)) >> 4;

        byte bgIndex = reader.ReadByte();

        PixelAspect = reader.ReadByte();

        // Global Colour Table Block
        Color32[] globalColourTable = null;
        BackgroundColour = new Color32(0, 0, 0, 0);

        if(colourTableFlag)
        {
            int colourTableSize = ((int)2) << (packed & 7);
            Debug.Log(string.Format("Colour Table Size: {0}", colourTableSize));
            globalColourTable = LoadPalette(reader.ReadBytes(colourTableSize * 3));
            if(bgIndex < globalColourTable.Length)
            {
                BackgroundColour = globalColourTable[bgIndex];
            }
        }

        // Graphics Control Extension
        if(header.Contains("89"))
        {
            reader.ReadBytes(2);

            byte gceByteSize = reader.ReadByte();
            byte gcePacketField = reader.ReadByte();
            short delayTime = reader.ReadInt16();
            byte gceTransparency = reader.ReadByte();
            
            reader.ReadByte();
        }

        // Image Descriptor


    }

    public static Color32[] LoadPalette(byte[] table)
    {
        Color32[] tab = new Color32[table.Length / 3];
        int i = 0;
        int j = 0;
        while(i < table.Length)
        {
            byte r = table[i++];
            byte g = table[i++];
            byte b = table[i++];
            Color32 c = new(r, g, b, 255);
            tab[j++] = c;
        }
        return tab;
    }
}

public class GifReader : BinaryReader
{
    public static readonly Encoding TextEncoding = Encoding.UTF8;

    public int PreviousX;
    public int PreviousY;

    public GifReader(Stream stream) : base(stream) { }

    public GifReader(byte[] src) : base(new MemoryStream(src)) { }

    public override string ReadString()
    {
        return ReadString((int)ReadCompressed());
    }

    public string ReadString(int length)
    {
        return TextEncoding.GetString(ReadBytes(length));
    }

    public ulong ReadCompressed()
    {
        byte c = ReadByte();
        switch(c)
        {
            case 251: return (ulong)ReadUInt16();
            case 252: return (ulong)ReadUInt24();
            case 253: return (ulong)ReadUInt32();
            case 254: return (ulong)ReadUInt64();
            default: return c;
        }
    }

    public uint ReadUInt24()
    {
        uint value = 0;
        int shift = 0;
        for (int i = 0; i < 3; i++)
        {
            value |= (uint)(ReadByte() << shift);
            shift += 8;
        }
        return value;
    }

    public int ReadInt24()
    {
        int value = 0;
        int shift = 0;
        for (int i = 0; i < 3; i++)
        {
            value |= (int)(ReadByte() << shift);
            shift += 8;
        }
        return value;
    }
}