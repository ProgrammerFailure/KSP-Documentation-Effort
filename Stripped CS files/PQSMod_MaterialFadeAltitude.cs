using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Fade Altitude")]
public class PQSMod_MaterialFadeAltitude : PQSMod
{
	public string floatName;

	public float fadeStart;

	public float fadeEnd;

	public float valueStart;

	public float valueEnd;

	[HideInInspector]
	public Material mat;

	public double a;

	public float t;

	public void Reset()
	{
		floatName = "_FadeAltitude";
		fadeStart = 0f;
		fadeEnd = 1f;
		valueStart = 0f;
		valueEnd = 1f;
	}

	public override void OnSetup()
	{
		if (GetComponent<Renderer>() != null)
		{
			mat = GetComponent<Renderer>().sharedMaterial;
		}
		else
		{
			mat = sphere.surfaceMaterial;
		}
	}

	public void Update()
	{
		if (!(sphere == null) && !(sphere.target == null))
		{
			a = sphere.visibleAltitude;
			if (a <= (double)fadeStart)
			{
				t = 1f;
			}
			else if (a >= (double)fadeEnd)
			{
				t = 0f;
			}
			else
			{
				t = (float)(1.0 - (a - (double)fadeStart) / (double)(fadeEnd - fadeStart));
			}
			mat.SetFloat(floatName, Mathf.Lerp(valueStart, valueEnd, t));
		}
	}
}
