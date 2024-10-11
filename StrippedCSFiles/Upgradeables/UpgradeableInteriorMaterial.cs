using System.Runtime.CompilerServices;
using UnityEngine;

namespace Upgradeables;

public class UpgradeableInteriorMaterial : UpgradeableInterior
{
	[SerializeField]
	private string[] levelImagePath;

	[SerializeField]
	private Material materialTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UpgradeableInteriorMaterial()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void UpdateLevel(float normLvl)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void SetLevel(int level)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override int GetLevelCount()
	{
		throw null;
	}
}
