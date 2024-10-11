using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace KSP.UI.Screens;

public class KbApp_VesselCrew : KbAppVessel
{
	public delegate bool boolDelegate_KbApp_VesselCrew(KbApp_VesselCrew kbapp, MapObject tgt);

	public GenericCascadingList cascadingListPrefab;

	public GenericCascadingList cascadingList;

	public KbItem_kerbalInfo kerbalInfoPrefab;

	public List<KeyValuePair<AvailablePart, List<ProtoCrewMember>>> pcmByPartName;

	public static boolDelegate_KbApp_VesselCrew CallbackActivate;

	public static boolDelegate_KbApp_VesselCrew CallbackAfterActivate;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KbApp_VesselCrew()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void ActivateApp(MapObject target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int CompareKVP(KeyValuePair<AvailablePart, List<ProtoCrewMember>> a, KeyValuePair<AvailablePart, List<ProtoCrewMember>> b)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static int CompareSeatIdx(ProtoCrewMember r1, ProtoCrewMember r2)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CreateVesselCrewList(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetupKerbalItem(ref KbItem_kerbalInfo kerbalInfo, ProtoCrewMember pcm, string seatName)
	{
		throw null;
	}
}
