using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXSequencer : MonoBehaviour
{
	[Serializable]
	public class SequenceFX
	{
		public float startTime;

		public float startTimeVariation;

		public float scheduled;

		public ParticleSystem particleSystem;

		public AudioClip audioFx;

		public AudioSource audioSource;

		public float pitchVariance;

		public void Play()
		{
			if (audioSource != null && audioFx != null)
			{
				audioSource.volume = GameSettings.AMBIENCE_VOLUME;
				if (pitchVariance != 0f)
				{
					audioSource.pitch = 1f + UnityEngine.Random.Range(0f - pitchVariance, pitchVariance);
				}
				audioSource.PlayOneShot(audioFx);
			}
			if (particleSystem != null)
			{
				particleSystem.Play();
			}
		}

		public float GetFXDuration()
		{
			float num = 0f;
			if (audioFx != null && audioSource != null)
			{
				num = audioFx.length;
			}
			if (particleSystem != null)
			{
				num = getSubFXHierarchyDuration(particleSystem, num);
			}
			return num;
		}

		public float getSubFXHierarchyDuration(ParticleSystem ps, float d)
		{
			ParticleSystem.MainModule main = ps.main;
			d = Mathf.Max(d, (main.startDelay.constant + main.duration + main.startLifetime.constant) * main.simulationSpeed);
			ParticleSystem[] componentsInChildren = ps.GetComponentsInChildren<ParticleSystem>(includeInactive: false);
			int num = componentsInChildren.Length;
			while (num-- > 0)
			{
				ParticleSystem.MainModule main2 = componentsInChildren[num].main;
				d = Mathf.Max(d, (main2.startDelay.constant + main2.duration + main2.startLifetime.constant) * main2.simulationSpeed);
			}
			return d;
		}
	}

	public bool PlayOnStart;

	public bool SelfDestructOnComplete;

	public List<SequenceFX> FXList;

	public bool isPlaying;

	public bool isComplete;

	public bool IsPlaying => isPlaying;

	public bool IsComplete => isComplete;

	public void Start()
	{
		if (PlayOnStart)
		{
			StartCoroutine(playSequence(new List<SequenceFX>(FXList), delegate
			{
			}));
		}
	}

	[ContextMenu("Play")]
	public void Play()
	{
		StartCoroutine(playSequence(new List<SequenceFX>(FXList), delegate
		{
		}));
	}

	public void Play(Callback<VFXSequencer> onComplete)
	{
		StartCoroutine(playSequence(new List<SequenceFX>(FXList), onComplete));
	}

	public IEnumerator playSequence(List<SequenceFX> fxList, Callback<VFXSequencer> onComplete)
	{
		float t = Time.timeSinceLevelLoad;
		isPlaying = true;
		float tToComplete = t;
		for (int num = fxList.Count - 1; num >= 0; num--)
		{
			if (fxList[num].startTimeVariation != 0f)
			{
				fxList[num].scheduled = fxList[num].startTime + UnityEngine.Random.Range(0f - fxList[num].startTimeVariation, fxList[num].startTimeVariation);
			}
			else
			{
				fxList[num].scheduled = fxList[num].startTime;
			}
			tToComplete = Mathf.Max(tToComplete, t + fxList[num].scheduled + fxList[num].GetFXDuration());
		}
		while (fxList.Count > 0)
		{
			for (int num2 = fxList.Count - 1; num2 >= 0; num2--)
			{
				if (fxList[num2].scheduled <= Time.timeSinceLevelLoad - t)
				{
					fxList[num2].Play();
					fxList.RemoveAt(num2);
				}
			}
			yield return null;
		}
		yield return new WaitForSeconds(tToComplete - Time.timeSinceLevelLoad);
		isPlaying = false;
		isComplete = true;
		onComplete(this);
		if (SelfDestructOnComplete)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
