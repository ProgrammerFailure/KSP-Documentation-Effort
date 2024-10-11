using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace SaveUpgradePipeline;

public abstract class PartReplace : PartOffset
{
	public class attachNodeOffset
	{
		public string id;

		public Vector3 offset;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public attachNodeOffset()
		{
			throw null;
		}
	}

	protected string replacementPartName;

	protected string referenceTransformName;

	protected Vector3 childPositionOffset;

	protected Vector3 attach0Offset;

	protected List<attachNodeOffset> attachNodeOffsets;

	private char[] commaSplitArray;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartReplace()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnInit()
	{
		throw null;
	}

	protected abstract void Setup(out string pName, out string replacementPartName, out string refTransformName, out Vector3 posOffset, out Quaternion rotOffset, out Vector3 childPosOffset, out Vector3 att0Offset, out List<attachNodeOffset> attNOffsets);

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Setup(out string pName, out Vector3 posOffset, out Quaternion rotOffset)
	{
		throw null;
	}

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

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustCraftChildPartsPos(ConfigNode node, ConfigNode parent, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustSFSChildPartsPos(ConfigNode node, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offset)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustSFSRootChildPartsPos(ConfigNode node, LoadContext loadContext, ConfigNode[] partNodes, Vector3 offsetTop, Vector3 offsetBottom)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AdjustPartByOffset(ConfigNode part, LoadContext loadContext, Vector3 offset)
	{
		throw null;
	}
}
