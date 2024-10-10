using System;
using System.IO;

namespace ns8;

public class FileStream : IDisposable
{
	public System.IO.FileStream wrapped;

	public string Name => wrapped.Name;

	public long Length => wrapped.Length;

	public bool IsAsync => wrapped.IsAsync;

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

	public FileStream(System.IO.FileStream wrappee)
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

	public long Seek(long offset, SeekOrigin origin)
	{
		try
		{
			return wrapped.Seek(offset, IOUtils.ConvertSeek(origin));
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

	public IAsyncResult BeginRead(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
	{
		try
		{
			return wrapped.BeginRead(array, offset, numBytes, userCallback, stateObject);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Unlock(long position, long length)
	{
		try
		{
			wrapped.Unlock(position, length);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public IAsyncResult BeginWrite(byte[] array, int offset, int numBytes, AsyncCallback userCallback, object stateObject)
	{
		try
		{
			return wrapped.BeginWrite(array, offset, numBytes, userCallback, stateObject);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Lock(long position, long length)
	{
		try
		{
			wrapped.Lock(position, length);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void EndWrite(IAsyncResult asyncResult)
	{
		try
		{
			wrapped.EndWrite(asyncResult);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(byte[] array, int offset, int count)
	{
		try
		{
			wrapped.Write(array, offset, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int Read(byte[] array, int offset, int count)
	{
		try
		{
			return wrapped.Read(array, offset, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int EndRead(IAsyncResult asyncResult)
	{
		try
		{
			return wrapped.EndRead(asyncResult);
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

	public void Dispose()
	{
		wrapped.Close();
	}
}
