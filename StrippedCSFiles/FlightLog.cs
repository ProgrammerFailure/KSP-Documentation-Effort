using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class FlightLog
{
	public enum EntryType
	{
		Land,
		Flight,
		Flyby,
		Orbit,
		Suborbit,
		Escape,
		Launch,
		ExitVessel,
		BoardVessel,
		PlantFlag,
		Recover,
		Die,
		Spawn,
		Training1,
		Training2,
		Training3,
		Training4,
		Training5
	}

	public class Entry
	{
		public int flight;

		public string type;

		public string target;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Entry(int flight, string type, string target = null)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Entry(int flight, EntryType type, string target = null)
		{
			throw null;
		}
	}

	private List<Entry> entries;

	private int flight;

	public List<Entry> Entries
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Flight
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int Count
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Entry this[int index]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlightLog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddFlight()
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
	public FlightLog CreateCopy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MergeWith(FlightLog log)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Clear()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEntry(Entry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEntry(EntryType type, string target = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEntry(string type, string target = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEntryUnique(Entry entry)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddEntryUnique(EntryType type, string target = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddEntryUnique(int flight, string type, string target = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Entry Last()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Entry[] GetEntries(EntryType type, string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Entry[] GetEntries(EntryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Entry[] GetEntries(string type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Entry[] GetEntries(string type, string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEntry(EntryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEntry(EntryType type, string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEntry(string type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasEntry(string type, string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDistinctTypes()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDistinctTypes(string target)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDistinctTargets(EntryType type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDistinctTargets(string type)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetDistinctTargets()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<FlightLog> GetFlights()
	{
		throw null;
	}
}
