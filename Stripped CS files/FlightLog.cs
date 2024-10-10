using System;
using System.Collections.Generic;

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

		public Entry(int flight, string type, string target = null)
		{
			this.flight = flight;
			this.type = type;
			if (target != null)
			{
				this.target = target;
			}
			else
			{
				this.target = "";
			}
		}

		public Entry(int flight, EntryType type, string target = null)
		{
			this.flight = flight;
			this.type = type.ToString();
			if (target != null)
			{
				this.target = target;
			}
			else
			{
				this.target = "";
			}
		}
	}

	public List<Entry> entries = new List<Entry>();

	public int flight;

	public List<Entry> Entries => entries;

	public int Flight => flight;

	public int Count => entries.Count;

	public Entry this[int index] => entries[index];

	public void AddFlight()
	{
		flight++;
	}

	public void Load(ConfigNode node)
	{
		if (node == null)
		{
			return;
		}
		if (node.HasValue("flight"))
		{
			flight = int.Parse(node.GetValue("flight"));
			node.RemoveValue("flight");
		}
		for (int i = 0; i < node.values.Count; i++)
		{
			int result = 0;
			if (!int.TryParse(node.values[i].name, out result))
			{
				result = 0;
			}
			string[] array = node.values[i].value.Split(new char[2] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length == 1)
			{
				entries.Add(new Entry(result, array[0]));
			}
			else if (array.Length == 2)
			{
				entries.Add(new Entry(result, array[0], array[1]));
			}
		}
	}

	public void Save(ConfigNode node)
	{
		node.AddValue("flight", flight);
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			Entry entry = entries[i];
			node.AddValue(entry.flight.ToString(), entry.type + ((!string.IsNullOrEmpty(entry.target)) ? ("," + entry.target) : ""));
		}
	}

	public FlightLog CreateCopy()
	{
		FlightLog flightLog = new FlightLog();
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			flightLog.AddEntry(entries[i]);
		}
		return flightLog;
	}

	public void MergeWith(FlightLog log)
	{
		for (int i = 0; i < log.entries.Count; i++)
		{
			entries.Add(log.entries[i]);
		}
	}

	public void Clear()
	{
		entries.Clear();
	}

	public void AddEntry(Entry entry)
	{
		entries.Add(new Entry(entry.flight, entry.type, entry.target));
	}

	public void AddEntry(EntryType type, string target = null)
	{
		entries.Add(new Entry(flight, type.ToString(), target));
	}

	public void AddEntry(string type, string target = null)
	{
		entries.Add(new Entry(flight, type, target));
	}

	public void AddEntryUnique(Entry entry)
	{
		AddEntryUnique(entry.flight, entry.type.ToString(), entry.target);
	}

	public void AddEntryUnique(EntryType type, string target = null)
	{
		AddEntryUnique(flight, type.ToString(), target);
	}

	public void AddEntryUnique(int flight, string type, string target = null)
	{
		if (target == null && !HasEntry(type))
		{
			entries.Add(new Entry(flight, type, target));
		}
		else if (!HasEntry(type, target))
		{
			entries.Add(new Entry(flight, type, target));
		}
	}

	public Entry Last()
	{
		if (entries.Count == 0)
		{
			return null;
		}
		return entries[entries.Count - 1];
	}

	public Entry[] GetEntries(EntryType type, string target)
	{
		return GetEntries(type.ToString(), target);
	}

	public Entry[] GetEntries(EntryType type)
	{
		return GetEntries(type.ToString());
	}

	public Entry[] GetEntries(string type)
	{
		List<Entry> list = new List<Entry>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (list[i].type == type)
			{
				list.Add(list[i]);
			}
		}
		list.Sort((Entry a, Entry b) => a.flight.CompareTo(b.flight));
		return list.ToArray();
	}

	public Entry[] GetEntries(string type, string target)
	{
		List<Entry> list = new List<Entry>();
		int i = 0;
		for (int count = list.Count; i < count; i++)
		{
			if (list[i].type == type && list[i].target == target)
			{
				list.Add(list[i]);
			}
		}
		list.Sort((Entry a, Entry b) => a.flight.CompareTo(b.flight));
		return list.ToArray();
	}

	public bool HasEntry(EntryType type)
	{
		return HasEntry(type.ToString());
	}

	public bool HasEntry(EntryType type, string target)
	{
		return HasEntry(type.ToString(), target);
	}

	public bool HasEntry(string type)
	{
		int num = 0;
		int count = entries.Count;
		while (true)
		{
			if (num < count)
			{
				if (entries[num].type == type)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public bool HasEntry(string type, string target)
	{
		int num = 0;
		int count = entries.Count;
		while (true)
		{
			if (num < count)
			{
				if (entries[num].type == type && entries[num].target == target)
				{
					break;
				}
				num++;
				continue;
			}
			return false;
		}
		return true;
	}

	public List<string> GetDistinctTypes()
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			if (!list.Contains(entries[i].type))
			{
				list.Add(entries[i].type);
			}
		}
		return list;
	}

	public List<string> GetDistinctTypes(string target)
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			if (entries[i].target == target && !list.Contains(entries[i].type))
			{
				list.Add(entries[i].type);
			}
		}
		return list;
	}

	public List<string> GetDistinctTargets(EntryType type)
	{
		return GetDistinctTargets(type.ToString());
	}

	public List<string> GetDistinctTargets(string type)
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			if (entries[i].type == type && !list.Contains(entries[i].target))
			{
				list.Add(entries[i].target);
			}
		}
		return list;
	}

	public List<string> GetDistinctTargets()
	{
		List<string> list = new List<string>();
		int i = 0;
		for (int count = entries.Count; i < count; i++)
		{
			if (!list.Contains(entries[i].target))
			{
				list.Add(entries[i].target);
			}
		}
		return list;
	}

	public List<FlightLog> GetFlights()
	{
		List<FlightLog> list = new List<FlightLog>();
		if (entries.Count == 0)
		{
			return list;
		}
		FlightLog flightLog = new FlightLog();
		list.Add(flightLog);
		int num = 0;
		for (int i = 0; i < Count; i++)
		{
			Entry entry = this[i];
			if (entry.flight != num)
			{
				num = entry.flight;
				flightLog = new FlightLog();
				list.Add(flightLog);
			}
			flightLog.AddEntry(entry);
		}
		return list;
	}
}
