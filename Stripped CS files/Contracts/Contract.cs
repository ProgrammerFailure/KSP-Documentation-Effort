using System;
using System.Collections.Generic;
using System.ComponentModel;
using Contracts.Agents;
using KSPAchievements;
using ns11;
using ns9;
using UnityEngine;

namespace Contracts;

public class Contract : IContractParameterHost
{
	public enum ContractPrestige
	{
		[Description("#autoLOC_7001024")]
		Trivial,
		[Description("#autoLOC_7001025")]
		Significant,
		[Description("#autoLOC_7001026")]
		Exceptional
	}

	public enum State
	{
		[Description("#autoLOC_900713")]
		Generated,
		[Description("#autoLOC_900337")]
		Offered,
		[Description("#autoLOC_900714")]
		OfferExpired,
		[Description("#autoLOC_900716")]
		Declined,
		[Description("#autoLOC_900711")]
		Cancelled,
		[Description("#autoLOC_900336")]
		Active,
		[Description("#autoLOC_900710")]
		Completed,
		[Description("#autoLOC_900715")]
		DeadlineExpired,
		[Description("#autoLOC_900708")]
		Failed,
		[Description("#autoLOC_8003155")]
		Withdrawn
	}

	public enum Viewed
	{
		Unseen,
		Seen,
		Read
	}

	public enum DeadlineType
	{
		Fixed,
		Floating,
		None
	}

	public enum ProgressState
	{
		Unreached,
		Reached,
		Incomplete,
		Complete
	}

	[SerializeField]
	public ContractPrestige prestige;

	[SerializeField]
	public DeadlineType expiryType = DeadlineType.Floating;

	[SerializeField]
	public DeadlineType deadlineType = DeadlineType.Floating;

	[SerializeField]
	public Agent agent;

	[SerializeField]
	public State state;

	[SerializeField]
	public Viewed viewed;

	[SerializeField]
	public double dateExpire;

	[SerializeField]
	public double dateAccepted;

	[SerializeField]
	public double dateDeadline;

	[SerializeField]
	public double dateFinished;

	[SerializeField]
	public long contractID;

	public Guid contractGuid;

	[SerializeField]
	public int missionSeed;

	public double TimeExpiry;

	public double TimeDeadline;

	public double FundsAdvance;

	public double FundsCompletion;

	public double FundsFailure;

	public float ScienceCompletion;

	public float ReputationCompletion;

	public float ReputationFailure;

	public bool AutoAccept;

	public bool IgnoresWeight;

	public List<string> keywords = new List<string>();

	public List<string> keywordsRequired = new List<string>();

	[SerializeField]
	public List<ContractParameter> parameters = new List<ContractParameter>();

	public EventData<State> OnStateChange = new EventData<State>("OnStateChange");

	public EventData<Viewed> OnViewedChange = new EventData<Viewed>("OnViewedChange");

	public static int contractsInExistance { get; set; }

	public ContractPrestige Prestige => prestige;

	public string Title => GetTitle();

	public string Synopsys => GetSynopsys();

	public string Description => GetDescription();

	public string Notes => GetNotes();

	public Agent Agent => agent;

	public State ContractState => state;

	public string LocalizedContractState => Localizer.Format(state.Description());

	public Viewed ContractViewed => viewed;

	public double DateExpire => dateExpire;

	public double DateAccepted => dateAccepted;

	public double DateDeadline => dateDeadline;

	public double DateFinished => dateFinished;

	public long ContractID => contractID;

	public Guid ContractGuid => contractGuid;

	public int MissionSeed => missionSeed;

	public List<string> Keywords => keywords;

	public List<string> KeywordsRequired => keywordsRequired;

	public IContractParameterHost Parent => null;

	public Contract Root => this;

	public int ParameterCount => parameters.Count;

	public ContractParameter this[int index] => GetParameter(index);

	public ContractParameter this[string id] => GetParameter(id);

	public ContractParameter this[Type type] => GetParameter(type);

	public IEnumerable<ContractParameter> AllParameters
	{
		get
		{
			int iC = parameters.Count;
			int i = 0;
			while (i < iC)
			{
				IEnumerator<ContractParameter> parameterEnumerator = parameters[i].AllParameters.GetEnumerator();
				while (parameterEnumerator.MoveNext())
				{
					yield return parameterEnumerator.Current;
				}
				int num = i + 1;
				i = num;
			}
		}
	}

	public static double GameTime
	{
		get
		{
			if (Planetarium.fetch != null)
			{
				return Planetarium.fetch.time;
			}
			return 0.0;
		}
	}

	public Contract()
	{
		contractsInExistance++;
	}

	~Contract()
	{
		contractsInExistance--;
	}

	public void SetExpiry()
	{
		SetExpiry(1, 7);
	}

	public void SetExpiry(int minDays, int maxDays)
	{
		TimeExpiry = Mathf.Round((float)UnityEngine.Random.Range(minDays, maxDays) * (float)KSPUtil.dateTimeFormatter.Day / 60f) * 60f;
	}

	public void SetExpiry(float minDays, float maxDays)
	{
		TimeExpiry = Mathf.Round(UnityEngine.Random.Range(minDays, maxDays) * (float)KSPUtil.dateTimeFormatter.Day / 60f) * 60f;
	}

	public void SetDeadlineDays(float days, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		TimeDeadline = days * (float)KSPUtil.dateTimeFormatter.Day * num;
	}

	public void SetDeadlineYears(float years, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		TimeDeadline = years * (float)KSPUtil.dateTimeFormatter.Year * num;
	}

	public void SetFunds(float advance, float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		FundsAdvance = Mathf.Round(advance * GameVariables.Instance.GetContractFundsAdvanceFactor(prestige) * num);
		FundsCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractFundsCompletionFactor(prestige) * num);
		FundsFailure = FundsAdvance;
	}

	public void SetFunds(float advance, float completion, float failure, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		FundsAdvance = Mathf.Round(advance * GameVariables.Instance.GetContractFundsAdvanceFactor(prestige) * num);
		FundsCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractFundsCompletionFactor(prestige) * num);
		FundsFailure = Math.Max(FundsAdvance, Mathf.Round(failure * GameVariables.Instance.GetContractFundsFailureFactor(prestige) * num));
	}

	public void SetReputation(float completion, float failure, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ReputationCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractReputationCompletionFactor(prestige) * num);
		ReputationFailure = Mathf.Round(failure * GameVariables.Instance.GetContractReputationFailureFactor(prestige) * num);
	}

	public void SetReputation(float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ReputationCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractReputationCompletionFactor(prestige) * num);
	}

	public void SetScience(float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ScienceCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractScienceCompletionFactor(prestige) * num);
	}

	public float GetDestinationWeight(CelestialBody body)
	{
		return GameVariables.Instance.GetContractDestinationWeight(body);
	}

	public bool AddKeywords(params string[] keywords)
	{
		if (keywords != null && keywords.Length != 0)
		{
			int num = 0;
			int num2 = keywords.Length;
			while (true)
			{
				if (num < num2)
				{
					if (!Keywords.Contains(keywords[num]))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			Keywords.Add(keywords[num]);
			return true;
		}
		return false;
	}

	public bool AddKeywordsRequired(params string[] keywords)
	{
		if (keywords != null && keywords.Length != 0)
		{
			int num = 0;
			int num2 = keywords.Length;
			while (true)
			{
				if (num < num2)
				{
					if (!KeywordsRequired.Contains(keywords[num]))
					{
						break;
					}
					num++;
					continue;
				}
				return false;
			}
			KeywordsRequired.Add(keywords[num]);
			return true;
		}
		return false;
	}

	public ContractParameter AddParameter(ContractParameter parameter, string id = null)
	{
		parameter.NestToParent(this);
		if (id != null)
		{
			parameter.String_0 = id;
		}
		parameters.Add(parameter);
		return parameter;
	}

	public void ParameterStateUpdate(ContractParameter p)
	{
		OnParameterStateChange(p);
	}

	public ContractParameter GetParameter(int index)
	{
		return parameters[index];
	}

	public ContractParameter GetParameter(string id)
	{
		int num = 0;
		int count = parameters.Count;
		while (true)
		{
			if (num < count)
			{
				if (parameters[num].String_0 == id)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return parameters[num];
	}

	public ContractParameter GetParameter(Type type)
	{
		int num = 0;
		int count = parameters.Count;
		while (true)
		{
			if (num < count)
			{
				if (parameters[num].GetType() == type)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return parameters[num];
	}

	public T GetParameter<T>(string id = null) where T : ContractParameter
	{
		int num = 0;
		int count = parameters.Count;
		while (true)
		{
			if (num < count)
			{
				if (parameters[num] is T)
				{
					if (id == null)
					{
						break;
					}
					if (id == parameters[num].String_0)
					{
						return (T)parameters[num];
					}
				}
				num++;
				continue;
			}
			return null;
		}
		return (T)parameters[num];
	}

	public void RemoveParameter(int index)
	{
		parameters.RemoveAt(index);
	}

	public void RemoveParameter(string id)
	{
		int num = 0;
		int count = parameters.Count;
		while (true)
		{
			if (num < count)
			{
				if (parameters[num].String_0 == id)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		parameters[num].Unregister();
		parameters.RemoveAt(num);
	}

	public void RemoveParameter(Type type)
	{
		int num = 0;
		int count = parameters.Count;
		while (true)
		{
			if (num < count)
			{
				if (parameters[num].GetType() == type)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		parameters[num].Unregister();
		parameters.RemoveAt(num);
	}

	public void RemoveParameter(ContractParameter parameter)
	{
		parameter.Unregister();
		parameters.Remove(parameter);
	}

	public virtual void OnParameterStateChange(ContractParameter p)
	{
		switch (p.State)
		{
		case ParameterState.Failed:
		{
			if (!p.AllowPartialFailure)
			{
				SetState(State.Failed);
				break;
			}
			bool flag = false;
			bool flag2 = false;
			int count2 = parameters.Count;
			while (count2-- > 0)
			{
				if (parameters[count2].State != 0)
				{
					if (parameters[count2].State == ParameterState.Complete)
					{
						flag2 = true;
					}
					continue;
				}
				flag = true;
				break;
			}
			if (!flag)
			{
				if (flag2)
				{
					SetState(State.Completed);
				}
				else
				{
					SetState(State.Failed);
				}
			}
			break;
		}
		case ParameterState.Complete:
		{
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				if (parameters[i].State != ParameterState.Complete)
				{
					return;
				}
			}
			SetState(State.Completed);
			break;
		}
		}
	}

	public virtual bool CanBeCancelled()
	{
		return true;
	}

	public virtual bool CanBeDeclined()
	{
		return true;
	}

	public virtual bool CanBeFailed()
	{
		return true;
	}

	public virtual bool MeetRequirements()
	{
		return false;
	}

	public virtual List<CelestialBody> GetWeightBodies()
	{
		return null;
	}

	public virtual string GetSynopsys()
	{
		return "";
	}

	public virtual string GetDescription()
	{
		return "";
	}

	public virtual string GetTitle()
	{
		return null;
	}

	public virtual string GetNotes()
	{
		return null;
	}

	public virtual bool Generate()
	{
		return false;
	}

	public virtual void OnRegister()
	{
	}

	public virtual void OnUnregister()
	{
	}

	public virtual void OnUpdate()
	{
	}

	public virtual void OnReset()
	{
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
	}

	public virtual void AwardAdvance()
	{
		if (FundsAdvance != 0.0)
		{
			Debug.Log("Awarding " + FundsAdvance + " funds to player for contract advance");
			if (Funding.Instance != null)
			{
				Funding.Instance.AddFunds(FundsAdvance, TransactionReasons.ContractAdvance);
			}
		}
	}

	public virtual void AwardCompletion()
	{
		if (FundsCompletion != 0.0)
		{
			Debug.Log("Awarding " + FundsCompletion + " funds to player for contract completion");
			if (Funding.Instance != null)
			{
				Funding.Instance.AddFunds(FundsCompletion, TransactionReasons.ContractReward);
			}
		}
		if (ScienceCompletion != 0f)
		{
			Debug.Log("Awarding " + ScienceCompletion + " science to player for contract completion");
			if (ResearchAndDevelopment.Instance != null)
			{
				ResearchAndDevelopment.Instance.AddScience(ScienceCompletion, TransactionReasons.ContractReward);
			}
		}
		if (ReputationCompletion != 0f)
		{
			Debug.Log("Awarding " + ReputationCompletion + " reputation to player for contract completion");
			if (Reputation.Instance != null)
			{
				Reputation.Instance.AddReputation(ReputationCompletion, TransactionReasons.ContractReward);
			}
		}
	}

	public virtual void PenalizeCancellation()
	{
		float num = Mathf.InverseLerp((float)dateAccepted, (float)dateDeadline, (float)dateFinished);
		double num2 = Mathfx.Lerp(FundsAdvance, FundsFailure, num);
		float num3 = Mathfx.Lerp(HighLogic.CurrentGame.Parameters.Career.RepLossDeclined, ReputationFailure, num);
		if (num2 != 0.0)
		{
			Debug.Log("Deducting " + num2.ToString("N0") + " funds for contract cancellation at " + (num * 100f).ToString("0.0") + "% time elapsed towards the deadline");
			if (Funding.Instance != null)
			{
				Funding.Instance.AddFunds(0.0 - Math.Abs(num2), TransactionReasons.ContractPenalty);
			}
		}
		if (num3 != 0f)
		{
			Debug.Log("Deducting " + num3.ToString("N0") + " reputation for contract cancellation at " + (num * 100f).ToString("0.0") + "% time elapsed towards the deadline");
			if (Reputation.Instance != null)
			{
				Reputation.Instance.AddReputation(0f - Mathf.Abs(num3), TransactionReasons.ContractPenalty);
			}
		}
		if (num > 0.01f)
		{
			SendStateMessage(Localizer.Format("#autoLOC_266201"), MessageCancelled() + StateMsgAddition(MessageCancellationPenalties(num2, num3)), MessageSystemButton.MessageButtonColor.const_0, MessageSystemButton.ButtonIcons.FAIL);
			Debug.Log("Sending message for cancellation");
		}
	}

	public virtual void PenalizeFailure()
	{
		if (FundsFailure != 0.0)
		{
			Debug.Log("Penalizing " + FundsFailure + " funds from player for contract failure");
			if (Funding.Instance != null)
			{
				Funding.Instance.AddFunds(0.0 - Math.Abs(FundsFailure), TransactionReasons.ContractPenalty);
			}
		}
		if (ReputationFailure != 0f)
		{
			Debug.Log("Penalizing " + ReputationFailure + " reputation from player for contract failure");
			if (Reputation.Instance != null)
			{
				Reputation.Instance.AddReputation(0f - Mathf.Abs(ReputationFailure), TransactionReasons.ContractPenalty);
			}
		}
	}

	public virtual void OnOffered()
	{
	}

	public virtual void OnOfferExpired()
	{
	}

	public virtual void OnAccepted()
	{
	}

	public virtual void OnCancelled()
	{
	}

	public virtual void OnCompleted()
	{
	}

	public virtual void OnDeclined()
	{
	}

	public virtual void OnFailed()
	{
	}

	public virtual void OnDeadlineExpired()
	{
	}

	public virtual void OnFinished()
	{
	}

	public virtual void OnGenerateFailed()
	{
	}

	public virtual void OnWithdrawn()
	{
	}

	public virtual void OnSeen()
	{
	}

	public virtual void OnRead()
	{
	}

	public virtual string GetHashString()
	{
		return "";
	}

	public virtual string MessageOffered()
	{
		return Localizer.Format("#autoLOC_6002414", Agent.Title, GetTitle());
	}

	public virtual string MessageOfferExpired()
	{
		return Localizer.Format("#autoLOC_6002415", Agent.Title, GetTitle());
	}

	public virtual string MessageCancelled()
	{
		return Localizer.Format("#autoLOC_6002416", Agent.Title, GetTitle());
	}

	public virtual string MessageAccepted()
	{
		return Localizer.Format("#autoLOC_266321", Agent.Title) + ": " + GetTitle();
	}

	public virtual string MessageDeadlineExpired()
	{
		return Localizer.Format("#autoLOC_266330", Agent.Title) + ": " + GetTitle();
	}

	public virtual string MessageFailed()
	{
		return Localizer.Format("#autoLOC_266339", Agent.Title) + ": " + GetTitle();
	}

	public virtual string MessageCompleted()
	{
		return Localizer.Format("#autoLOC_266348", Agent.Title) + ": " + GetTitle();
	}

	public virtual string MessageAdvances()
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractAdvance, (float)FundsAdvance, 0f, 0f);
		if (FundsAdvance != 0.0)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsAdvance).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text != string.Empty)
		{
			text = RichTextUtil.TextAdvance(Localizer.Format("#autoLOC_266379"), text);
		}
		return text;
	}

	public virtual string MessageRewards()
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractReward, (float)FundsCompletion, ScienceCompletion, ReputationCompletion);
		if (FundsCompletion != 0.0)
		{
			text = text + "\n<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "\n<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ScienceCompletion != 0f)
		{
			text = text + "\n<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Science) + ScienceCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "\n<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ReputationCompletion != 0f)
		{
			text = text + "\n<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "\n<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text != string.Empty)
		{
			text = RichTextUtil.TextAward(Localizer.Format("#autoLOC_266421"), text);
		}
		return text;
	}

	public virtual string MessageFailurePenalties()
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractPenalty, (float)FundsFailure, 0f, ReputationFailure);
		if (FundsFailure != 0.0)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ReputationFailure != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text != string.Empty)
		{
			text = RichTextUtil.TextPenalty(Localizer.Format("#autoLOC_266459"), text);
		}
		return text;
	}

	public virtual string MessageCancellationPenalties(double fundsPenalty, float repPenalty)
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractPenalty, (float)fundsPenalty, 0f, repPenalty);
		if (FundsFailure != 0.0)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + fundsPenalty).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ReputationFailure != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + repPenalty).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#" + RUIutils.ColorToHex(RichTextUtil.colorPenalty) + "><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text != string.Empty)
		{
			text = RichTextUtil.TextPenalty(Localizer.Format("#autoLOC_266501", KSPUtil.PrintTime(dateFinished - dateAccepted, 2, explicitPositive: false)), text);
		}
		return text;
	}

	public virtual string MissionControlTextRich()
	{
		string text = "";
		text += RichTextUtil.Title(Localizer.Format("#autoLOC_266514"));
		text = text + "<color=#CCCCCC>" + Description + "</color>\n\n";
		text = text + "<b>" + Synopsys + "</b>\n\n";
		text += RichTextUtil.TextParam(Localizer.Format("#autoLOC_266518"), prestige.displayDescription());
		switch (ContractState)
		{
		case State.Active:
			text += KSPRichTextUtil.TextDate(Localizer.Format("#autoLOC_266539"), DateAccepted);
			if (DateDeadline != 0.0)
			{
				text += KSPRichTextUtil.TextDateCompact(Localizer.Format("#autoLOC_266542"), DateDeadline);
			}
			break;
		case State.Offered:
			if (DateExpire != 0.0)
			{
				text += KSPRichTextUtil.TextDateCompact(Localizer.Format("#autoLOC_266526"), DateExpire);
			}
			if (DateDeadline != 0.0)
			{
				text += KSPRichTextUtil.TextDateCompact(Localizer.Format("#autoLOC_266530"), DateDeadline);
			}
			else if (TimeDeadline != 0.0)
			{
				text += KSPRichTextUtil.TextDurationCompact(Localizer.Format("#autoLOC_266534"), TimeDeadline);
			}
			break;
		}
		if (parameters.Count > 0)
		{
			text += RichTextUtil.Title(Localizer.Format("#autoLOC_266553"));
			string text2 = MissionNotes();
			if (text2 != string.Empty)
			{
				text = text + text2 + "\n";
			}
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				ContractParameter parameter = parameters[i];
				text = text + MissionParameter(parameter, 0) + "\n";
			}
		}
		text += RichTextUtil.Title(Localizer.Format("#autoLOC_266566"));
		string text3 = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractAdvance, (float)FundsAdvance, 0f, 0f);
		if (FundsAdvance != 0.0)
		{
			text3 = text3 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsAdvance).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text3 = text3 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text3 = text3 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text3 = text3 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		string text4 = "";
		currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractReward, (float)FundsCompletion, ScienceCompletion, ReputationCompletion);
		if (FundsCompletion != 0.0)
		{
			text4 = text4 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text4 = text4 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ScienceCompletion != 0f)
		{
			text4 = text4 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Science) + ScienceCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text4 = text4 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (ReputationCompletion != 0f)
		{
			text4 = text4 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text4 = text4 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		string text5 = "";
		currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractPenalty, (float)FundsFailure, 0f, ReputationFailure);
		if (FundsFailure != 0.0)
		{
			text5 = text5 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text4 = text4 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text3 = text3 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (ReputationFailure != 0f)
		{
			text5 = text5 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text4 = text4 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text3 != string.Empty)
		{
			text += RichTextUtil.TextAdvance(Localizer.Format("#autoLOC_266649"), text3);
		}
		if (text4 != string.Empty)
		{
			text += RichTextUtil.TextAward(Localizer.Format("#autoLOC_266651"), text4);
		}
		if (CanBeFailed() && text5 != string.Empty)
		{
			text += RichTextUtil.TextPenalty(Localizer.Format("#autoLOC_266653"), text5);
		}
		if (CanBeDeclined() && HighLogic.CurrentGame.Parameters.Career.RepLossDeclined > 0f)
		{
			text += RichTextUtil.TextPenalty(Localizer.Format("#autoLOC_266656"), "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1>-" + HighLogic.CurrentGame.Parameters.Career.RepLossDeclined + "  </color>");
		}
		return text + "\n";
	}

	public string MissionNotes()
	{
		string text = "";
		string notes = Notes;
		if (!string.IsNullOrEmpty(notes))
		{
			text += notes;
		}
		for (int i = 0; i < ParameterCount; i++)
		{
			text += ParameterNotes(GetParameter(i));
		}
		return text;
	}

	public string ParameterNotes(ContractParameter p)
	{
		string text = "";
		string notes = p.Notes;
		if (!string.IsNullOrEmpty(notes))
		{
			text = text + notes + "\n";
		}
		for (int i = 0; i < p.ParameterCount; i++)
		{
			text += ParameterNotes(p.GetParameter(i));
		}
		return text;
	}

	public string MissionParameter(ContractParameter parameter, int indent)
	{
		string text = "";
		text = text + "<b><color=#" + RUIutils.ColorToHex(RichTextUtil.colorParams) + ">" + new string('\t', indent) + parameter.Title + ":</color></b> " + parameter.State.displayDescription() + "\n";
		string text2 = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractReward, (float)parameter.FundsCompletion, parameter.ScienceCompletion, parameter.ReputationCompletion);
		if (parameter.FundsCompletion != 0.0)
		{
			text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + parameter.FundsCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (parameter.ScienceCompletion != 0f)
		{
			text2 = text2 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Science) + parameter.ScienceCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text2 = text2 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (parameter.ReputationCompletion != 0f)
		{
			text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + parameter.ReputationCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text2 != string.Empty)
		{
			text += Localizer.Format("#autoLOC_266732", RUIutils.ColorToHex(RichTextUtil.colorAwards), new string('\t', indent + 1), text2);
		}
		text2 = "";
		currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractPenalty, (float)parameter.FundsFailure, 0f, parameter.ReputationFailure);
		if (parameter.FundsFailure + (double)currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0.0)
		{
			text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + parameter.FundsFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text2 = text2 + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1>  -" + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text2 = text2 + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (parameter.ReputationFailure + currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + parameter.ReputationFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text2 = text2 + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> -" + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
		}
		if (text2 != string.Empty)
		{
			text += Localizer.Format("#autoLOC_266766", RUIutils.ColorToHex(RichTextUtil.colorAwards), new string('\t', indent + 1), text2);
		}
		for (int i = 0; i < parameter.ParameterCount; i++)
		{
			ContractParameter parameter2 = parameter.GetParameter(i);
			text += MissionParameter(parameter2, indent + 1);
		}
		return text;
	}

	public void SetupSeed(int seed = -1)
	{
		if (seed == -1)
		{
			seed = (int)(KSPUtil.SystemDateTime.DateTimeNow().Ticks & 0x7FFFFFFFL);
		}
		missionSeed = seed;
		UnityEngine.Random.InitState(missionSeed);
	}

	public void SetupID()
	{
		long num = 17L;
		num = 391L + GetType().Name.GetHashCode_Net35();
		string hashString = GetHashString();
		if (!string.IsNullOrEmpty(hashString))
		{
			num = num * 23L + GetHashString().GetHashCode_Net35();
		}
		int i = 0;
		for (int count = parameters.Count; i < count; i++)
		{
			ContractParameter contractParameter = parameters[i];
			hashString = contractParameter.HashString;
			num = num * 23L + contractParameter.GetType().Name.GetHashCode_Net35();
			if (!string.IsNullOrEmpty(hashString))
			{
				num = num * 23L + hashString.GetHashCode_Net35();
			}
		}
		contractID = num;
	}

	public void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, "<b>" + GetTitle() + "</b>\n\n" + message, color, icon));
		}
		Debug.Log("Contract (" + GetTitle() + "): " + message);
	}

	public bool Offer()
	{
		if (state == State.Generated)
		{
			if (expiryType == DeadlineType.Floating)
			{
				dateExpire = GameTime + TimeExpiry;
			}
			if (deadlineType == DeadlineType.Fixed)
			{
				dateDeadline = GameTime + TimeDeadline;
			}
			SetState(State.Offered);
			return true;
		}
		return false;
	}

	public bool Accept()
	{
		if (state == State.Offered)
		{
			dateAccepted = GameTime;
			if (deadlineType == DeadlineType.Floating)
			{
				dateDeadline = GameTime + TimeDeadline;
			}
			SetState(State.Active);
			return true;
		}
		return false;
	}

	public bool Decline()
	{
		if (state == State.Offered)
		{
			dateFinished = GameTime;
			SetState(State.Declined);
			if (Reputation.Instance != null && HighLogic.CurrentGame.Parameters.Career.RepLossDeclined > 0f)
			{
				Reputation.Instance.AddReputation(0f - HighLogic.CurrentGame.Parameters.Career.RepLossDeclined, TransactionReasons.ContractDecline);
			}
			return true;
		}
		return false;
	}

	public bool Cancel()
	{
		if (state == State.Active)
		{
			dateFinished = GameTime;
			SetState(State.Cancelled);
			return true;
		}
		return false;
	}

	public void Update()
	{
		switch (state)
		{
		case State.Active:
		{
			if (deadlineType != DeadlineType.None && GameTime >= dateDeadline)
			{
				dateFinished = dateDeadline;
				SetState(State.DeadlineExpired);
				break;
			}
			if (!MeetRequirements())
			{
				dateFinished = dateExpire;
				SetState(State.OfferExpired);
				break;
			}
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				parameters[i].Update();
			}
			try
			{
				OnUpdate();
				break;
			}
			catch (Exception ex)
			{
				Debug.LogError("[Contract] - Call to OnUpdate failed " + ex);
				break;
			}
		}
		case State.Offered:
			if (expiryType != DeadlineType.None && GameTime >= dateExpire)
			{
				dateFinished = dateExpire;
				SetState(State.OfferExpired);
			}
			else if (!MeetRequirements())
			{
				dateFinished = dateExpire;
				SetState(State.OfferExpired);
			}
			break;
		}
	}

	public void Reset()
	{
		if (state == State.Active)
		{
			int count = parameters.Count;
			while (count-- > 0)
			{
				parameters[count].Reset();
			}
			OnReset();
		}
	}

	public void Withdraw()
	{
		if (state != State.Withdrawn)
		{
			SetState(State.Withdrawn);
		}
	}

	public bool Fail()
	{
		if (state == State.Active)
		{
			dateFinished = GameTime;
			SetState(State.Failed);
			return true;
		}
		return false;
	}

	public bool Complete()
	{
		if (state == State.Active)
		{
			dateFinished = GameTime;
			SetState(State.Completed);
			return true;
		}
		return false;
	}

	public void SetState(State newState)
	{
		if (newState == state)
		{
			return;
		}
		if (newState == State.Active)
		{
			Register();
		}
		else
		{
			Unregister();
		}
		state = newState;
		OnStateChange.Fire(state);
		if (!IgnoresWeight && !AutoAccept)
		{
			ContractSystem.AdjustWeight(GetType().Name, this);
			List<CelestialBody> weightBodies = GetWeightBodies();
			if (weightBodies != null)
			{
				int count = weightBodies.Count;
				while (count-- > 0)
				{
					if (weightBodies[count] != null)
					{
						ContractSystem.AdjustWeight(weightBodies[count].name, this);
					}
				}
			}
		}
		switch (state)
		{
		case State.Offered:
			OnOffered();
			GameEvents.Contract.onOffered.Fire(this);
			break;
		case State.OfferExpired:
			OnOfferExpired();
			GameEvents.Contract.onFinished.Fire(this);
			break;
		case State.Declined:
			OnDeclined();
			GameEvents.Contract.onDeclined.Fire(this);
			break;
		case State.Cancelled:
			PenalizeCancellation();
			OnCancelled();
			OnFinished();
			AnalyticsUtil.LogContractCancelled(this);
			GameEvents.Contract.onCancelled.Fire(this);
			GameEvents.Contract.onFailed.Fire(this);
			GameEvents.Contract.onFinished.Fire(this);
			break;
		case State.Active:
			AwardAdvance();
			OnAccepted();
			AnalyticsUtil.LogContractAccepted(this);
			GameEvents.Contract.onAccepted.Fire(this);
			break;
		case State.Completed:
			AwardCompletion();
			OnCompleted();
			OnFinished();
			SendStateMessage(Localizer.Format("#autoLOC_267072"), MessageCompleted() + StateMsgAddition(MessageRewards()), MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.COMPLETE);
			AnalyticsUtil.LogContractCompleted(this);
			GameEvents.Contract.onCompleted.Fire(this);
			GameEvents.Contract.onFinished.Fire(this);
			break;
		case State.DeadlineExpired:
			PenalizeFailure();
			OnDeadlineExpired();
			OnFinished();
			SendStateMessage(Localizer.Format("#autoLOC_267084"), MessageDeadlineExpired() + StateMsgAddition(MessageFailurePenalties()), MessageSystemButton.MessageButtonColor.const_0, MessageSystemButton.ButtonIcons.FAIL);
			GameEvents.Contract.onFailed.Fire(this);
			GameEvents.Contract.onFinished.Fire(this);
			break;
		case State.Failed:
			PenalizeFailure();
			OnFailed();
			OnFinished();
			SendStateMessage(Localizer.Format("#autoLOC_267092"), MessageFailed() + StateMsgAddition(MessageFailurePenalties()), MessageSystemButton.MessageButtonColor.const_0, MessageSystemButton.ButtonIcons.FAIL);
			GameEvents.Contract.onFailed.Fire(this);
			GameEvents.Contract.onFinished.Fire(this);
			break;
		case State.Withdrawn:
			OnWithdrawn();
			OnFinished();
			GameEvents.Contract.onFinished.Fire(this);
			break;
		}
	}

	public string StateMsgAddition(string addition)
	{
		if (string.IsNullOrEmpty(addition))
		{
			return string.Empty;
		}
		return "\n\n" + addition;
	}

	public bool IsFinished()
	{
		State state = this.state;
		if ((uint)(state - 2) > 2u && (uint)(state - 6) > 3u)
		{
			return false;
		}
		return true;
	}

	public void Register()
	{
		int i = 0;
		for (int count = parameters.Count; i < count; i++)
		{
			parameters[i].Register();
		}
		OnRegister();
	}

	public void Unregister()
	{
		int i = 0;
		for (int count = parameters.Count; i < count; i++)
		{
			parameters[i].Unregister();
		}
		OnUnregister();
	}

	public void Kill()
	{
		OnFinished();
		GameEvents.Contract.onFinished.Fire(this);
		Unregister();
	}

	public void SetViewed(Viewed viewed)
	{
		if (viewed > this.viewed)
		{
			this.viewed = viewed;
			OnViewedChange.Fire(viewed);
			switch (viewed)
			{
			case Viewed.Read:
				OnRead();
				GameEvents.Contract.onRead.Fire(this);
				break;
			case Viewed.Seen:
				OnSeen();
				GameEvents.Contract.onSeen.Fire(this);
				break;
			}
		}
	}

	public static Contract Generate(Type contractType, ContractPrestige difficulty, int seed, State state)
	{
		Contract contract = (Contract)Activator.CreateInstance(contractType);
		contract.contractGuid = Guid.NewGuid();
		contract.SetupSeed(seed);
		contract.prestige = difficulty;
		contract.state = state;
		if (!contract.MeetRequirements())
		{
			return null;
		}
		if (!contract.Generate())
		{
			return null;
		}
		if (contract.agent == null)
		{
			contract.agent = AgentList.Instance.GetSuitableAgentForContract(contract);
		}
		if (contract.agent == null)
		{
			contract.OnGenerateFailed();
			return null;
		}
		if (!contract.agent.ProcessContract(contract))
		{
			contract.OnGenerateFailed();
			return null;
		}
		contract.FundsFailure = Math.Max(contract.FundsFailure, contract.FundsAdvance);
		contract.SetupID();
		return contract;
	}

	public static Contract Load(Contract contract, ConfigNode node)
	{
		if (node.HasValue("guid"))
		{
			contract.contractGuid = new Guid(node.GetValue("guid"));
		}
		else
		{
			contract.contractGuid = Guid.NewGuid();
		}
		string text = node.GetValue("prestige");
		if (text == null)
		{
			text = node.GetValue("diff");
			if (text == "Easy")
			{
				text = "0";
			}
			if (text == "Medium")
			{
				text = "1";
			}
			if (text == "Hard")
			{
				text = "2";
			}
		}
		string value = node.GetValue("seed");
		string value2 = node.GetValue("state");
		string value3 = node.GetValue("viewed");
		string value4 = node.GetValue("values");
		string value5 = node.GetValue("agent");
		string name = "";
		bool flag;
		if (flag = node.HasValue("agentName"))
		{
			name = node.GetValue("agentName");
		}
		if (text != null && value != null && value2 != null && value4 != null && value5 != null)
		{
			contract.SetupSeed(int.Parse(value));
			contract.prestige = (ContractPrestige)int.Parse(text);
			contract.state = (State)Enum.Parse(typeof(State), value2);
			contract.viewed = ((value3 != null) ? ((Viewed)Enum.Parse(typeof(Viewed), value3)) : Viewed.Unseen);
			if (flag)
			{
				contract.agent = AgentList.Instance.GetAgent(name);
			}
			else
			{
				contract.agent = AgentList.Instance.GetAgentbyTitle(value5);
			}
			if (contract.agent == null)
			{
				Debug.LogError("Contract: Contract agent does not exist, randomizing agent");
				contract.agent = AgentList.Instance.GetAgentRandom();
			}
			if (node.HasValue("deadlineType"))
			{
				contract.deadlineType = (DeadlineType)Enum.Parse(typeof(DeadlineType), node.GetValue("deadlineType"));
			}
			if (node.HasValue("expiryType"))
			{
				contract.expiryType = (DeadlineType)Enum.Parse(typeof(DeadlineType), node.GetValue("expiryType"));
			}
			if (node.HasValue("autoAccept"))
			{
				contract.AutoAccept = bool.Parse(node.GetValue("autoAccept"));
			}
			if (node.HasValue("ignoresWeight"))
			{
				contract.IgnoresWeight = bool.Parse(node.GetValue("ignoresWeight"));
			}
			string[] array = value4.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length != 12)
			{
				Debug.LogError("Contract: Contract config values are invalid");
				return null;
			}
			contract.TimeExpiry = double.Parse(array[0]);
			contract.TimeDeadline = double.Parse(array[1]);
			contract.FundsAdvance = double.Parse(array[2]);
			contract.FundsCompletion = double.Parse(array[3]);
			contract.FundsFailure = double.Parse(array[4]);
			contract.ScienceCompletion = float.Parse(array[5]);
			contract.ReputationCompletion = float.Parse(array[6]);
			contract.ReputationFailure = float.Parse(array[7]);
			contract.dateExpire = double.Parse(array[8]);
			contract.dateAccepted = double.Parse(array[9]);
			contract.dateDeadline = double.Parse(array[10]);
			contract.dateFinished = double.Parse(array[11]);
			ConfigNode[] nodes = node.GetNodes("PARAM");
			int num = 0;
			int num2 = nodes.Length;
			string value6;
			while (true)
			{
				if (num < num2)
				{
					ConfigNode configNode = nodes[num];
					value6 = configNode.GetValue("name");
					if (value6 != null)
					{
						Type parameterType = ContractSystem.GetParameterType(value6);
						if (parameterType == null)
						{
							break;
						}
						ContractParameter contractParameter = (ContractParameter)Activator.CreateInstance(parameterType);
						contract.AddParameter(contractParameter);
						contractParameter.Load(configNode);
						num++;
						continue;
					}
					Debug.LogError("Contract: Contract parameter config is invalid");
					return null;
				}
				try
				{
					contract.OnLoad(node);
					contract.SetupID();
				}
				catch (Exception ex)
				{
					Debug.LogError("Contract: Exception while OnLoad() or SetupID() on contract " + contract.Title + ", exception " + ex);
				}
				return contract;
			}
			Debug.LogError("Contract: Contract parameter type name '" + value6 + "' is invalid");
			return null;
		}
		Debug.LogError("Contract: Contract config is invalid");
		return null;
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("guid", contractGuid.ToString());
		node.AddValue("type", GetType().Name);
		node.AddValue("prestige", ((int)Prestige).ToString());
		node.AddValue("seed", MissionSeed);
		node.AddValue("state", ContractState);
		node.AddValue("viewed", ContractViewed);
		node.AddValue("agent", agent.Title);
		node.AddValue("agentName", agent.Name);
		node.AddValue("deadlineType", deadlineType);
		node.AddValue("expiryType", expiryType);
		if (AutoAccept)
		{
			node.AddValue("autoAccept", AutoAccept.ToString());
		}
		if (IgnoresWeight)
		{
			node.AddValue("ignoresWeight", IgnoresWeight.ToString());
		}
		string value = TimeExpiry + "," + TimeDeadline + "," + FundsAdvance + "," + FundsCompletion + "," + FundsFailure + "," + ScienceCompletion + "," + ReputationCompletion + "," + ReputationFailure + "," + dateExpire + "," + dateAccepted + "," + dateDeadline + "," + dateFinished;
		node.AddValue("values", value);
		OnSave(node);
		int i = 0;
		for (int count = parameters.Count; i < count; i++)
		{
			ContractParameter contractParameter = parameters[i];
			ConfigNode node2 = node.AddNode("PARAM");
			contractParameter.Save(node2);
		}
	}

	public void GenerateFailed()
	{
		OnGenerateFailed();
	}

	public static List<CelestialBody> GetBodies(bool includeKerbin, bool includeSun)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin))
			{
				list.Add(PSystemManager.Instance.localBodies[i]);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		return list;
	}

	public static List<CelestialBody> GetBodies_Reached(bool includeKerbin, bool includeSun)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && ProgressTracking.Instance.NodeReached(PSystemManager.Instance.localBodies[i].name))
			{
				list.Add(PSystemManager.Instance.localBodies[i]);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		return list;
	}

	public static List<CelestialBody> GetBodies_NotReached(bool includeKerbin, bool includeSun)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && !ProgressTracking.Instance.NodeReached(PSystemManager.Instance.localBodies[i].name))
			{
				list.Add(PSystemManager.Instance.localBodies[i]);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		return list;
	}

	public static List<CelestialBody> GetBodies_InComplete(bool includeKerbin, bool includeSun, string notComplete)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && !ProgressTracking.Instance.NodeComplete(PSystemManager.Instance.localBodies[i].name, notComplete))
			{
				list.Add(PSystemManager.Instance.localBodies[i]);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		return list;
	}

	public static List<CelestialBody> GetBodies_Complete(bool includeKerbin, bool includeSun, string complete)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && ProgressTracking.Instance.NodeComplete(PSystemManager.Instance.localBodies[i].name, complete))
			{
				list.Add(PSystemManager.Instance.localBodies[i]);
			}
		}
		list.Sort((CelestialBody b1, CelestialBody b2) => b1.scienceValues.RecoveryValue.CompareTo(b2.scienceValues.RecoveryValue));
		return list;
	}

	public static int CountBodies_Complete(bool includeKerbin, bool includeSun, string nodeComplete)
	{
		int num = 0;
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && ProgressTracking.Instance.NodeComplete(PSystemManager.Instance.localBodies[i].name, nodeComplete))
			{
				num++;
			}
		}
		return num;
	}

	public static int CountBodies_Reached(bool includeKerbin, bool includeSun)
	{
		int num = 0;
		for (int i = 0; i < PSystemManager.Instance.localBodies.Count; i++)
		{
			if ((i != 0 || includeSun) && (i != 1 || includeKerbin) && ProgressTracking.Instance.NodeReached(PSystemManager.Instance.localBodies[i].name))
			{
				num++;
			}
		}
		return num;
	}

	public static List<CelestialBody> GetBodies(string nodeName, ProgressState nodeState, Func<CelestialBody, bool> where = null)
	{
		List<CelestialBody> list = new List<CelestialBody>();
		AddBodyNodes(nodeState, nodeName, list, ProgressTracking.Instance.GetBodyTree(), where);
		return list;
	}

	public static void AddBodyNodes(ProgressState nodeState, string nodeName, List<CelestialBody> bodies, CelestialBodySubtree tree, Func<CelestialBody, bool> where = null)
	{
		ProgressNode progressNode = tree.Subtree[nodeName];
		if (progressNode != null)
		{
			switch (nodeState)
			{
			case ProgressState.Unreached:
				if (!progressNode.IsReached && (where == null || where(tree.Body)))
				{
					bodies.Add(tree.Body);
				}
				break;
			case ProgressState.Reached:
				if (progressNode.IsReached && (where == null || where(tree.Body)))
				{
					bodies.Add(tree.Body);
				}
				break;
			case ProgressState.Complete:
				if (progressNode.IsComplete && (where == null || where(tree.Body)))
				{
					bodies.Add(tree.Body);
				}
				break;
			}
		}
		for (int i = 0; i < tree.childTrees.Length; i++)
		{
			AddBodyNodes(nodeState, nodeName, bodies, tree.childTrees[i]);
		}
	}

	public static List<CelestialBody> GetBodies(ProgressState bodyState, Func<CelestialBody, bool> where = null)
	{
		List<CelestialBodySubtree> list = new List<CelestialBodySubtree>();
		AddBodyTrees(bodyState, list, ProgressTracking.Instance.GetBodyTree());
		List<CelestialBody> list2 = new List<CelestialBody>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (where == null || where(list[i].Body))
			{
				list2.Add(list[i].Body);
			}
		}
		return list2;
	}

	public static void AddBodyTrees(ProgressState bodyState, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree)
	{
		switch (bodyState)
		{
		case ProgressState.Unreached:
			if (!tree.IsReached)
			{
				bodies.Add(tree);
			}
			break;
		case ProgressState.Reached:
			if (tree.IsReached)
			{
				bodies.Add(tree);
			}
			break;
		case ProgressState.Complete:
			if (tree.IsComplete)
			{
				bodies.Add(tree);
			}
			break;
		}
		for (int i = 0; i < tree.childTrees.Length; i++)
		{
			AddBodyTrees(bodyState, bodies, tree.childTrees[i]);
		}
	}

	public static List<CelestialBody> GetBodies(ProgressState bodyState, string nodeName, ProgressState nodeState, Func<CelestialBody, bool> where = null)
	{
		List<CelestialBodySubtree> list = new List<CelestialBodySubtree>();
		AddBodyTrees(bodyState, nodeName, nodeState, list, ProgressTracking.Instance.GetBodyTree());
		List<CelestialBody> list2 = new List<CelestialBody>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (where == null || where(list[i].Body))
			{
				list2.Add(list[i].Body);
			}
		}
		return list2;
	}

	public static void AddBodyTrees(ProgressState bodyState, string nodeName, ProgressState nodeState, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree)
	{
		if ((bodyState == ProgressState.Reached && tree.IsReached) || (bodyState == ProgressState.Complete && tree.IsComplete))
		{
			ProgressNode progressNode = tree.Subtree[nodeName];
			if (progressNode != null)
			{
				switch (nodeState)
				{
				case ProgressState.Unreached:
					if (!progressNode.IsReached)
					{
						bodies.Add(tree);
					}
					break;
				case ProgressState.Reached:
					if (progressNode.IsReached)
					{
						bodies.Add(tree);
					}
					break;
				case ProgressState.Complete:
					if (progressNode.IsComplete)
					{
						bodies.Add(tree);
					}
					break;
				}
			}
		}
		for (int i = 0; i < tree.childTrees.Length; i++)
		{
			AddBodyTrees(bodyState, nodeName, nodeState, bodies, tree.childTrees[i]);
		}
	}

	public static List<CelestialBody> GetBodies_NextUnreached(int depth, Func<CelestialBody, bool> where = null)
	{
		List<CelestialBodySubtree> list = new List<CelestialBodySubtree>();
		AddNextUnreachedBodyTrees(ref depth, list, ProgressTracking.Instance.GetBodyTree(), where);
		List<CelestialBody> list2 = new List<CelestialBody>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			list2.Add(list[i].Body);
		}
		return list2;
	}

	public CelestialBody WeightedBodyChoice(IList<CelestialBody> bodies, System.Random generator = null)
	{
		KSPRandom kSPRandom = null;
		if (generator != null)
		{
			kSPRandom = generator as KSPRandom;
		}
		if (IgnoresWeight)
		{
			if (kSPRandom != null)
			{
				return bodies[kSPRandom.Next(0, bodies.Count)];
			}
			return bodies[UnityEngine.Random.Range(0, bodies.Count)];
		}
		return ContractSystem.WeightedBodyChoice(bodies, (kSPRandom != null) ? kSPRandom : generator);
	}

	public static void AddNextUnreachedBodyTrees(ref int depth, List<CelestialBodySubtree> bodies, CelestialBodySubtree tree, Func<CelestialBody, bool> where)
	{
		if (!tree.IsReached && (where == null || where(tree.Body)))
		{
			bodies.Add(tree);
			depth--;
		}
		for (int i = 0; i < tree.childTrees.Length; i++)
		{
			if (depth <= 0)
			{
				break;
			}
			AddNextUnreachedBodyTrees(ref depth, bodies, tree.childTrees[i], where);
		}
	}
}
