using System.Runtime.CompilerServices;

namespace TMPro;

public struct TMP_LinkInfo
{
	public TMP_Text textComponent;

	public int hashCode;

	public int linkIdFirstCharacterIndex;

	public int linkIdLength;

	public int linkTextfirstCharacterIndex;

	public int linkTextLength;

	internal char[] linkID;

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetLinkID(char[] text, int startIndex, int length)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLinkText()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public string GetLinkID()
	{
		throw null;
	}
}
