using UnityEngine;

namespace ns11;

public class SpriteTumblerObject : MonoBehaviour
{
	public delegate void afterAltModeTumbleTo(double n, int tumble);

	public delegate void afterAltModeUpdateDelta(float deltaTime, float sharpness);

	public RectTransform[] images;

	public Sprite[] sprites;

	public RectTransform colorBar;

	public double tgtRot;

	public double currRot = 36.0;

	public double double_0;

	public bool tumbling;

	public static afterAltModeTumbleTo AfterAltTumbleTo;

	public static afterAltModeUpdateDelta AfterAltModeUpdateDelta;

	[SerializeField]
	public double delta;

	[SerializeField]
	public int index;

	[SerializeField]
	public int spriteIndexOffset;

	[SerializeField]
	public float colorBarMinY = 9f;

	[SerializeField]
	public float colorBarMaxY = 35f;

	public float colorBarTarget;

	public float colorBarCurrent;

	public float colorBarSection = 28f;

	public float IconSection = 36f;

	public void Awake()
	{
		double_0 = 0.0;
		tgtRot = 0.0;
		currRot = tgtRot;
		colorBarCurrent = 33f;
		for (int i = 0; i < images.Length; i++)
		{
			images[i].anchoredPosition = new Vector2(0f, (float)double_0 - (float)((i - spriteIndexOffset) * 23));
		}
		if (colorBar != null)
		{
			colorBar.anchoredPosition = new Vector2(colorBar.anchoredPosition.x, Mathf.Clamp(colorBarCurrent, colorBarMinY, colorBarMaxY));
		}
	}

	public void TumbleTo(double n, int tumble)
	{
		if (n != double_0)
		{
			tgtRot = (double)IconSection * n;
			colorBarTarget = colorBarSection * (float)n * 2f - (float)n;
			double_0 = n;
			tumbling = true;
			if (AfterAltTumbleTo != null)
			{
				AfterAltTumbleTo(n, tumble);
			}
		}
	}

	public void UpdateDelta(float deltaTime, float sharpness)
	{
		if ((double)(sharpness * deltaTime) > 1.0)
		{
			currRot = tgtRot;
			colorBarCurrent = colorBarTarget;
		}
		else
		{
			currRot += (tgtRot - currRot) * (double)(sharpness * deltaTime);
			colorBarCurrent += (colorBarTarget - colorBarCurrent) * (sharpness * deltaTime);
		}
		index = Mathf.FloorToInt((float)currRot / IconSection);
		delta = currRot - (double)((float)index * IconSection);
		if (tumbling)
		{
			for (int i = 0; i < images.Length; i++)
			{
				images[i].anchoredPosition = new Vector2(0f, (float)delta * 23f / IconSection - (float)((i - spriteIndexOffset) * 23) + (float)index * 23f);
			}
		}
		if (colorBar != null)
		{
			colorBar.anchoredPosition = new Vector2(colorBar.anchoredPosition.x, Mathf.Clamp(colorBarCurrent, colorBarMinY, colorBarMaxY));
		}
		if (AfterAltModeUpdateDelta != null)
		{
			AfterAltModeUpdateDelta(deltaTime, sharpness);
		}
		if (delta > (double)(IconSection - 0.01f) || delta < 0.009999999776482582)
		{
			tumbling = false;
		}
	}
}
