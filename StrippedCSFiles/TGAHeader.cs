using System.Runtime.CompilerServices;

public class TGAHeader
{
	public int idLength;

	public bool hasColorMap;

	public TGAImageType imageType;

	public bool rteEncoding;

	public byte[] colorMap;

	public short xOrigin;

	public short yOrigin;

	public ushort width;

	public ushort height;

	public byte pixelDepth;

	public byte imageDesc;

	public int nPixels;

	public int bpp;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TGAHeader()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TGAHeader(byte[] data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public byte[] GetData()
	{
		throw null;
	}
}
