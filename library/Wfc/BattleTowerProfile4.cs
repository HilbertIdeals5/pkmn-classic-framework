﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using PkmnFoundations.Structures;
using PkmnFoundations.Support;

namespace PkmnFoundations.Wfc
{
    public class BattleTowerProfile4 : BattleTowerProfileBase
    {
        public BattleTowerProfile4()
        {
        }

        public BattleTowerProfile4(EncodedString4 name, Versions version, 
            Languages language, byte country, byte region, uint ot,
            TrendyPhrase4 phrase_leader, byte gender, byte unknown)
        {
            if (name == null) throw new ArgumentNullException("name");
            if (name.Size != 16) throw new ArgumentException("name");
            if (phrase_leader == null) throw new ArgumentNullException("phrase_leader");
            
            Name = name; // todo: clone
            Version = version;
            Language = language;
            Country = country;
            Region = region;
            OT = ot;
            PhraseLeader = phrase_leader; // todo: clone
            Gender = gender;
            Unknown = unknown;
        }

        public BattleTowerProfile4(byte[] data)
        {
            Load(data, 0);
        }

        public BattleTowerProfile4(byte[] data, int start)
        {
            Load(data, start);
        }

        public EncodedString4 Name;
        public Versions Version;
        public Languages Language;
        public byte Country;
        public byte Region;
        public uint OT;
        public TrendyPhrase4 PhraseLeader;
        // Different from GTS, 0 = male, 2 = female, 1 = Plato???? 
        public byte Gender;

        // 3: Lass
        // 6: Bug Catcher
        // 10: Battle Girl
        // 14: Black Belt
        // 18: Cowgirl
        // 24: Ace Trainer M
        // 25: Ace Trainer F
        // 32: Rich Boy
        // 33: Lady
        // 35: Socialite F
        // 36: Beauty
        // 48: Ruin Maniac
        // 49: Psychic M
        // 57: Roughneck
        // 60: School Kid M
        // 85: Idol
        public byte Unknown; // Probably trainer class.

        public byte[] Save()
        {
            byte[] data = new byte[0x22];
            MemoryStream ms = new MemoryStream(data);
            BinaryWriter writer = new BinaryWriter(ms);

            writer.Write(Name.RawData, 0, 16);
            writer.Write((byte)Version);
            writer.Write((byte)Language);
            writer.Write(Country);
            writer.Write(Region);
            writer.Write(OT);
            writer.Write(PhraseLeader.Data, 0, 8);
            writer.Write(Gender);
            writer.Write(Unknown);

            writer.Flush();
            ms.Flush();
            return data;
        }

        public void Load(byte[] data, int start)
        {
            if (start + 0x22 > data.Length) throw new ArgumentOutOfRangeException("start");

            Name = new EncodedString4(data, start, 0x10);
            Version = (Versions)data[0x10 + start];
            Language = (Languages)data[0x11 + start];
            Country = data[0x12 + start];
            Region = data[0x13 + start];
            OT = BitConverter.ToUInt32(data, 0x14 + start);
            byte[] trendyPhrase = new byte[8];
            Array.Copy(data, 0x18 + start, trendyPhrase, 0, 8);
            PhraseLeader = new TrendyPhrase4(trendyPhrase);
            Gender = data[0x20 + start];
            Unknown = data[0x21 + start];
        }
    }
}
