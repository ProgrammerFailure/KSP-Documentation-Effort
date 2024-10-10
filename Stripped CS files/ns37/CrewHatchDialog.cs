using System.Collections.Generic;
using ns2;
using ns9;
using TMPro;
using UnityEngine;

namespace ns37;

public class CrewHatchDialog : MonoBehaviour
{
	[SerializeField]
	public TextMeshProUGUI textHeader;

	[SerializeField]
	public TextMeshProUGUI textModuleCrew;

	[SerializeField]
	public GameObject widgetPrefab;

	[SerializeField]
	public GameObject emptyModuleObject;

	[SerializeField]
	public TextMeshProUGUI emptyModuleText;

	[SerializeField]
	public Transform listContainer;

	[SerializeField]
	public XSelectable hoverArea;

	public bool showTransfer;

	public bool showEVA;

	public Callback<ProtoCrewMember> onEVAEvtHandler;

	public Callback<ProtoCrewMember> onTransferEvtHandler;

	public Callback onDestroyEvtHandler;

	public Part part;

	public List<CrewHatchDialogWidget> widgets;

	public bool hover => hoverArea.Hover;

	public Part Part => part;

	public static CrewHatchDialog Spawn(Part p, bool showEVA, bool showTransfer, Callback<ProtoCrewMember> onEVAEvtHandler, Callback<ProtoCrewMember> onTransferEvtHandler, Callback onDismiss)
	{
		CrewHatchDialog component = Object.Instantiate(AssetBase.GetPrefab("CrewHatchDialog")).GetComponent<CrewHatchDialog>();
		(component.transform as RectTransform).SetParent(DialogCanvasUtil.DialogCanvasRect, worldPositionStays: false);
		component.part = p;
		component.showEVA = showEVA;
		component.showTransfer = showTransfer;
		component.onEVAEvtHandler = onEVAEvtHandler;
		component.onTransferEvtHandler = onTransferEvtHandler;
		component.onDestroyEvtHandler = onDismiss;
		return component;
	}

	public void Terminate()
	{
		if (onDestroyEvtHandler != null)
		{
			onDestroyEvtHandler();
		}
		Object.Destroy(base.gameObject);
	}

	public void Awake()
	{
		widgets = new List<CrewHatchDialogWidget>();
	}

	public void Start()
	{
		CreatePanelContent();
		GameEvents.onVesselChange.Add(onVesselChange);
		GameEvents.onVesselWasModified.Add(onVesselWasModified);
		GameEvents.OnFlightUIModeChanged.Add(OnFlightUIModeChanged);
		GameEvents.OnCameraChange.Add(OnCameraChange);
	}

	public void OnDestroy()
	{
		GameEvents.OnCameraChange.Remove(OnCameraChange);
		GameEvents.OnFlightUIModeChanged.Remove(OnFlightUIModeChanged);
		GameEvents.onVesselWasModified.Remove(onVesselWasModified);
		GameEvents.onVesselChange.Remove(onVesselChange);
	}

	public void OnCameraChange(CameraManager.CameraMode data)
	{
		Terminate();
	}

	public void OnFlightUIModeChanged(FlightUIMode data)
	{
		Terminate();
	}

	public void onVesselWasModified(Vessel data)
	{
		Terminate();
	}

	public void onVesselChange(Vessel data)
	{
		Terminate();
	}

	public void CreatePanelContent()
	{
		textHeader.text = Localizer.Format("#autoLOC_900979", part.partInfo.title);
		Debug.Log("Part Name: " + part.partInfo.title);
		textModuleCrew.text = Localizer.Format("#autoLOC_6002258", part.protoModuleCrew.Count, part.CrewCapacity);
		emptyModuleText.text = Localizer.Format("#autoLOC_6002404");
		CreateList(part.protoModuleCrew);
	}

	public void ClearList()
	{
		int count = widgets.Count;
		while (count-- > 0)
		{
			widgets[count].Terminate();
		}
		widgets.Clear();
	}

	public void CreateList(List<ProtoCrewMember> crew)
	{
		ClearList();
		UIAvailability transferBtnAvail = ((!showTransfer) ? UIAvailability.Hidden : UIAvailability.Available);
		UIAvailability evaBtnAvail = (showEVA ? UIAvailability.GreyedOut : UIAvailability.Hidden);
		int i = 0;
		for (int count = crew.Count; i < count; i++)
		{
			if (!crew[i].inactive)
			{
				if (showEVA && HighLogic.CurrentGame.Parameters.Flight.CanEVA && crew[i].type != ProtoCrewMember.KerbalType.Tourist)
				{
					evaBtnAvail = UIAvailability.Available;
				}
				CrewHatchDialogWidget component = Object.Instantiate(widgetPrefab).GetComponent<CrewHatchDialogWidget>();
				component.Init(crew[i], OnBtnEVA, OnBtnTransfer, evaBtnAvail, transferBtnAvail);
				component.transform.SetParent(listContainer, worldPositionStays: false);
				widgets.Add(component);
			}
		}
		emptyModuleObject.SetActive(widgets.Count == 0);
	}

	public void LateUpdate()
	{
		if (!hover && (Mouse.Left.GetButtonDown() || Mouse.Right.GetButtonDown()))
		{
			Terminate();
		}
	}

	public void OnBtnEVA(ProtoCrewMember crew)
	{
		if (onEVAEvtHandler != null)
		{
			onEVAEvtHandler(crew);
		}
		Terminate();
	}

	public void OnBtnTransfer(ProtoCrewMember crew)
	{
		if (onTransferEvtHandler != null)
		{
			onTransferEvtHandler(crew);
		}
		Terminate();
	}
}
