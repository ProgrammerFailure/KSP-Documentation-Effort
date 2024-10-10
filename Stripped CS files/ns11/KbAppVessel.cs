using UnityEngine.EventSystems;

namespace ns11;

public abstract class KbAppVessel : KbApp
{
	public Vessel vessel;

	public KbAppVessel()
	{
	}

	public override void Awake()
	{
		base.Awake();
		GameEvents.onVesselRename.Add(OnVesselRename);
		GameEvents.onInputLocksModified.Add(OnInputLocksChange);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onVesselRename.Remove(OnVesselRename);
		GameEvents.onInputLocksModified.Remove(OnInputLocksChange);
	}

	public override void Setup()
	{
		base.Setup();
		appFrame.headerClickHandler.onPointerClick.AddListener(OnPanelHeaderTap);
	}

	public override void DisplayApp()
	{
	}

	public override void HideApp()
	{
	}

	public void OnVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		if (vessel == nChg.host)
		{
			appFrame.appName.text = vessel.DiscoveryInfo.displayName.Value;
		}
	}

	public void OnPanelHeaderTap(PointerEventData eventData)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS) && vessel != null && eventData.clickCount == 2 && vessel.vesselType != VesselType.const_11 && vessel.vesselType != VesselType.Flag)
		{
			appFrame.appName.text = "|";
			vessel.RenameVessel();
		}
	}

	public void OnInputLocksChange(GameEvents.FromToAction<ControlTypes, ControlTypes> iChg)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS) && vessel != null)
		{
			appFrame.appName.text = vessel.DiscoveryInfo.displayName.Value;
		}
	}
}
