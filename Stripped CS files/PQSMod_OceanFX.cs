using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/OceanFX")]
public class PQSMod_OceanFX : PQSMod
{
	public Texture2D[] watermain;

	[HideInInspector]
	public Texture2D refraction;

	[HideInInspector]
	public Texture2D bump;

	[HideInInspector]
	public Texture2D fresnel;

	[HideInInspector]
	public Cubemap reflection;

	public float framesPerSecond = 10f;

	public bool cycleTextures;

	[HideInInspector]
	public double spaceAltitude;

	[HideInInspector]
	public float blendA;

	[HideInInspector]
	public float blendB;

	[HideInInspector]
	public Material waterMat;

	[HideInInspector]
	public float waterMainLength;

	[HideInInspector]
	public int txIndex;

	[HideInInspector]
	public float texBlend;

	[HideInInspector]
	public float angle;

	[HideInInspector]
	public Color specColor;

	[HideInInspector]
	public float oceanOpacity;

	[HideInInspector]
	public float spaceSurfaceBlend;

	public float t;

	public static int waterTexPropertyID;

	public static int waterTex1PropertyID;

	public static int mixPropertyID;

	public static int specColorPropertyID;

	public override void OnSetup()
	{
		waterTexPropertyID = Shader.PropertyToID("_WaterTex");
		waterTex1PropertyID = Shader.PropertyToID("_WaterTex1");
		mixPropertyID = Shader.PropertyToID("_Mix");
		specColorPropertyID = Shader.PropertyToID("_SpecColor");
		if (GetComponent<Renderer>() != null)
		{
			waterMat = GetComponent<Renderer>().sharedMaterial;
		}
		else
		{
			waterMat = sphere.surfaceMaterial;
		}
		waterMainLength = watermain.Length - 1;
		waterMat.SetTexture(waterTexPropertyID, watermain[0]);
		waterMat.SetTexture("_WaterTex1", watermain[1]);
	}

	public override void OnUpdateFinished()
	{
		t = Time.time * framesPerSecond / 20f;
		txIndex = (int)(t % waterMainLength);
		texBlend = t % 1f;
		if (sphere.secondaryTarget != null)
		{
			angle = (Vector3.Angle(sphere.secondaryTarget.forward, sphere.relativeSecondaryTargetPosition) / 180f - 0.5f) * 2f;
		}
		specColor = Color.Lerp(Color.white, Color.black, angle);
		spaceSurfaceBlend = Mathf.Clamp01((float)(sphere.visibleAltitude / spaceAltitude));
		if (cycleTextures)
		{
			waterMat.SetTexture(waterTexPropertyID, watermain[txIndex]);
			if ((float)txIndex == waterMainLength)
			{
				waterMat.SetTexture(waterTex1PropertyID, watermain[0]);
			}
			else
			{
				waterMat.SetTexture(waterTex1PropertyID, watermain[txIndex + 1]);
			}
		}
		waterMat.SetFloat(mixPropertyID, texBlend);
		waterMat.SetColor(specColorPropertyID, specColor);
	}
}
