using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PartUpgradeHandler : IEnumerable
{
	public class Upgrade
	{
		public string name;

		public string partIcon;

		public string techRequired;

		public float entryCost;

		public float cost;

		public bool cumulativeCost;

		public string title;

		public string basicInfo;

		public string manufacturer;

		public string description;

		public ListDictionary<Part, PartModule> instances;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Upgrade()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void SetFromInfo(ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void SetFromUntrackedUpgrade(AvailablePart ap, ConfigNode node)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual void AddUsedBy(Part p, PartModule m)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual ListDictionary<Part, PartModule> GetUsedBy()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual bool IsUsed()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public virtual List<string[]> GetUsedByStrings()
		{
			throw null;
		}
	}

	protected Dictionary<string, Upgrade> upgrades;

	protected ListDictionary<string, Upgrade> techToUpgrades;

	protected Dictionary<Upgrade, bool> unlocks;

	protected Dictionary<Upgrade, bool> enableds;

	public static bool AllEnabled;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartUpgradeHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartUpgradeHandler()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void FillUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void LinkUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool AddUpgrade(Upgrade up)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool RemoveUpgrade(string name)
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
	public virtual bool CanHaveUpgrades()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsUnlocked(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsAvailableToUnlock(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool IsEnabled(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetUnlocked(string name, bool val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual void SetEnabled(string name, bool val)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual Upgrade GetUpgrade(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual float GetUpgradeCost(string name)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual bool UgpradesAllowed()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual List<Upgrade> GetUpgradesForTech(string tech)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public virtual IEnumerator GetEnumerator()
	{
		throw null;
	}
}
