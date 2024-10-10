using UnityEngine;

public class CollisionAnimationHandler : MonoBehaviour, ICollisionEvents
{
	public Animation[] animations;

	public void Start()
	{
		animations = base.gameObject.GetComponentsInChildren<Animation>();
	}

	public void OnCollisionEnter(Collision c)
	{
		if (animations != null)
		{
			for (int i = 0; i < animations.Length; i++)
			{
				animations[i].Stop();
			}
		}
	}

	public void OnCollisionStay(Collision c)
	{
	}

	public void OnCollisionExit(Collision c)
	{
	}

	public MonoBehaviour GetInstance()
	{
		return this;
	}
}
