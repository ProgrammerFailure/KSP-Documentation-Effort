using System.Runtime.CompilerServices;
using UnityEngine;

namespace EdyCommonTools;

public class HelpBoxAttribute : PropertyAttribute
{
	public string text;

	public HelpBoxMessageType messageType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HelpBoxAttribute(string text, HelpBoxMessageType messageType = HelpBoxMessageType.None)
	{
		throw null;
	}
}
