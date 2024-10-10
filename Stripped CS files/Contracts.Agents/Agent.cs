using System;
using System.Collections.Generic;
using ns9;
using UnityEngine;

namespace Contracts.Agents;

public class Agent
{
	[SerializeField]
	public string name = "";

	[SerializeField]
	public string title = "";

	[SerializeField]
	public string description = "";

	[SerializeField]
	public string logoURL;

	[SerializeField]
	public Texture2D logo;

	[SerializeField]
	public Texture2D logoScaled;

	[SerializeField]
	public List<AgentMentality> mentality = new List<AgentMentality>();

	[SerializeField]
	public List<AgentStanding> standings = new List<AgentStanding>();

	public string Name => name;

	public string Title => title;

	public string Description => description;

	public string DescriptionRich
	{
		get
		{
			string text = "";
			text += Localizer.Format("#autoLOC_264161", title);
			text += Localizer.Format("#autoLOC_264163");
			if (!string.IsNullOrEmpty(description))
			{
				text = text + "<color=#CCCCCC>  " + description + "</color>\n\n\n";
			}
			if (mentality.Count > 0)
			{
				text += Localizer.Format("#autoLOC_264172");
				int i = 0;
				for (int count = mentality.Count; i < count; i++)
				{
					AgentMentality agentMentality = mentality[i];
					if (!string.IsNullOrEmpty(agentMentality.Description))
					{
						text = text + "<b>" + agentMentality.DisplayName + "</b>\n";
						text = text + "<color=#CCCCCC>  " + agentMentality.Description + "</color>\n\n";
					}
				}
			}
			return text;
		}
	}

	public string LogoURL => logoURL;

	public Texture2D Logo => logo;

	public Texture2D LogoScaled => logoScaled;

	public List<AgentMentality> Mentality => mentality;

	public List<AgentStanding> Standings => standings;

	public Agent(string name, string title, string logoURL, string logoScaledURL)
	{
		this.name = name;
		this.title = title;
		this.logoURL = logoURL;
		if ((bool)GameDatabase.Instance)
		{
			logo = GameDatabase.Instance.GetTexture(logoURL, asNormalMap: false);
			if (logo == null)
			{
				Debug.LogError("[Agent]: Cannot find logo at url '" + logoURL + "'");
			}
			if (logoScaledURL != null)
			{
				logoScaled = GameDatabase.Instance.GetTexture(logoScaledURL, asNormalMap: false);
				if (logoScaled == null)
				{
					Debug.LogError("[Agent]: Cannot find logo scaled at url '" + logoScaledURL + "'");
				}
				else
				{
					logoScaled.filterMode = FilterMode.Point;
				}
			}
		}
		if (logoScaled == null && logo != null)
		{
			try
			{
				logoScaled = new Texture2D(logo.width, logo.height, TextureFormat.RGBA32, mipChain: false);
				logoScaled.SetPixels(logo.GetPixels());
				logoScaled.filterMode = FilterMode.Point;
				logoScaled.Apply();
				TextureScale.Bilinear(logoScaled, AgentList.Instance.LogoWidth, AgentList.Instance.LogoHeight);
			}
			catch (Exception ex)
			{
				Debug.LogError("[Agent]: Failed to scale the logo [" + logoURL + "] into a logoScaled. Exception: " + ex);
			}
		}
	}

	public bool CanProcessContract(Contract contract)
	{
		if (contract.KeywordsRequired.Count > 0 && !HasExclusiveKeywords(contract))
		{
			return false;
		}
		int num = 0;
		int count = mentality.Count;
		while (true)
		{
			if (num < count)
			{
				if (mentality[num].CanProcessContract(contract))
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool HasExclusiveKeywords(Contract contract)
	{
		if (contract.KeywordsRequired.Count == 0)
		{
			return true;
		}
		int i = 0;
		for (int count = mentality.Count; i < count; i++)
		{
			for (int j = 0; j < contract.KeywordsRequired.Count; j++)
			{
				if (mentality[i].ScoreKeyword(contract.KeywordsRequired[j]) > KeywordScore.None)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int ScoreAgentSuitability(Contract contract)
	{
		int num = 1;
		int i = 0;
		for (int count = mentality.Count; i < count; i++)
		{
			if (mentality[i].CanProcessContract(contract))
			{
				for (int j = 0; j < contract.Keywords.Count; j++)
				{
					num = (int)(num + mentality[i].ScoreKeyword(contract.Keywords[j]));
				}
			}
		}
		return num;
	}

	public bool ProcessContract(Contract contract)
	{
		if (CheatOptions.IgnoreAgencyMindsetOnContracts)
		{
			return true;
		}
		int i = 0;
		for (int count = mentality.Count; i < count; i++)
		{
			if (mentality[i].CanProcessContract(contract))
			{
				mentality[i].ProcessContract(contract);
			}
		}
		return true;
	}

	public string GetMindsetString()
	{
		string text = "";
		int i = 0;
		for (int count = mentality.Count; i < count; i++)
		{
			text += mentality[i].GetType().Name;
		}
		return text;
	}

	public static Agent LoadAgent(ConfigNode node)
	{
		string value = node.GetValue("name");
		string value2 = node.GetValue("logoURL");
		if (value != null && value2 != null)
		{
			if (AgentList.Instance.GetAgent(value) != null)
			{
				Debug.LogError("[Agent]: Cannot load, node of name '" + value + "' as there is already an agent with that name");
				return null;
			}
			string value3 = node.GetValue("title");
			if (value3 == null)
			{
				Debug.LogError("[Agent]: Is missing its 'title' value");
			}
			Agent agent = new Agent(value, value3, value2, node.GetValue("logoScaledURL"));
			if (node.HasValue("description"))
			{
				agent.description = node.GetValue("description");
			}
			AgentMentality.AddMentality(agent, "Cooperative", 0.5f);
			AgentMentality.AddMentality(agent, "Competitive", 0.5f);
			string[] values = node.GetValues("mentality");
			int i = 0;
			for (int num = values.Length; i < num; i++)
			{
				string[] array = values[i].Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
				if (array.Length >= 1 && array.Length <= 2)
				{
					string mentalityName = array[0].Trim();
					float result = 0.5f;
					if (array.Length == 2 && !float.TryParse(array[1], out result))
					{
						Debug.LogError("[Agent]: Mentality of agent '" + value + "' has a float value which could not be parsed");
					}
					else
					{
						AgentMentality.AddMentality(agent, mentalityName, result);
					}
				}
				else
				{
					Debug.LogError("[Agent]: Mentality of agent '" + value + "' should consist a mentalityName and an optional float factor");
				}
			}
			string[] values2 = node.GetValues("standing");
			int j = 0;
			for (int num2 = values2.Length; j < num2; j++)
			{
				string[] array2 = values2[j].Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);
				if (array2.Length != 2)
				{
					Debug.LogError("[Agent]: Standing of agent '" + value + "' should consist of two comma seperated values; agentName, standingFloat");
					continue;
				}
				string agentName = array2[0].Trim();
				float result2 = 0f;
				if (!float.TryParse(array2[1], out result2))
				{
					Debug.LogError("[Agent]: Standings of agent '" + value + "' has a float value which could not be parsed");
				}
				else
				{
					agent.standings.Add(new AgentStanding(agent, agentName, result2));
				}
			}
			return agent;
		}
		if (value == null)
		{
			Debug.LogError("[Agent]: Cannot load, node which does not have a valid 'name' value");
		}
		if (value2 == null)
		{
			Debug.LogError("[Agent]: Cannot load, node which does not have a valid 'logoURL' value");
		}
		return null;
	}
}
