using System;
using UnityEngine;

[Serializable]
public class InternalDialIncrement : IConfigNode
{
	public Vector2[] increments;

	public float maxAngle => increments[increments.Length - 1].x;

	public float maxValue => increments[increments.Length - 1].y;

	public InternalDialIncrement()
	{
		increments = new Vector2[0];
	}

	public void Load(ConfigNode node)
	{
		string[] values = node.values.GetValues("inc");
		increments = new Vector2[values.Length];
		int i = 0;
		for (int num = values.Length; i < num; i++)
		{
			increments[i] = ConfigNode.ParseVector2(values[i]);
		}
	}

	public void Save(ConfigNode node)
	{
		int i = 0;
		for (int num = increments.Length; i < num; i++)
		{
			node.AddValue("inc", ConfigNode.WriteVector(increments[i]));
		}
	}

	public float CalculateAngle(float vSpeed)
	{
		bool flag = vSpeed < 0f;
		float num = 0f;
		if (flag)
		{
			vSpeed = 0f - vSpeed;
		}
		int num2 = increments.Length;
		while (num2-- > 0)
		{
			if (!(vSpeed <= increments[num2].y))
			{
				num = ((num2 >= increments.Length - 1) ? increments[num2].x : Mathf.Lerp(increments[num2].x, increments[num2 + 1].x, (vSpeed - increments[num2].y) / (increments[num2 + 1].y - increments[num2].y)));
				break;
			}
		}
		if (flag)
		{
			num = 0f - num;
		}
		return num;
	}
}
