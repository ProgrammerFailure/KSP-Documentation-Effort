using System.IO;

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

	public DDSPixelFormat(BinaryReader br)
	{
		dwSize = br.ReadUInt32();
		dwFlags = br.ReadUInt32();
		dwFourCC = br.ReadUInt32();
		dwRGBBitCount = br.ReadUInt32();
		dwRBitMask = br.ReadUInt32();
		dwGBitMask = br.ReadUInt32();
		dwBBitMask = br.ReadUInt32();
		dwABitMask = br.ReadUInt32();
	}
}
