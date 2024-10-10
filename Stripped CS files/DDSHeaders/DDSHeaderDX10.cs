using System.IO;

namespace DDSHeaders;

public class DDSHeaderDX10
{
	public DXGI_FORMAT dxgiFormat;

	public D3D10_RESOURCE_DIMENSION resourceDimension;

	public DDSHeaderDX10MiscFlags miscFlag;

	public uint arraySize;

	public uint miscFlags2;

	public DDSHeaderDX10(BinaryReader br)
	{
		dxgiFormat = (DXGI_FORMAT)br.ReadUInt32();
		resourceDimension = (D3D10_RESOURCE_DIMENSION)br.ReadUInt32();
		miscFlag = (DDSHeaderDX10MiscFlags)br.ReadUInt32();
		arraySize = br.ReadUInt32();
		miscFlags2 = br.ReadUInt32();
	}
}
