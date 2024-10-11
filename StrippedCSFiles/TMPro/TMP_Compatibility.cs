using System.Runtime.CompilerServices;

namespace TMPro;

public static class TMP_Compatibility
{
	public enum AnchorPositions
	{
		TopLeft,
		Top,
		TopRight,
		Left,
		Center,
		Right,
		BottomLeft,
		Bottom,
		BottomRight,
		BaseLine,
		None
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static TextAlignmentOptions ConvertTextAlignmentEnumValues(TextAlignmentOptions oldValue)
	{
		throw null;
	}
}
