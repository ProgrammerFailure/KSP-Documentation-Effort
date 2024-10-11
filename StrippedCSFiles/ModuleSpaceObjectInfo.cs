using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class ModuleSpaceObjectInfo : PartModule
{
	protected class ResourceData
	{
		public string Name;

		public float Mass;

		public float Purity;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ResourceData()
		{
			throw null;
		}
	}

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_462423")]
	public string displayMass;

	[KSPField(isPersistant = true)]
	public string massThreshold;

	[KSPField(isPersistant = true)]
	public string currentMass;

	[KSPField(guiActive = true, guiActiveEditor = false, guiName = "#autoLOC_6001746")]
	public string resources;

	public float totalResourceMass;

	public double totalResourcePercent;

	public virtual double currentMassVal
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

	public virtual double massThresholdVal
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModuleSpaceObjectInfo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void SetupSpaceObjectResources(List<ModuleSpaceObjectResource> resInfoList)
	{
		throw null;
	}
}
