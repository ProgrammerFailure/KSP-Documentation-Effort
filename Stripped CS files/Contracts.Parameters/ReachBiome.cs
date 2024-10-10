using System;

namespace Contracts.Parameters;

[Serializable]
public class ReachBiome : ContractParameter
{
	public string BiomeName;

	public string title = "";

	public bool trackerActive;

	public bool IsWithinBiome;

	public ReachBiome()
	{
	}

	public ReachBiome(string biomeName, string title)
	{
		BiomeName = biomeName ?? "";
		this.title = title;
	}

	public override string GetTitle()
	{
		return title + BiomeName;
	}

	public static string GetTitleStringShort(string biomeName)
	{
		return biomeName;
	}

	public override void OnReset()
	{
		SetIncomplete();
	}

	public override void OnLoad(ConfigNode node)
	{
		if (node.HasValue("biome"))
		{
			BiomeName = node.GetValue("biome");
		}
		if (node.HasValue("title"))
		{
			title = node.GetValue("title");
		}
	}

	public override void OnSave(ConfigNode node)
	{
		node.AddValue("biome", BiomeName);
		node.AddValue("title", title);
	}

	public override void OnUpdate()
	{
		if (base.Root.ContractState == Contract.State.Active && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready)
		{
			IsWithinBiome = false;
			if (bodyHasBiome(FlightGlobals.ActiveVessel.mainBody))
			{
				IsWithinBiome = checkVesselWithinBiome(FlightGlobals.ActiveVessel);
			}
			if (IsWithinBiome && base.State == ParameterState.Incomplete)
			{
				SetComplete();
			}
			if (!IsWithinBiome && base.State == ParameterState.Complete)
			{
				SetIncomplete();
			}
		}
	}

	public bool checkVesselWithinBiome(Vessel v)
	{
		if (!string.IsNullOrEmpty(BiomeName))
		{
			if (v.mainBody.BiomeMap != null)
			{
				return v.mainBody.BiomeMap.GetAtt(v.latitude, v.longitude).name == BiomeName;
			}
			return false;
		}
		return true;
	}

	public bool bodyHasBiome(CelestialBody body)
	{
		if (!string.IsNullOrEmpty(BiomeName) && !(body.BiomeMap == null))
		{
			int num = body.BiomeMap.Attributes.Length;
			do
			{
				if (num-- <= 0)
				{
					return false;
				}
			}
			while (!(body.BiomeMap.Attributes[num].name == BiomeName));
			return true;
		}
		return false;
	}
}
