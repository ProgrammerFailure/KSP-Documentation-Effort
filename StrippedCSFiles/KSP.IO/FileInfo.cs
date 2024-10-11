using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace KSP.IO;

public class FileInfo
{
	private System.IO.FileInfo wrapped;

	private Type type;

	private Vessel flight;

	public string Name
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public long Length
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string DirectoryName
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsReadOnly
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	public bool Exists
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private FileInfo(System.IO.FileInfo wrappee, Type t, Vessel flight)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static FileInfo CreateForType<T>(string filename, Vessel flight = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Encrypt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Delete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string ToString()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream Create()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream OpenWrite()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream Open(FileMode mode, FileAccess access)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream OpenRead()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MoveTo(string destFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream Open(FileMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileInfo CopyTo(string destFileName, bool overwrite)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileInfo Replace(string destinationFileName, string destinationBackupFileName, bool ignoreMetadataErrors)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileInfo Replace(string destinationFileName, string destinationBackupFileName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextWriter CreateText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Decrypt()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextReader OpenText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileStream Open(FileMode mode, FileAccess access, FileShare share)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TextWriter AppendText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FileInfo CopyTo(string destFileName)
	{
		throw null;
	}
}
