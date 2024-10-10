using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

public class RDTech : MonoBehaviour
{
	public enum State
	{
		Unavailable,
		Available
	}

	public enum OperationResult
	{
		Successful,
		NotEnoughFunds,
		ScienceCostLimitExceeded,
		Failure
	}

	public string techID;

	public int scienceCost;

	public string title;

	public string description;

	public List<AvailablePart> partsAssigned;

	public List<AvailablePart> partsPurchased;

	public State state;

	public ResearchAndDevelopment host;

	public ProtoTechNode techState;

	public bool hideIfNoParts;

	public void Warmup()
	{
		List<AvailablePart> list = null;
		if ((bool)PartLoader.Instance)
		{
			list = PartLoader.LoadedPartsList;
		}
		else if ((bool)RDTestSceneLoader.Instance)
		{
			list = RDTestSceneLoader.LoadedPartsList;
		}
		partsAssigned = new List<AvailablePart>();
		partsPurchased = new List<AvailablePart>();
		if (list == null)
		{
			Debug.LogWarning("RDTech: No loaded part lists available!");
			return;
		}
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			AvailablePart availablePart = list[i];
			if (availablePart.TechRequired == techID)
			{
				partsAssigned.Add(availablePart);
			}
		}
	}

	public void Start()
	{
		Warmup();
		host = ResearchAndDevelopment.Instance;
		if (host != null)
		{
			techState = host.GetTechState(techID);
			if (techState != null)
			{
				state = techState.state;
				partsPurchased = techState.partsPurchased;
			}
		}
	}

	public void OnDestroy()
	{
	}

	public OperationResult ResearchTech()
	{
		if (state != State.Available)
		{
			if (host != null)
			{
				if (!CurrencyModifierQuery.RunQuery(TransactionReasons.RnDTechResearch, 0f, -scienceCost, 0f).CanAfford(delegate(Currency c)
				{
					Debug.Log(StringBuilderCache.Format("[RDTech]: Not enough {0} to research this node.", c), base.gameObject);
					ScreenMessages.PostScreenMessage(StringBuilderCache.Format(Localizer.Format("#autoLOC_299393", c.Description())), 3f, ScreenMessageStyle.UPPER_CENTER);
				}))
				{
					GameEvents.OnTechnologyResearched.Fire(new GameEvents.HostTargetAction<RDTech, OperationResult>(this, OperationResult.NotEnoughFunds));
					return OperationResult.NotEnoughFunds;
				}
				float scienceCostLimit = GameVariables.Instance.GetScienceCostLimit(ScenarioUpgradeableFacilities.GetFacilityLevel(SpaceCenterFacility.ResearchAndDevelopment));
				if ((float)scienceCost > scienceCostLimit)
				{
					Debug.Log("[RDTech]: Node exceeds Science cost limit.", base.gameObject);
					ScreenMessages.PostScreenMessage(Localizer.Format("#autoLOC_299404", scienceCostLimit.ToString("N0")), 3f, ScreenMessageStyle.UPPER_CENTER);
					GameEvents.OnTechnologyResearched.Fire(new GameEvents.HostTargetAction<RDTech, OperationResult>(this, OperationResult.ScienceCostLimitExceeded));
					return OperationResult.ScienceCostLimitExceeded;
				}
				host.AddScience(-scienceCost, TransactionReasons.RnDTechResearch);
			}
			UnlockTech(host != null);
			return OperationResult.Successful;
		}
		Debug.LogError("[RDTech]: Node is already available", base.gameObject);
		GameEvents.OnTechnologyResearched.Fire(new GameEvents.HostTargetAction<RDTech, OperationResult>(this, OperationResult.Failure));
		return OperationResult.Failure;
	}

	public void UnlockTech(bool updateGameState)
	{
		if (host != null && updateGameState)
		{
			if (techState == null)
			{
				techState = new ProtoTechNode();
			}
			state = State.Available;
			if (Funding.Instance == null || HighLogic.CurrentGame.Parameters.Difficulty.BypassEntryPurchaseAfterResearch)
			{
				AutoPurchaseAllParts();
			}
			techState.UpdateFromTechNode(this);
			host.SetTechState(techID, techState);
			AnalyticsUtil.LogTechTreeNodeUnlocked(techState, ResearchAndDevelopment.Instance.Science);
		}
		else
		{
			state = State.Available;
		}
		GameEvents.OnTechnologyResearched.Fire(new GameEvents.HostTargetAction<RDTech, OperationResult>(this, OperationResult.Successful));
	}

	public bool PartIsPurchased(AvailablePart ap)
	{
		return partsPurchased.Contains(ap);
	}

	public void PurchasePart(AvailablePart ap)
	{
		if (partsAssigned.Contains(ap) && !partsPurchased.Contains(ap))
		{
			partsPurchased.Add(ap);
			GameEvents.OnPartPurchased.Fire(ap);
			HandlePurchase(ap);
			techState.UpdateFromTechNode(this);
			host.SetTechState(techID, techState);
		}
	}

	public void HandlePurchase(AvailablePart partInfo)
	{
		string[] array = partInfo.identicalParts.Split(',');
		int num = array.Length;
		while (num-- > 0)
		{
			AvailablePart partInfoByName = PartLoader.getPartInfoByName(array[num].Replace('_', '.').Trim());
			if (partInfoByName != null && !partsPurchased.Contains(partInfoByName) && partInfoByName.TechRequired == partInfo.TechRequired)
			{
				partInfoByName.costsFunds = false;
				partsPurchased.Add(partInfoByName);
				GameEvents.OnPartPurchased.Fire(partInfoByName);
				partInfoByName.costsFunds = true;
			}
		}
	}

	public void AutoPurchaseAllParts()
	{
		int count = partsAssigned.Count;
		while (count-- > 0)
		{
			AvailablePart availablePart = partsAssigned[count];
			if (!partsPurchased.Contains(availablePart))
			{
				partsPurchased.Add(availablePart);
				GameEvents.OnPartPurchased.Fire(availablePart);
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("id", techID);
		node.AddValue("title", title);
		node.AddValue("description", description);
		node.AddValue("cost", scienceCost);
		node.AddValue("hideEmpty", hideIfNoParts);
	}

	public void Load(ConfigNode node)
	{
		if (node.HasValue("id"))
		{
			techID = node.GetValue("id");
		}
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
		if (node.HasValue("description"))
		{
			description = node.GetValue("description");
		}
		if (node.HasValue("cost"))
		{
			scienceCost = int.Parse(node.GetValue("cost"));
			if (scienceCost == 0)
			{
				state = State.Available;
			}
		}
		if (node.HasValue("hideEmpty"))
		{
			hideIfNoParts = bool.Parse(node.GetValue("hideEmpty"));
		}
	}
}
