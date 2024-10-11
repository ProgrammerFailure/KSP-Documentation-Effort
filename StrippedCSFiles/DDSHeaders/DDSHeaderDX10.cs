using System.IO;
using System.Runtime.CompilerServices;

namespace DDSHeaders;

public class DDSHeaderDX10
{
	public DXGI_FORMAT dxgiFormat;

	public D3D10_RESOURCE_DIMENSION resourceDimension;

	public DDSHeaderDX10MiscFlags miscFlag;

	public uint arraySize;

	public uint miscFlags2;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DDSHeaderDX10(BinaryReader br)
	{
		throw null;
	}
}
