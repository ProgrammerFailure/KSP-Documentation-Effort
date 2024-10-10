using System;
using System.IO;

namespace ns8;

public class File
{
	public static bool Exists<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return System.IO.File.Exists(filePathFor);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static void Delete<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			System.IO.File.Delete(filePathFor);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static string[] ReadAllLines<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return System.IO.File.ReadAllLines(filePathFor);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static byte[] ReadAllBytes<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return System.IO.File.ReadAllBytes(filePathFor);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static string ReadAllText<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return System.IO.File.ReadAllText(filePathFor);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static void WriteAllLines<T>(string[] data, string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			System.IO.File.WriteAllLines(filePathFor, data);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static void WriteAllBytes<T>(byte[] data, string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			System.IO.File.WriteAllBytes(filePathFor, data);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static void WriteAllText<T>(string data, string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			System.IO.File.WriteAllText(filePathFor, data);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static void AppendAllText<T>(string data, string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			System.IO.File.AppendAllText(filePathFor, data);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static TextWriter AppendText<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new TextWriter(System.IO.File.AppendText(filePathFor));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static FileStream Create<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new FileStream(System.IO.File.Create(filePathFor));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static TextWriter CreateText<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new TextWriter(System.IO.File.CreateText(filePathFor));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static FileStream Open<T>(string filename, FileMode mode, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new FileStream(System.IO.File.Open(filePathFor, IOUtils.ConvertMode(mode)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static TextReader OpenText<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new TextReader(System.IO.File.OpenText(filePathFor));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public static FileStream OpenWrite<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new FileStream(System.IO.File.OpenWrite(filePathFor));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
