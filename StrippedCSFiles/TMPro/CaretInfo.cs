using System.Runtime.CompilerServices;

namespace TMPro;

public struct CaretInfo
{
	public int index;

	public CaretPosition position;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CaretInfo(int index, CaretPosition position)
	{
		throw null;
	}
}
