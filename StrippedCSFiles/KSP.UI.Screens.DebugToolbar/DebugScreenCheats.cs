using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.DebugToolbar;

public class DebugScreenCheats : MonoBehaviour
{
	public Toggle hackGravity;

	public Slider hackGravityFactor;

	public TextMeshProUGUI hackGravityText;

	public Toggle pauseOnVesselUnpack;

	public Toggle unbreakableJoints;

	public Toggle noCrashDamage;

	public Toggle ignoreMaxTemperature;

	public Toggle infinitePropellant;

	public Toggle infiniteElectricity;

	public Toggle biomesVisibleInMap;

	public Toggle allowPartClippingInEditors;

	public Toggle nonStrictPartAttachmentOrientationChecks;

	public static DebugScreenCheats Instance
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
	public DebugScreenCheats()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}
}
