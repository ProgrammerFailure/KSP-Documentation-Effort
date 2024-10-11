using System;

namespace DDSHeaders;

[Flags]
public enum DDSHeaderDX10MiscFlags : uint
{
	COMPLEX = 8u,
	MIPMAP = 0x400000u,
	TEXTURE = 0x1000u
}
