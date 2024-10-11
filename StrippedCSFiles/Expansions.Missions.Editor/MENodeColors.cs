using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MENodeColors
{
	public Color startNodeColor;

	public Color categoryDefaultColor;

	public List<MENodeCategoryColor> categoryColors;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MENodeColors()
	{
		throw null;
	}
}
