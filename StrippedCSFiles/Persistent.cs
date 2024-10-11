using System;
using System.Runtime.CompilerServices;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class Persistent : Attribute
{
	public string name;

	public bool isPersistant;

	public int pass;

	public string collectionIndex;

	public PersistentRelation relationship;

	public bool link;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Persistent()
	{
		throw null;
	}
}
