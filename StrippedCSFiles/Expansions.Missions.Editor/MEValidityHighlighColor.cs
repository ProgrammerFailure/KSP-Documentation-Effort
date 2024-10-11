using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEValidityHighlighColor
{
	public ValidationStatus status;

	public Color highlightColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEValidityHighlighColor()
	{
		throw null;
	}
}
