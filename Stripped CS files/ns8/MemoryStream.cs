using System;
using System.IO;

namespace ns8;

public class MemoryStream
{
	public System.IO.MemoryStream wrapped;

	public long Length => wrapped.Length;

	public int Capacity
	{
		get
		{
			return wrapped.Capacity;
		}
		set
		{
			wrapped.Capacity = value;
		}
	}

	public bool CanRead => wrapped.CanRead;

	public bool CanSeek => wrapped.CanSeek;

	public bool CanWrite => wrapped.CanWrite;

	public long Position
	{
		get
		{
			return wrapped.Position;
		}
		set
		{
			wrapped.Position = value;
		}
	}

	public MemoryStream()
	{
		wrapped = new System.IO.MemoryStream();
	}

	public MemoryStream(byte[] buffer)
	{
		wrapped = new System.IO.MemoryStream(buffer);
	}

	public MemoryStream(System.IO.MemoryStream wrappee)
	{
		wrapped = wrappee;
	}

	public void SetLength(long value)
	{
		try
		{
			wrapped.SetLength(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteByte(byte value)
	{
		try
		{
			wrapped.WriteByte(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int ReadByte()
	{
		try
		{
			return wrapped.ReadByte();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public long Seek(long offset, SeekOrigin loc)
	{
		try
		{
			return wrapped.Seek(offset, IOUtils.ConvertSeek(loc));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public byte[] ToArray()
	{
		try
		{
			return wrapped.ToArray();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public byte[] GetBuffer()
	{
		try
		{
			return wrapped.GetBuffer();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int Read(byte[] buffer, int offset, int count)
	{
		try
		{
			return wrapped.Read(buffer, offset, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(byte[] buffer, int offset, int count)
	{
		try
		{
			wrapped.Write(buffer, offset, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Flush()
	{
		try
		{
			wrapped.Flush();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
