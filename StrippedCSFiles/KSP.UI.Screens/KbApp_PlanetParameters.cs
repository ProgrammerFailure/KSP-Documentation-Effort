using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens;

public class KbApp_PlanetParameters : KbApp
{
	public delegate bool boolDelegate_KbApp_PlanetParameters(KbApp_PlanetParameters kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public CelestialBody currentBody;

	public static boolDelegate_KbApp_PlanetParameters CallbackActivate;

	public static boolDelegate_KbApp_PlanetParameters CallbackAfterActivate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_PlanetParameters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<UIListItem> CreatePhysicalCharacteristics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<UIListItem> CreateAtmosphericCharacteristics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void DisplayApp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void HideApp()
	{
		throw null;
	}
}
