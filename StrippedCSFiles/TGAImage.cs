using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TGAImage
{
	private static Color32[] colorData;

	private static TGAHeader header;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TGAImage()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadImage(string filePath)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool ReadImage(FileInfo file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color32[] ReadImage(TGAHeader header, byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color32[] ReadTrueColorImage(TGAHeader header, byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color32[] ReadRTETrueColorImage(TGAHeader header, byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color32[] ReadRTETrueColorImageAlpha(TGAHeader header, byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Color32[] ReadRTETrueColorImageNoAlpha(TGAHeader header, byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D CreateTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Texture2D CreateTexture(bool mipmap, bool linear, bool compress, bool compressHighQuality, bool allowRead)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~TGAImage()
	{
		throw null;
	}
}
