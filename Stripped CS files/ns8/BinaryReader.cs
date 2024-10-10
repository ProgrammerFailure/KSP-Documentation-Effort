using System;
using System.IO;

namespace ns8;

public class BinaryReader : IDisposable
{
	public System.IO.BinaryReader wrapped;

	public Stream BaseStream => wrapped.BaseStream;

	public BinaryReader(System.IO.BinaryReader wrappee)
	{
		wrapped = wrappee;
	}

	public static BinaryReader CreateForType<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new BinaryReader(new System.IO.BinaryReader(System.IO.File.OpenRead(filePathFor)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public bool ReadBoolean()
	{
		try
		{
			return wrapped.ReadBoolean();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public ushort ReadUInt16()
	{
		try
		{
			return wrapped.ReadUInt16();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public string ReadString()
	{
		try
		{
			return wrapped.ReadString();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int Read(byte[] buffer, int index, int count)
	{
		try
		{
			return wrapped.Read(buffer, index, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int Read()
	{
		try
		{
			return wrapped.Read();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public short ReadInt16()
	{
		try
		{
			return wrapped.ReadInt16();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public byte ReadByte()
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

	public int PeekChar()
	{
		try
		{
			return wrapped.PeekChar();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public long ReadInt64()
	{
		try
		{
			return wrapped.ReadInt64();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public double ReadDouble()
	{
		try
		{
			return wrapped.ReadDouble();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public uint ReadUInt32()
	{
		try
		{
			return wrapped.ReadUInt32();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public byte[] ReadBytes(int count)
	{
		try
		{
			return wrapped.ReadBytes(count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public float ReadSingle()
	{
		try
		{
			return wrapped.ReadSingle();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public decimal ReadDecimal()
	{
		try
		{
			return wrapped.ReadDecimal();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int Read(char[] buffer, int index, int count)
	{
		try
		{
			return wrapped.Read(buffer, index, count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public char ReadChar()
	{
		try
		{
			return wrapped.ReadChar();
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

	public char[] ReadChars(int count)
	{
		try
		{
			return wrapped.ReadChars(count);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public sbyte ReadSByte()
	{
		try
		{
			return wrapped.ReadSByte();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public ulong ReadUInt64()
	{
		try
		{
			return wrapped.ReadUInt64();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public int ReadInt32()
	{
		try
		{
			return wrapped.ReadInt32();
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
