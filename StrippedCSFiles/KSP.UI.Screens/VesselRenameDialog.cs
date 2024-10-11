using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class VesselRenameDialog : MonoBehaviour
{
	[SerializeField]
	private TMP_InputField nameField;

	private PointerClickHandler nameFieldClickHandler;

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
	private TypeButton toggleSpaceObj;

	[SerializeField]
	private TypeButton toggleDebris;

	[SerializeField]
	private TypeButton toggleAircraft;

	[SerializeField]
	private TypeButton toggleCommunicationsRelay;

	[SerializeField]
	private TypeButton toggleDeployedScience;

	[SerializeField]
	private TMP_Text title;

	[SerializeField]
	private GameObject NamePriorityControls;

	[SerializeField]
	private TMP_Text priorityValue;

	[SerializeField]
	private Slider prioritySlider;

	private bool adjustingViaPart;

	[SerializeField]
	private Button buttonAccept;

	[SerializeField]
	private Button buttonCancel;

	[SerializeField]
	private Button buttonRemove;

	private VesselType vesselType;

	private string vesselName;

	private bool hasValidName;

	[SerializeField]
	private bool allowTypeChange;

	private VesselType lowestType;

	private Callback<string, VesselType> onAccept;

	private Callback onDismiss;

	private Callback onRemove;

	private Callback<string, VesselType, int> onAcceptPart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public VesselRenameDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselRenameDialog Spawn(Vessel v, Callback<string, VesselType> onAccept, Callback onDismiss, bool allowTypeChange, VesselType lowestType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static VesselRenameDialog SpawnNameFromPart(Part p, Callback<string, VesselType, int> onAccept, Callback onDismiss, Callback onRemove, bool allowTypeChange, VesselType lowestType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPriorityChanged(float newValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Terminate()
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
	protected void OnNameFieldModified(string newName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnNameFieldEndEdit(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnNameFieldSelected(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonAccept()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnButtonRemove()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Update()
	{
		throw null;
	}
}
