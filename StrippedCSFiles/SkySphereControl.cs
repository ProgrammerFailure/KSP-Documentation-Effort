using System.Runtime.CompilerServices;
using UnityEngine;

public class SkySphereControl : MonoBehaviour
{
	private static int shaderPropertyColor2;

	private static int shaderPropertyDayNightBlend;

	private static int shaderPropertySpaceBlend;

	public float skyFadeStart;

	public float atmosphereLimit;

	public Color dayTimeSpaceColorShift;

	public Sun sunRef;

	public Camera tgt;

	public double sunSrfAngle;

	public double sunCamAngle;

	private QuaternionD initRot;

	private Renderer _renderer;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SkySphereControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FixedUpdate()
	{
		throw null;
	}
}
