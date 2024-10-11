using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Util;

public class UISkinDefSO : ScriptableObject
{
	[SerializeField]
	private UISkinDef skinDef;

	public UISkinDef SkinDef
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UISkinDefSO()
	{
		throw null;
	}
}
