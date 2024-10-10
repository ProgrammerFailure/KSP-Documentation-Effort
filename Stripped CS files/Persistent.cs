using System;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public class Persistent : Attribute
{
	public string name;

	public bool isPersistant;

	public int pass;

	public string collectionIndex;

	public PersistentRelation relationship;

	public bool link;

	public Persistent()
	{
		name = "";
		isPersistant = true;
		pass = 0;
		collectionIndex = "Item";
		relationship = PersistentRelation.SameObject;
		link = false;
	}
}
