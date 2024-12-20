using System;

namespace DDSHeaders;

[Flags]
public enum DDSPixelFormatCaps2 : uint
{
	CUBEMAP = 0x200u,
	CUBEMAP_POSITIVEX = 0x400u,
	CUBEMAP_NEGATIVEX = 0x800u,
	CUBEMAP_POSITIVEY = 0x1000u,
	CUBEMAP_NEGATIVEY = 0x2000u,
	CUBEMAP_POSITIVEZ = 0x4000u,
	CUBEMAP_NEGATIVEZ = 0x8000u,
	CUBEMAP_VOLUME = 0x200000u
}
