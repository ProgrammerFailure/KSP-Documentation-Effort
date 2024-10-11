using System;

namespace DDSHeaders;

[Flags]
public enum DDSHeaderDX10MiscFlags2 : uint
{
	DDS_ALPHA_MODE_UNKNOWN = 0u,
	DDS_ALPHA_MODE_STRAIGHT = 1u,
	DDS_ALPHA_MODE_PREMULTIPLIED = 2u,
	DDS_ALPHA_MODE_OPAQUE = 3u,
	DDS_ALPHA_MODE_CUSTOM = 4u
}
