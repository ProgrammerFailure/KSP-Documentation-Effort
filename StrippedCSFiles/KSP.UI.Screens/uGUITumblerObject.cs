using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class uGUITumblerObject : MonoBehaviour
{
	[SerializeField]
	private RectTransform[] Labels;

	[SerializeField]
	private string[] Values;

	public double tgtRot;

	public double currRot;

	public double N;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public uGUITumblerObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void TumbleTo(double n, int tumble)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateDelta(float deltaTime, float sharpness)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected string GetValue(int index)
	{
		throw null;
	}
}
