using System.Runtime.CompilerServices;

public class ProtoPartResourceSnapshot
{
	protected ConfigNode resourceValues;

	public string resourceName;

	public PartResource resourceRef;

	public double amount;

	public double maxAmount;

	public bool flowState;

	public PartResourceDefinition definition
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
	public ProtoPartResourceSnapshot(PartResource resource)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ProtoPartResourceSnapshot(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Save(ConfigNode node)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateConfigNodeAmounts()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Load(Part hostPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AvailablePart.ResourceInfo GetCurrentResourceInfo()
	{
		throw null;
	}
}
