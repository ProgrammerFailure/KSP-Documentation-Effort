using System.Runtime.CompilerServices;
using KSP.UI.Util;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.SpaceCenter.MissionSummaryDialog;

public class CrewWidget : MissionSummaryWidget
{
	public ProtoCrewMember crew;

	public bool isTourist;

	public bool isOrangeCrew;

	public bool isFemale;

	public float xpGained;

	public int levelsGained;

	public int newLevel;

	public Image crewIcon;

	public TextMeshProUGUI statusField;

	public TextMeshProUGUI roleField;

	public ImgText xpEarnedField;

	public ImgText lvlProgressField;

	public Sprite[] lvlImages;

	public Sprite crewIconMaleStandard;

	public Sprite crewIconMaleOrange;

	public Sprite crewIconMaleTourist;

	public Sprite crewIconFemaleStandard;

	public Sprite crewIconFemaleOrange;

	public Sprite crewIconFemaleTourist;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public CrewWidget()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static CrewWidget Create(ProtoCrewMember crew, MissionRecoveryDialog mrd)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateFields()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Sprite selectSprite()
	{
		throw null;
	}
}
