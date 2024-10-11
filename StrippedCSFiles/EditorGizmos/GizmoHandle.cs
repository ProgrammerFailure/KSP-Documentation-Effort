using System.Runtime.CompilerServices;
using UnityEngine;

namespace EditorGizmos;

public abstract class GizmoHandle : MonoBehaviour, IMouseEvents
{
	[SerializeField]
	protected Color normalColor;

	[SerializeField]
	protected Color hoverColor;

	[SerializeField]
	protected Color downColor;

	[SerializeField]
	protected Color disabledColor;

	protected bool hover;

	protected bool drag;

	[SerializeField]
	protected Renderer primaryRenderer;

	[SerializeField]
	protected Renderer highlightRenderer;

	public bool Hover
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Drag
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected GizmoHandle()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseEnter()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseDown()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseDrag()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseUp()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnMouseExit()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnMouseOut()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MonoBehaviour GetInstance()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetLock(bool lockSt)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void BaseSetup()
	{
		throw null;
	}

	protected abstract bool CanHover();

	protected abstract void On_MouseEnter();

	protected abstract void On_MouseDown();

	protected abstract void On_MouseDrag();

	protected abstract void On_MouseUp();

	protected abstract void On_MouseExit();
}
