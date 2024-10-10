using System;
using System.Collections.Generic;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MENodeColors
{
	public Color startNodeColor;

	public Color categoryDefaultColor;

	public List<MENodeCategoryColor> categoryColors;
}
