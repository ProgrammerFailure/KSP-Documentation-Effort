using UnityEngine;

public class LaunchPadFX : MonoBehaviour
{
	[SerializeField]
	public ParticleSystem[] ps;

	[SerializeField]
	public Material smokeParticleMaterial;

	[Range(0f, 1f)]
	[SerializeField]
	public float fxScale;

	[SerializeField]
	public float maxFX = 5f;

	public float totalFX;

	public void Start()
	{
		int num = ps.Length;
		while (num-- > 0)
		{
			FloatingOrigin.RegisterParticleSystem(ps[num]);
		}
	}

	public void OnDestroy()
	{
		int num = ps.Length;
		while (num-- > 0)
		{
			FloatingOrigin.UnregisterParticleSystem(ps[num]);
		}
	}

	public void AddFX(float fx)
	{
		totalFX = Mathf.Clamp(totalFX + fx, 0f, maxFX);
		fxScale = totalFX / maxFX;
	}

	public void LateUpdate()
	{
		int num = ps.Length;
		while (num-- > 0)
		{
			if (fxScale > 0f)
			{
				ParticleSystem.MainModule main = ps[num].main;
				if (!ps[num].isPlaying)
				{
					ps[num].Play();
				}
				if (!ps[num].emission.enabled)
				{
					ParticleSystem.EmissionModule emission = ps[num].emission;
					emission.enabled = true;
				}
				main.startColor = main.startColor.color.smethod_0(Mathf.Lerp(0f, 0.5f, fxScale));
			}
			if (fxScale == 0f)
			{
				ParticleSystem.EmissionModule emission2 = ps[num].emission;
				emission2.enabled = false;
			}
		}
		totalFX = 0f;
		fxScale = 0f;
	}
}
