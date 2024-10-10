using System.Collections.Generic;

public class FlagNameManager
{
	public enum NameType
	{
		SizeName,
		DisplayName
	}

	public Dictionary<FlagOrientation, Dictionary<int, Dictionary<NameType, List<string>>>> flagNames;

	public const int STRUCTURAL_PART = 1000;

	public const int NON_STRUCTURAL_PART = 0;

	public FlagNameManager(List<FlagMesh> flagMeshes = null)
	{
		if (flagMeshes != null)
		{
			Initialize(flagMeshes);
		}
	}

	public void Initialize(List<FlagMesh> flagMeshes)
	{
		for (int i = 0; i < flagMeshes.Count; i++)
		{
			FlagMesh flagMesh = flagMeshes[i];
			if (flagNames == null)
			{
				flagNames = new Dictionary<FlagOrientation, Dictionary<int, Dictionary<NameType, List<string>>>>();
			}
			if (!flagNames.ContainsKey(flagMesh.flagOrientation))
			{
				flagNames.Add(flagMesh.flagOrientation, new Dictionary<int, Dictionary<NameType, List<string>>>());
			}
			if (!flagNames[flagMesh.flagOrientation].ContainsKey(flagMesh.indexOffset))
			{
				flagNames[flagMesh.flagOrientation].Add(flagMesh.indexOffset, new Dictionary<NameType, List<string>>());
			}
			if (!flagNames[flagMesh.flagOrientation][flagMesh.indexOffset].ContainsKey(NameType.SizeName))
			{
				flagNames[flagMesh.flagOrientation][flagMesh.indexOffset].Add(NameType.SizeName, new List<string>());
			}
			if (!flagNames[flagMesh.flagOrientation][flagMesh.indexOffset].ContainsKey(NameType.DisplayName))
			{
				flagNames[flagMesh.flagOrientation][flagMesh.indexOffset].Add(NameType.DisplayName, new List<string>());
			}
			flagNames[flagMesh.flagOrientation][flagMesh.indexOffset][NameType.SizeName].Add(flagMesh.name);
			flagNames[flagMesh.flagOrientation][flagMesh.indexOffset][NameType.DisplayName].Add(flagMesh.displayName);
		}
	}

	public List<string> SizeNames(FlagOrientation flagOrientation, int structureType)
	{
		if (flagNames != null && flagNames.ContainsKey(flagOrientation) && flagNames[flagOrientation].ContainsKey(structureType))
		{
			return flagNames[flagOrientation][structureType][NameType.SizeName];
		}
		return new List<string>();
	}

	public bool HasSizeNames(FlagOrientation flagOrientation, int strutureType)
	{
		if (flagNames != null && flagNames.ContainsKey(flagOrientation) && flagNames[flagOrientation].ContainsKey(strutureType) && flagNames[flagOrientation][strutureType].ContainsKey(NameType.SizeName))
		{
			return flagNames[flagOrientation][strutureType][NameType.SizeName].Count > 0;
		}
		return false;
	}

	public List<string> DisplayNames(FlagOrientation flagOrientation, int structureType)
	{
		if (flagNames != null && flagNames.ContainsKey(flagOrientation) && flagNames[flagOrientation].ContainsKey(structureType))
		{
			return flagNames[flagOrientation][structureType][NameType.DisplayName];
		}
		return new List<string>();
	}
}
