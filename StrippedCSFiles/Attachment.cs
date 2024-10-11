using System.Runtime.CompilerServices;
using UnityEngine;

internal class Attachment
{
	public bool possible;

	public bool collision;

	public AttachModes mode;

	public Part potentialParent;

	public Part caller;

	public AttachNode callerPartNode;

	public AttachNode otherPartNode;

	public Vector3 position;

	public Quaternion rotation;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Attachment()
	{
		throw null;
	}
}
