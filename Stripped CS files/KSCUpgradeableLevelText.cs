using System;
using UnityEngine;

[Serializable]
public class KSCUpgradeableLevelText : ScriptableObject
{
	[HideInInspector]
	public string textBase;

	[HideInInspector]
	public SpaceCenterFacility facility;

	[HideInInspector]
	public string linePrefix = "*";
}
