using System.IO;
using System.Runtime.CompilerServices;

namespace DDSHeaders;

public class DDSPixelFormat
{
	public uint dwSize;

	public uint dwFlags;

	public uint dwFourCC;

	public uint dwRGBBitCount;

	public uint dwRBitMask;

	public uint dwGBitMask;

	public uint dwBBitMask;

	public uint dwABitMask;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DDSPixelFormat(BinaryReader br)
	{
		throw null;
	}
}
