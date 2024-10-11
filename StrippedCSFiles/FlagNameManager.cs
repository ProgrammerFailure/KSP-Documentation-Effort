using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class FlagNameManager
{
	public enum NameType
	{
		SizeName,
		DisplayName
	}

	private Dictionary<FlagOrientation, Dictionary<int, Dictionary<NameType, List<string>>>> flagNames;

	public const int STRUCTURAL_PART = 1000;

	public const int NON_STRUCTURAL_PART = 0;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public FlagNameManager(List<FlagMesh> flagMeshes = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Initialize(List<FlagMesh> flagMeshes)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> SizeNames(FlagOrientation flagOrientation, int structureType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool HasSizeNames(FlagOrientation flagOrientation, int strutureType)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> DisplayNames(FlagOrientation flagOrientation, int structureType)
	{
		throw null;
	}
}
