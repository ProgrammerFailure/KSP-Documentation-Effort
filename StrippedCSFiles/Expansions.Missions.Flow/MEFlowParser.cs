using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Flow;

public class MEFlowParser : MonoBehaviour
{
	[SerializeField]
	internal Transform flowObjectsParent;

	[SerializeField]
	private TMP_Text emptyObjectivesMessage;

	private static Object parserPrefab;

	private Object textPrefab;

	public bool showNonObjectives;

	public bool showEvents;

	private Mission mission;

	public Color[] GroupColors;

	private MEFlowUINode.ButtonAction buttonAction;

	private ToggleGroup toggleGroup;

	private Callback<MEFlowUINode> toggleCallback;

	private Callback<PointerEventData> buttonCallback;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowParser()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateMissionFlowUI_Toggle(Mission mission, Callback<MEFlowUINode> toggleCallback, ToggleGroup toggleGroup, bool showEvents = false, bool showNonObjectives = false, bool showStartNodes = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateMissionFlowUI_Button(Mission mission, MEFlowUINode.ButtonAction buttonAction = MEFlowUINode.ButtonAction.None, Callback<PointerEventData> buttonCallback = null, bool showEvents = false, bool showNonObjectives = false, bool showStartNodes = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateMissionFlowUI(Mission mission, bool showEvents, bool showNonObjectives, bool showStartNodes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateFlowStartNodes(MENode startNode, Transform parent, int colorIndex = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ClearChildren()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowParser Create(Transform parent, Transform flowObjectsParent, TMP_Text emptyObjectivesMessage)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowParser Create(Transform parent, Transform flowObjectsParent, TMP_Text emptyObjectivesMessage, Color[] groupColors)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowBlock ParseMission(Mission mission)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<MEPath> BuildPathsByDFS(MENode startNode, HashSet<MENode> visited)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<MEPath> BuildReversePathsByDFS(MENode startNode, HashSet<MENode> visited)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static MEFlowThenBlock ParseBlock(List<MEPath> paths, MENode insertNode = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static List<MEPathGroup> ExtractPathGroups(List<MEPath> paths)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateFlowUIBlock(IMEFlowBlock start, Transform parent, int colorIndex = -1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFlowUIItems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFlowUIItems(IMEFlowBlock start)
	{
		throw null;
	}
}
