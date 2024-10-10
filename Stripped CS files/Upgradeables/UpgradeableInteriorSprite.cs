using UnityEngine;
using UnityEngine.UI;

namespace Upgradeables;

public class UpgradeableInteriorSprite : UpgradeableInterior
{
	[SerializeField]
	public Sprite[] levels;

	[SerializeField]
	public Image replaceTarget;

	public override void UpdateLevel(float normLvl)
	{
		FacilityLevel = Mathf.FloorToInt(normLvl * (float)levels.Length);
		FacilityLevel = Mathf.Clamp(FacilityLevel, 0, levels.Length - 1);
		SetLevel(FacilityLevel);
	}

	public override void SetLevel(int level)
	{
		replaceTarget.sprite = levels[level];
		FacilityLevel = level;
	}

	public override int GetLevelCount()
	{
		return levels.Length;
	}
}
