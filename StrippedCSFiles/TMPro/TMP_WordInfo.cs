using System.Runtime.CompilerServices;

namespace TMPro;

public struct TMP_WordInfo
{
	public TMP_Text textComponent;

	public int firstCharacterIndex;

	public int lastCharacterIndex;

	public int characterCount;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetWord()
	{
		throw null;
	}
}
