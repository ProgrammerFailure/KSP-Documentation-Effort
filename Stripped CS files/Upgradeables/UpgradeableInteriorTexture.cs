using UnityEngine;
using UnityEngine.UI;

namespace Upgradeables;

public class UpgradeableInteriorTexture : UpgradeableInterior
{
	[SerializeField]
	public string[] levelImagePath;

	[SerializeField]
	public RawImage replaceTarget;

	public override void UpdateLevel(float normLvl)
	{
		FacilityLevel = Mathf.FloorToInt(normLvl * (float)levelImagePath.Length);
		FacilityLevel = Mathf.Clamp(FacilityLevel, 0, levelImagePath.Length - 1);
		SetLevel(FacilityLevel);
	}

	public override void SetLevel(int level)
	{
		Texture texture = GameDatabase.Instance.GetTexture(levelImagePath[level], asNormalMap: false);
		if (texture != null)
		{
			replaceTarget.texture = texture;
		}
		FacilityLevel = level;
	}

	public override int GetLevelCount()
	{
		return levelImagePath.Length;
	}
}
