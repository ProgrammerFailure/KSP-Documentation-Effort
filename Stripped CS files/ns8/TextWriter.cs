using System;
using System.IO;
using System.Text;

namespace ns8;

public class TextWriter : IDisposable
{
	public StreamWriter writer;

	public System.IO.TextWriter wrapped;

	public Encoding Encoding => wrapped.Encoding;

	public string NewLine
	{
		get
		{
			return wrapped.NewLine;
		}
		set
		{
			wrapped.NewLine = value;
		}
	}

	public IFormatProvider FormatProvider => wrapped.FormatProvider;

	public TextWriter(System.IO.TextWriter wrappee)
	{
		wrapped = wrappee;
	}

	public static TextWriter CreateForType<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new TextWriter(new StreamWriter(filePathFor));
		}
		catch (System.IO.IOException e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine()
	{
		try
		{
			wrapped.WriteLine();
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

	public void Write(string format, object arg0, object arg1)
	{
		try
		{
			wrapped.Write(format, arg0, arg1);
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

	public void WriteLine(long value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(string format, object arg0)
	{
		try
		{
			wrapped.WriteLine(format, arg0);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(string format, params object[] arg)
	{
		try
		{
			wrapped.WriteLine(format, arg);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(decimal value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(bool value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(char value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(double value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char[] buffer)
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

	public void WriteLine(char[] buffer, int index, int count)
	{
		try
		{
			wrapped.WriteLine(buffer, index, count);
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

	public void Write(object value)
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

	public void Write(string format, object arg0, object arg1, object arg2)
	{
		try
		{
			wrapped.Write(format, arg0, arg1, arg2);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(ulong value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(object value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char[] buffer, int index, int count)
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

	public void WriteLine(string format, object arg0, object arg1)
	{
		try
		{
			wrapped.WriteLine(format, arg0, arg1);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(string format, object arg0, object arg1, object arg2)
	{
		try
		{
			wrapped.WriteLine(format, arg0, arg1, arg2);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(float value)
	{
		try
		{
			wrapped.WriteLine(value);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(string format, params object[] arg)
	{
		try
		{
			wrapped.Write(format, arg);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(char[] buffer)
	{
		try
		{
			wrapped.WriteLine(buffer);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Write(char value)
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

	public void WriteLine(string value)
	{
		try
		{
			wrapped.WriteLine(value);
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

	public void WriteLine(uint value)
	{
		try
		{
			wrapped.WriteLine(value);
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
			wrapped.Dispose();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void WriteLine(int value)
	{
		try
		{
			wrapped.WriteLine(value);
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

	public void Write(string format, object arg0)
	{
		try
		{
			wrapped.Write(format, arg0);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
