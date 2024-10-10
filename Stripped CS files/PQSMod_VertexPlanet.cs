using System;
using LibNoise;
using UnityEngine;

[AddComponentMenu("PQuadSphere/Mods/Vertex/Fractal Planet")]
public class PQSMod_VertexPlanet : PQSMod
{
	[Serializable]
	public class SimplexWrapper
	{
		public double deformity;

		public double octaves;

		public double persistance;

		public double frequency;

		public Simplex simplex { get; set; }

		public SimplexWrapper(SimplexWrapper copyFrom)
		{
			deformity = copyFrom.deformity;
			octaves = copyFrom.octaves;
			persistance = copyFrom.persistance;
			frequency = copyFrom.frequency;
		}

		public SimplexWrapper(double deformity, double octaves, double persistance, double frequency)
		{
			this.deformity = deformity;
			this.octaves = octaves;
			this.persistance = persistance;
			this.frequency = frequency;
		}

		public void Setup(int seed)
		{
			simplex = new Simplex(seed, octaves, persistance, frequency);
		}
	}

	[Serializable]
	public class NoiseModWrapper
	{
		public double deformity;

		public int octaves;

		public double persistance;

		public double frequency;

		public int seed { get; set; }

		public IModule noise { get; set; }

		public NoiseModWrapper(NoiseModWrapper copyFrom)
		{
			deformity = copyFrom.deformity;
			octaves = copyFrom.octaves;
			persistance = copyFrom.persistance;
			frequency = copyFrom.frequency;
		}

		public NoiseModWrapper(double deformity, int octaves, double persistance, double frequency)
		{
			this.deformity = deformity;
			this.octaves = octaves;
			this.persistance = persistance;
			this.frequency = frequency;
		}

		public void Setup(IModule mod)
		{
			noise = mod;
		}
	}

	[Serializable]
	public class LandClass
	{
		public string name;

		public double fractalStart;

		public double fractalEnd;

		public Color baseColor;

		public Color colorNoise;

		public double colorNoiseAmount;

		public SimplexWrapper colorNoiseMap;

		public bool lerpToNext;

		public double startHeight { get; set; }

		public double endHeight { get; set; }

		public double fractalDelta { get; set; }

		public LandClass(string name, double fractalStart, double fractalEnd, Color baseColor, Color colorNoise, double colorNoiseAmount)
		{
			this.name = name;
			this.fractalStart = fractalStart;
			this.fractalEnd = fractalEnd;
			this.baseColor = baseColor;
			this.colorNoise = colorNoise;
			this.colorNoiseAmount = colorNoiseAmount;
		}
	}

	public int seed;

	public double deformity;

	public double colorDeformity;

	public double oceanLevel;

	public double oceanStep;

	public double oceanDepth;

	public bool oceanSnap;

	public double terrainSmoothing = 0.25;

	public double terrainShapeStart = 2.0;

	public double terrainShapeEnd = -2.0;

	public double terrainRidgesMin = 0.4;

	public double terrainRidgesMax = 1.0;

	public SimplexWrapper continental;

	public SimplexWrapper continentalRuggedness;

	public SimplexWrapper continentalSmoothing;

	public SimplexWrapper continentalSharpnessMap;

	public NoiseModWrapper continentalSharpness;

	public SimplexWrapper terrainType;

	public bool buildHeightColors;

	public double continentialHeight;

	public double continentialSharpnessValue;

	public double continentialSharpnessMapValue;

	public double continentialHeightSmoothed;

	public double continentialTerrainNoise;

	public double continentalRHeight;

	public double continental2Height;

	public double continentalDelta;

	public int itr;

	public int lcCount;

	public LandClass lcSelected;

	public int lcSelectedIndex;

	public double ct2;

	public double ct3;

	public double tHeight;

	public Color c1;

	public Color c2;

	public LandClass lcLerp;

	public double vHeight;

	public LandClass[] landClasses;

	public double terrainRidgeBalance = 0.1;

	public double continentalDeformity;

	public double d1;

	public double continentialHeightPreSmooth;

	public void Reset()
	{
		seed = 23123;
		deformity = 1.0;
		colorDeformity = 1.0;
		continental = new SimplexWrapper(1.0, 10.0, 0.7, 0.5);
		continentalRuggedness = new SimplexWrapper(1.0, 4.0, 0.5, 0.5);
		continentalSmoothing = new SimplexWrapper(1.0, 12.0, 0.7, 2.0);
		terrainType = new SimplexWrapper(1.0, 12.0, 0.7, 0.5);
		landClasses = new LandClass[5];
		landClasses[0] = new LandClass("AbyPl", 0.0, 0.5, new Color(0.7f, 0.7f, 0f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[1] = new LandClass("Beach", 0.5, 0.55, new Color(0.9f, 0.9f, 0.6f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[2] = new LandClass("Grass", 0.55, 0.75, new Color(0.1f, 0.6f, 0f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[3] = new LandClass("Rocky", 0.75, 0.8, new Color(0.6f, 0.6f, 0.6f), new Color(0.4f, 0.4f, 0f), 0.0);
		landClasses[4] = new LandClass("Snow", 0.8, 1.0, new Color(1f, 1f, 1f), new Color(0.4f, 0.4f, 0f), 0.0);
	}

	public override void OnSetup()
	{
		requirements = GClass4.ModiferRequirements.MeshColorChannel | GClass4.ModiferRequirements.MeshCustomNormals;
		lcCount = landClasses.Length;
		for (itr = 0; itr < lcCount; itr++)
		{
			landClasses[itr].fractalDelta = landClasses[itr].fractalEnd - landClasses[itr].fractalStart;
			landClasses[itr].colorNoiseMap.Setup(seed + itr + 10);
		}
		int num = seed;
		continental.Setup(num++);
		continentalRuggedness.Setup(num++);
		continentalSmoothing = new SimplexWrapper(continental);
		continentalSmoothing.Setup(seed);
		continentalSmoothing.persistance = continental.persistance * terrainSmoothing;
		continentalSmoothing.frequency = continental.frequency;
		terrainType.Setup(num++);
		continentalSharpness.Setup(new RidgedMultifractal(continentalSharpness.frequency, continentalSharpness.persistance, continentalSharpness.octaves, num++, NoiseQuality.High));
		continentalSharpnessMap.Setup(num++);
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

	public override void OnVertexBuildHeight(GClass4.VertexBuildData data)
	{
		continentalDeformity = 1.0;
		continental2Height = continentalSmoothing.simplex.noiseNormalized(data.directionFromCenter);
		continental.simplex.persistence = continental.persistance - continentalSmoothing.persistance * continental2Height;
		continentialHeight = continental.simplex.noiseNormalized(data.directionFromCenter);
		continentialSharpnessValue = (continentalSharpness.noise.GetValue(data.directionFromCenter) + 1.0) * 0.5;
		continentialSharpnessValue *= Lerp(continentalSharpness.deformity, continentalSharpness.deformity * terrainRidgeBalance, (continental2Height + continentialSharpnessValue) * 0.5);
		continentialSharpnessMapValue = Clamp((continentalSharpnessMap.simplex.noise(data.directionFromCenter) + 1.0) * 0.5, terrainRidgesMin, terrainRidgesMax);
		continentialSharpnessMapValue = (continentialSharpnessMapValue - terrainRidgesMin) / (terrainRidgesMax - terrainRidgesMin) * continentalSharpnessMap.deformity;
		continentialSharpnessValue += Lerp(0.0, continentialSharpnessValue, continentialSharpnessMapValue);
		continentialHeight += continentialSharpnessValue;
		continentalDeformity += continentalSharpness.deformity * continentalSharpnessMap.deformity;
		continentialHeight /= continentalDeformity;
		continentalDelta = (continentialHeight - oceanLevel) / (1.0 - oceanLevel);
		if (continentialHeight < oceanLevel)
		{
			if (oceanSnap)
			{
				vHeight = 0.0 - oceanStep;
			}
			else
			{
				vHeight = continentalDelta * oceanDepth - oceanStep;
			}
			continentialHeightPreSmooth = vHeight;
		}
		else
		{
			continentalRuggedness.simplex.persistence = continentalRuggedness.persistance * continentalDelta;
			continentalRHeight = continentalRuggedness.simplex.noiseNormalized(data.directionFromCenter) * continentalDelta * continentalDelta;
			continentialHeight = continentalDelta * continental.deformity + continentalRHeight * continentalRuggedness.deformity;
			continentialHeight /= continental.deformity + continentalRuggedness.deformity;
			continentialHeightPreSmooth = continentialHeight;
			continentialHeight = CubicHermite(0.0, 1.0, terrainShapeStart, terrainShapeEnd, continentialHeight);
			vHeight = continentialHeight;
		}
		data.vertHeight += System.Math.Round(vHeight, 5) * deformity;
	}

	public override void OnVertexBuild(GClass4.VertexBuildData data)
	{
		if (buildHeightColors)
		{
			float num = (float)((data.vertHeight - sphere.radius) / colorDeformity);
			data.vertColor.r = num;
			data.vertColor.g = num;
			data.vertColor.b = num;
			return;
		}
		float num2 = (float)((data.vertHeight - sphere.radius) / colorDeformity);
		d1 = terrainType.simplex.noiseNormalized(data.directionFromCenter);
		tHeight = Mathf.Clamp01((float)((continentialHeightPreSmooth + d1 * terrainType.deformity) * (double)num2));
		SelectLandClassByHeight(tHeight);
		c1 = Color.Lerp(lcSelected.baseColor, lcSelected.colorNoise, (float)(lcSelected.colorNoiseAmount * lcSelected.colorNoiseMap.simplex.noiseNormalized(data.directionFromCenter)));
		if (lcSelected.lerpToNext)
		{
			lcLerp = landClasses[lcSelectedIndex + 1];
			c2 = Color.Lerp(lcLerp.baseColor, lcLerp.colorNoise, (float)(lcLerp.colorNoiseAmount * lcLerp.colorNoiseMap.simplex.noiseNormalized(data.directionFromCenter)));
			c1 = Color.Lerp(c1, c2, (float)((tHeight - lcSelected.fractalStart) / (lcSelected.fractalEnd - lcSelected.fractalStart)));
		}
		data.vertColor = c1;
		data.vertColor.a = (float)continentialHeightPreSmooth;
	}

	public override double GetVertexMaxHeight()
	{
		return deformity * 1.2;
	}

	public override double GetVertexMinHeight()
	{
		return 0.0;
	}

	public void SelectLandClassByHeight(double height)
	{
		itr = 0;
		while (true)
		{
			if (itr < lcCount)
			{
				if (height >= landClasses[itr].fractalStart && !(height > landClasses[itr].fractalEnd))
				{
					break;
				}
				itr++;
				continue;
			}
			lcSelected = landClasses[0];
			lcSelectedIndex = 0;
			return;
		}
		lcSelected = landClasses[itr];
		lcSelectedIndex = itr;
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
}
