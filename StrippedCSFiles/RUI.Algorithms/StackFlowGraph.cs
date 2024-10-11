using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RUI.Algorithms;

public class StackFlowGraph
{
	public FlowGraph<Part> delivery
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

	public FlowGraph<Part> request
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
	public StackFlowGraph(List<Part> ship)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdatePartVertex(bool request, Part part, Dictionary<Part, Vertex<Part>> lookup)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void BuildTransformGuides()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void TransformGuideOnVertex(bool request, Vertex<Part> partVertex, Dictionary<Part, Vertex<Part>> lookup)
	{
		throw null;
	}
}
