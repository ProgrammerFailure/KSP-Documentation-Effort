using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace KSP.IO;

public class TextReader : IDisposable
{
	private StreamReader wrapped;

	public Encoding CurrentEncoding
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Stream BaseStream
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool EndOfStream
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal TextReader(StreamReader wrappee)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TextReader CreateForType<T>(string filename, Vessel flight = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Read()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Peek()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ReadLine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DiscardBufferedData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int Read(char[] buffer, int index, int count)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Close()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string ReadToEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Dispose()
	{
		throw null;
	}
}
