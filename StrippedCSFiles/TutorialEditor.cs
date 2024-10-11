using System.Runtime.CompilerServices;
using UnityEngine;

public class TutorialEditor : TutorialScenario
{
	public string stateName;

	public bool complete;

	public string vesselName;

	private Texture2D stagingStackIcon;

	private EditorPartListFilter<AvailablePart> tutorialFilter_none;

	private EditorPartListFilter<AvailablePart> tutorialFilter_pod;

	private EditorPartListFilter<AvailablePart> tutorialFilter_chute;

	private EditorPartListFilter<AvailablePart> tutorialFilter_bacc;

	private EditorPartListFilter<AvailablePart> tutorialFilter_flea;

	private EditorPartListFilter<AvailablePart> tutorialFilter_final;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TutorialEditor()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnAssetSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnTutorialSetup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void OnOnDestroy()
	{
		throw null;
	}
}
