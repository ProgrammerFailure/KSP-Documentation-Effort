using UnityEngine;

namespace SoftMasking;

public interface IMaterialReplacer
{
	int order { get; }

	Material Replace(Material material);
}
