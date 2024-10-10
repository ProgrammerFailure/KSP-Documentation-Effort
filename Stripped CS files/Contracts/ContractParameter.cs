using System;
using System.Collections.Generic;
using ns11;
using ns9;
using UnityEngine;

namespace Contracts;

public class ContractParameter : IContractParameterHost
{
	[NonSerialized]
	public IContractParameterHost parent;

	[NonSerialized]
	public Contract root;

	[SerializeField]
	public ParameterState state;

	[SerializeField]
	public string id;

	[SerializeField]
	public bool disableOnStateChange = true;

	[SerializeField]
	public bool optional;

	[SerializeField]
	public bool allowPartialFailure;

	public bool enabled = true;

	[SerializeField]
	public List<ContractParameter> parameters = new List<ContractParameter>();

	public double FundsCompletion;

	public double FundsFailure;

	public float ScienceCompletion;

	public float ReputationCompletion;

	public float ReputationFailure;

	public EventData<ContractParameter, ParameterState> OnStateChange = new EventData<ContractParameter, ParameterState>("OnStateChange");

	public IContractParameterHost Parent => parent;

	public Contract Root => root;

	public ParameterState State => state;

	public string String_0
	{
		get
		{
			return id;
		}
		set
		{
			id = value;
		}
	}

	public bool DisableOnStateChange
	{
		get
		{
			return disableOnStateChange;
		}
		set
		{
			disableOnStateChange = value;
		}
	}

	public bool Optional
	{
		get
		{
			return optional;
		}
		set
		{
			optional = value;
		}
	}

	public bool AllowPartialFailure
	{
		get
		{
			return allowPartialFailure;
		}
		set
		{
			allowPartialFailure = value;
		}
	}

	public bool Enabled
	{
		get
		{
			return enabled;
		}
		set
		{
			if (value)
			{
				Enable();
			}
			else
			{
				Disable();
			}
		}
	}

	public int ParameterCount => parameters.Count;

	public ContractParameter this[int index] => GetParameter(index);

	public ContractParameter this[string id] => GetParameter(id);

	public ContractParameter this[Type type] => GetParameter(type);

	public string HashString
	{
		get
		{
			string text = "";
			string hashString = GetHashString();
			if (hashString != null)
			{
				text += hashString;
			}
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				text += parameters[i].HashString;
			}
			return text;
		}
	}

	public string Title => GetTitle();

	public string Notes => GetNotes();

	public string MessageComplete => GetMessageComplete();

	public string MessageFailed => GetMessageFailed();

	public string MessageIncomplete => GetMessageIncomplete();

	public IEnumerable<ContractParameter> AllParameters
	{
		get
		{
			yield return this;
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

	public void NestToParent(IContractParameterHost parent)
	{
		this.parent = parent;
		root = parent.Root;
	}

	public void Enable()
	{
		if (!enabled)
		{
			enabled = true;
			Register();
		}
	}

	public void Disable(bool recursive = false)
	{
		if (!enabled)
		{
			return;
		}
		enabled = false;
		Unregister();
		if (recursive)
		{
			int count = parameters.Count;
			while (count-- > 0)
			{
				parameters[count].Disable(recursive: true);
			}
		}
	}

	public void ParameterStateUpdate(ContractParameter p)
	{
		GameEvents.Contract.onParameterChange.Fire(root, p);
		Parent.ParameterStateUpdate(p);
		OnParameterStateChange(p);
	}

	public ContractParameter AddParameter(ContractParameter parameter, string id = null)
	{
		parameter.NestToParent(this);
		if (id != null)
		{
			parameter.id = id;
		}
		parameters.Add(parameter);
		return parameter;
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
				if (parameters[num].String_0 == id && parameters[num] is T)
				{
					break;
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
		parameters.RemoveAt(num);
	}

	public void RemoveParameter(ContractParameter parameter)
	{
		parameters.Remove(parameter);
	}

	public virtual void OnParameterStateChange(ContractParameter p)
	{
	}

	public bool AllChildParametersComplete()
	{
		int count = parameters.Count;
		do
		{
			if (count-- <= 0)
			{
				return true;
			}
		}
		while (parameters[count].State == ParameterState.Complete);
		return false;
	}

	public bool AnyChildParametersFailed()
	{
		int count = parameters.Count;
		do
		{
			if (count-- <= 0)
			{
				return false;
			}
		}
		while (parameters[count].State != ParameterState.Failed);
		return true;
	}

	public void SetFunds(float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		FundsCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractFundsCompletionFactor(Root.Prestige) * num);
	}

	public void SetFunds(float completion, float failure, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		FundsCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractFundsCompletionFactor(Root.Prestige) * num);
		FundsFailure = Mathf.Round(failure * GameVariables.Instance.GetContractFundsFailureFactor(Root.Prestige) * num);
	}

	public void SetReputation(float completion, float failure, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ReputationCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractReputationCompletionFactor(Root.Prestige) * num);
		ReputationFailure = Mathf.Round(failure * GameVariables.Instance.GetContractReputationFailureFactor(Root.Prestige) * num);
	}

	public void SetReputation(float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ReputationCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractReputationCompletionFactor(Root.Prestige) * num);
	}

	public void SetScience(float completion, CelestialBody body = null)
	{
		float num = 1f;
		if (body != null)
		{
			num = GetDestinationWeight(body);
		}
		ScienceCompletion = Mathf.Round(completion * GameVariables.Instance.GetContractScienceCompletionFactor(Root.Prestige) * num);
	}

	public float GetDestinationWeight(CelestialBody body)
	{
		return GameVariables.Instance.GetContractDestinationWeight(body);
	}

	public virtual void AwardCompletion()
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractReward, (float)FundsCompletion, ScienceCompletion, ReputationCompletion);
		if (FundsCompletion != 0.0)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1> " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
			Funding.Instance.AddFunds(FundsCompletion, TransactionReasons.ContractReward);
			FundsCompletion = 0.0;
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (ScienceCompletion + currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Science) + ScienceCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
			ResearchAndDevelopment.Instance.AddScience(ScienceCompletion, TransactionReasons.ContractReward);
			ScienceCompletion = 0f;
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (ReputationCompletion + currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationCompletion).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
			Reputation.Instance.AddReputation(ReputationCompletion, TransactionReasons.ContractReward);
			ReputationCompletion = 0f;
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (!string.IsNullOrEmpty(text))
		{
			text = MessageComplete + "\n\n" + text;
			SendStateMessage(Localizer.Format("#autoLOC_268165"), text, MessageSystemButton.MessageButtonColor.GREEN, MessageSystemButton.ButtonIcons.COMPLETE);
		}
	}

	public virtual void PenalizeFailure()
	{
		string text = "";
		CurrencyModifierQuery currencyModifierQuery = CurrencyModifierQuery.RunQuery(TransactionReasons.ContractPenalty, (float)FundsFailure, 0f, ReputationFailure);
		if (FundsFailure + (double)currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0.0)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1> " + ((double)currencyModifierQuery.GetEffectDelta(Currency.Funds) + FundsFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
			Funding.Instance.AddFunds(0.0 - Math.Abs(FundsFailure), TransactionReasons.ContractPenalty);
			FundsFailure = 0.0;
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Funds) != 0f)
		{
			text = text + "<color=#B4D455><sprite=\"CurrencySpriteAsset\" name=\"Funds\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Funds, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (currencyModifierQuery.GetEffectDelta(Currency.Science) != 0f)
		{
			text = text + "<color=#6DCFF6><sprite=\"CurrencySpriteAsset\" name=\"Science\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Science, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (ReputationFailure + currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + (currencyModifierQuery.GetEffectDelta(Currency.Reputation) + ReputationFailure).ToString("N0") + " " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "  </color>";
			Reputation.Instance.AddReputation(0f - Math.Abs(ReputationFailure), TransactionReasons.ContractPenalty);
			ReputationFailure = 0f;
		}
		else if (currencyModifierQuery.GetEffectDelta(Currency.Reputation) != 0f)
		{
			text = text + "<color=#E0D503><sprite=\"CurrencySpriteAsset\" name=\"Reputation\" tint=1> " + currencyModifierQuery.GetEffectDeltaText(Currency.Reputation, "N0", CurrencyModifierQuery.TextStyling.OnGUI) + "   </color>";
		}
		if (string.IsNullOrEmpty(text))
		{
			text = MessageFailed;
		}
		if (!string.IsNullOrEmpty(text) || allowPartialFailure)
		{
			SendStateMessage(message: (!string.IsNullOrEmpty(text)) ? (MessageFailed + "\n\n" + text) : MessageFailed, title: Localizer.Format("#autoLOC_268204"), color: MessageSystemButton.MessageButtonColor.const_0, icon: MessageSystemButton.ButtonIcons.FAIL);
		}
	}

	public void Load(ConfigNode node)
	{
		string value = node.GetValue("state");
		if (value != null)
		{
			state = (ParameterState)Enum.Parse(typeof(ParameterState), value);
		}
		if (node.HasValue("enabled"))
		{
			enabled = bool.Parse(node.GetValue("enabled"));
		}
		if (node.HasValue("optional"))
		{
			optional = bool.Parse(node.GetValue("optional"));
		}
		if (node.HasValue("id"))
		{
			id = node.GetValue("id");
		}
		if (node.HasValue("disableOnStateChange"))
		{
			disableOnStateChange = bool.Parse(node.GetValue("disableOnStateChange"));
		}
		if (node.HasValue("allowPartialFailure"))
		{
			allowPartialFailure = bool.Parse(node.GetValue("allowPartialFailure"));
		}
		if (node.HasValue("values"))
		{
			string[] array = node.GetValue("values").Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 5)
			{
				FundsCompletion = double.Parse(array[0]);
				FundsFailure = double.Parse(array[1]);
				ScienceCompletion = float.Parse(array[2]);
				ReputationCompletion = float.Parse(array[3]);
				ReputationFailure = float.Parse(array[4]);
			}
		}
		OnLoad(node);
		ConfigNode[] nodes = node.GetNodes("PARAM");
		int i = 0;
		for (int num = nodes.Length; i < num; i++)
		{
			ConfigNode configNode = nodes[i];
			string value2 = configNode.GetValue("name");
			if (value2 == null)
			{
				Debug.LogError("ContractParameter: Contract parameter config is invalid");
				continue;
			}
			Type parameterType = ContractSystem.GetParameterType(value2);
			if (parameterType == null)
			{
				Debug.LogError("ContractParameter: Contract parameter type name '" + value2 + "' is invalid");
				continue;
			}
			ContractParameter contractParameter = (ContractParameter)Activator.CreateInstance(parameterType);
			AddParameter(contractParameter);
			contractParameter.Load(configNode);
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("name", GetType().Name);
		if (!string.IsNullOrEmpty(id))
		{
			node.AddValue("id", id);
		}
		if (!enabled)
		{
			node.AddValue("enabled", enabled.ToString());
		}
		if (optional)
		{
			node.AddValue("optional", optional.ToString());
		}
		node.AddValue("state", state.ToString());
		if (!disableOnStateChange)
		{
			node.AddValue("disableOnStateChange", disableOnStateChange.ToString());
		}
		if (allowPartialFailure)
		{
			node.AddValue("allowPartialFailure", allowPartialFailure);
		}
		string value = FundsCompletion + "," + FundsFailure + "," + ScienceCompletion + "," + ReputationCompletion + "," + ReputationFailure;
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

	public void Register()
	{
		if (enabled)
		{
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				parameters[i].Register();
			}
			OnRegister();
		}
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

	public void Update()
	{
		if (enabled)
		{
			int i = 0;
			for (int count = parameters.Count; i < count; i++)
			{
				parameters[i].Update();
			}
			try
			{
				OnUpdate();
			}
			catch (Exception ex)
			{
				Debug.LogError("[ContractParameter] - Call to OnUpdate failed " + ex);
			}
		}
	}

	public void Reset()
	{
		if (enabled)
		{
			int count = parameters.Count;
			while (count-- > 0)
			{
				parameters[count].Reset();
			}
			OnReset();
		}
	}

	public virtual string GetHashString()
	{
		return "";
	}

	public virtual string GetTitle()
	{
		return Localizer.Format("#autoLOC_268399");
	}

	public virtual string GetNotes()
	{
		return null;
	}

	public virtual string GetMessageComplete()
	{
		return Localizer.Format("#autoLOC_268411", Title);
	}

	public virtual string GetMessageFailed()
	{
		return Localizer.Format("#autoLOC_268417", Title);
	}

	public virtual string GetMessageIncomplete()
	{
		return Localizer.Format("#autoLOC_268423", Title);
	}

	public virtual void OnLoad(ConfigNode node)
	{
	}

	public virtual void OnSave(ConfigNode node)
	{
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

	public long CreateID()
	{
		long num = 17L;
		num = 391L + GetType().Name.GetHashCode_Net35();
		string hashString = HashString;
		if (hashString != null)
		{
			num = num * 23L + hashString.GetHashCode_Net35();
		}
		int i = 0;
		for (int count = parameters.Count; i < count; i++)
		{
			ContractParameter contractParameter = parameters[i];
			hashString = contractParameter.HashString;
			if (hashString != null)
			{
				num = num * 23L + (contractParameter.GetType().Name + hashString).GetHashCode_Net35();
			}
		}
		return num;
	}

	public void SetComplete()
	{
		if (state != ParameterState.Complete)
		{
			try
			{
				AwardCompletion();
			}
			catch (Exception message)
			{
				Debug.LogErrorFormat("[ContractParameter]: {0} on Contract {1} failed to process Completion Award processing.", id, parent.Title);
				Debug.LogError(message);
			}
			state = ParameterState.Complete;
			if (disableOnStateChange)
			{
				Disable();
			}
			OnStateChange.Fire(this, state);
			GameEvents.Contract.onParameterChange.Fire(Root, this);
			parent.ParameterStateUpdate(this);
			if (GameSettings.VERBOSE_DEBUG_LOG)
			{
				Debug.LogFormat("[ContractParameter]: {0} on Contract {1} set to Complete.", id, parent.Title);
			}
		}
	}

	public void SetIncomplete()
	{
		if (state != 0)
		{
			if (disableOnStateChange)
			{
				Disable();
			}
			state = ParameterState.Incomplete;
			OnStateChange.Fire(this, state);
			GameEvents.Contract.onParameterChange.Fire(Root, this);
			parent.ParameterStateUpdate(this);
			if (GameSettings.VERBOSE_DEBUG_LOG)
			{
				Debug.LogFormat("[ContractParameter]: {0} on Contract {1} set to Incomplete.", id, parent.Title);
			}
		}
	}

	public void SetFailed()
	{
		if (state == ParameterState.Failed)
		{
			return;
		}
		try
		{
			PenalizeFailure();
		}
		catch (Exception message)
		{
			Debug.LogErrorFormat("[ContractParameter]: {0} on Contract {1} failed to process Penalize Failure Award processing.", id, parent.Title);
			Debug.LogError(message);
		}
		if (disableOnStateChange)
		{
			Disable();
		}
		state = ParameterState.Failed;
		OnStateChange.Fire(this, state);
		GameEvents.Contract.onParameterChange.Fire(Root, this);
		parent.ParameterStateUpdate(this);
		if (allowPartialFailure)
		{
			int count = parameters.Count;
			while (count-- > 0)
			{
				if (GameSettings.VERBOSE_DEBUG_LOG)
				{
					Debug.LogFormat("[ContractParameter]: {0} on Contract {1} set to Disabled due to partial failure.", parameters[count].id, parent.Title);
				}
				parameters[count].Disable(recursive: true);
			}
		}
		if (GameSettings.VERBOSE_DEBUG_LOG)
		{
			Debug.LogFormat("[ContractParameter]: {0} on Contract {1} set to Failed.", id, parent.Title);
		}
	}

	public void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		if (MessageSystem.Instance != null)
		{
			MessageSystem.Instance.AddMessage(new MessageSystem.Message(title, "<b>" + Root.Title + "</b>\n\n" + message, color, icon));
		}
		Debug.Log("Contract (" + Root.Title + "): " + message);
	}
}
