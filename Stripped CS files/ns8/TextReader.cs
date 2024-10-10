using System;
using System.IO;
using System.Text;

namespace ns8;

public class TextReader : IDisposable
{
	public StreamReader wrapped;

	public Encoding CurrentEncoding => wrapped.CurrentEncoding;

	public Stream BaseStream => wrapped.BaseStream;

	public bool EndOfStream => wrapped.EndOfStream;

	public TextReader(StreamReader wrappee)
	{
		wrapped = wrappee;
	}

	public static TextReader CreateForType<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new TextReader(new StreamReader(filePathFor));
		}
		catch (System.IO.IOException e)
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

	public int Peek()
	{
		try
		{
			return wrapped.Peek();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public string ReadLine()
	{
		try
		{
			return wrapped.ReadLine();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void DiscardBufferedData()
	{
		try
		{
			wrapped.DiscardBufferedData();
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

	public string ReadToEnd()
	{
		try
		{
			return wrapped.ReadToEnd();
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
