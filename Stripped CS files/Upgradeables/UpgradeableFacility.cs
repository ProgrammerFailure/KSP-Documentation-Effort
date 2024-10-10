using System.Collections;
using UnityEngine;

namespace Upgradeables;

[SelectionBase]
public class UpgradeableFacility : UpgradeableObject
{
	public override void OnAwake()
	{
		setLevel(base.MaxLevel);
	}

	public override void OnStart()
	{
		GameEvents.onLevelWasLoaded.Add(OnLevelLoaded);
		GameEvents.Mission.onMissionsBuilderPQSLoaded.Add(OnMissionsBuilderPQSLoaded);
	}

	public override void OnOnDestroy()
	{
		GameEvents.onLevelWasLoaded.Remove(OnLevelLoaded);
		GameEvents.Mission.onMissionsBuilderPQSLoaded.Remove(OnMissionsBuilderPQSLoaded);
	}

	public bool RegisterInstance()
	{
		if (!preCompiledId)
		{
			id = HierarchyUtil.CompileID(base.transform, "SpaceCenter");
		}
		bool result = false;
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogError("[UpgradeableFacility]: ID for this facility is not defined correctly. Make sure all IDs are compiled and serialized before running the game.", base.gameObject);
		}
		else
		{
			result = ScenarioUpgradeableFacilities.RegisterUpgradeable(this, id);
		}
		return result;
	}

	public void UnregisterInstance()
	{
		if (string.IsNullOrEmpty(id))
		{
			Debug.LogError("[UpgradeableFacility]: ID for this facility is not defined correctly. Cannot unregister.", base.gameObject);
			return;
		}
		ScenarioUpgradeableFacilities.UnregisterUpgradeable(this, id);
		Debug.Log("[" + id + "]: Instance unregistered.");
	}

	public void OnMissionsBuilderPQSLoaded()
	{
		if (HighLogic.LoadedScene == GameScenes.MISSIONBUILDER && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(LoadOnSceneStart());
		}
	}

	public void OnLevelLoaded(GameScenes scn)
	{
		if (((uint)(scn - 5) <= 2u || scn == GameScenes.MISSIONBUILDER) && base.gameObject.activeInHierarchy)
		{
			StartCoroutine(LoadOnSceneStart());
		}
	}

	public IEnumerator LoadOnSceneStart()
	{
		yield return new WaitForEndOfFrame();
		if (!RegisterInstance())
		{
			Reset();
		}
	}

	public void Reset()
	{
		setLevel(base.MaxLevel);
	}

	public override void SetLevel(int lvl)
	{
		GameEvents.OnKSCFacilityUpgrading.Fire(this, lvl);
		setLevel(lvl);
		if (base.gameObject.activeInHierarchy)
		{
			StartCoroutine(waitForLevelSpawn(lvl));
		}
	}

	public IEnumerator waitForLevelSpawn(int lvl)
	{
		while (base.CurrentLevel == null || !base.CurrentLevel.Spawned)
		{
			yield return null;
		}
		yield return null;
		GameEvents.OnKSCFacilityUpgraded.Fire(this, lvl);
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("lvl", GetNormLevel());
	}

	public void Load(ConfigNode node)
	{
		if (node == null)
		{
			Debug.Log("[" + id + "]: Loading... Node is null", base.gameObject);
			setLevel(0);
			return;
		}
		if (node.HasValue("lvl"))
		{
			float normLevel = float.Parse(node.GetValue("lvl"));
			setNormLevel(normLevel);
		}
		if (node.HasValue("level"))
		{
			facilityLevel = int.Parse(node.GetValue("level"));
			setLevel(facilityLevel);
		}
	}

	public void setNormLevel(float lvl)
	{
		lvl = Mathf.Clamp01(lvl);
		facilityLevel = Mathf.FloorToInt(lvl * (float)(base.MaxLevel + 1));
		facilityLevel = Mathf.Clamp(facilityLevel, 0, base.MaxLevel);
		setLevel(facilityLevel);
	}

	public float GetNormLevel()
	{
		return (float)facilityLevel / (float)base.MaxLevel;
	}

	public float GetUpgradeCost()
	{
		if (facilityLevel != base.MaxLevel)
		{
			if (HighLogic.LoadedSceneIsGame)
			{
				return upgradeLevels[facilityLevel + 1].levelCost * HighLogic.CurrentGame.Parameters.Career.FundsLossMultiplier;
			}
			return upgradeLevels[facilityLevel + 1].levelCost;
		}
		return 0f;
	}

	public float GetDowngradeCost()
	{
		if (facilityLevel != 0)
		{
			if (HighLogic.LoadedSceneIsGame)
			{
				return upgradeLevels[facilityLevel - 1].levelCost * 0.667f * HighLogic.CurrentGame.Parameters.Career.FundsLossMultiplier;
			}
			return upgradeLevels[facilityLevel - 1].levelCost * 0.667f;
		}
		return 0f;
	}

	public string GetLevelText()
	{
		return GetLevelText(base.FacilityLevel);
	}

	public string GetNextLevelText()
	{
		return GetLevelText(base.FacilityLevel + 1);
	}

	public string GetLevelText(int lvl)
	{
		if (lvl <= base.MaxLevel && lvl >= 0)
		{
			if (upgradeLevels[lvl].levelStats != null)
			{
				return upgradeLevels[lvl].levelStats.GetText((float)lvl / (float)base.MaxLevel);
			}
			if (upgradeLevels[lvl].levelCost != 0f)
			{
				Debug.LogError("[" + base.name + "]: No Level Text for level " + lvl + "!");
			}
			return "";
		}
		return "";
	}
}
