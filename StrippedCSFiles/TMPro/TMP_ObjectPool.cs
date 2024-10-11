using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine.Events;

namespace TMPro;

internal class TMP_ObjectPool<T> where T : new()
{
	private readonly Stack<T> m_Stack;

	private readonly UnityAction<T> m_ActionOnGet;

	private readonly UnityAction<T> m_ActionOnRelease;

	public int countAll
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public int countActive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int countInactive
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_ObjectPool(UnityAction<T> actionOnGet, UnityAction<T> actionOnRelease)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public T Get()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Release(T element)
	{
		throw null;
	}
}
