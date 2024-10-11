using System;
using System.Runtime.CompilerServices;
using RUI.Icons.Selectable;
using UnityEngine;

namespace KSP.UI.Screens.PartListCategories;

[Serializable]
public abstract class PartCategory
{
	public string name;

	public string displayName;

	public Icon icon;

	public string iconUrl;

	public Color color;

	public Color iconColor;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected PartCategory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual PartCategorizer.Category GetCategory()
	{
		throw null;
	}

	public abstract bool ExclusionCriteria(AvailablePart aP);
}
