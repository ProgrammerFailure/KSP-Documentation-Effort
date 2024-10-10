using System;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Color/Height-based (Noise)")]
public class PQSMod_HeightColorMapNoise : PQSMod
{
	[Serializable]
	public class LandClass
	{
		public string name;

		public double altStart;

		public double altEnd;

		public Color color;

		public bool lerpToNext;

		public double fractalDelta { get; set; }

		public LandClass(string name, double fractalStart, double fractalEnd, Color baseColor, Color colorNoise, double colorNoiseAmount)
		{
			this.name = name;
			altStart = fractalStart;
			altEnd = fractalEnd;
			color = baseColor;
		}
	}

	public LandClass[] landClasses;

	public float blend;

	[HideInInspector]
	public int lcCount;

	public int itr;

	public LandClass lcSelected;

	public int lcSelectedIndex;

	public double vHeight;

	public double ct2;

	public double ct3;

	public void Reset()
	{
		blend = 1f;
		landClasses = new LandClass[5];
		landClasses[0] = new LandClass("AbyPl", 0.0, 0.5, new Color(0.7f, 0.7f, 0f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[1] = new LandClass("Beach", 0.5, 0.55, new Color(0.9f, 0.9f, 0.6f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[2] = new LandClass("Grass", 0.55, 0.75, new Color(0.1f, 0.6f, 0f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[3] = new LandClass("Rocky", 0.75, 0.8, new Color(0.6f, 0.6f, 0.6f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[4] = new LandClass("Snow", 0.8, 1.0, new Color(1f, 1f, 1f), new Color(0.4f, 0.4f, 0f), 0.0);
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel;
		lcCount = landClasses.Length;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		vHeight = (data.vertHeight - sphere.radiusMin) / sphere.radiusDelta;
		SelectLandClassByHeight(vHeight);
		if (lcSelected.lerpToNext)
		{
			data.vertColor = Color.Lerp(data.vertColor, Color.Lerp(lcSelected.color, landClasses[lcSelectedIndex + 1].color, (float)((vHeight - lcSelected.altStart) / (lcSelected.altEnd - lcSelected.altStart))), blend);
		}
		else
		{
			data.vertColor = Color.Lerp(data.vertColor, lcSelected.color, blend);
		}
	}

	public void SelectLandClassByHeight(double height)
	{
		itr = 0;
		while (true)
		{
			if (itr < lcCount)
			{
				if (height >= landClasses[itr].altStart && !(height > landClasses[itr].altEnd))
				{
					break;
				}
				itr++;
				continue;
			}
			lcSelectedIndex = lcCount - 1;
			lcSelected = landClasses[lcSelectedIndex];
			return;
		}
		lcSelectedIndex = itr;
		lcSelected = landClasses[itr];
	}

	public static double Lerp(double v2, double v1, double dt)
	{
		return v1 * dt + v2 * (1.0 - dt);
	}

	public double CubicHermite(double start, double end, double startTangent, double endTangent, double t)
	{
		ct2 = t * t;
		ct3 = ct2 * t;
		return start * (2.0 * ct3 - 3.0 * ct2 + 1.0) + startTangent * (ct3 - 2.0 * ct2 + t) + end * (-2.0 * ct3 + 3.0 * ct2) + endTangent * (ct3 - ct2);
	}

	public static double Clamp(double v, double low, double high)
	{
		if (v < low)
		{
			return low;
		}
		if (v > high)
		{
			return high;
		}
		return v;
	}
}
