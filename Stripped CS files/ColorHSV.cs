using UnityEngine;

public class ColorHSV
{
	public float h;

	public float s;

	public float v;

	public float a;

	public ColorHSV(float h, float s, float v, float a)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		this.a = a;
	}

	public ColorHSV(float h, float s, float v)
	{
		this.h = h;
		this.s = s;
		this.v = v;
		a = 1f;
	}

	public ColorHSV(Color color)
	{
		h = 1f;
		s = 1f;
		v = 1f;
		a = 1f;
		FromColor(color);
	}

	public void FromColor(Color color)
	{
		float num;
		float num2;
		float num3;
		float num4;
		if (color.r > color.g && color.r > color.b)
		{
			num = 0f;
			num2 = color.r;
			num3 = color.g;
			num4 = color.b;
		}
		else if (color.g > color.b)
		{
			num = 2f;
			num2 = color.g;
			num3 = color.b;
			num4 = color.r;
		}
		else
		{
			num = 4f;
			num2 = color.b;
			num3 = color.r;
			num4 = color.g;
		}
		if (num2 != 0f)
		{
			float num5 = ((num4 < num3) ? num4 : num3);
			float num6 = num2 - num5;
			if (num6 != 0f)
			{
				h = num + (num3 - num4) / num6;
				s = num6 / num2;
			}
			else
			{
				h = num + num3 - num4;
				s = 0f;
			}
			h /= 6f;
			if (h < 0f)
			{
				h += 1f;
			}
		}
		else
		{
			h = 0f;
			s = 0f;
		}
		v = num2;
		a = color.a;
	}

	public Color ToColor()
	{
		if (s == 0f)
		{
			return new Color(v, v, v, a);
		}
		if (v == 0f)
		{
			return Color.black;
		}
		float num = h * 6f;
		int num2 = Mathf.FloorToInt(num);
		float num3 = num - (float)num2;
		float num4 = v * (1f - s);
		float num5 = v * (1f - s * num3);
		float num6 = v * (1f - s * (1f - num3));
		return num2 switch
		{
			-1 => new Color(v, num4, num5, a), 
			0 => new Color(v, num6, num4, a), 
			1 => new Color(num5, v, num4, a), 
			2 => new Color(num4, v, num6, a), 
			3 => new Color(num4, num5, v, a), 
			4 => new Color(num6, num4, v, a), 
			5 => new Color(v, num4, num5, a), 
			6 => new Color(v, num6, num4, a), 
			_ => Color.black, 
		};
	}
}
