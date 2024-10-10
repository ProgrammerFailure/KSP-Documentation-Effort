using System;
using System.Collections.Generic;
using UnityEngine;

public class Performance : MonoBehaviour
{
	[SerializeField]
	public int frameTimeCount = 100;

	public float[] frameTimes;

	public int frameTimeIndex;

	public float frameTimeSum;

	public float frameTimeAvg;

	public float frameTimeMin;

	public float frameTimeMax;

	public bool forceGC;

	public bool doCleanup;

	public bool requireCleanup;

	public float unloadTime = 60f;

	public float nextUnloadTime;

	public static Performance Instance { get; set; }

	public int FrameTimeCount => frameTimeCount;

	public float FrameTimeAverage => frameTimeAvg;

	public float FrameTimeMin => frameTimeMin;

	public float FrameTimeMax => frameTimeMax;

	public float FramesPerSecond => 1f / frameTimeAvg;

	public void Awake()
	{
		Instance = this;
		frameTimes = new float[frameTimeCount];
	}

	public void OnDestroy()
	{
		if (Instance != null && Instance == this)
		{
			Instance = null;
		}
	}

	public void Start()
	{
		nextUnloadTime = Time.realtimeSinceStartup + unloadTime;
	}

	public void Update()
	{
		UpdateFrameTimes();
		if (forceGC)
		{
			GC.Collect();
		}
		if (doCleanup && (requireCleanup || nextUnloadTime < Time.realtimeSinceStartup))
		{
			Resources.UnloadUnusedAssets();
			nextUnloadTime = Time.realtimeSinceStartup + unloadTime;
			requireCleanup = false;
		}
	}

	public void UpdateFrameTimes()
	{
		frameTimeSum -= frameTimes[frameTimeIndex];
		frameTimes[frameTimeIndex] = Time.deltaTime;
		frameTimeSum += Time.deltaTime;
		frameTimeAvg = frameTimeSum / (float)frameTimeCount;
		frameTimeIndex++;
		if (frameTimeIndex == frameTimeCount)
		{
			frameTimeIndex = 0;
		}
		frameTimeMin = float.MaxValue;
		frameTimeMax = float.MinValue;
		for (int i = 0; i < frameTimeCount; i++)
		{
			if (frameTimes[i] < frameTimeMin)
			{
				frameTimeMin = frameTimes[i];
			}
			if (frameTimes[i] > frameTimeMax)
			{
				frameTimeMax = frameTimes[i];
			}
		}
	}

	public Vector2[] GetFrameTimePoints()
	{
		float num = 1f / (float)(frameTimeCount - 1);
		Vector2[] array = new Vector2[frameTimeCount];
		int i = 0;
		int num2 = frameTimeIndex;
		for (; i < frameTimeCount; i++)
		{
			array[i] = new Vector2(num * (float)i, frameTimes[num2]);
			num2++;
			if (num2 == frameTimeCount)
			{
				num2 = 0;
			}
		}
		return array;
	}

	public Vector2[] GetFramePerSecondPoints()
	{
		Vector2[] array = new Vector2[frameTimeCount];
		int i = 0;
		int num = frameTimeIndex;
		for (; i < frameTimeCount; i++)
		{
			if (frameTimes[num] == 0f)
			{
				array[i].y = 0f;
			}
			else
			{
				array[i].y = 1f / frameTimes[num];
			}
		}
		GetFramePerSecondPoints(array);
		return array;
	}

	public void GetFramePerSecondPoints(Vector2[] points)
	{
		int i = 0;
		int num = frameTimeIndex;
		for (; i < frameTimeCount; i++)
		{
			if (frameTimes[num] == 0f)
			{
				points[i].y = 0f;
			}
			else
			{
				points[i].y = 1f / frameTimes[num];
			}
			num++;
			if (num == frameTimeCount)
			{
				num = 0;
			}
		}
	}

	public void GetFramePerSecondPoints(List<Vector2> points, float scaleX = 0f, float scaleY = 0f)
	{
		float num = 1f / (float)(frameTimeCount - 1);
		int i = 0;
		int num2 = frameTimeIndex;
		for (; i < frameTimeCount; i++)
		{
			if (frameTimes[num2] == 0f)
			{
				points[i] = new Vector2((float)i * num * scaleX, 0f);
			}
			else
			{
				points[i] = new Vector2((float)i * num * scaleX, Mathf.InverseLerp(frameTimeMin, frameTimeMax, frameTimes[num2]) * scaleY);
			}
			num2++;
			if (num2 == frameTimeCount)
			{
				num2 = 0;
			}
		}
	}

	public void GetFramePerSecondPoints(List<Vector2> points, float scaleX, float scaleY, ref float min, ref float max)
	{
		float num = 1f / (float)(frameTimeCount - 1);
		int i = 0;
		int num2 = frameTimeIndex;
		for (; i < frameTimeCount; i++)
		{
			float num3 = frameTimes[num2];
			if (num3 != 0f)
			{
				num3 = 1f / num3;
				if (num3 < min)
				{
					min = num3;
				}
				if (num3 > max)
				{
					max = num3;
				}
				points[i] = new Vector2((float)i * num * scaleX, Mathf.InverseLerp(min, max, frameTimes[num2]) * scaleY);
			}
			else
			{
				points[i] = new Vector2((float)i * num * scaleX, 0f);
			}
			num2++;
			if (num2 == frameTimeCount)
			{
				num2 = 0;
			}
		}
	}
}
