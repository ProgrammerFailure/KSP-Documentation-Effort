using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Flow;

public class MEFlowUIGroup_Then : MonoBehaviour
{
	[SerializeField]
	private Image bracket;

	[SerializeField]
	internal Transform ChildHolder;

	private MEFlowThenBlock thenBlock;

	private MEFlowParser parser;

	internal static Object groupPrefab;

	private Color bracketColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowUIGroup_Then()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowUIGroup_Then Create(MEFlowThenBlock thenBlock, MEFlowParser parser)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetBracketColor(Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUnreachable(bool unreachable = true)
	{
		throw null;
	}
}
