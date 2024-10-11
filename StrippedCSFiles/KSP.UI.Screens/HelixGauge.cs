using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[ExecuteInEditMode]
public class HelixGauge : MonoBehaviour
{
	public Image tgtImage;

	public float FillatMinValue;

	public float FillatMaxValue;

	public float MinValue;

	public float MaxValue;

	public float currentValue;

	public float currentAngle;

	public float angleAtMinValue;

	public float angleAtMaxValue;

	public bool showReadout;

	public RectTransform readoutField;

	public float readoutStandoff;

	[SerializeField]
	private float readoutStandOffPreScale;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HelixGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateGauge()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void UpdateReadoutField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected Vector3 GetReadoutWorldPos(float gaugeAngle, float spacing, Transform trf)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public float GetAngle(float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetAngle90(float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetAngle180(float v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected float GetAngle360(float v)
	{
		throw null;
	}
}
