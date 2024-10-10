using System.Collections.Generic;
using Highlighting;
using ns9;
using UnityEngine;

public abstract class PartItemTransfer : MonoBehaviour
{
	public enum DismissAction
	{
		Cancelled,
		Interrupted,
		ItemMoved
	}

	public static PartItemTransfer Instance;

	public Part srcPart;

	public List<Part> semiValidParts;

	public List<Part> validParts;

	public string type = "Item";

	public string semiValidMessage = "#autoLOC_135736";

	public Callback<DismissAction, Part> onDismiss;

	public ScreenMessage scMsgInstruction;

	public ScreenMessage scMsgWarning;

	public List<PartSelector> partSelectors;

	public PartItemTransfer()
	{
	}

	public virtual void Setup(Part src, string itemType, string itemName, string semiValidMsg, Callback<DismissAction, Part> onDialogDismiss)
	{
		type = Localizer.Format(itemType);
		onDismiss = onDialogDismiss;
		srcPart = src;
		semiValidMessage = semiValidMsg;
		scMsgInstruction = new ScreenMessage("", 15f, ScreenMessageStyle.UPPER_CENTER);
		scMsgWarning = new ScreenMessage("", 3f, ScreenMessageStyle.UPPER_CENTER);
		if (Instance != null)
		{
			DismissAlreadyRunning();
			return;
		}
		Instance = this;
		semiValidParts = new List<Part>();
		validParts = new List<Part>();
		int count = srcPart.vessel.parts.Count;
		while (count-- > 0)
		{
			Part part = srcPart.vessel.parts[count];
			if (part != srcPart)
			{
				if (IsValidPart(part))
				{
					validParts.Add(part);
				}
				else if (IsSemiValidPart(part))
				{
					semiValidParts.Add(part);
				}
			}
		}
		AfterPartsFound();
		partSelectors = new List<PartSelector>();
		int count2 = semiValidParts.Count;
		while (count2-- > 0)
		{
			Part part = semiValidParts[count2];
			partSelectors.Add(PartSelector.Create(part, OnSemiValidPartSelect, Highlighter.colorPartTransferSourceHighlight, Highlighter.colorPartTransferSourceHover));
		}
		int count3 = validParts.Count;
		while (count3-- > 0)
		{
			Part part = validParts[count3];
			partSelectors.Add(PartSelector.Create(part, OnPartSelect, Highlighter.colorPartTransferDestHighlight, Highlighter.colorPartTransferDestHover));
		}
		partSelectors.Add(PartSelector.Create(srcPart, OnSrcPartSelect, Highlighter.colorPartTransferSourceHighlight, Highlighter.colorPartTransferSourceHover));
		InputLockManager.SetControlLock(~(ControlTypes.flag_2 | ControlTypes.CAMERAMODES | ControlTypes.CAMERACONTROLS), "itemTransfer");
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_135800", XKCDColors.HexFormat.BrightAqua, itemName), scMsgInstruction);
		GameEvents.onVesselWasModified.Add(onVesselModified);
		GameEvents.onVesselSituationChange.Add(onVesselSituationChange);
		GameEvents.onVesselChange.Add(onVesselChanged);
		HookAdditionalEvents();
		GameEvents.onItemTransferStarted.Fire(this);
	}

	public virtual void DismissInterrupted()
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_135812", type), scMsgWarning);
		Dismiss(DismissAction.Interrupted, null);
	}

	public static void DismissActive()
	{
		if (Instance != null)
		{
			Instance.DismissInterrupted();
		}
	}

	public virtual void DismissAlreadyRunning()
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_135824", type), scMsgWarning);
		Dismiss(DismissAction.Interrupted, null);
	}

	public virtual void Dismiss(DismissAction dma, Part p)
	{
		if (onDismiss != null)
		{
			onDismiss(dma, p);
		}
		ScreenMessages.RemoveMessage(scMsgInstruction);
		Object.Destroy(base.gameObject);
	}

	public virtual void OnDestroy()
	{
		GameEvents.onVesselWasModified.Remove(onVesselModified);
		GameEvents.onVesselSituationChange.Remove(onVesselSituationChange);
		GameEvents.onVesselChange.Remove(onVesselChanged);
		UnhookAdditionalEvents();
		int count = partSelectors.Count;
		while (count-- > 0)
		{
			partSelectors[count].Dismiss();
		}
		InputLockManager.RemoveControlLock("itemTransfer");
		if (Instance == this)
		{
			Instance = null;
		}
	}

	public abstract bool IsValidPart(Part p);

	public abstract bool IsSemiValidPart(Part p);

	public virtual void AfterPartsFound()
	{
	}

	public virtual void HookAdditionalEvents()
	{
	}

	public virtual void UnhookAdditionalEvents()
	{
	}

	public virtual void onVesselModified(Vessel v)
	{
		if (v == srcPart.vessel)
		{
			DismissInterrupted();
		}
	}

	public virtual void onVesselSituationChange(GameEvents.HostedFromToAction<Vessel, Vessel.Situations> vs)
	{
		if (vs.host == srcPart.vessel)
		{
			DismissInterrupted();
		}
	}

	public virtual void onVesselChanged(Vessel v)
	{
		DismissInterrupted();
	}

	public virtual void OnSrcPartSelect(Part p)
	{
		ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_135891"), scMsgWarning);
	}

	public virtual void OnSemiValidPartSelect(Part p)
	{
		ScreenMessages.PostScreenMessage(Localizer.Format(semiValidMessage), scMsgWarning);
	}

	public virtual void OnPartSelect(Part p)
	{
		Dismiss(DismissAction.ItemMoved, p);
	}

	public void LateUpdate()
	{
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			Dismiss(DismissAction.Cancelled, null);
		}
	}
}
