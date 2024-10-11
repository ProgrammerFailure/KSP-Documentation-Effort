using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class SpriteTumblerObject : MonoBehaviour
{
	public delegate void afterAltModeTumbleTo(double n, int tumble);

	public delegate void afterAltModeUpdateDelta(float deltaTime, float sharpness);

	public RectTransform[] images;

	public Sprite[] sprites;

	public RectTransform colorBar;

	public double tgtRot;

	public double currRot;

	public double N;

	public bool tumbling;

	public static afterAltModeTumbleTo AfterAltTumbleTo;

	public static afterAltModeUpdateDelta AfterAltModeUpdateDelta;

	[SerializeField]
	private double delta;

	[SerializeField]
	private int index;

	[SerializeField]
	private int spriteIndexOffset;

	[SerializeField]
	private float colorBarMinY;

	[SerializeField]
	private float colorBarMaxY;

	public float colorBarTarget;

	public float colorBarCurrent;

	public float colorBarSection;

	public float IconSection;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SpriteTumblerObject()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
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
}
