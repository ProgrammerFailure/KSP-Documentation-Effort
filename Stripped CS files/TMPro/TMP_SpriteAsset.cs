using System.Collections.Generic;
using UnityEngine;

namespace TMPro;

public class TMP_SpriteAsset : TMP_Asset
{
	public Dictionary<int, int> m_UnicodeLookup;

	public Dictionary<int, int> m_NameLookup;

	public static TMP_SpriteAsset m_defaultSpriteAsset;

	public Texture spriteSheet;

	public List<TMP_Sprite> spriteInfoList;

	[SerializeField]
	public List<TMP_SpriteAsset> fallbackSpriteAssets;

	public static List<int> k_searchedSpriteAssets;

	public static TMP_SpriteAsset defaultSpriteAsset
	{
		get
		{
			if (m_defaultSpriteAsset == null)
			{
				m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
			}
			return m_defaultSpriteAsset;
		}
	}

	public void OnEnable()
	{
	}

	public Material GetDefaultSpriteMaterial()
	{
		ShaderUtilities.GetShaderPropertyIDs();
		Material obj = new Material(Shader.Find("TextMeshPro/Sprite"));
		obj.SetTexture(ShaderUtilities.ID_MainTex, spriteSheet);
		obj.hideFlags = HideFlags.HideInHierarchy;
		return obj;
	}

	public void UpdateLookupTables()
	{
		if (m_NameLookup == null)
		{
			m_NameLookup = new Dictionary<int, int>();
		}
		m_NameLookup.Clear();
		if (m_UnicodeLookup == null)
		{
			m_UnicodeLookup = new Dictionary<int, int>();
		}
		m_UnicodeLookup.Clear();
		for (int i = 0; i < spriteInfoList.Count; i++)
		{
			int key = spriteInfoList[i].hashCode;
			if (!m_NameLookup.ContainsKey(key))
			{
				m_NameLookup.Add(key, i);
			}
			int unicode = spriteInfoList[i].unicode;
			if (!m_UnicodeLookup.ContainsKey(unicode))
			{
				m_UnicodeLookup.Add(unicode, i);
			}
		}
	}

	public int GetSpriteIndexFromHashcode(int hashCode)
	{
		if (m_NameLookup == null)
		{
			UpdateLookupTables();
		}
		int value = 0;
		if (m_NameLookup.TryGetValue(hashCode, out value))
		{
			return value;
		}
		return -1;
	}

	public int GetSpriteIndexFromUnicode(int unicode)
	{
		if (m_UnicodeLookup == null)
		{
			UpdateLookupTables();
		}
		int value = 0;
		if (m_UnicodeLookup.TryGetValue(unicode, out value))
		{
			return value;
		}
		return -1;
	}

	public int GetSpriteIndexFromName(string name)
	{
		if (m_NameLookup == null)
		{
			UpdateLookupTables();
		}
		int simpleHashCode = TMP_TextUtilities.GetSimpleHashCode(name);
		return GetSpriteIndexFromHashcode(simpleHashCode);
	}

	public static TMP_SpriteAsset SearchForSpriteByUnicode(TMP_SpriteAsset spriteAsset, int unicode, bool includeFallbacks, out int spriteIndex)
	{
		if (spriteAsset == null)
		{
			spriteIndex = -1;
			return null;
		}
		spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (k_searchedSpriteAssets == null)
		{
			k_searchedSpriteAssets = new List<int>();
		}
		k_searchedSpriteAssets.Clear();
		int instanceID = spriteAsset.GetInstanceID();
		k_searchedSpriteAssets.Add(instanceID);
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, includeFallbacks, out spriteIndex);
		}
		if (includeFallbacks && TMP_Settings.defaultSpriteAsset != null)
		{
			return SearchForSpriteByUnicodeInternal(TMP_Settings.defaultSpriteAsset, unicode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	public static TMP_SpriteAsset SearchForSpriteByUnicodeInternal(List<TMP_SpriteAsset> spriteAssets, int unicode, bool includeFallbacks, out int spriteIndex)
	{
		int num = 0;
		TMP_SpriteAsset tMP_SpriteAsset;
		while (true)
		{
			if (num < spriteAssets.Count)
			{
				tMP_SpriteAsset = spriteAssets[num];
				if (!(tMP_SpriteAsset == null))
				{
					int instanceID = tMP_SpriteAsset.GetInstanceID();
					if (!k_searchedSpriteAssets.Contains(instanceID))
					{
						k_searchedSpriteAssets.Add(instanceID);
						tMP_SpriteAsset = SearchForSpriteByUnicodeInternal(tMP_SpriteAsset, unicode, includeFallbacks, out spriteIndex);
						if (tMP_SpriteAsset != null)
						{
							break;
						}
					}
				}
				num++;
				continue;
			}
			spriteIndex = -1;
			return null;
		}
		return tMP_SpriteAsset;
	}

	public static TMP_SpriteAsset SearchForSpriteByUnicodeInternal(TMP_SpriteAsset spriteAsset, int unicode, bool includeFallbacks, out int spriteIndex)
	{
		spriteIndex = spriteAsset.GetSpriteIndexFromUnicode(unicode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByUnicodeInternal(spriteAsset.fallbackSpriteAssets, unicode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	public static TMP_SpriteAsset SearchForSpriteByHashCode(TMP_SpriteAsset spriteAsset, int hashCode, bool includeFallbacks, out int spriteIndex)
	{
		if (spriteAsset == null)
		{
			spriteIndex = -1;
			return null;
		}
		spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (k_searchedSpriteAssets == null)
		{
			k_searchedSpriteAssets = new List<int>();
		}
		k_searchedSpriteAssets.Clear();
		int instanceID = spriteAsset.GetInstanceID();
		k_searchedSpriteAssets.Add(instanceID);
		if (includeFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, includeFallbacks, out spriteIndex);
		}
		if (includeFallbacks && TMP_Settings.defaultSpriteAsset != null)
		{
			return SearchForSpriteByHashCodeInternal(TMP_Settings.defaultSpriteAsset, hashCode, includeFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}

	public static TMP_SpriteAsset SearchForSpriteByHashCodeInternal(List<TMP_SpriteAsset> spriteAssets, int hashCode, bool searchFallbacks, out int spriteIndex)
	{
		int num = 0;
		TMP_SpriteAsset tMP_SpriteAsset;
		while (true)
		{
			if (num < spriteAssets.Count)
			{
				tMP_SpriteAsset = spriteAssets[num];
				if (!(tMP_SpriteAsset == null))
				{
					int instanceID = tMP_SpriteAsset.GetInstanceID();
					if (!k_searchedSpriteAssets.Contains(instanceID))
					{
						k_searchedSpriteAssets.Add(instanceID);
						tMP_SpriteAsset = SearchForSpriteByHashCodeInternal(tMP_SpriteAsset, hashCode, searchFallbacks, out spriteIndex);
						if (tMP_SpriteAsset != null)
						{
							break;
						}
					}
				}
				num++;
				continue;
			}
			spriteIndex = -1;
			return null;
		}
		return tMP_SpriteAsset;
	}

	public static TMP_SpriteAsset SearchForSpriteByHashCodeInternal(TMP_SpriteAsset spriteAsset, int hashCode, bool searchFallbacks, out int spriteIndex)
	{
		spriteIndex = spriteAsset.GetSpriteIndexFromHashcode(hashCode);
		if (spriteIndex != -1)
		{
			return spriteAsset;
		}
		if (searchFallbacks && spriteAsset.fallbackSpriteAssets != null && spriteAsset.fallbackSpriteAssets.Count > 0)
		{
			return SearchForSpriteByHashCodeInternal(spriteAsset.fallbackSpriteAssets, hashCode, searchFallbacks, out spriteIndex);
		}
		spriteIndex = -1;
		return null;
	}
}
