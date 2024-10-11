using System.Runtime.CompilerServices;

namespace TMPro;

public struct KerningPairKey
{
	public uint ascii_Left;

	public uint ascii_Right;

	public uint key;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerningPairKey(uint ascii_left, uint ascii_right)
	{
		throw null;
	}
}
