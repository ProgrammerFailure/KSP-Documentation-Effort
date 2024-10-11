using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class GAPCelestialBodyGizmoHandle : MonoBehaviour
{
	public MeshRenderer handleRenderer;

	[SerializeField]
	protected Color normalColor;

	[SerializeField]
	protected Color hoverColor;

	[SerializeField]
	protected Color downColor;

	[SerializeField]
	protected Color disabledColor;

	public Vector3 moveDirection;

	private Color currentColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public GAPCelestialBodyGizmoHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHoverStart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnHoverEnd()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAlpha(float newAlpha)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetColor(Color newColor)
	{
		throw null;
	}
}
