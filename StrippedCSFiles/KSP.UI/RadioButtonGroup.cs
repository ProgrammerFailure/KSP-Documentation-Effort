using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace KSP.UI;

public class RadioButtonGroup
{
	private static List<RadioButtonGroup> groups;

	public int groupID;

	public List<IRadioButton> buttons;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public RadioButtonGroup(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static RadioButtonGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	~RadioButtonGroup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IRadioButton GetSelected(GameObject go)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static IRadioButton GetSelected(int id)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static RadioButtonGroup GetGroup(int id)
	{
		throw null;
	}
}
