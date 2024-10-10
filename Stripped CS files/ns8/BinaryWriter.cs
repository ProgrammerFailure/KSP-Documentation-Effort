using System;
using System.IO;

namespace ns8;

public class BinaryWriter : IDisposable
{
	public System.IO.BinaryWriter wrapped;

	public Stream BaseStream => wrapped.BaseStream;

	public BinaryWriter(System.IO.BinaryWriter wrappee)
	{
		wrapped = wrappee;
	}

	public static BinaryWriter CreateForType<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new BinaryWriter(new System.IO.BinaryWriter(System.IO.File.OpenWrite(filePathFor)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public long Seek(int offset, SeekOrigin origin)
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

	public void Dispose()
	{
		try
		{
			wrapped.Close();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(double value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(decimal value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(float value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(uint value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(string value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(ulong value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char ch)
	{
		try
		{
			wrapped.Write(ch);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(int value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char[] chars, int index, int count)
	{
		try
		{
			wrapped.Write(chars, index, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(ushort value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char[] chars)
	{
		try
		{
			wrapped.Write(chars);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(sbyte value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(byte value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(bool value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Close()
	{
		try
		{
			wrapped.Close();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(byte[] buffer)
	{
		try
		{
			wrapped.Write(buffer);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(byte[] buffer, int index, int count)
	{
		try
		{
			wrapped.Write(buffer, index, count);
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

	public void Write(long value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(short value)
	{
		try
		{
			wrapped.Write(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
