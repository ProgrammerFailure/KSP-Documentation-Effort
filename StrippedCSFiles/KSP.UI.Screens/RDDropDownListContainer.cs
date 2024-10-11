using System;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI.Screens;

public class RDDropDownListContainer : MonoBehaviour
{
	public UIList scrollList;

	public RDDropDownList dropDownListPrefab;

	[NonSerialized]
	public RDDropDownList[] lists;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RDDropDownListContainer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(params RDDropDownList[] lists)
	{
		throw null;
	}
}
