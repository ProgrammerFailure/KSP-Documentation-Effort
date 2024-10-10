using UnityEngine;

namespace Upgradeables;

public abstract class UpgradeableInterior : MonoBehaviour
{
	[SerializeField]
	public string facilityName;

	public PSystemSetup.SpaceCenterFacility facility;

	public int FacilityLevel;

	public UpgradeableInterior()
	{
	}

	public void OnEnable()
	{
		facility = PSystemSetup.Instance.GetSpaceCenterFacility(facilityName);
		UpdateLevel(facility.GetFacilityLevel());
	}

	public abstract void UpdateLevel(float normLvl);

	public abstract void SetLevel(int level);

	public abstract int GetLevelCount();
}
