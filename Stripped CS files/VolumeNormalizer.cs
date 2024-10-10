using UnityEngine;

public class VolumeNormalizer : MonoBehaviour
{
	public float threshold = 1f;

	public int sampleCount = 500;

	public float level;

	public float volume;

	public float sharpness = 8f;

	public float[] samples;

	public void Awake()
	{
		samples = new float[sampleCount];
	}

	public void Update()
	{
		if (GameSettings.SOUND_NORMALIZER_ENABLED)
		{
			threshold = GameSettings.SOUND_NORMALIZER_THRESHOLD;
			sharpness = GameSettings.SOUND_NORMALIZER_RESPONSIVENESS;
			AudioListener.GetOutputData(samples, 0);
			level = 0f;
			for (int i = 0; i < sampleCount; i += 1 + GameSettings.SOUND_NORMALIZER_SKIPSAMPLES)
			{
				level = Mathf.Max(level, Mathf.Abs(samples[i]));
			}
			if (level > threshold)
			{
				volume = threshold / level;
			}
			else
			{
				volume = 1f;
			}
			AudioListener.volume = Mathf.Lerp(AudioListener.volume, volume * GameSettings.MASTER_VOLUME, sharpness * Time.deltaTime);
		}
		else
		{
			AudioListener.volume = Mathf.Lerp(AudioListener.volume, GameSettings.MASTER_VOLUME, sharpness * Time.deltaTime);
		}
	}
}
