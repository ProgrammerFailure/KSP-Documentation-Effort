using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Threading;
using KSP.UI;
using KSP.UI.Screens;
using UnityEngine;
using UnityEngine.EventSystems;

public class ManeuverTool : UIApp
{
	public static ManeuverTool Instance;

	public static bool Ready;

	[SerializeField]
	private ManeuverToolUIFrame appFramePrefab;

	public ManeuverToolUIFrame appFrame;

	private RectTransform appRectTransform;

	[SerializeField]
	private int appWidth;

	[SerializeField]
	private int appHeight;

	private DictionaryValueList<string, TransferTypeBase> transferTypes;

	public TransferTypeBase currentSelectedTransferType;

	public bool currentTimeDisplayUT;

	public bool currentTimeDisplaySeconds;

	private DictionaryValueList<string, ManeuverNode> positions;

	private bool creatingManNode;

	[SerializeField]
	private GameObject[] transferVisualPrefabs;

	private bool priorManeuversDeleteReq;

	private bool initialHeightSet;

	private bool createAlarm;

	private static Thread mainThread;

	private static object lockObject;

	private static readonly Queue actions;

	public bool isMainThread
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ManeuverTool()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static ManeuverTool()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppInitialized()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAppDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override bool OnAppAboutToStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override ApplicationLauncher.AppScenes GetAppScenes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override Vector3 GetAppScreenPos(Vector3 defaultAnchorPos)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Reposition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ReadjustPosition()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Initial_MouseInput_PointerExit(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GameObject GetTransferVisualPrefab(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnManeuverChanged(Vessel vsl, PatchedConicSolver solver)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnVesselSwitching(Vessel oldVsl, Vessel newVsl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GenerateTransferTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PopulateTransferTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PopulatePositionList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnTransferTypeChanged(int itemIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPositionChanged(int itemIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ChangeTime(bool UT, bool seconds)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateManeuver(bool createAlarm)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPriorCheckOk()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPriorCheckCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FollowingManevuersCheck()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFollowingCheckOk()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFollowingLeave()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFollowingCheckCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateManeuverChecksComplete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool IsAnyDropdownOpen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DeletePriorManeuvers()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void InvokeAsync(Action action)
	{
		throw null;
	}
}
