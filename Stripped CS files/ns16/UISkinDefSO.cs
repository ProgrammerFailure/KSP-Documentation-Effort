using UnityEngine;

namespace ns16;

public class UISkinDefSO : ScriptableObject
{
	[SerializeField]
	public UISkinDef skinDef;

	public UISkinDef SkinDef => skinDef;
}
