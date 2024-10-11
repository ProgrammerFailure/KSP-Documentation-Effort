using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

[RequireComponent(typeof(RectTransform))]
public class UIRectScaler : MonoBehaviour
{
	public enum ScaleType
	{
		UIScale,
		DivUIScale,
		OverFlowProtect
	}

	private RectTransform rt;

	[SerializeField]
	protected ScaleType scaleType;

	[SerializeField]
	protected float scale;

	public bool setInAwake;

	public bool setInStart;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public UIRectScaler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[ContextMenu("Set")]
	public void Set()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Set(ScaleType scaleType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Set(float scale)
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
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetGameSettings()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetGameSettingsReciprocal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OverFlowProtector()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}
}
