using System.Collections.Generic;
using UnityEngine;

namespace ns3;

public class Vertex<T>
{
	public HashSet<int> contained = new HashSet<int>();

	public int Index { get; set; }

	public int LowLink { get; set; }

	public T Value { get; set; }

	public List<Vertex<T>> Dependencies { get; set; }

	public Dictionary<Vertex<T>, KeyValuePair<Transform, Transform>> transformGuide { get; set; }

	public Vertex()
	{
		Index = -1;
		Dependencies = new List<Vertex<T>>();
	}

	public Vertex(T value)
		: this()
	{
		Value = value;
	}

	public Vertex(IEnumerable<Vertex<T>> dependencies)
	{
		Index = -1;
		Dependencies = new List<Vertex<T>>(dependencies);
	}

	public Vertex(T value, IEnumerable<Vertex<T>> dependencies)
		: this(dependencies)
	{
		Value = value;
	}
}
