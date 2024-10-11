using System.IO;
using System.Runtime.CompilerServices;

namespace DDSHeaders;

public class DDSHeader
{
	public uint dwSize;

	public uint dwFlags;

	public uint dwHeight;

	public uint dwWidth;

	public uint dwPitchOrLinearSize;

	public uint dwDepth;

	public uint dwMipMapCount;

	public uint[] dwReserved1;

	public DDSPixelFormat ddspf;

	public DDSPixelFormatCaps dwCaps;

	public DDSPixelFormatCaps2 dwCaps2;

	public uint dwCaps3;

	public uint dwCaps4;

	public uint dwReserved2;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DDSHeader(BinaryReader br)
	{
		throw null;
	}
}
