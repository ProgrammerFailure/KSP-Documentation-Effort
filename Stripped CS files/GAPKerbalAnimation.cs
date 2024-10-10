using System.Collections;
using UnityEngine;

public class GAPKerbalAnimation : MonoBehaviour
{
	public Animation animation;

	public float randomDelayMin = 10f;

	public float randomDelayMax = 15f;

	public int clipCount;

	public AnimationState idleState;

	public void Start()
	{
		clipCount = animation.GetClipCount();
		int num = 0;
		foreach (AnimationState item in animation)
		{
			if (num != 0)
			{
				num++;
				continue;
			}
			idleState = item;
			item.wrapMode = WrapMode.Loop;
			break;
		}
		StartCoroutine(PlayAnimations());
	}

	public void OnDestroy()
	{
		StopAllCoroutines();
	}

	public IEnumerator PlayAnimations()
	{
		while (true)
		{
			if (!animation.isPlaying || !(animation.clip != idleState.clip))
			{
				animation.clip = idleState.clip;
				animation.wrapMode = WrapMode.Loop;
				animation.Play();
				yield return new WaitForSeconds(Random.Range(randomDelayMin, randomDelayMax));
				if (!(animation.clip == idleState.clip))
				{
					continue;
				}
				int num = (int)Random.Range(1f, (float)clipCount - 1f);
				if (clipCount == 1)
				{
					num = 0;
				}
				int num2 = 0;
				foreach (AnimationState item in animation)
				{
					if (num != num2)
					{
						num2++;
						continue;
					}
					animation.clip = item.clip;
					animation.wrapMode = WrapMode.Once;
					break;
				}
				animation.Play();
			}
			else
			{
				yield return null;
			}
		}
	}
}
