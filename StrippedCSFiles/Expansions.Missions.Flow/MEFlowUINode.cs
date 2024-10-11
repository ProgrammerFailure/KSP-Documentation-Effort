using System.Runtime.CompilerServices;
using KSP.UI;
using KSP.UI.TooltipTypes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Expansions.Missions.Flow;

public class MEFlowUINode : MonoBehaviour
{
	public enum ButtonAction
	{
		None,
		ToggleDetails,
		Callback,
		CallbackToggle
	}

	internal static Object nodePrefab;

	internal static Object nodeTogglePrefab;

	public TMP_Text title;

	public TMP_Text description;

	public TooltipController_Text completedUT;

	public GameObject leader;

	public GameObject leaderComplete;

	public GameObject leaderEvent;

	public GameObject leaderNext;

	public GameObject leaderEnd;

	public GameObject leaderNextAndEnd;

	public GameObject leaderVessel;

	public Image iconComplete;

	public Image iconEvent;

	public Image unreachableOverlay;

	public Button titleButton;

	public Callback<PointerEventData> buttonCallback;

	public ButtonAction buttonAction;

	public Toggle toggle;

	public Callback<MEFlowUINode> toggleCallback;

	[SerializeField]
	private TooltipController_Text topLineTooltip;

	private PointerClickHandler titleButtonHandler;

	private MENode node;

	private MEFlowParser parser;

	private bool selected;

	public MENode Node
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEFlowUINode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowUINode Create(MENode node, ButtonAction buttonAction, Callback<PointerEventData> buttonCallback, MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static MEFlowUINode Create(MENode node, Callback<MEFlowUINode> toggleCallback, ToggleGroup toggleGroup, MEFlowParser parser)
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnToggle(bool st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnButtonClick(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNode(MENode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetNodeInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStatus(MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnUpdateFlowUI(MEFlowParser parser)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselNodeButtonCallback(PointerEventData data)
	{
		throw null;
	}
}
