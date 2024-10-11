using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class ActionGroupsPanel : MonoBehaviour
{
	public ToggleGroup overrideButtonGroup;

	public Toggle overrideButtonPrefab;

	public Button editActionGroupsButtonPrefab;

	public GameObject spacerPrefab;

	private Toggle[] agToggles;

	private Button editActionGroupsButton;

	private GameObject spacer;

	private List<GameObject> actionPanelObjects;

	private List<Selectable> selectables;

	private PointerEnterExitHandler pointerEnterExitHandler;

	public static bool actionGroupPanelOpen;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ActionGroupsPanel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEnable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetGroupName(Toggle toggle, string groupName, int groupIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateButtons(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SelectOverride(int groupOverride, bool on)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateButtons()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateActionGroups()
	{
		throw null;
	}
}
