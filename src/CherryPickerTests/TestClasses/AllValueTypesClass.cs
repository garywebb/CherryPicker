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

        public int[] IntArray { get; set; }
        public int?[] NullIntArray { get; set; }
        public short[] ShortArray { get; set; }
        public short?[] NullShortArray { get; set; }
        public bool[] BoolArray { get; set; }
        public bool?[] NullBoolArray { get; set; }
        public byte[] ByteArray { get; set; }
        public byte?[] NullByteArray { get; set; }
        public char[] CharArray { get; set; }
        public char?[] NullCharArray { get; set; }
        public decimal[] DecimalArray { get; set; }
        public decimal?[] NullDecimalArray { get; set; }
        public double[] DoubleArray { get; set; }
        public double?[] NullDoubleArray { get; set; }
        public MyEnum[] MyEnumArray { get; set; }
        public MyEnum?[] NullMyEnumArray { get; set; }
        public float[] FloatArray { get; set; }
        public float?[] NullFloatArray { get; set; }
        public long[] LongArray { get; set; }
        public long?[] NullLongArray { get; set; }
        public sbyte[] SByteArray { get; set; }
        public sbyte?[] NullSByteArray { get; set; }
        public uint[] UIntArray { get; set; }
        public uint?[] NullUIntArray { get; set; }
        public ulong[] ULongArray { get; set; }
        public ulong?[] NullULongArray { get; set; }
        public ushort[] UShortArray { get; set; }
        public ushort?[] NullUShortArray { get; set; }

        public string[] StringArray { get; set; }
    }

    public enum MyEnum
    {
        Value1,
        Value2,
    }
}
