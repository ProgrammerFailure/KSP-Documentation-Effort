using UnityEngine;

namespace Upgradeables;

public class UpgradeableInteriorMaterial : UpgradeableInterior
{
	[SerializeField]
	public string[] levelImagePath;

	[SerializeField]
	public Material materialTarget;

	public override void UpdateLevel(float normLvl)
	{
		FacilityLevel = Mathf.FloorToInt(normLvl * (float)levelImagePath.Length);
		FacilityLevel = Mathf.Clamp(FacilityLevel, 0, levelImagePath.Length - 1);
		SetLevel(FacilityLevel);
	}

	public override void SetLevel(int level)
	{
		Texture2D texture = GameDatabase.Instance.GetTexture(levelImagePath[level], asNormalMap: false);
		if (texture != null)
		{
			materialTarget.SetTexture("_MainTex", texture);
		}
		FacilityLevel = level;
	}

	public override int GetLevelCount()
	{
		return levelImagePath.Length;
	}
}
