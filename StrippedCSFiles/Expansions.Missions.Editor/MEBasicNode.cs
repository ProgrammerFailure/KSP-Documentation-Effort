using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

[Serializable]
public class MEBasicNode : IConfigNode
{
	public class ExtendedInfo
	{
		public string name;

		public string information;

		public List<ExtendedInfo> scoringInformation;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ExtendedInfo()
		{
			throw null;
		}
	}

	public string name;

	public string displayName;

	public string description;

	public string tooltipDescription;

	public string categoryShortDescription;

	public bool isObjective;

	public bool isLogicNode;

	public Color headerColor;

	public string category;

	public string categoryDisplayName;

	public string iconURL;

	public List<string> tests;

	public List<string> actions;

	public List<DisplayParameter> defaultSAPParameters;

	public List<DisplayParameter> defaultNodeBodyParameters;

	public List<ExtendedInfo> extInfoActionModules;

	public List<ExtendedInfo> extInfoTestModules;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEBasicNode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ExtendedInfo BuildExtendedInfoListFromObject(IMENodeDisplay module, bool supportsGlobalScoringModules)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}
}
