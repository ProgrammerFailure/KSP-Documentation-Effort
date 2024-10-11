using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;

namespace KSP.UI.Screens;

[ExecuteInEditMode]
public class RotationalGaugeOffsetMarker : RotationalGauge
{
	[SerializeField]
	private RectTransform offsetPointer;

	[SerializeField]
	private float nextToCurrentAngle;

	[SerializeField]
	private float offsetAngle;

	[SerializeField]
	private TextMeshProUGUI offsetTextField;

	[SerializeField]
	private GameObject stageMarker;

	[SerializeField]
	internal GameObject offsetObject;

	[SerializeField]
	private GameObject offsetGameObjectPrefab;

	[SerializeField]
	private double stageDV;

	public double StageDV
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RotationalGaugeOffsetMarker()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void LateUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetNextToValue(float nextAngle)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTextField(string text, int opacity)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleOffsetMarker(bool active)
	{
		throw null;
	}
}
