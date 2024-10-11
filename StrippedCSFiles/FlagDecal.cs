using System.Runtime.CompilerServices;
using UnityEngine;

public class FlagDecal : PartModule
{
	[KSPField]
	public string textureQuadName;

	[KSPField(isPersistant = true)]
	public bool flagDisplayed;

	[KSPField(isPersistant = true)]
	public bool isMirrored;

	protected Renderer textureQuadRenderer;

	protected Texture2D flagTexture;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagDecal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001400")]
	public void ToggleFlag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnAwake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnVariantApplied(Part appliedPart, PartVariant partVariant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void updateFlag(string flagURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UpdateFlagTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[KSPEvent(guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6006062")]
	public void MirrorFlag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FlipTexture()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
