using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HelmetSuitPickerWindow : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI title;

	[SerializeField]
	private TextMeshProUGUI kerbalName;

	[SerializeField]
	private Button closeButton;

	[SerializeField]
	private SuitButton suitButtonPrefab;

	protected CrewListItem crewItem;

	public ProtoCrewMember crew;

	private List<GameObject> previewCamList;

	private int thumbnailCameraMask;

	[SerializeField]
	private float thumbnailCameraSize;

	[NonSerialized]
	public Camera thumbnailCamera;

	[NonSerialized]
	public RenderTexture thumbnailRenderTexture;

	private List<ProtoCrewMember.KerbalSuit> availableSuitTypes;

	public SuitCombos suitCombos;

	public List<SuitButton> suitButtons;

	public List<PreviewType> previewTypes;

	[SerializeField]
	private GameObject prefabComboButton;

	public CrewListItem.KerbalTypes kerbalType;

	private List<GameObject> lightPreviewList;

	private bool isAC;

	private SuitCombo initialSuit;

	private string initialSuitId;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HelmetSuitPickerWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCloseClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CloseWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupSuitTypeButtons(CrewListItem crewListItem, ProtoCrewMember crew, CrewListItem.KerbalTypes kerbalType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitSelection()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InitFromProtoCrewData()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CleanWindow()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void GetAvailableSuits()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupHelmetNeckringButton(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupStockCombos(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupExtraCombos(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupPreview(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateThumbnailCamera(out Camera camRef, float camSize, LayerMask layerMask, out RenderTexture rtRef, int rtSize, int rtDepth, Transform kerbalPreview)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupComboSelector(ComboSelector comboSelector, SuitCombo suitCombo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupComboButton(ComboSelector comboSelector, ComboButton comboButton, SuitCombo suitCombo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OpenSuitLightPicker(SuitLightColorPicker suitLightColorPicker)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnHelmetNeckringSelection(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnComboSelected(ComboSelector comboSelector, string comboId, int comboIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool CheckStock(string comboId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetDefaultCombo(ComboSelector comboSelector, string comboId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetExtraCombo(ComboSelector comboSelector, string comboId)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupGlowColor(SuitButton suitButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateDisplayName(string displayName, ProtoCrewMember.KerbalSuit suitType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string LocalizedSuitText(ProtoCrewMember.KerbalSuit suitType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSceneSwitch(GameEvents.FromToAction<GameScenes, GameScenes> data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupWindowTransform(RectTransform suitPickerWindowRT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnACSpawn()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnACDespawn()
	{
		throw null;
	}
}
