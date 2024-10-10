using System.IO;
using UnityEngine;

[AddComponentMenu("KSP/Texture Tools")]
public class TextureTools : MonoBehaviour
{
	public class Vector3Curve
	{
		public AnimationCurve x;

		public AnimationCurve y;

		public AnimationCurve z;

		public float minTime { get; set; }

		public float maxTime { get; set; }

		public Vector3Curve()
		{
			x = new AnimationCurve();
			y = new AnimationCurve();
			z = new AnimationCurve();
			minTime = float.MaxValue;
			maxTime = float.MinValue;
		}

		public void Add(float time, Vector3 value)
		{
			x.AddKey(time, value.x);
			y.AddKey(time, value.y);
			z.AddKey(time, value.z);
			minTime = Mathf.Min(minTime, time);
			maxTime = Mathf.Max(maxTime, time);
		}

		public void Add(float time, Color value)
		{
			x.AddKey(time, value.r);
			y.AddKey(time, value.g);
			z.AddKey(time, value.b);
			minTime = Mathf.Min(minTime, time);
			maxTime = Mathf.Max(maxTime, time);
		}

		public Vector3 EvaluateVector(float time)
		{
			return new Vector3(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
		}

		public Color EvaluateColor(float time)
		{
			return new Color(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time));
		}
	}

	public class Vector4Curve
	{
		public AnimationCurve x;

		public AnimationCurve y;

		public AnimationCurve z;

		public AnimationCurve w;

		public float minTime { get; set; }

		public float maxTime { get; set; }

		public Vector4Curve()
		{
			x = new AnimationCurve();
			y = new AnimationCurve();
			z = new AnimationCurve();
			w = new AnimationCurve();
			minTime = float.MaxValue;
			maxTime = float.MinValue;
		}

		public void Add(float time, Vector4 value)
		{
			x.AddKey(time, value.x);
			y.AddKey(time, value.y);
			z.AddKey(time, value.z);
			w.AddKey(time, value.w);
			minTime = Mathf.Min(minTime, time);
			maxTime = Mathf.Max(maxTime, time);
		}

		public void Add(float time, Color value)
		{
			x.AddKey(time, value.r);
			y.AddKey(time, value.g);
			z.AddKey(time, value.b);
			w.AddKey(time, value.a);
			minTime = Mathf.Min(minTime, time);
			maxTime = Mathf.Max(maxTime, time);
		}

		public Vector4 EvaluateVector(float time)
		{
			return new Vector4(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
		}

		public Color EvaluateColor(float time)
		{
			return new Color(x.Evaluate(time), y.Evaluate(time), z.Evaluate(time), w.Evaluate(time));
		}
	}

	public string outputFilename;

	public string inputPath;

	public string fileName;

	public string fileExtension;

	public string suffixDiffuse;

	public string suffixSpecular;

	public string suffixOcclusion;

	public string suffixNormal;

	public void Reset()
	{
		outputFilename = "/Parts/TestPart/Something.png";
		inputPath = "/Parts/";
		fileName = "TestName";
		fileExtension = ".png";
		suffixDiffuse = "_COLOR";
		suffixSpecular = "_SPEC";
		suffixOcclusion = "_OCC";
		suffixNormal = "_NRM";
	}

	[ContextMenu("Convert!")]
	public void Convert()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.LoadImage(File.ReadAllBytes(inputPath + fileName + suffixDiffuse + fileExtension));
		Texture2D texture2D2 = new Texture2D(1, 1);
		texture2D2.LoadImage(File.ReadAllBytes(inputPath + fileName + suffixDiffuse + fileExtension));
		Texture2D texture2D3 = new Texture2D(1, 1);
		texture2D3.LoadImage(File.ReadAllBytes(inputPath + fileName + suffixDiffuse + fileExtension));
		int width = texture2D.width;
		int height = texture2D.height;
		Texture2D texture2D4 = new Texture2D(width, height, TextureFormat.ARGB32, mipChain: false);
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				Color color = texture2D.GetPixel(j, i) * texture2D3.GetPixel(j, i).grayscale;
				color.a = texture2D2.GetPixel(j, i).grayscale;
				texture2D4.SetPixel(j, i, color);
			}
		}
		File.WriteAllBytes(outputFilename, texture2D4.EncodeToPNG());
	}
}
