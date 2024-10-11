using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.DebugToolbar.Screens.GamePlay;

public class KerbalScreenItem : MonoBehaviour
{
	public TextMeshProUGUI nameText;

	public TextMeshProUGUI traitText;

	public TextMeshProUGUI typeText;

	public TextMeshProUGUI levelText;

	public TextMeshProUGUI experienceText;

	public TextMeshProUGUI rosterStatusText;

	public ProtoCrewMember ProtoCrewMember
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KerbalScreenItem()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(ProtoCrewMember pcm)
	{
		throw null;
	}
}
