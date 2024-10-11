using System.Runtime.CompilerServices;

namespace FinePrint.Utilities;

public class PreBuiltCraftPosition
{
	public float CenterLatitude;

	public float CenterLongitude;

	public float SearchRadius;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PreBuiltCraftPosition()
	{
		throw null;
	}
}
