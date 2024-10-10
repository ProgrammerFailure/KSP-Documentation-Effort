using UnityEngine;

public static class MaterialExtension
{
	public static void CopyKeywordsFrom(this Material m, Material copyFrom)
	{
		if (!(copyFrom == null))
		{
			string[] shaderKeywords = copyFrom.shaderKeywords;
			string[] array = new string[shaderKeywords.Length];
			shaderKeywords.CopyTo(array, 0);
			m.shaderKeywords = array;
		}
	}
}
