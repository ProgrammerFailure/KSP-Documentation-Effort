using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RUI.Algorithms;

public class SCCFlowGraph
{
	public class GraphSet
	{
		public FlowGraph<HashSet<Part>> flowGraph
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

		public LinkedList<HashSet<Part>> sCC
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

		public Dictionary<Part, Vertex<HashSet<Part>>> lookup
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

		public Dictionary<Vertex<HashSet<Part>>, HashSet<Part>> supersetLookup
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
		internal GraphSet(LinkedList<HashSet<Part>> sCC)
		{
			throw null;
		}
	}

	public GraphSet requests
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

	public GraphSet delivery
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

	public StackFlowGraph stackFlowGraph
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
	public SCCFlowGraph(List<Part> ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HashSet<Part> GetAllRequests(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public HashSet<Part> GetAllDeliveries(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedList<HashSet<Part>> GetConnectedComponentsInRequests()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public LinkedList<HashSet<Part>> GetConnectedComponentsInDeliveries()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Process(GraphSet sCCFlowGraphSet, FlowGraph<Part> stackFlowGraph)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private HashSet<Part> Connect(GraphSet graph, Vertex<HashSet<Part>> vhp, HashSet<Part> hp)
	{
		throw null;
	}
}
