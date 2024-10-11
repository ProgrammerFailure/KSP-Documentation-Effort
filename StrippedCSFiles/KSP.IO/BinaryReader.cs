using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace KSP.IO;

public class BinaryReader : IDisposable
{
	private System.IO.BinaryReader wrapped;

	public Stream BaseStream
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private BinaryReader(System.IO.BinaryReader wrappee)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static BinaryReader CreateForType<T>(string filename, Vessel flight = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadBoolean()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ushort ReadUInt16()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ReadString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Read(byte[] buffer, int index, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Read()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public short ReadInt16()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public byte ReadByte()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int PeekChar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public long ReadInt64()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public double ReadDouble()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uint ReadUInt32()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public byte[] ReadBytes(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float ReadSingle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public decimal ReadDecimal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Read(char[] buffer, int index, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public char ReadChar()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public char[] ReadChars(int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public sbyte ReadSByte()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ulong ReadUInt64()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int ReadInt32()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
