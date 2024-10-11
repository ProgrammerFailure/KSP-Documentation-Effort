using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ModuleServiceModule : PartModule, IStageSeparator, IStageSeparatorChild, IScalarModule
{
	[KSPField(isPersistant = true)]
	public bool IsDeployed;

	[KSPField]
	public string ExteriorColliderName;

	[KSPField]
	public bool UseJettisonZones;

	[KSPField]
	public string JettisonZoneNames;

	[KSPField]
	public string ShellMeshName;

	[KSPField]
	public bool partDecoupled;

	[UI_Toggle(disabledText = "#autoLOC_6006087", enabledText = "#autoLOC_6006086")]
	[KSPField(isPersistant = true, guiActive = false, guiActiveEditor = true, guiName = "#autoLOC_6001455")]
	public bool showMesh;

	[UI_Toggle(disabledText = "#autoLOC_6006089", enabledText = "#autoLOC_6006088")]
	[KSPField(isPersistant = true, guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6005001")]
	public bool openCutaway;

	private Transform exteriorColliderTransform;

	private Transform shellTransform;

	private List<ModuleJettison> _jettisonModules;

	private List<Part> _jettisonParts;

	private ModuleCargoBay moduleCargoBay;

	private EventData<float, float> OnDeployStart;

	private EventData<float> OnDeployEnd;

	[KSPField]
	public string moduleID;

	public bool IsShrouded
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float GetScalar
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanMove
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float, float> OnMoving
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public EventData<float> OnStop
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public string ScalarModuleID
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleServiceModule()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnStart(StartState state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnFieldUpdated(BaseField field, object obj)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleCutaway(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateCutaway(ModuleJettison jettison, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ProcessJettisonModules(bool state, Action<ModuleJettison, bool> modAction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleShrouds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ToggleShellMesh(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateShrouds(ModuleJettison jettison, bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetupJettisonModules()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckCutaway(Vessel vFrom, Vessel vTo)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override void OnActive()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ShedExternalParts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BuildJettisonPartsList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool PointInBox(Vector3 point, BoxCollider box)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetExteriorCollider(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStaging()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStaging(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public int GetStageIndex(int fallback)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool PartDetaches(out List<Part> decoupledParts)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsEnginePlate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetScalar(float t)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIRead(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetUIWrite(bool state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool IsMoving()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public override string GetModuleDisplayName()
	{
		throw null;
	}
}
