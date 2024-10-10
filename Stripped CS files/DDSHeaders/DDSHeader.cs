using System.IO;

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

	public DDSHeader(BinaryReader br)
	{
		dwSize = br.ReadUInt32();
		dwFlags = br.ReadUInt32();
		dwHeight = br.ReadUInt32();
		dwWidth = br.ReadUInt32();
		dwPitchOrLinearSize = br.ReadUInt32();
		dwDepth = br.ReadUInt32();
		dwMipMapCount = br.ReadUInt32();
		dwReserved1 = new uint[11];
		for (int i = 0; i < 11; i++)
		{
			dwReserved1[i] = br.ReadUInt32();
		}
		ddspf = new DDSPixelFormat(br);
		dwCaps = (DDSPixelFormatCaps)br.ReadUInt32();
		dwCaps2 = (DDSPixelFormatCaps2)br.ReadUInt32();
		dwCaps3 = br.ReadUInt32();
		dwCaps4 = br.ReadUInt32();
		dwReserved2 = br.ReadUInt32();
	}
}
