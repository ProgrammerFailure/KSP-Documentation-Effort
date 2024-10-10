using UnityEngine;

public class ExplosionTester : MonoBehaviour
{
	public GameObject explosion;

	public float normalOffset = 5f;

	public void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			RaycastHit hitInfo = default(RaycastHit);
			if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hitInfo, float.MaxValue))
			{
				Object.Instantiate(explosion, hitInfo.point + hitInfo.normal * normalOffset, Quaternion.Euler(Random.insideUnitSphere * 90f));
			}
		}
	}
}
