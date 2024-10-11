using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Expansions.Missions.Editor;

public class MEGUIParametersController : MonoBehaviour
{
	public GameObject defaultCompoundParameter;

	public List<MEGUIParameter> parameterPrefabs;

	private Dictionary<Type, MEGUIParameter> fieldControlTypes;

	public static MEGUIParametersController Instance
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParametersController()
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
	private void SetupParameterControls()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public MEGUIParameter GetControl(Type uiControlType)
	{
		throw null;
	}
}
