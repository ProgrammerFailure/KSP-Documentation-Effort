using System;
using System.Collections.Generic;
using FinePrint;
using FinePrint.Utilities;
using ns11;
using ns9;
using UnityEngine;

public class ProgressNode : IConfigNode
{
	public string id;

	public bool reached;

	public bool complete;

	public ProgressTree subtree = new ProgressTree();

	public double AchieveDate;

	public Callback OnDeploy;

	public Callback OnStow;

	public Action<Vessel> OnIterateVessels;

	public Func<string, string> OnGenerateSummary;

	public string Id => id;

	public bool IsReached => reached;

	public bool IsComplete => complete;

	public bool IsCompleteManned { get; set; }

	public bool IsCompleteUnmanned { get; set; }

	public ProgressTree Subtree => subtree;

	public ProgressNode(string id, bool startReached)
	{
		this.id = id;
		reached = startReached;
	}

	public void Reach()
	{
		reached = true;
		Debug.Log("[Progress Node Reached]: " + id);
		GameEvents.OnProgressReached.Fire(this);
		AchieveDate = Planetarium.GetUniversalTime();
	}

	public void Achieve()
	{
		GameEvents.OnProgressAchieved.Fire(this);
	}

	public void Complete()
	{
		if (!reached)
		{
			Reach();
		}
		complete = true;
		Debug.Log("[Progress Node Complete]: " + id);
		AchieveDate = Planetarium.GetUniversalTime();
		GameEvents.OnProgressComplete.Fire(this);
	}

	public void CheatComplete()
	{
		IsCompleteManned = true;
		IsCompleteUnmanned = true;
		if (!IsComplete)
		{
			Complete();
		}
	}

	public void Load(ConfigNode node)
	{
		GameEvents.onProgressNodeLoad.Fire(new GameEvents.FromToAction<ProgressNode, ConfigNode>(this, node));
		reached = true;
		if (node.HasValue("reached"))
		{
			AchieveDate = double.Parse(node.GetValue("reached"));
		}
		else if (node.HasValue("completed"))
		{
			complete = true;
			IsCompleteManned = true;
			IsCompleteUnmanned = true;
			AchieveDate = double.Parse(node.GetValue("completed"));
		}
		else if (node.HasValue("completedManned"))
		{
			complete = true;
			IsCompleteManned = true;
			AchieveDate = double.Parse(node.GetValue("completedManned"));
		}
		else if (node.HasValue("completedUnmanned"))
		{
			complete = true;
			IsCompleteUnmanned = true;
			AchieveDate = double.Parse(node.GetValue("completedUnmanned"));
		}
		OnLoad(node);
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public void Save(ConfigNode node)
	{
		if (complete)
		{
			if (IsCompleteManned && !IsCompleteUnmanned)
			{
				node.AddValue("completedManned", AchieveDate);
			}
			else if (!IsCompleteManned && IsCompleteUnmanned)
			{
				node.AddValue("completedUnmanned", AchieveDate);
			}
			else
			{
				node.AddValue("completed", AchieveDate);
			}
		}
		else if (reached)
		{
			node.AddValue("reached", AchieveDate);
		}
		OnSave(node);
		GameEvents.onProgressNodeSave.Fire(new GameEvents.FromToAction<ProgressNode, ConfigNode>(this, node));
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public string GetNodeSummary(string baseID)
	{
		if (OnGenerateSummary != null)
		{
			return OnGenerateSummary(baseID);
		}
		return baseID + Id + "=" + KSPUtil.LocalizeNumber(AchieveDate, "0.0");
	}

	public void CrewSensitiveComplete(Vessel v)
	{
		if (!IsCompleteManned || !IsCompleteUnmanned)
		{
			int count = v.GetVesselCrew().Count;
			if (!IsCompleteManned && count > 0)
			{
				IsCompleteManned = true;
			}
			if (!IsCompleteUnmanned && count <= 0)
			{
				IsCompleteUnmanned = true;
			}
		}
	}

	public void CrewSensitiveComplete(ProtoVessel pv)
	{
		if (!IsCompleteManned || !IsCompleteUnmanned)
		{
			int count = pv.GetVesselCrew().Count;
			if (!IsCompleteManned && count > 0)
			{
				IsCompleteManned = true;
			}
			if (!IsCompleteUnmanned && count <= 0)
			{
				IsCompleteUnmanned = true;
			}
		}
	}

	public void AwardProgress(string description, float funds = 0f, float science = 0f, float reputation = 0f, CelestialBody body = null)
	{
		if (HighLogic.CurrentGame.Mode != Game.Modes.CAREER)
		{
			return;
		}
		if (funds > 0f && Funding.Instance != null)
		{
			Funding.Instance.AddFunds(funds, TransactionReasons.Progression);
		}
		if (science > 0f && ResearchAndDevelopment.Instance != null)
		{
			ResearchAndDevelopment.Instance.AddScience(science, TransactionReasons.Progression);
		}
		if (reputation > 0f && Reputation.Instance != null)
		{
			Reputation.Instance.AddReputation(reputation, TransactionReasons.Progression);
		}
		List<string> list = StringUtilities.FormattedCurrencies(funds, science, reputation, symbols: true, verbose: true, TransactionReasons.Progression);
		if (list.Count > 0)
		{
			string text = string.Empty;
			int i = 0;
			for (int count = list.Count; i < count; i++)
			{
				text = ((i >= list.Count - 1) ? (text + list[i]) : (text + list[i].Replace("</>", "   </>")));
			}
			AddOrAppendWorldFirstMessage(description, text);
		}
	}

	public void AddOrAppendWorldFirstMessage(string title, string body)
	{
		if (MessageSystem.Instance == null)
		{
			return;
		}
		string messageTitle = Localizer.Format("#autoLOC_296780");
		List<MessageSystem.Message> list = MessageSystem.Instance.FindMessages((MessageSystem.Message msg) => msg.messageTitle.Equals(messageTitle));
		if (list.Count > 0)
		{
			int count = list.Count;
			while (count-- > 0)
			{
				list[count].IsRead = false;
				MessageSystem.Message message = list[count];
				message.message = message.message + "\n " + title + "\n " + body + "\n";
				if (list[count].button != null)
				{
					list[count].button.SetAsRead();
				}
			}
		}
		else
		{
			string message2 = Localizer.Format("#autoLOC_7001007", title, body);
			MessageSystem.Message message3 = new MessageSystem.Message(messageTitle, message2, MessageSystemButton.MessageButtonColor.BLUE, MessageSystemButton.ButtonIcons.ACHIEVE);
			MessageSystem.Instance.AddMessage(message3, animate: false);
		}
	}

	public void AwardProgressStandard(string description, ProgressType progress, CelestialBody body = null)
	{
		float funds = ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.PROGRESS, Currency.Funds, progress, body);
		float science = ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.PROGRESS, Currency.Science, progress, body);
		float reputation = ProgressUtilities.WorldFirstStandardReward(ProgressRewardType.PROGRESS, Currency.Reputation, progress, body);
		AwardProgress(description, funds, science, reputation, body);
	}

	public void AwardProgressInterval(string description, int currentInterval, int totalIntervals, ProgressType progress, CelestialBody body = null)
	{
		float funds = ProgressUtilities.WorldFirstIntervalReward(ProgressRewardType.PROGRESS, Currency.Funds, progress, body, currentInterval, totalIntervals);
		float science = ProgressUtilities.WorldFirstIntervalReward(ProgressRewardType.PROGRESS, Currency.Science, progress, body, currentInterval, totalIntervals);
		float reputation = ProgressUtilities.WorldFirstIntervalReward(ProgressRewardType.PROGRESS, Currency.Reputation, progress, body, currentInterval, totalIntervals);
		AwardProgress(description, funds, science, reputation, body);
	}

	public void AwardProgressRandomTech(string description, int seed)
	{
		if ((HighLogic.CurrentGame.Mode == Game.Modes.CAREER || HighLogic.CurrentGame.Mode == Game.Modes.SCIENCE_SANDBOX) && !(ResearchAndDevelopment.Instance == null))
		{
			List<ProtoTechNode> nextUnavailableNodes = AssetBase.RnDTechTree.GetNextUnavailableNodes();
			if (nextUnavailableNodes.Count > 0)
			{
				KSPRandom kSPRandom = new KSPRandom(seed);
				ProtoTechNode protoTechNode = nextUnavailableNodes[kSPRandom.Next(0, nextUnavailableNodes.Count)];
				ResearchAndDevelopment.Instance.UnlockProtoTechNode(protoTechNode);
				ResearchAndDevelopment.RefreshTechTreeUI();
				ContractDefs.Instance.TechResearched(default(GameEvents.HostTargetAction<RDTech, RDTech.OperationResult>));
				string body = Localizer.Format("#autoLOC_296834", XKCDColors.HexFormat.PurplishPink, "<sprite=\"CurrencySpriteAsset\" name=\"Flask\" tint=1>", Localizer.Format(ResearchAndDevelopment.GetTechnologyTitle(protoTechNode.techID)));
				AddOrAppendWorldFirstMessage(description, body);
			}
		}
	}
}
