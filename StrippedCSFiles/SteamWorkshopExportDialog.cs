using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SteamWorkshopExportDialog : MonoBehaviour
{
	public class ReturnItems
	{
		public bool setPublic;

		public string changeLog;

		public string modsText;

		public VesselType vesselType;

		public bool roboticTag;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ReturnItems()
		{
			throw null;
		}
	}

	[Header("UI Components")]
	[SerializeField]
	private TextMeshProUGUI textHeader;

	[SerializeField]
	private TextMeshProUGUI textDescription;

	[SerializeField]
	private TextMeshProUGUI textCancel;

	[SerializeField]
	private Button buttonCancel;

	[SerializeField]
	private TextMeshProUGUI textConfirmation;

	[SerializeField]
	private Button buttonConfirmation;

	[SerializeField]
	private TextMeshProUGUI textWorkshopVisibility;

	[SerializeField]
	private Toggle toggleWorkshopVisibility;

	[SerializeField]
	private GameObject steamChangeLogObject;

	[SerializeField]
	private TextMeshProUGUI steamChangeLog;

	[SerializeField]
	private GameObject modsObject;

	[SerializeField]
	private TextMeshProUGUI modsText;

	[SerializeField]
	private Button modsPopulateButton;

	[SerializeField]
	private Button steamLegalAgreement;

	[SerializeField]
	private GameObject vesselTypeOptions;

	private TypeButton selectedToggle;

	[SerializeField]
	private ToggleGroup typeIconsGroup;

	[SerializeField]
	private TypeButton toggleShip;

	[SerializeField]
	private TypeButton toggleLander;

	[SerializeField]
	private TypeButton toggleRover;

	[SerializeField]
	private TypeButton toggleStation;

	[SerializeField]
	private TypeButton toggleProbe;

	[SerializeField]
	private TypeButton toggleBase;

	[SerializeField]
	private TypeButton toggleAircraft;

	[SerializeField]
	private TypeButton toggleCommunicationsRelay;

	[SerializeField]
	private Toggle roboticTagButton;

	public bool modal;

	private CanvasGroup canvasGroup;

	private Callback<ReturnItems> onOk;

	private Callback<bool> onCancel;

	private bool showVisibilityOption;

	private bool showChangelog;

	private bool showVesselTypeSection;

	private VesselType vesselType;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SteamWorkshopExportDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SteamWorkshopExportDialog Spawn(string title, string message, Callback<ReturnItems> onOk, Callback<bool> onCancel, bool showCancelBtn = true, bool showVisibilityOption = true, bool showChangeLog = true, bool showModsSection = false, bool showVesselTypeSection = false, VesselType vesselType = VesselType.Ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SteamWorkshopExportDialog Spawn(string title, string message, Callback<ReturnItems> onOk, bool showCancelBtn = true, bool showVisibilityOption = true, bool showChangeLog = true, bool showModsSection = false, bool showVesselTypeSection = false, VesselType vesselType = VesselType.Ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static SteamWorkshopExportDialog Spawn(string title, string message, string textCancel, string textOK, string textDontShowAgain, Callback<ReturnItems> onOk, Callback<bool> onCancel, bool showCancelBtn = true, bool showVisibilityOption = true, bool showChangeLog = true, bool showModsSection = false, bool showVesselTypeSection = false, VesselType vesselType = VesselType.Ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void ToggleSetup(TypeButton t, UnityAction<bool> onValueChangedCallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnToggle(TypeButton t, bool b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnCancel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onBtnLegalAgreement()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnModsList()
	{
		throw null;
	}
}
