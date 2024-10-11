using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleStatusLight : PartModule
{
	[KSPField]
	public string lightObjName;

	[KSPField]
	public string lightMeshRendererName;

	[KSPField]
	public string lightMatPropertyName;

	[KSPField]
	public string colorOn;

	[KSPField]
	public string colorOff;

	private Light lightObj;

	private Material lightMat;

	private Color cOn;

	private Color cOff;

	public bool IsOn
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
	public ModuleStatusLight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnInventoryModeDisable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetStatus(bool status)
	{
		throw null;
	}
}
