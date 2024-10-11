using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Field)]
public abstract class UI_Control : Attribute
{
	public UI_Scene scene;

	public bool controlEnabled;

	public bool requireFullControl;

	public UI_Scene affectSymCounterparts;

	protected BaseField field;

	public UIPartActionItem partActionItem;

	public Callback<BaseField, object> onFieldChanged;

	public Callback<BaseField, object> onSymmetryFieldChanged;

	public bool suppressEditorShipModified;

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected UI_Control()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Setup(BaseField field)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSceneVisibility(UI_Scene scene, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Load(ConfigNode node, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void Save(ConfigNode node, object host)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool ParseEnabled(out bool value, ConfigNode node, string valueName, string FieldUIControlName)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool ParseFloat(out float value, ConfigNode node, string valueName, string FieldUIControlName, string errorNoValue)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static bool ParseString(out string value, ConfigNode node, string valueName, string FieldUIControlName, string errorNoValue)
	{
		throw null;
	}
}
