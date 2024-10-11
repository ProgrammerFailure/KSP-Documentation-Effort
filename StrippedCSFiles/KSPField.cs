using System.Runtime.CompilerServices;
using UnityEngine;

public class KSPField : FieldAttribute
{
	public bool isPersistant;

	public bool guiActive;

	public bool guiActiveEditor;

	public bool guiActiveUnfocused;

	[SerializeField]
	private string _guiUnits;

	public string guiFormat;

	public string category;

	public bool advancedTweakable;

	public float unfocusedRange;

	public string groupName;

	public string groupDisplayName;

	public bool groupStartCollapsed;

	public string guiUnits
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public KSPField()
	{
		throw null;
	}
}
