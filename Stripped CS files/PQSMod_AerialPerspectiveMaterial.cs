using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Material/Aerial Perspective")]
public class PQSMod_AerialPerspectiveMaterial : PQSMod
{
	public float globalDensity;

	public float heightFalloff;

	public float atmosphereDepth;

	public float oceanDepth;

	public bool DEBUG_SetEveryFrame;

	public double cameraAlt;

	public float cameraAtmosAlt;

	public float heightDensAtViewer;

	public override void OnSetup()
	{
	}

	public override void OnSphereActive()
	{
		Shader.SetGlobalFloat("_globalDensity", globalDensity);
		Shader.SetGlobalFloat("_heightFallOff", heightFalloff);
		Shader.SetGlobalFloat("_atmosphereDepth", atmosphereDepth);
	}

	public override void OnUpdateFinished()
	{
		if (DEBUG_SetEveryFrame)
		{
			Shader.SetGlobalFloat("_globalDensity", globalDensity);
			Shader.SetGlobalFloat("_heightFallOff", heightFalloff);
			Shader.SetGlobalFloat("_atmosphereDepth", atmosphereDepth);
		}
		if (sphere.secondaryTarget != null)
		{
			cameraAlt = sphere.targetSecondaryAltitude;
		}
		else
		{
			cameraAlt = sphere.targetAltitude;
		}
		cameraAtmosAlt = (float)(cameraAlt / (double)atmosphereDepth);
		heightDensAtViewer = (float)Math.Exp((0f - heightFalloff) * cameraAtmosAlt);
		Shader.SetGlobalFloat("_cameraAltitude", cameraAtmosAlt);
		Shader.SetGlobalFloat("_heightDensityAtViewer", heightDensAtViewer);
	}
}
