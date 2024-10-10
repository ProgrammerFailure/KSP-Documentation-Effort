using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Fade Altitude Double")]
public class PQSMod_MaterialFadeAltitudeDouble : PQSMod
{
	public string floatName;

	public float inFadeStart;

	public float inFadeEnd;

	public float outFadeStart;

	public float outFadeEnd;

	public float valueStart;

	public float valueMid;

	public float valueEnd;

	[HideInInspector]
	public Material mat;

	public double a;

	public float t;

	public void Reset()
	{
		floatName = "_FadeAltitude";
		inFadeStart = 0f;
		inFadeEnd = 0.25f;
		outFadeStart = 0.75f;
		outFadeEnd = 1f;
		valueStart = 0f;
		valueMid = 1f;
		valueEnd = 0f;
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
			if (a <= (double)inFadeStart)
			{
				t = valueStart;
			}
			else if (a < (double)inFadeEnd)
			{
				t = Mathf.Lerp(valueStart, valueMid, (float)(1.0 - (a - (double)inFadeStart) / (double)(inFadeEnd - inFadeStart)));
			}
			else if (a <= (double)outFadeStart)
			{
				t = valueMid;
			}
			else if (a < (double)outFadeEnd)
			{
				t = Mathf.Lerp(valueStart, valueMid, (float)(1.0 - (a - (double)outFadeStart) / (double)(outFadeEnd - outFadeStart)));
			}
			else
			{
				t = valueEnd;
			}
			mat.SetFloat(floatName, t);
		}
	}

	public void OnDestroy()
	{
		if (mat != null)
		{
			mat.SetFloat(floatName, 1f);
		}
	}
}
