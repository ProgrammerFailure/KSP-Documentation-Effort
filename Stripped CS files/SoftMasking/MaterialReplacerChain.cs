using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace SoftMasking;

public class MaterialReplacerChain : IMaterialReplacer
{
	public readonly List<IMaterialReplacer> _replacers;

	public int order { get; set; }

	public MaterialReplacerChain(IEnumerable<IMaterialReplacer> replacers, IMaterialReplacer yetAnother)
	{
		_replacers = replacers.ToList();
		_replacers.Add(yetAnother);
		Initialize();
	}

	public Material Replace(Material material)
	{
		int num = 0;
		Material material2;
		while (true)
		{
			if (num < _replacers.Count)
			{
				material2 = _replacers[num].Replace(material);
				if (material2 != null)
				{
					break;
				}
				num++;
				continue;
			}
			return null;
		}
		return material2;
	}

	public void Initialize()
	{
		_replacers.Sort((IMaterialReplacer a, IMaterialReplacer b) => a.order.CompareTo(b.order));
		order = _replacers[0].order;
	}
}
