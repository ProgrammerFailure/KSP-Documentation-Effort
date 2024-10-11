using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar.Screens.Debug;

public class ScreenVesselMassInfo : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI activeVesselName_text;

	[SerializeField]
	private TextMeshProUGUI vesselMass_text;

	[SerializeField]
	private Button updateStats;

	[SerializeField]
	private RectTransform contentParent;

	[SerializeField]
	private GameObject partItemPrefab;

	[SerializeField]
	private List<ScreenVesselMassPartInfo> screenObjects;

	public bool resetScreenObjects;

	[SerializeField]
	private Vessel vessel;

	[SerializeField]
	private ShipConstruct ship;

	private bool noVessel;

	private string cached_autoLOC_901099;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenVesselMassInfo()
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
	private void ClearScreenObjects()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FindVessel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorRestoreState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorLoad(ShipConstruct ship, CraftBrowserDialog.LoadType loadType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onEditorPodPicked(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeVessel(Vessel vessel)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeShip(ShipConstruct ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ChangeCrew(VesselCrewManifest crew)
	{
		throw null;
	}
}
