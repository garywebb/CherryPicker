using System;
using System.Collections.Generic;
using System.Text;

namespace CherryPickerTests.TestClasses
{
    public class AllValueTypesClass
    {
        public int Int { get; set; }
        public int? NullInt { get; set; }
        public short Short { get; set; }
        public short? NullShort { get; set; }
        public bool Bool { get; set; }
        public bool? NullBool { get; set; }
        public byte Byte { get; set; }
        public byte? NullByte { get; set; }
        public char Char { get; set; }
        public char? NullChar { get; set; }
        public decimal Decimal { get; set; }
        public decimal? NullDecimal { get; set; }
        public double Double { get; set; }
        public double? NullDouble { get; set; }
        public MyEnum MyEnum { get; set; }
        public MyEnum? NullMyEnum { get; set; }
        public float Float { get; set; }
        public float? NullFloat { get; set; }
        public long Long { get; set; }
        public long? NullLong { get; set; }
        public sbyte SByte { get; set; }
        public sbyte? NullSByte { get; set; }
        public uint UInt { get; set; }
        public uint? NullUInt { get; set; }
        public ulong ULong { get; set; }
        public ulong? NullULong { get; set; }
        public ushort UShort { get; set; }
        public ushort? NullUShort { get; set; }

        public string String { get; set; }
    }

    public enum MyEnum
    {
        Value1,
        Value2,
    }
}
