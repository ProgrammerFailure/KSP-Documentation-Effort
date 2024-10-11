using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace Upgradeables;

public class UpgradeableInteriorTexture : UpgradeableInterior
{
	[SerializeField]
	private string[] levelImagePath;

	[SerializeField]
	private RawImage replaceTarget;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UpgradeableInteriorTexture()
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
