using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace TMPro;

[Serializable]
public class TMP_Style
{
	[SerializeField]
	private string m_Name;

	[SerializeField]
	private int m_HashCode;

	[SerializeField]
	private string m_OpeningDefinition;

	[SerializeField]
	private string m_ClosingDefinition;

	[SerializeField]
	private int[] m_OpeningTagArray;

	[SerializeField]
	private int[] m_ClosingTagArray;

	public string name
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

	public int hashCode
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

	public string styleOpeningDefinition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string styleClosingDefinition
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int[] styleOpeningTagArray
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int[] styleClosingTagArray
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TMP_Style()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RefreshStyle()
	{
		throw null;
	}
}
