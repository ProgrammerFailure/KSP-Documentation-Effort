using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class FXCurve
{
	[Serializable]
	public class FXKeyFrame
	{
		public float time;

		public float value;

		public Keyframe Keyframe => new Keyframe(time, value);

		public FXKeyFrame(float time, float value)
		{
			this.time = time;
			this.value = value;
		}
	}

	public float singleValue = -1f;

	public string valueName = "";

	public float lastValue = 1f;

	public bool evalSingle = true;

	public List<FXKeyFrame> keyFrames = new List<FXKeyFrame>();

	public AnimationCurve fCurve;

	public bool fCurveCompiled;

	public static char[] delimiters = new char[4] { ' ', ',', ';', '\t' };

	public FXCurve(string valueName, float value)
	{
		fCurve = null;
		lastValue = value;
		singleValue = value;
		evalSingle = true;
	}

	public void AddKeyframe(float time, float value)
	{
		FXKeyFrame item = new FXKeyFrame(time, value);
		keyFrames.Add(item);
	}

	public void CompileCurve()
	{
		fCurve = new AnimationCurve();
		fCurve.postWrapMode = WrapMode.ClampForever;
		fCurve.preWrapMode = WrapMode.ClampForever;
		int i = 0;
		for (int count = keyFrames.Count; i < count; i++)
		{
			fCurve.AddKey(keyFrames[i].Keyframe);
		}
		fCurveCompiled = true;
	}

	public float Value()
	{
		if (evalSingle)
		{
			return singleValue;
		}
		if (!fCurveCompiled)
		{
			CompileCurve();
		}
		return fCurve.Evaluate(0f);
	}

	public float Value(float time)
	{
		if (evalSingle)
		{
			return singleValue;
		}
		if (!fCurveCompiled)
		{
			CompileCurve();
		}
		return fCurve.Evaluate(time);
	}

	public void Load(string valueName, ConfigNode node)
	{
		this.valueName = valueName;
		string[] values = node.GetValues(valueName);
		if (values.Length == 0)
		{
			fCurve = null;
			evalSingle = true;
			return;
		}
		if (values.Length == 1)
		{
			string[] array = values[0].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			singleValue = float.Parse(array[0]);
			lastValue = singleValue;
			fCurve = null;
			evalSingle = true;
			return;
		}
		if (keyFrames != null)
		{
			keyFrames.Clear();
			fCurve = null;
		}
		int num = values.Length;
		for (int i = 0; i < num; i++)
		{
			string[] array = values[i].Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
			if (array.Length < 2)
			{
				Debug.LogError("FloatCurve: Invalid line. Requires two values, 'time' and 'value'");
			}
			else
			{
				AddKeyframe(float.Parse(array[0]), float.Parse(array[1]));
			}
		}
		evalSingle = false;
		CompileCurve();
	}

	public void Save(ConfigNode node)
	{
		if (evalSingle)
		{
			node.AddValue(valueName, singleValue.ToString());
			return;
		}
		int i = 0;
		for (int count = keyFrames.Count; i < count; i++)
		{
			node.AddValue(valueName, keyFrames[i].time + " " + keyFrames[i].value);
		}
	}

	public static implicit operator float(FXCurve instance)
	{
		return instance.lastValue;
	}
}
