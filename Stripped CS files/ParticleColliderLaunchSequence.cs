using System.Collections;
using UnityEngine;

public class ParticleColliderLaunchSequence : MonoBehaviour
{
	public Transform[] chaosSpinners;

	public float spinChaos;

	public float chaosDrop;

	public float chaosDropTime;

	public Transform[] chaosJudders;

	public float judderChaos;

	public float totalTime;

	public Vector3[,] chaosSpinnerPositions;

	public Vector3[] chaosJudderAxis;

	public Collider[] colliders;

	public void Reset()
	{
		spinChaos = 1f;
		chaosDrop = 10f;
		chaosDropTime = 1f;
		judderChaos = 0.1f;
		totalTime = 10f;
	}

	public void Start()
	{
		chaosSpinnerPositions = new Vector3[chaosSpinners.Length, 2];
		int i = 0;
		for (int num = chaosSpinners.Length; i < num; i++)
		{
			chaosSpinnerPositions[i, 0] = chaosSpinners[i].position;
			chaosSpinnerPositions[i, 1] = chaosSpinners[i].position + new Vector3(0f, 0f - chaosDrop, 0f);
		}
		chaosJudderAxis = new Vector3[chaosJudders.Length];
		int j = 0;
		for (int num2 = chaosJudders.Length; j < num2; j++)
		{
			chaosJudderAxis[j] = chaosJudders[j].up;
		}
		colliders = GetComponentsInChildren<Collider>();
		int num3 = colliders.Length;
		while (num3-- > 0)
		{
			colliders[num3].enabled = false;
		}
		GoForLaunch();
	}

	public IEnumerator LaunchSequence()
	{
		float t = 0f;
		float d = 1f / chaosDropTime;
		int num = colliders.Length;
		while (num-- > 0)
		{
			colliders[num].enabled = true;
		}
		for (; t < totalTime; t += Time.fixedDeltaTime)
		{
			if (t < chaosDropTime)
			{
				for (num = 0; num < chaosSpinners.Length; num++)
				{
					chaosSpinners[num].position = Vector3.Lerp(chaosSpinnerPositions[num, 0], chaosSpinnerPositions[num, 1], t * d);
					chaosSpinners[num].Rotate(Random.insideUnitSphere.normalized, Random.value * 360f * spinChaos);
				}
			}
			for (num = 0; num < chaosJudders.Length; num++)
			{
				chaosJudders[num].up = (chaosJudderAxis[num] + Random.insideUnitSphere * judderChaos).normalized;
			}
			yield return new WaitForFixedUpdate();
		}
		num = colliders.Length;
		while (num-- > 0)
		{
			colliders[num].enabled = false;
		}
	}

	[ContextMenu("Go For Launch")]
	public void GoForLaunch()
	{
		StopCoroutine("GoForLaunch");
		StartCoroutine(LaunchSequence());
	}
}
