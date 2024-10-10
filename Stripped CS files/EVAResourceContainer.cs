using System;
using System.Collections;
using ns9;
using UnityEngine;

public class EVAResourceContainer : PartModule
{
	[KSPField]
	public string resourceName = "Supplies";

	[KSPField]
	public float interactionRange = 2f;

	[KSPField]
	public string resourcePullActionName = "Take Resources";

	[KSPField]
	public string resourcePushActionName = "Store Resources";

	[KSPField]
	public string resourceStopActionName = "Stop";

	[KSPField]
	public float ResourceFlowRate = 10f;

	public PartResource resource;

	public override void OnStart(StartState state)
	{
		resource = base.part.Resources.Get(resourceName.GetHashCode());
		updateModuleUI();
		GameEvents.onPartActionUICreate.Add(onPartMenuOpen);
	}

	public void OnDestroy()
	{
		GameEvents.onPartActionUICreate.Remove(onPartMenuOpen);
		StopAllCoroutines();
	}

	public void onPartMenuOpen(Part p)
	{
		if (p == base.part)
		{
			updateModuleUI();
		}
	}

	public void updateModuleUI()
	{
		base.Events["GetResourceExternalEvent"].active = resource != null && resource.amount > 0.0;
		base.Events["GetResourceExternalEvent"].guiName = resourcePullActionName + " (" + resource.amount.ToString("0") + ")";
		base.Events["GetResourceExternalEvent"].unfocusedRange = interactionRange;
		base.Events["PutResourceExternalEvent"].active = resource != null && resource.amount < resource.maxAmount;
		base.Events["PutResourceExternalEvent"].guiName = resourcePushActionName + " (" + resource.amount.ToString("0") + ")";
		base.Events["PutResourceExternalEvent"].unfocusedRange = interactionRange;
		base.Events["StopTransfer"].active = false;
		base.Events["StopTransfer"].guiName = resourceStopActionName;
		base.Events["StopTransfer"].unfocusedRange = interactionRange;
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void GetResourceExternalEvent()
	{
		PartResource partResource = FlightGlobals.ActiveVessel.rootPart.Resources.Get(resource.info.id);
		if (partResource != null)
		{
			base.Events["GetResourceExternalEvent"].active = false;
			base.Events["PutResourceExternalEvent"].active = false;
			base.Events["StopTransfer"].active = true;
			StartCoroutine(transferResources(resource, partResource, ResourceFlowRate * Time.deltaTime, interactionRange));
			return;
		}
		ScreenMessages.PostScreenMessage("<color=#ff9900ff>[" + base.part.partInfo.title + "]: " + Localizer.Format("#autoLOC_222053", resource.info.displayName, FlightGlobals.ActiveVessel.GetDisplayName()) + "</color>", 5f, ScreenMessageStyle.UPPER_LEFT);
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void PutResourceExternalEvent()
	{
		PartResource partResource = FlightGlobals.ActiveVessel.rootPart.Resources.Get(resource.info.id);
		if (partResource != null)
		{
			base.Events["GetResourceExternalEvent"].active = false;
			base.Events["PutResourceExternalEvent"].active = false;
			base.Events["StopTransfer"].active = true;
			StartCoroutine(transferResources(partResource, resource, ResourceFlowRate * Time.deltaTime, interactionRange));
			return;
		}
		ScreenMessages.PostScreenMessage("<color=#ff9900ff>[" + base.part.partInfo.title + "]: " + Localizer.Format("#autoLOC_222053", resource.info.displayName, FlightGlobals.ActiveVessel.GetDisplayName()) + "</color>", 5f, ScreenMessageStyle.UPPER_LEFT);
	}

	[KSPEvent(active = true, guiActiveUnfocused = true, externalToEVAOnly = true, guiActive = false, unfocusedRange = 1f)]
	public void StopTransfer()
	{
		StopAllCoroutines();
		updateModuleUI();
	}

	public IEnumerator transferResources(PartResource src, PartResource dest, double transferRate, float maxDist)
	{
		double startAmount = dest.amount;
		while (src != null && dest != null && dest.amount < dest.maxAmount && src.amount > 0.0 && !((src.part.transform.position - dest.part.transform.position).sqrMagnitude >= maxDist * maxDist))
		{
			transferRate = Math.Min(transferRate, src.amount);
			transferRate = Math.Min(transferRate, dest.maxAmount - dest.amount);
			src.part.TransferResource(src, 0.0 - transferRate, dest.part);
			dest.part.TransferResource(dest, transferRate, src.part);
			yield return null;
		}
		ScreenMessages.PostScreenMessage("<color=#99ff00ff>[" + dest.part.partInfo.title + "]: " + (dest.amount - startAmount).ToString("0") + " " + Localizer.Format("#autoLOC_222102", resource.info.displayName, src.part.partInfo.title) + "</color>", 5f, ScreenMessageStyle.UPPER_LEFT);
		updateModuleUI();
	}
}
