using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class LinearControlGauges : MonoBehaviour
{
	public List<Image> inputGaugeImages;

	public LinearGauge pitch;

	public LinearGauge yaw;

	public LinearGauge roll;

	public LinearGauge linX;

	public LinearGauge linY;

	public LinearGauge linZ;

	public bool linXInverted;

	public bool linYInverted;

	public bool linZInverted;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinearControlGauges()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void onPrecisionModeToggle(bool precisionMode)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}
}
