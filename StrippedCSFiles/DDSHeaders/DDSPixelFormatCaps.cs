using System;

namespace DDSHeaders;

[Flags]
public enum DDSPixelFormatCaps : uint
{
	COMPLEX = 8u,
	MIPMAP = 0x400000u,
	TEXTURE = 0x1000u
}
