using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Flow;

public class MEFlowUIGroup_Or : MonoBehaviour
{
	[SerializeField]
	internal Transform ChildHolder;

	internal static Object groupPrefab;

	private MEFlowOrBlock orBlock;

	private MEFlowParser parser;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowUIGroup_Or()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowUIGroup_Or Create(MEFlowOrBlock orBlock, MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateFlowUI(MEFlowParser parser)
	{
		throw null;
	}
}
