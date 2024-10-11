using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Upgradeables;

public class UpgradeableInteriorSprite : UpgradeableInterior
{
	[SerializeField]
	private Sprite[] levels;

	[SerializeField]
	private Image replaceTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UpgradeableInteriorSprite()
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
