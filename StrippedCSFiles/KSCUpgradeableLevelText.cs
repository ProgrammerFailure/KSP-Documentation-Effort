using System;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class KSCUpgradeableLevelText : ScriptableObject
{
	[HideInInspector]
	public string textBase;

	[HideInInspector]
	public SpaceCenterFacility facility;

	[HideInInspector]
	public string linePrefix;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSCUpgradeableLevelText()
	{
		throw null;
	}
}
