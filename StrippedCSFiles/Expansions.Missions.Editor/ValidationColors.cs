using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class ValidationColors
{
	public Color offColor;

	public Color passColor;

	public Color warnColor;

	public Color failColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ValidationColors()
	{
		throw null;
	}
}
