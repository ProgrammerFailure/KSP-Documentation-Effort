using System.Collections.Generic;
using UnityEngine;

public static class PartGeometryUtil
{
	public class PosRot
	{
		public Vector3 vector;

		public Quaternion quaternion;

		public PosRot(Vector3 v, Quaternion q)
		{
			vector = v;
			quaternion = q;
		}
	}

	public static List<string> disabledVariantGOs;

	public static Bounds GetRendererBounds(this GameObject p)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		Renderer[] componentsInChildren = p.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (flag)
			{
				result.Encapsulate(componentsInChildren[i].bounds);
				continue;
			}
			result = componentsInChildren[i].bounds;
			flag = true;
		}
		return result;
	}

	public static Bounds GetRendererBoundsWithoutParticles(this GameObject p)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		Renderer[] componentsInChildren = p.GetComponentsInChildren<Renderer>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (componentsInChildren[i].GetType() != typeof(ParticleSystemRenderer))
			{
				if (flag)
				{
					result.Encapsulate(componentsInChildren[i].bounds);
					continue;
				}
				result = componentsInChildren[i].bounds;
				flag = true;
			}
		}
		return result;
	}

	public static Bounds GetColliderBounds(this GameObject p)
	{
		Bounds result = default(Bounds);
		bool flag = false;
		Collider[] componentsInChildren = p.GetComponentsInChildren<Collider>();
		int i = 0;
		for (int num = componentsInChildren.Length; i < num; i++)
		{
			if (flag)
			{
				result.Encapsulate(componentsInChildren[i].bounds);
				continue;
			}
			result = componentsInChildren[i].bounds;
			flag = true;
		}
		return result;
	}

	public static Bounds GetPartRendererBound(this Part p)
	{
		List<Renderer> list = p.FindModelComponents<Renderer>();
		bool flag = false;
		Bounds result = default(Bounds);
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			if (flag)
			{
				result.Encapsulate(list[i].bounds);
				continue;
			}
			result = list[i].bounds;
			flag = true;
		}
		return result;
	}

	public static Bounds[] GetRendererBounds(this Part p)
	{
		return GetPartRendererBounds(p);
	}

	public static Bounds[] GetColliderBounds(this Part p)
	{
		return GetPartColliderBounds(p);
	}

	public static Bounds[] GetPartRendererBounds(Part p)
	{
		if (disabledVariantGOs == null)
		{
			disabledVariantGOs = new List<string>();
		}
		else
		{
			disabledVariantGOs.Clear();
		}
		if (p.variants != null)
		{
			PartVariant selectedVariant = p.variants.SelectedVariant;
			for (int i = 0; i < selectedVariant.InfoGameObjects.Count; i++)
			{
				if (!selectedVariant.InfoGameObjects[i].Status)
				{
					disabledVariantGOs.Add(selectedVariant.InfoGameObjects[i].Name);
				}
			}
		}
		List<MeshRenderer> list = p.FindModelMeshRenderersCached();
		List<SkinnedMeshRenderer> list2 = p.FindModelSkinnedMeshRenderersCached();
		int count = list.Count;
		while (count-- > 0)
		{
			if (p.partRendererBoundsIgnore.Contains(list[count].name))
			{
				list.RemoveAt(count);
			}
			else if (disabledVariantGOs.Contains(list[count].name))
			{
				list.RemoveAt(count);
			}
		}
		int count2 = list2.Count;
		while (count2-- > 0)
		{
			if (p.partRendererBoundsIgnore.Contains(list2[count2].name))
			{
				list2.RemoveAt(count2);
			}
			else if (disabledVariantGOs.Contains(list2[count2].name))
			{
				list2.RemoveAt(count2);
			}
		}
		Bounds[] array = new Bounds[list.Count + list2.Count];
		int num = 0;
		for (int j = 0; j < list.Count; j++)
		{
			array[num++] = list[j].bounds;
		}
		for (int k = 0; k < list2.Count; k++)
		{
			array[num++] = list2[k].bounds;
		}
		return array;
	}

	public static Bounds[] GetPartColliderBounds(Part p)
	{
		List<Collider> list = p.FindModelComponents<Collider>();
		Bounds[] array = new Bounds[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			array[i] = list[i].bounds;
		}
		return array;
	}

	public static Bounds MergeBounds(Bounds[] bounds, Transform relativeTo)
	{
		Bounds result = default(Bounds);
		result.center = relativeTo.position;
		int num = bounds.Length;
		while (num-- > 0)
		{
			result.Encapsulate(bounds[num].max);
			result.Encapsulate(bounds[num].min);
		}
		return result;
	}

	public static Bounds MergeBounds(Bounds[] bounds)
	{
		Bounds result = default(Bounds);
		if (bounds != null && bounds.Length >= 1)
		{
			result.center = bounds[0].center;
			int num = bounds.Length;
			while (num-- > 0)
			{
				result.Encapsulate(bounds[num].max);
				result.Encapsulate(bounds[num].min);
			}
			return result;
		}
		return result;
	}

	public static Bounds MergeBounds(Dictionary<Bounds[], PosRot> centeredBounds, PosRot relativeTo)
	{
		Bounds result = default(Bounds);
		result.center = relativeTo.vector;
		Dictionary<Bounds[], PosRot>.Enumerator enumerator = centeredBounds.GetEnumerator();
		while (enumerator.MoveNext())
		{
			Bounds[] key = enumerator.Current.Key;
			PosRot value = enumerator.Current.Value;
			int num = key.Length;
			for (int i = 0; i < num; i++)
			{
				result.Encapsulate(relativeTo.quaternion * value.vector + value.quaternion * key[i].max);
				result.Encapsulate(relativeTo.quaternion * value.vector + value.quaternion * key[i].min);
			}
		}
		return result;
	}

	public static Vector3 FindBoundsCentroid(Bounds[] bounds, Transform localTo)
	{
		Vector3 zero = Vector3.zero;
		float num = 0f;
		int num2 = bounds.Length;
		while (num2-- > 0)
		{
			Bounds bounds2 = bounds[num2];
			float magnitude = bounds2.size.magnitude;
			if (localTo != null)
			{
				zero += localTo.InverseTransformPoint(bounds2.center) * magnitude;
			}
			else
			{
				zero += bounds2.center * magnitude;
			}
			num += magnitude;
		}
		return zero / num;
	}
}
