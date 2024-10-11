using System.Runtime.CompilerServices;
using UnityEngine;

namespace Upgradeables;

public abstract class UpgradeableInterior : MonoBehaviour
{
	[SerializeField]
	protected string facilityName;

	protected PSystemSetup.SpaceCenterFacility facility;

	public int FacilityLevel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UpgradeableInterior()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnEnable()
	{
		throw null;
	}

	protected abstract void UpdateLevel(float normLvl);

	public abstract void SetLevel(int level);

	public abstract int GetLevelCount();
}
