using System.Runtime.CompilerServices;

namespace TMPro;

public struct TMP_BasicXmlTagStack
{
	public byte bold;

	public byte italic;

	public byte underline;

	public byte strikethrough;

	public byte highlight;

	public byte superscript;

	public byte subscript;

	public byte uppercase;

	public byte lowercase;

	public byte smallcaps;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public byte Add(FontStyles style)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public byte Remove(FontStyles style)
	{
		throw null;
	}
}
