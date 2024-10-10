using UnityEngine;

public class AdvancedRagdoll : MonoBehaviour
{
	public bool debug;

	public Transform ragdollRoot;

	public bool copyMaterials;

	public LayerMask groundLayer = -1;

	public void Start()
	{
		if (debug)
		{
			Debug.Log("AdvancedRagdoll.Start() " + base.name);
		}
	}

	public void SynchRagdollIn(Transform src)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.SynchRagdollIn() " + base.name + " passed null src!");
			return;
		}
		if (copyMaterials)
		{
			CopyMaterials(src, base.transform);
		}
		CopyTransforms(src, base.transform);
	}

	public void SynchRagdollOut(Transform dest)
	{
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.SynchRagdollOut() " + base.name + " passed null dest!");
			return;
		}
		if (copyMaterials)
		{
			CopyMaterials(base.transform, dest);
		}
		PoseToAnimation(base.transform, dest);
	}

	public void CopyTransforms(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.CopyTransforms() " + base.name + " passed null dest!");
			return;
		}
		dest.localPosition = src.localPosition;
		dest.localRotation = src.localRotation;
		dest.localScale = src.localScale;
		foreach (Transform item in src)
		{
			Transform transform2 = dest.Find(item.name);
			if ((bool)transform2)
			{
				CopyTransforms(item, transform2);
			}
		}
	}

	public void PoseToAnimation(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.PoseToAnimation() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.PoseToAnimation() " + base.name + " passed null dest!");
			return;
		}
		if (!dest.GetComponent<Animation>())
		{
			dest.gameObject.AddComponent<Animation>();
		}
		CopyTransforms(src, dest);
		AnimationClip animationClip = new AnimationClip();
		animationClip.legacy = true;
		TransformToAnimationCurve(src, src, dest, dest, animationClip);
		dest.gameObject.GetComponent<Animation>().AddClip(animationClip, "RagdollPose");
		dest.gameObject.GetComponent<Animation>()["RagdollPose"].wrapMode = WrapMode.Loop;
		dest.gameObject.GetComponent<Animation>().Play("RagdollPose");
	}

	public void TransformToAnimationCurve(Transform src, Transform srcRoot, Transform dest, Transform destRoot, AnimationClip clip)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null dest!");
			return;
		}
		if (!clip)
		{
			Debug.LogError("AdvancedRagdoll.TransformToAnimationClip() " + base.name + " passed null clip!");
			return;
		}
		string transformPathToRoot = GetTransformPathToRoot(src, srcRoot);
		string transformPathToRoot2 = GetTransformPathToRoot(dest, destRoot);
		if (debug)
		{
			Debug.Log("AdvancedRagdoll.TransformToAnimationClip() " + base.name + "\n srcPath = " + transformPathToRoot + "\n destPath = " + transformPathToRoot2);
		}
		AnimationCurve curve = AnimationCurve.Linear(0f, src.localPosition.x, 1f, src.localPosition.x);
		AnimationCurve curve2 = AnimationCurve.Linear(0f, src.localPosition.y, 1f, src.localPosition.y);
		AnimationCurve curve3 = AnimationCurve.Linear(0f, src.localPosition.z, 1f, src.localPosition.z);
		AnimationCurve curve4 = AnimationCurve.Linear(0f, src.localRotation.w, 1f, src.localRotation.w);
		AnimationCurve curve5 = AnimationCurve.Linear(0f, src.localRotation.x, 1f, src.localRotation.x);
		AnimationCurve curve6 = AnimationCurve.Linear(0f, src.localRotation.y, 1f, src.localRotation.y);
		AnimationCurve curve7 = AnimationCurve.Linear(0f, src.localRotation.z, 1f, src.localRotation.z);
		AnimationCurve curve8 = AnimationCurve.Linear(0f, src.localScale.x, 1f, src.localScale.x);
		AnimationCurve curve9 = AnimationCurve.Linear(0f, src.localScale.y, 1f, src.localScale.y);
		AnimationCurve curve10 = AnimationCurve.Linear(0f, src.localScale.z, 1f, src.localScale.z);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.x", curve);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.y", curve2);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localPosition.z", curve3);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.w", curve4);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.x", curve5);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.y", curve6);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localRotation.z", curve7);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.x", curve8);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.y", curve9);
		clip.SetCurve(transformPathToRoot2, typeof(Transform), "localScale.z", curve10);
		foreach (Transform item in src)
		{
			Transform transform2 = dest.Find(item.name);
			if ((bool)transform2)
			{
				TransformToAnimationCurve(item, srcRoot, transform2, destRoot, clip);
			}
		}
	}

	public string GetTransformPathToRoot(Transform t, Transform root)
	{
		if (!t)
		{
			Debug.LogError("AdvancedRagdoll.GetTransformPathToRoot() " + base.name + " passed null t!");
			return "";
		}
		if (!root)
		{
			Debug.LogError("AdvancedRagdoll.GetTransformPathToRoot() " + base.name + " passed null root!");
			return "";
		}
		string text = t.name;
		Transform parent = t.parent;
		while (parent != null && parent != root)
		{
			text = text.Insert(0, parent.name + "/");
			parent = parent.parent;
		}
		return text;
	}

	public void CopyMaterials(Transform src, Transform dest)
	{
		if (!src)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " passed null src!");
			return;
		}
		if (!dest)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " passed null dest!");
			return;
		}
		Renderer[] componentsInChildren = src.GetComponentsInChildren<Renderer>();
		Renderer[] componentsInChildren2 = dest.GetComponentsInChildren<Renderer>();
		if (componentsInChildren.Length != componentsInChildren2.Length)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " number of src renderers (" + componentsInChildren.Length + ") does not match dest (" + componentsInChildren2.Length + ")");
		}
		else if (componentsInChildren.Length == 0)
		{
			Debug.LogError("AdvancedRagdoll.CopyMaterials() " + base.name + " has no renderers!");
		}
		else
		{
			int i = 0;
			for (int num = componentsInChildren.Length; i < num; i++)
			{
				componentsInChildren2[i].materials = componentsInChildren[i].materials;
			}
		}
	}
}
