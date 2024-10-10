using System;
using System.Collections.Generic;
using UnityEngine;

namespace SoftMasking;

public class MaterialReplacements
{
	public class MaterialOverride
	{
		public int _useCount;

		public Material original { get; set; }

		public Material replacement { get; set; }

		public MaterialOverride(Material original, Material replacement)
		{
			this.original = original;
			this.replacement = replacement;
			_useCount = 1;
		}

		public Material Get()
		{
			_useCount++;
			return replacement;
		}

		public bool Release()
		{
			return --_useCount == 0;
		}
	}

	public readonly IMaterialReplacer _replacer;

	public readonly Action<Material> _applyParameters;

	public readonly List<MaterialOverride> _overrides = new List<MaterialOverride>();

	public MaterialReplacements(IMaterialReplacer replacer, Action<Material> applyParameters)
	{
		_replacer = replacer;
		_applyParameters = applyParameters;
	}

	public Material Get(Material original)
	{
		int num = 0;
		MaterialOverride materialOverride;
		while (true)
		{
			if (num < _overrides.Count)
			{
				materialOverride = _overrides[num];
				if ((object)materialOverride.original == original)
				{
					break;
				}
				num++;
				continue;
			}
			Material material = _replacer.Replace(original);
			if ((bool)material)
			{
				material.hideFlags = HideFlags.HideAndDontSave;
				_applyParameters(material);
			}
			_overrides.Add(new MaterialOverride(original, material));
			return material;
		}
		Material material2 = materialOverride.Get();
		if ((bool)material2)
		{
			material2.CopyPropertiesFromMaterial(original);
			_applyParameters(material2);
		}
		return material2;
	}

	public void Release(Material replacement)
	{
		int num = 0;
		while (true)
		{
			if (num < _overrides.Count)
			{
				MaterialOverride materialOverride = _overrides[num];
				if (materialOverride.replacement == replacement && materialOverride.Release())
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		UnityEngine.Object.DestroyImmediate(replacement);
		_overrides.RemoveAt(num);
	}

	public void ApplyAll()
	{
		for (int i = 0; i < _overrides.Count; i++)
		{
			Material replacement = _overrides[i].replacement;
			if ((bool)replacement)
			{
				_applyParameters(replacement);
			}
		}
	}

	public void DestroyAllAndClear()
	{
		for (int i = 0; i < _overrides.Count; i++)
		{
			UnityEngine.Object.DestroyImmediate(_overrides[i].replacement);
		}
		_overrides.Clear();
	}
}
