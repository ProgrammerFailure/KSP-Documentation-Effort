using UnityEngine;

public interface ICMTweakTarget
{
	bool TweakingTarget { get; set; }

	void SelectTweakTarget(Vector3 mousePosition);

	Transform GetReferenceTransform();

	bool SetSymmetryValues(Vector3 newPosition, Quaternion newRotation);

	Collider[] GetSelectedColliders();
}
