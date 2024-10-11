using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaveUpgradePipeline;

public abstract class PartOffset : UpgradeScript
{
	protected string partName;

	protected Vector3 positionOffset;

	protected Quaternion rotationOffset;

	protected Quaternion rotation0;

	protected Quaternion rotation;

	protected Vector3 position0;

	protected Vector3 position;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartOffset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInit()
	{
		throw null;
	}

	protected abstract void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset);

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override TestResult OnTest(ConfigNode node, LoadContext loadContext, ref string nodeName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnUpgrade(ConfigNode node, LoadContext loadContext, ConfigNode parentNode)
	{
		throw null;
	}
}
