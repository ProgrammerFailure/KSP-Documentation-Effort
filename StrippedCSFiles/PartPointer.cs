using System.Runtime.CompilerServices;
using UnityEngine;

public class PartPointer : MonoBehaviour
{
	[SerializeField]
	private Part part;

	public Part PartReference
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartPointer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetPart(Part part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal Part GetPart()
	{
		throw null;
	}
}
