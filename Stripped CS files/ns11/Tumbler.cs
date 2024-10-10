using System;
using UnityEngine;

namespace ns11;

public class Tumbler : MonoBehaviour
{
	public enum TumbleDirection
	{
		Up,
		Down
	}

	[Serializable]
	public class TumblerObject
	{
		public Transform transform;

		public double tgtRot;

		public double currRot;

		public double double_0;

		public TumblerObject(Transform transform)
		{
			this.transform = transform;
		}

		public void TumbleTo(double n, TumbleDirection tumble)
		{
			n = Math.Floor(n % 10.0);
			tgtRot = 36.0 * n;
			switch (tumble)
			{
			case TumbleDirection.Down:
				if (n > double_0)
				{
					currRot += 360.0;
				}
				break;
			case TumbleDirection.Up:
				if (n < double_0)
				{
					currRot -= 360.0;
				}
				break;
			}
			double_0 = n;
		}

		public void Update(float deltaTime, float sharpness)
		{
			if ((double)(sharpness * deltaTime) > 1.0)
			{
				currRot = tgtRot;
			}
			else
			{
				currRot += (tgtRot - currRot) * (double)(sharpness * deltaTime);
			}
			transform.localRotation = Quaternion.Euler((float)(0.0 - currRot), 0f, 0f);
		}
	}

	public Transform[] tumblerTransforms;

	public uGUITumblerObject unitTumbler;

	public MeshRenderer[] tumblerRenderers;

	public TumblerObject[] tumblers;

	public float sharpness = 1f;

	public double currValue;

	public int lastUnit;

	public float lastUpdateTime;

	public float maxValue;

	public bool negative;

	public Color negativeColor = new Color(0.6f, 0f, 0f);

	public Color positiveColor = Color.black;

	public double Value => currValue;

	public void Awake()
	{
		currValue = 0.0;
		tumblers = new TumblerObject[tumblerTransforms.Length];
		int i = 0;
		for (int num = tumblerTransforms.Length; i < num; i++)
		{
			tumblers[i] = new TumblerObject(tumblerTransforms[i]);
		}
		maxValue = Mathf.Pow(10f, tumblerTransforms.Length);
		lastUpdateTime = Time.realtimeSinceStartup;
		SetColor(positiveColor);
	}

	public void LateUpdate()
	{
		float deltaTime = Time.realtimeSinceStartup - lastUpdateTime;
		lastUpdateTime = Time.realtimeSinceStartup;
		int num = tumblerTransforms.Length;
		while (num-- > 0)
		{
			tumblers[num].Update(deltaTime, sharpness);
		}
		if (unitTumbler != null)
		{
			unitTumbler.UpdateDelta(deltaTime, sharpness);
		}
	}

	public void SetValue(double val)
	{
		double num = Math.Abs(val);
		if (unitTumbler != null)
		{
			int num2 = 0;
			while (num >= (double)maxValue)
			{
				num /= 1000.0;
				val /= 1000.0;
				num2++;
			}
			unitTumbler.TumbleTo(num2, (num2 <= lastUnit) ? 1 : 0);
			lastUnit = num2;
		}
		if ((int)val >= 0)
		{
			if (negative)
			{
				negative = false;
				SetColor(positiveColor);
			}
		}
		else if (!negative)
		{
			negative = true;
			SetColor(negativeColor);
		}
		int num3 = tumblers.Length;
		while (num3-- > 0)
		{
			tumblers[num3].TumbleTo(num / Math.Pow(10.0, num3), (!(num > Math.Abs(currValue))) ? TumbleDirection.Down : TumbleDirection.Up);
		}
		currValue = val;
	}

	public void SetColor(Color newColor)
	{
		if (tumblerRenderers != null)
		{
			int num = tumblerRenderers.Length;
			while (num-- > 0)
			{
				tumblerRenderers[num].material.SetColor("_Color", newColor);
			}
		}
	}
}
