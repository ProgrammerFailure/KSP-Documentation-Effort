using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens.Flight;

public class METDisplay : MonoBehaviour
{
	public TextMeshProUGUI text;

	public TextMeshProUGUI buttonText;

	public bool displayUT;

	private bool gamePaused;

	private static string cacheMETDatePortion;

	private string currentMETDatePortion;

	private static string cacheAutoLOC_7003242;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public METDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static METDisplay()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Reset()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleTimeMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTimeMode(bool displayUT)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGamePause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void onGameUnPause()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
