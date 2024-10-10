using UnityEngine;

public class PrefabSpawner : MonoBehaviour
{
	public GameObject prefab;

	public void Awake()
	{
		if (!(prefab == null))
		{
			GameObject obj = Object.Instantiate(prefab, base.transform.position, base.transform.rotation);
			obj.transform.parent = base.gameObject.transform;
			obj.transform.localPosition = Vector3.zero;
			obj.transform.localRotation = Quaternion.identity;
		}
	}
}
