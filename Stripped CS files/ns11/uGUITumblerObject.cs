using System;
using TMPro;
using UnityEngine;

namespace ns11;

public class uGUITumblerObject : MonoBehaviour
{
	[SerializeField]
	public RectTransform[] Labels;

	[SerializeField]
	public string[] Values;

	public double tgtRot;

	public double currRot;

	public double double_0;

	public void TumbleTo(double n, int tumble)
	{
		n = Math.Floor(n % 10.0);
		tgtRot = 36.0 * n;
		switch (tumble)
		{
		case 1:
			if (n > double_0)
			{
				currRot += 360.0;
			}
			break;
		case 0:
			if (n < double_0)
			{
				currRot -= 360.0;
			}
			break;
		}
		double_0 = n;
	}

	public void UpdateDelta(float deltaTime, float sharpness)
	{
		if ((double)(sharpness * deltaTime) > 1.0)
		{
			currRot = tgtRot;
		}
		else
		{
			currRot += (tgtRot - currRot) * (double)(sharpness * deltaTime);
		}
		int num = Mathf.FloorToInt((float)currRot / 36f);
		double num2 = currRot - (double)((float)num * 36f);
		for (int i = 0; i < Labels.Length; i++)
		{
			Labels[i].anchoredPosition = new Vector2(0f, (float)(0.0 - num2) * 23f / 36f - (float)((i - 2) * 23));
			Labels[i].GetComponent<TextMeshProUGUI>().text = GetValue(num + 2 - i);
		}
	}

	public string GetValue(int index)
	{
		index = ((index < 0) ? ((index % Values.Length != 0) ? (Values.Length + index % Values.Length) : 0) : (index % Values.Length));
		return Values[index];
	}
}
