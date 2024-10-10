using UnityEngine;

public interface ICollisionEvents
{
	void OnCollisionEnter(Collision c);

	void OnCollisionStay(Collision c);

	void OnCollisionExit(Collision c);

	MonoBehaviour GetInstance();
}
