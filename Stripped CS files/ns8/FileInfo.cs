using System;
using System.IO;

namespace ns8;

public class FileInfo
{
	public System.IO.FileInfo wrapped;

	public Type type;

	public Vessel flight;

	public string Name => wrapped.Name;

	public long Length => wrapped.Length;

	public string DirectoryName => wrapped.DirectoryName;

	public bool IsReadOnly
	{
		get
		{
			return wrapped.IsReadOnly;
		}
		set
		{
			wrapped.IsReadOnly = value;
		}
	}

	public bool Exists => wrapped.Exists;

	public FileInfo(System.IO.FileInfo wrappee, Type t, Vessel flight)
	{
		type = t;
		this.flight = flight;
		wrapped = wrappee;
	}

	public static FileInfo CreateForType<T>(string filename, Vessel flight = null)
	{
		try
		{
			string filePathFor = IOUtils.GetFilePathFor(typeof(T), filename, flight);
			string directoryName = Path.GetDirectoryName(filePathFor);
			if (!Directory.Exists(directoryName))
			{
				Directory.CreateDirectory(directoryName);
			}
			return new FileInfo(new System.IO.FileInfo(filePathFor), typeof(T), flight);
		}
		catch (System.IO.IOException e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Encrypt()
	{
		try
		{
			wrapped.Encrypt();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Delete()
	{
		try
		{
			wrapped.Delete();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public override string ToString()
	{
		try
		{
			return wrapped.ToString();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream Create()
	{
		try
		{
			return new FileStream(wrapped.Create());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream OpenWrite()
	{
		try
		{
			return new FileStream(wrapped.OpenWrite());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream Open(FileMode mode, FileAccess access)
	{
		try
		{
			return new FileStream(wrapped.Open(IOUtils.ConvertMode(mode), IOUtils.ConvertAccess(access)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream OpenRead()
	{
		try
		{
			return new FileStream(wrapped.OpenRead());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void MoveTo(string destFileName)
	{
		try
		{
			wrapped.MoveTo(destFileName);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream Open(FileMode mode)
	{
		try
		{
			return new FileStream(wrapped.Open(IOUtils.ConvertMode(mode)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileInfo CopyTo(string destFileName, bool overwrite)
	{
		try
		{
			destFileName = IOUtils.GetFilePathFor(type, destFileName, flight);
			return new FileInfo(wrapped.CopyTo(destFileName, overwrite), type, flight);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
	{
		try
		{
			destinationFileName = IOUtils.GetFilePathFor(type, destinationFileName, flight);
			destinationBackupFileName = IOUtils.GetFilePathFor(type, destinationBackupFileName, flight);
			return new FileInfo(wrapped.Replace(destinationFileName, destinationBackupFileName, ignoreMetadataErrors), type, flight);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
	{
		try
		{
			destinationFileName = IOUtils.GetFilePathFor(type, destinationFileName, flight);
			destinationBackupFileName = IOUtils.GetFilePathFor(type, destinationBackupFileName, flight);
			return new FileInfo(wrapped.Replace(destinationFileName, destinationBackupFileName), type, flight);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public TextWriter CreateText()
	{
		try
		{
			return new TextWriter(wrapped.CreateText());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public void Decrypt()
	{
		try
		{
			wrapped.Decrypt();
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public TextReader OpenText()
	{
		try
		{
			return new TextReader(wrapped.OpenText());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileStream Open(FileMode mode, FileAccess access, FileShare share)
	{
		try
		{
			return new FileStream(wrapped.Open(IOUtils.ConvertMode(mode), IOUtils.ConvertAccess(access), IOUtils.ConvertShare(share)));
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public TextWriter AppendText()
	{
		try
		{
			return new TextWriter(wrapped.AppendText());
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}

	public FileInfo CopyTo(string destFileName)
	{
		try
		{
			destFileName = IOUtils.GetFilePathFor(type, destFileName, flight);
			return new FileInfo(wrapped.CopyTo(destFileName), type, flight);
		}
		catch (Exception e)
		{
			throw IOUtils.WrapException(e);
		}
	}
}
