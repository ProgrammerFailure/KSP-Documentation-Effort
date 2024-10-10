using UnityEngine;

public class DisableOnPlay : MonoBehaviour
{
	public void Start()
	{
		base.gameObject.SetActive(value: false);
	}
}
