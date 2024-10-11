using System;

namespace DDSHeaders;

[Flags]
public enum DDSPixelFormatFlags : uint
{
	DDPF_RGB = 0x40u,
	DDPF_FOURCC = 4u,
	DDPF_ALPHAPIXELS = 1u,
	DDPF_NORMALA = 0x80000u,
	DDPF_NORMALB = 0x80000000u
}
