using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;

namespace KSP.UI;

[Serializable]
public abstract class AppUI_Data : IConfigNode
{
	protected Callback onDataChanged;

	internal AppUIInputPanel uiPanel;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected AppUI_Data()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void SetupDataChangeCallback(Callback onDataChanged)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void CallOnDataChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UIInputPanelDataChanged()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void UIInputPanelUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnLoad(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void OnSave(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static List<T> CreateAppUIDataList<T>(ConfigNode[] nodes) where T : AppUI_Data
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T CreateInstanceOfAppUIData<T>(ConfigNode node) where T : AppUI_Data
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static T CreateInstanceOfAppUIData<T>(string className) where T : AppUI_Data
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}
}
