using UnityEngine;

public class SmokeTrailControl : MonoBehaviour
{
	public float maxAlpha = 0.9f;

	public float airDensity = 1f;

	public float fadeStartAlt = 15000f;

	public float fadeEndAlt = 34000f;

	public float fadeStartDns = 0.06089431f;

	public float fadeEndDns = 0.001362253f;

	public string shaderColorName = "_TintColor";

	public Color fadedColor;

	public bool fadeToColor;

	public Renderer r;

	public void Start()
	{
		r = GetComponent<Renderer>();
	}

	public void Update()
	{
		if (!HighLogic.LoadedSceneIsFlight || !FlightGlobals.ready || FlightDriver.Pause)
		{
			return;
		}
		airDensity = Mathf.InverseLerp(fadeEndDns, fadeStartDns, (float)FlightGlobals.ship_dns);
		if (airDensity > 0.001f)
		{
			if (fadeToColor)
			{
				r.material.SetColor(shaderColorName, new Color(Mathf.Lerp(fadedColor.r, maxAlpha, airDensity), Mathf.Lerp(fadedColor.g, maxAlpha, airDensity), Mathf.Lerp(fadedColor.b, maxAlpha, airDensity), Mathf.Lerp(fadedColor.a, maxAlpha, airDensity)));
			}
			else
			{
				r.material.SetColor(shaderColorName, new Color(1f, 1f, 1f, Mathf.Lerp(0f, maxAlpha, airDensity)));
			}
			r.enabled = true;
		}
		else
		{
			r.enabled = false;
		}
	}
}
