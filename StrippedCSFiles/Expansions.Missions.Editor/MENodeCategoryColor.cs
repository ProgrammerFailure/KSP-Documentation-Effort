using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MENodeCategoryColor
{
	public string name;

	public Color headerColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodeCategoryColor()
	{
		throw null;
	}
}
