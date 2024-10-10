using System;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEValidityHighlighColor
{
	public ValidationStatus status;

	public Color highlightColor = Color.gray;
}
