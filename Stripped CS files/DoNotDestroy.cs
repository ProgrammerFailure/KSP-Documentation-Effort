using UnityEngine;

public class DoNotDestroy : MonoBehaviour
{
	public void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}
}
