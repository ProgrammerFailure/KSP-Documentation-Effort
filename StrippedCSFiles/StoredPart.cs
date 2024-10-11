using System;
using System.Runtime.CompilerServices;

[Serializable]
public class StoredPart
{
	public int slotIndex;

	public string partName;

	public ProtoPartSnapshot snapshot;

	public int quantity;

	public int stackCapacity;

	public string variantName;

	public bool IsEmpty
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsFull
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool CanStack
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public StoredPart(string partName, int slotIndex)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal StoredPart(ConfigNode node)
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal StoredPart Copy()
	{
		throw null;
	}
}
