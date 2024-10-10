using UnityEngine;

public class FXFadeByPressure : MonoBehaviour
{
	public float alphaNewSystem;

	public float maxAlpha = 0.9f;

	public float airDensity = 1f;

	public float fadeStartDns = 0.05089431f;

	public float fadeEndDns = 0.001362253f;

	public ParticleSystem particleSystem;

	public ParticleSystemRenderer particleSystemRenderer;

	public Color[] modifiedColors;

	public void Start()
	{
		particleSystem = base.transform.GetComponent<ParticleSystem>();
		particleSystemRenderer = GetComponent<ParticleSystemRenderer>();
		ParticleSystem.ColorOverLifetimeModule colorOverLifetime = particleSystem.colorOverLifetime;
		colorOverLifetime.enabled = true;
		alphaNewSystem = colorOverLifetime.color.color.a;
	}

	public void Update()
	{
		if (!(particleSystem == null) && HighLogic.LoadedSceneIsFlight && FlightGlobals.ready && !FlightDriver.Pause)
		{
			airDensity = Mathf.InverseLerp(fadeEndDns, fadeStartDns, (float)FlightGlobals.ship_dns);
			if (airDensity > 0.001f)
			{
				Color color = particleSystem.colorOverLifetime.color.color;
				color.a = Mathf.Lerp(0f, alphaNewSystem, airDensity);
				particleSystemRenderer.enabled = true;
			}
			else
			{
				particleSystemRenderer.enabled = false;
			}
		}
	}
}
