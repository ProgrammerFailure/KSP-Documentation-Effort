using UnityEngine.EventSystems;

namespace ns11;

public abstract class KbAppUnowned : KbApp
{
	public IDiscoverable currentUnowned;

	public KbAppUnowned()
	{
	}

	public override void Awake()
	{
		base.Awake();
		GameEvents.onVesselRename.Add(onVesselRename);
		GameEvents.onInputLocksModified.Add(OnInputLocksChange);
		GameEvents.onKnowledgeChanged.Add(OnKnowledgeChange);
	}

	public override void OnDestroy()
	{
		base.OnDestroy();
		GameEvents.onVesselRename.Remove(onVesselRename);
		GameEvents.onInputLocksModified.Remove(OnInputLocksChange);
		GameEvents.onKnowledgeChanged.Remove(OnKnowledgeChange);
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

	public void onVesselRename(GameEvents.HostedFromToAction<Vessel, string> nChg)
	{
		if ((Vessel)currentUnowned == nChg.host)
		{
			appFrame.appName.text = currentUnowned.DiscoveryInfo.displayName.Value;
		}
	}

	public void OnPanelHeaderTap(PointerEventData eventData)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS) && currentUnowned != null && currentUnowned is Vessel && currentUnowned.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.Name) && eventData.clickCount == 2 && (currentUnowned as Vessel).vesselType != VesselType.const_11 && (currentUnowned as Vessel).vesselType != VesselType.Flag)
		{
			appFrame.appName.text = "|";
			(currentUnowned as Vessel).RenameVessel();
		}
	}

	public void OnInputLocksChange(GameEvents.FromToAction<ControlTypes, ControlTypes> iChg)
	{
		if (InputLockManager.IsUnlocked(ControlTypes.EDITOR_EDIT_NAME_FIELDS) && currentUnowned != null && currentUnowned is Vessel && currentUnowned.DiscoveryInfo.HaveKnowledgeAbout(DiscoveryLevels.Name))
		{
			appFrame.appName.text = currentUnowned.DiscoveryInfo.displayName.Value;
		}
	}

	public void OnKnowledgeChange(GameEvents.HostedFromToAction<IDiscoverable, DiscoveryLevels> kChg)
	{
		if (currentUnowned == kChg.host && kChg.to != DiscoveryLevels.Owned && appFrame != null && appFrame.appName != null && currentUnowned != null)
		{
			appFrame.appName.text = currentUnowned.DiscoveryInfo.displayName.Value;
		}
	}
}
