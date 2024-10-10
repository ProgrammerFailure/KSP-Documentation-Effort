using System;
using System.IO;
using UnityEngine;

namespace EdyCommonTools;

public class TextureCanvas
{
	public enum LineType
	{
		Solid,
		Dotted,
		Dashed
	}

	public delegate Color ContourPlotEvaluateColor(float x, float y);

	public Texture2D m_texture;

	public int m_pixelsWd;

	public int m_pixelsHt;

	public bool m_dirty;

	public Color32[] m_pixels;

	public Color32[] m_buffer;

	public Rect m_canvasRect;

	public float m_scaleX;

	public float m_scaleY;

	public Rect m_clipArea;

	public int m_pixelsXMin;

	public int m_pixelsXMax;

	public int m_pixelsYMin;

	public int m_pixelsYMax;

	public Color32 m_pixelColor;

	public Color m_color = Color.white;

	public float m_alpha = 1f;

	public bool m_alphaBlend;

	public float m_srcBlend = 1f;

	public float m_dstBlend;

	public float m_moveX;

	public float m_moveY;

	public int m_step;

	public Rect rect
	{
		get
		{
			return m_canvasRect;
		}
		set
		{
			m_canvasRect = value;
			m_scaleX = (float)m_pixelsWd / m_canvasRect.width;
			m_scaleY = (float)m_pixelsHt / m_canvasRect.height;
			m_clipArea = value;
			m_pixelsXMin = 0;
			m_pixelsYMin = 0;
			m_pixelsXMax = m_pixelsWd;
			m_pixelsYMax = m_pixelsHt;
		}
	}

	public int width => m_pixelsWd;

	public int height => m_pixelsHt;

	public Rect clipArea
	{
		get
		{
			return m_clipArea;
		}
		set
		{
			m_clipArea.xMin = Mathf.Max(value.xMin, m_canvasRect.xMin);
			m_clipArea.xMax = Mathf.Min(value.xMax, m_canvasRect.xMax);
			m_clipArea.yMin = Mathf.Max(value.yMin, m_canvasRect.yMin);
			m_clipArea.yMax = Mathf.Min(value.yMax, m_canvasRect.yMax);
			m_pixelsXMin = GetPixelX(m_clipArea.xMin);
			m_pixelsXMax = GetPixelX(m_clipArea.xMax);
			m_pixelsYMin = GetPixelY(m_clipArea.yMin);
			m_pixelsYMax = GetPixelY(m_clipArea.yMax);
		}
	}

	public Color color
	{
		get
		{
			return m_color;
		}
		set
		{
			m_color = value;
			SetupPixelColor();
		}
	}

	public float alpha
	{
		get
		{
			return m_alpha;
		}
		set
		{
			m_alpha = value;
			SetupPixelColor();
		}
	}

	public bool alphaBlend
	{
		get
		{
			return m_alphaBlend;
		}
		set
		{
			m_alphaBlend = value;
			SetupPixelColor();
		}
	}

	public LineType lineType { get; set; }

	public int dotInterval { get; set; }

	public int dashInterval { get; set; }

	public int functionResolution { get; set; }

	public Texture2D texture
	{
		get
		{
			ApplyChanges();
			return m_texture;
		}
	}

	public TextureCanvas(int pixelsWd, int pixelsHt, Rect canvasRect)
	{
		CreateCanvas(pixelsWd, pixelsHt);
		rect = canvasRect;
		dotInterval = 5;
		dashInterval = 5;
		functionResolution = 3;
		SetupPixelColor();
	}

	public TextureCanvas(int pixelsWd, int pixelsHt, float canvasWd, float canvasHt)
		: this(pixelsWd, pixelsHt, new Rect(0f, 0f, canvasWd, canvasHt))
	{
	}

	public TextureCanvas(int pixelsWd, int pixelsHt)
		: this(pixelsWd, pixelsHt, new Rect(0f, 0f, 1f, 1f))
	{
	}

	public TextureCanvas()
		: this(1, 1)
	{
	}

	[Obsolete("TextureCanvas(pixelsWd, pixelsHt, canvasX0, canvasY0, canvasWd, canvasHt) is obsolete. Use TextureCanvas(pixelsWd, pixelsHt, rect) instead")]
	public TextureCanvas(int pixelsWd, int pixelsHt, float canvasX0, float canvasY0, float canvasWd, float canvasHt)
		: this(pixelsWd, pixelsHt, new Rect(canvasX0, canvasY0, canvasWd, canvasHt))
	{
	}

	public void ResizeTexture(int pixelsWd, int pixelsHt)
	{
		UnityEngine.Object.DestroyImmediate(m_texture);
		CreateCanvas(pixelsWd, pixelsHt);
		rect = m_canvasRect;
	}

	public void DestroyTexture()
	{
		UnityEngine.Object.DestroyImmediate(m_texture);
	}

	public float Pixels2CanvasX(int pixels)
	{
		return ((float)pixels + 0.5f) / m_scaleX;
	}

	public float Pixels2CanvasY(int pixels)
	{
		return ((float)pixels + 0.5f) / m_scaleY;
	}

	public int GetPixelX(float x)
	{
		return Mathf.RoundToInt((x - m_canvasRect.xMin) * m_scaleX);
	}

	public int GetPixelY(float y)
	{
		return Mathf.RoundToInt((y - m_canvasRect.yMin) * m_scaleY);
	}

	public int GetPixelWidth(float width)
	{
		return Mathf.RoundToInt(width * m_scaleX);
	}

	public int GetPixelHeight(float height)
	{
		return Mathf.RoundToInt(height * m_scaleY);
	}

	public void MoveTo(float x0, float y0)
	{
		if (!float.IsNaN(x0) && !float.IsNaN(y0))
		{
			m_moveX = x0;
			m_moveY = y0;
			m_step = 0;
		}
	}

	public void LineTo(float x1, float y1)
	{
		if (float.IsNaN(x1) || float.IsNaN(y1))
		{
			return;
		}
		float num = m_moveX;
		float num2 = m_moveY;
		m_moveX = x1;
		m_moveY = y1;
		if (num > x1)
		{
			float num3 = num;
			num = x1;
			x1 = num3;
			float num4 = num2;
			num2 = y1;
			y1 = num4;
		}
		float num5 = (y1 - num2) / (x1 - num);
		if (num < m_clipArea.xMin)
		{
			num2 += (m_clipArea.xMin - num) * num5;
			num = m_clipArea.xMin;
		}
		if (x1 > m_clipArea.xMax)
		{
			y1 += (m_clipArea.xMax - x1) * num5;
			x1 = m_clipArea.xMax;
		}
		if (!(num > m_clipArea.xMax) && !(x1 < m_clipArea.xMin) && (!(num2 < m_clipArea.yMin) || !(y1 < m_clipArea.yMin)) && (!(num2 > m_clipArea.yMax) || y1 <= m_clipArea.yMax))
		{
			if (num2 < m_clipArea.yMin)
			{
				num += (m_clipArea.yMin - num2) / num5;
				num2 = m_clipArea.yMin;
			}
			if (num2 > m_clipArea.yMax)
			{
				num += (m_clipArea.yMax - num2) / num5;
				num2 = m_clipArea.yMax;
			}
			if (y1 < m_clipArea.yMin)
			{
				x1 += (m_clipArea.yMin - y1) / num5;
				y1 = m_clipArea.yMin;
			}
			if (y1 > m_clipArea.yMax)
			{
				x1 += (m_clipArea.yMax - y1) / num5;
				y1 = m_clipArea.yMax;
			}
			TexLine(GetPixelX(num), GetPixelY(num2), GetPixelX(x1), GetPixelY(y1));
			m_dirty = true;
		}
	}

	public void Line(float x0, float y0, float x1, float y1)
	{
		MoveTo(x0, y0);
		LineTo(x1, y1);
	}

	public void HorizontalLine(float y)
	{
		if (!float.IsNaN(y))
		{
			m_step = 0;
			TexSegmentH(m_pixelsXMin, m_pixelsXMax, GetPixelY(y));
			m_dirty = true;
		}
	}

	public void VerticalLine(float x)
	{
		if (!float.IsNaN(x))
		{
			m_step = 0;
			TexSegmentV(GetPixelX(x), m_pixelsYMin, m_pixelsYMax);
			m_dirty = true;
		}
	}

	public void Circumference(float x, float y, float radius)
	{
		m_step = 0;
		int pixelWidth = GetPixelWidth(radius);
		TexEllipse(GetPixelX(x), GetPixelY(y), pixelWidth, pixelWidth);
		m_dirty = true;
	}

	public void Circle(float x, float y, float radius)
	{
		m_step = 0;
		int pixelWidth = GetPixelWidth(radius);
		TexFillEllipse(GetPixelX(x), GetPixelY(y), pixelWidth, pixelWidth);
		m_dirty = true;
	}

	public void Ellipse(float x, float y, float rx, float ry)
	{
		m_step = 0;
		TexEllipse(GetPixelX(x), GetPixelY(y), GetPixelWidth(rx), GetPixelHeight(ry));
		m_dirty = true;
	}

	public void FillEllipse(float x, float y, float rx, float ry)
	{
		m_step = 0;
		TexFillEllipse(GetPixelX(x), GetPixelY(y), GetPixelWidth(rx), GetPixelHeight(ry));
		m_dirty = true;
	}

	public void Clear()
	{
		int i = 0;
		for (int num = m_pixels.Length; i < num; i++)
		{
			m_pixels[i] = m_pixelColor;
		}
		m_dirty = true;
	}

	public void Grid(float stepX, float stepY)
	{
		if (stepX < Pixels2CanvasX(2))
		{
			stepX = Pixels2CanvasX(2);
		}
		if (stepY < Pixels2CanvasY(2))
		{
			stepY = Pixels2CanvasY(2);
		}
		float num = (float)(int)(m_canvasRect.x / stepX) * stepX;
		float num2 = (float)(int)(m_canvasRect.y / stepY) * stepY;
		for (float num3 = num; num3 <= m_canvasRect.xMax; num3 += stepX)
		{
			VerticalLine(num3);
		}
		for (float num3 = num2; num3 <= m_canvasRect.yMax; num3 += stepY)
		{
			HorizontalLine(num3);
		}
	}

	public void HorizontalGrid(float stepY)
	{
		if (stepY < Pixels2CanvasY(2))
		{
			stepY = Pixels2CanvasY(2);
		}
		for (float num = (float)(int)(m_canvasRect.y / stepY) * stepY; num <= m_canvasRect.yMax; num += stepY)
		{
			HorizontalLine(num);
		}
	}

	public void VerticalGrid(float stepX)
	{
		if (stepX < Pixels2CanvasX(2))
		{
			stepX = Pixels2CanvasX(2);
		}
		for (float num = (float)(int)(m_canvasRect.x / stepX) * stepX; num <= m_canvasRect.xMax; num += stepX)
		{
			VerticalLine(num);
		}
	}

	public void DotGrid(float stepX, float stepY)
	{
		if (stepX < Pixels2CanvasX(2))
		{
			stepX = Pixels2CanvasX(2);
		}
		if (stepY < Pixels2CanvasY(2))
		{
			stepY = Pixels2CanvasY(2);
		}
		float num = (float)(int)(m_canvasRect.x / stepX) * stepX;
		for (float num2 = (float)(int)(m_canvasRect.y / stepY) * stepY; num2 <= m_canvasRect.yMax; num2 += stepY)
		{
			for (float num3 = num; num3 <= m_canvasRect.xMax; num3 += stepX)
			{
				Dot(num3, num2);
			}
		}
	}

	public void Pixel(float x, float y)
	{
		if (!float.IsNaN(x) && !float.IsNaN(y))
		{
			TexPixel(GetPixelX(x), GetPixelY(y));
			m_dirty = true;
		}
	}

	public void Dot(float x, float y)
	{
		if (!float.IsNaN(x) && !float.IsNaN(y))
		{
			int pixelX = GetPixelX(x);
			int pixelY = GetPixelY(y);
			TexPixel(pixelX, pixelY - 1);
			TexPixel(pixelX - 1, pixelY);
			TexPixel(pixelX, pixelY);
			TexPixel(pixelX + 1, pixelY);
			TexPixel(pixelX, pixelY + 1);
			m_dirty = true;
		}
	}

	public void Cross(float x, float y, int radiusX = 5, int radiusY = 5)
	{
		if (!float.IsNaN(x) && !float.IsNaN(y))
		{
			int pixelX = GetPixelX(x);
			int pixelY = GetPixelY(y);
			for (int i = pixelX - radiusX; i <= pixelX + radiusX; i++)
			{
				TexPixel(i, pixelY);
			}
			for (int j = pixelY - radiusY; j <= pixelY + radiusY; j++)
			{
				TexPixel(pixelX, j);
			}
			m_dirty = true;
		}
	}

	public void Rectangle(float x, float y, float width, float height)
	{
		MoveTo(x, y);
		LineTo(x + width, y);
		LineTo(x + width, y + height);
		LineTo(x, y + height);
		LineTo(x, y);
	}

	public void FillRect(float x, float y, float width, float height)
	{
		if (!float.IsNaN(x) && !float.IsNaN(y) && !float.IsNaN(width) && !float.IsNaN(height))
		{
			int pixelX = GetPixelX(x);
			int num = GetPixelY(y);
			int pixelX2 = GetPixelX(x + width);
			int num2 = GetPixelY(y + height);
			if (num2 < num)
			{
				int num3 = num;
				num = num2;
				num2 = num3;
			}
			for (int i = num; i <= num2; i++)
			{
				m_step = 0;
				TexSegmentH(pixelX, pixelX2, i);
			}
			m_dirty = true;
		}
	}

	public void Function(Func<float, float> func, float x0, float x1, float stepSize = -1f)
	{
		if (stepSize <= 0f)
		{
			if (functionResolution < 1)
			{
				functionResolution = 1;
			}
			stepSize = Pixels2CanvasX(functionResolution);
		}
		else
		{
			float num = Pixels2CanvasX(1);
			if (stepSize < num)
			{
				stepSize = num;
			}
		}
		MoveTo(x0, func(x0));
		float num2;
		for (num2 = x0; num2 <= x1; num2 += stepSize)
		{
			LineTo(num2, func(num2));
		}
		if (num2 < x1)
		{
			LineTo(x1, func(x1));
		}
	}

	public void Function(Func<float, float> func, float stepSize = -1f)
	{
		Function(func, m_canvasRect.xMin, m_canvasRect.xMax, stepSize);
	}

	public void SolidFunction(Func<float, float> func, float x0, float x1)
	{
		int pixelX = GetPixelX(x0);
		int num = GetPixelX(x1) - pixelX;
		if (num > 0)
		{
			int pixelY = GetPixelY(0f);
			float num2 = (x1 - x0) / (float)(num + 1);
			for (int i = 0; i < num; i++)
			{
				m_step = 0;
				TexSegmentV(pixelX + i, pixelY, GetPixelY(func(x0)));
				x0 += num2;
			}
			m_dirty = true;
		}
	}

	public void SolidFunction(Func<float, float> func)
	{
		SolidFunction(func, m_canvasRect.xMin, m_canvasRect.xMax);
	}

	public void ContourPlot(ContourPlotEvaluateColor evaluateColorFn)
	{
		for (int i = m_pixelsYMin; i < m_pixelsYMax; i++)
		{
			float y = m_canvasRect.yMin + Pixels2CanvasY(i);
			for (int j = m_pixelsXMin; j < m_pixelsXMax; j++)
			{
				float x = m_canvasRect.xMin + Pixels2CanvasX(j);
				color = evaluateColorFn(x, y);
				TexPixel(j, i);
			}
		}
		m_dirty = true;
	}

	public void HorizontalAction(Action<float> action, float interval, float offset = 0f)
	{
		if (interval < Pixels2CanvasX(2))
		{
			interval = Pixels2CanvasX(2);
		}
		for (float num = offset + (float)(int)(m_canvasRect.x / interval) * interval; num <= m_canvasRect.xMax; num += interval)
		{
			action(num);
		}
	}

	public void VerticalAction(Action<float> action, float interval, float offset = 0f)
	{
		if (interval < Pixels2CanvasY(2))
		{
			interval = Pixels2CanvasY(2);
		}
		for (float num = offset + (float)(int)(m_canvasRect.y / interval) * interval; num <= m_canvasRect.yMax; num += interval)
		{
			action(num);
		}
	}

	public void Save()
	{
		if (m_buffer == null)
		{
			m_buffer = m_pixels.Clone() as Color32[];
		}
		else
		{
			m_pixels.CopyTo(m_buffer, 0);
		}
	}

	public void Restore()
	{
		if (m_buffer != null)
		{
			m_buffer.CopyTo(m_pixels, 0);
			m_dirty = true;
		}
	}

	public void GUIDraw(int x, int y)
	{
		ApplyChanges();
		GUI.DrawTexture(new Rect(x, y, m_pixelsWd, m_pixelsHt), m_texture);
	}

	public void GUIStretchDraw(int x, int y, int width, int height)
	{
		ApplyChanges();
		GUI.DrawTexture(new Rect(x, y, width, height), m_texture);
	}

	public void GUIStretchDraw(int x, int y, int width)
	{
		ApplyChanges();
		float num = (float)m_pixelsHt / (float)m_pixelsWd;
		GUI.DrawTexture(new Rect(x, y, width, (float)width * num), m_texture);
	}

	public void SaveToPNG(string fileNameAndPath)
	{
		ApplyChanges();
		byte[] bytes = m_texture.EncodeToPNG();
		File.WriteAllBytes(fileNameAndPath, bytes);
	}

	[Obsolete("CanvasWidth() is obsolete. Use TextureCanvas.rect.width instead")]
	public float CanvasWidth()
	{
		return rect.width;
	}

	[Obsolete("CanvasHeight() is obsolete. Use TextureCanvas.rect.height instead")]
	public float CanvasHeight()
	{
		return rect.height;
	}

	[Obsolete("PixelsWidth() is obsolete. Use TextureCanvas.width instead")]
	public int PixelsWidth()
	{
		return width;
	}

	[Obsolete("SetAlpha(alpha) is obsolete. Use TextureCanvas.alpha instead")]
	public void SetAlpha(float value)
	{
		alpha = value;
	}

	[Obsolete("SetAlphaBlend(alphaBlend) is obsolete. Use TextureCanvas.alphaBlend instead")]
	public void SetAlphaBlend(bool value)
	{
		alphaBlend = value;
	}

	[Obsolete("SetFunctionResolution(value) is obsolete. Use TextureCanvas.functionResolution instead")]
	public void SetFunctionResolution(int value)
	{
		functionResolution = value;
	}

	[Obsolete("Clear(color) is obsolete. Use TextureCanvas.color and TextureCanvas.Clear() instead")]
	public void Clear(Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Clear();
		this.color = color;
	}

	[Obsolete("Grid(stepX, stepY, color) is obsolete. Use TextureCanvas.color and TextureCanvas.Grid(stepX, stepY) instead")]
	public void Grid(float stepX, float stepY, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Grid(stepX, stepY);
		this.color = color;
	}

	[Obsolete("LineV(x, color) is obsolete. Use TextureCanvas.color and TextureCanvas.VerticalLine(x) instead")]
	public void LineV(float x, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		VerticalLine(x);
		this.color = color;
	}

	[Obsolete("LineH(y, color) is obsolete. Use TextureCanvas.color and TextureCanvas.HorizontalLine(y) instead")]
	public void LineH(float y, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		HorizontalLine(y);
		this.color = color;
	}

	[Obsolete("DashedLineV(x, step, color) is obsolete. Use TextureCanvas.lineType, TextureCanvas.color, TextureCanvas.dashInterval and TextureCanvas.VerticalLine(x) instead")]
	public void DashedLineV(float x, int step, Color col)
	{
		LineType lineType = this.lineType;
		Color32 color = this.color;
		int num = dashInterval;
		this.lineType = LineType.Dashed;
		dashInterval = step;
		this.color = col;
		VerticalLine(x);
		this.lineType = lineType;
		this.color = color;
		dashInterval = num;
	}

	[Obsolete("DottedLineV(x, step, color) is obsolete. Use TextureCanvas.lineType, TextureCanvas.color, TextureCanvas.dotInterval and TextureCanvas.VerticalLine(x) instead")]
	public void DottedLineV(float x, int step, Color col)
	{
		LineType lineType = this.lineType;
		Color32 color = this.color;
		int num = dotInterval;
		this.lineType = LineType.Dotted;
		dotInterval = step;
		this.color = col;
		VerticalLine(x);
		this.lineType = lineType;
		this.color = color;
		dashInterval = num;
	}

	[Obsolete("DottedGrid(stepX, stepY, dotStep, color) is obsolete. Use TextureCanvas.lineType, TextureCanvas.color, TextureCanvas.dotInterval and TextureCanvas.Grid(stepX, stepY) instead")]
	public void DottedGrid(float stepX, float stepY, int step, Color col)
	{
		LineType lineType = this.lineType;
		Color32 color = this.color;
		int num = dotInterval;
		this.lineType = LineType.Dotted;
		dotInterval = step;
		this.color = col;
		Grid(stepX, stepY);
		this.lineType = lineType;
		this.color = color;
		dashInterval = num;
	}

	[Obsolete("Dot(x, y, color) is obsolete. Use TextureCanvas.color and TextureCanvas.Dot(x, y) instead")]
	public void Dot(float x, float y, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Dot(x, y);
		this.color = color;
	}

	[Obsolete("Circumference(x, y, radius, color) is obsolete. Use TextureCanvas.color and TextureCanvas.Circumference(x, y, radius) instead")]
	public void Circumference(float x, float y, float radius, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Circumference(x, y, radius);
		this.color = color;
	}

	[Obsolete("Rect(x, y, width, height, color) is obsolete. Use TextureCanvas.color and TextureCanvas.Rectangle(x, y, width, height) instead")]
	public void Rect(float x, float y, float width, float height, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Rectangle(x, y, width, height);
		this.color = color;
	}

	[Obsolete("FillRect(x, y, width, height, color) is obsolete. Use TextureCanvas.color and TextureCanvas.FillRect(x, y, width, height) instead")]
	public void FillRect(float x, float y, float width, float height, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		FillRect(x, y, width, height);
		this.color = color;
	}

	[Obsolete("Function(func, color) is obsolete. Use TextureCanvas.color and TextureCanvas.Function(func) instead")]
	public void Function(Func<float, float> func, Color col)
	{
		Color32 color = this.color;
		this.color = col;
		Function(func);
		this.color = color;
	}

	[Obsolete("Function(func, x0, x1, color, stepSize) is obsolete. Use TextureCanvas.color and TextureCanvas.Function(func, x0, x1, stepSize) instead")]
	public void Function(Func<float, float> func, float x0, float x1, Color col, float stepSize)
	{
		Color32 color = this.color;
		this.color = col;
		Function(func, x0, x1, stepSize);
		this.color = color;
	}

	public void ApplyChanges()
	{
		if (m_dirty)
		{
			m_texture.SetPixels32(m_pixels);
			m_texture.Apply(updateMipmaps: false);
			m_dirty = false;
		}
	}

	public void CreateCanvas(int pixelsWd, int pixelsHt)
	{
		if (int.Parse(Application.unityVersion.Split('.')[0]) >= 2018)
		{
			m_texture = new Texture2D(pixelsWd, pixelsHt, TextureFormat.ARGB32, mipChain: false, linear: false);
		}
		else
		{
			m_texture = new Texture2D(pixelsWd, pixelsHt, TextureFormat.ARGB32, mipChain: false, linear: true);
		}
		m_texture.hideFlags = HideFlags.HideAndDontSave;
		m_pixelsWd = pixelsWd;
		m_pixelsHt = pixelsHt;
		m_pixels = new Color32[pixelsWd * pixelsHt];
		m_buffer = null;
	}

	public void SetupPixelColor()
	{
		m_pixelColor = m_color;
		m_pixelColor.a = (byte)(Mathf.Clamp01(m_alpha) * 255f);
		m_srcBlend = m_color.a;
		m_dstBlend = 1f - m_srcBlend;
	}

	public Color32 GetAlphaBlendedPixel(Color32 dst)
	{
		Color32 result = default(Color32);
		result.r = (byte)((float)(int)m_pixelColor.r * m_srcBlend + (float)(int)dst.r * m_dstBlend);
		result.g = (byte)((float)(int)m_pixelColor.g * m_srcBlend + (float)(int)dst.g * m_dstBlend);
		result.b = (byte)((float)(int)m_pixelColor.b * m_srcBlend + (float)(int)dst.b * m_dstBlend);
		result.a = m_pixelColor.a;
		return result;
	}

	public bool CheckForPixel()
	{
		if (lineType == LineType.Solid)
		{
			return true;
		}
		if (lineType == LineType.Dotted)
		{
			return m_step++ % dotInterval == 0;
		}
		if (lineType == LineType.Dashed)
		{
			int num = dashInterval;
			return m_step++ % (num * 2) < num;
		}
		return true;
	}

	public void TexPixel(int x, int y)
	{
		if (x >= m_pixelsXMin && x < m_pixelsXMax && y >= m_pixelsYMin && y < m_pixelsYMax)
		{
			int num = y * m_pixelsWd + x;
			m_pixels[num] = (alphaBlend ? GetAlphaBlendedPixel(m_pixels[num]) : m_pixelColor);
		}
	}

	public void TexLine(int x0, int y0, int x1, int y1)
	{
		int num = y1 - y0;
		int num2 = x1 - x0;
		if (num2 == 0)
		{
			TexSegmentV(x0, y0, y1);
			return;
		}
		if (num == 0)
		{
			TexSegmentH(x0, x1, y0);
			return;
		}
		int num3;
		if (num < 0)
		{
			num = -num;
			num3 = -1;
		}
		else
		{
			num3 = 1;
		}
		int num4;
		if (num2 < 0)
		{
			num2 = -num2;
			num4 = -1;
		}
		else
		{
			num4 = 1;
		}
		num <<= 1;
		num2 <<= 1;
		if (CheckForPixel())
		{
			TexPixel(x0, y0);
		}
		if (num2 > num)
		{
			int num5 = num - (num2 >> 1);
			while (x0 != x1)
			{
				if (num5 >= 0)
				{
					y0 += num3;
					num5 -= num2;
				}
				x0 += num4;
				num5 += num;
				if (CheckForPixel())
				{
					TexPixel(x0, y0);
				}
			}
			return;
		}
		int num6 = num2 - (num >> 1);
		while (y0 != y1)
		{
			if (num6 >= 0)
			{
				x0 += num4;
				num6 -= num;
			}
			y0 += num3;
			num6 += num2;
			if (CheckForPixel())
			{
				TexPixel(x0, y0);
			}
		}
	}

	public void TexSegmentV(int x, int y0, int y1)
	{
		if (y0 > y1)
		{
			int num = y0;
			y0 = y1;
			y1 = num;
		}
		if (x < m_pixelsXMin || x >= m_pixelsXMax || y1 < m_pixelsYMin || y0 >= m_pixelsYMax)
		{
			return;
		}
		if (y0 < m_pixelsYMin)
		{
			y0 = m_pixelsYMin;
		}
		if (y1 >= m_pixelsYMax)
		{
			y1 = m_pixelsYMax;
		}
		int num2 = y0 * m_pixelsWd + x;
		if (!alphaBlend)
		{
			if (lineType == LineType.Solid)
			{
				for (int i = y0; i < y1; i++)
				{
					m_pixels[num2] = m_pixelColor;
					num2 += m_pixelsWd;
				}
				return;
			}
			for (int j = y0; j < y1; j++)
			{
				if (CheckForPixel())
				{
					m_pixels[num2] = m_pixelColor;
				}
				num2 += m_pixelsWd;
			}
			return;
		}
		if (lineType == LineType.Solid)
		{
			for (int k = y0; k < y1; k++)
			{
				m_pixels[num2] = GetAlphaBlendedPixel(m_pixels[num2]);
				num2 += m_pixelsWd;
			}
			return;
		}
		for (int l = y0; l < y1; l++)
		{
			if (CheckForPixel())
			{
				m_pixels[num2] = GetAlphaBlendedPixel(m_pixels[num2]);
			}
			num2 += m_pixelsWd;
		}
	}

	public void TexSegmentH(int x0, int x1, int y)
	{
		if (x0 > x1)
		{
			int num = x0;
			x0 = x1;
			x1 = num;
		}
		if (y < m_pixelsYMin || y >= m_pixelsYMax || x1 < m_pixelsXMin || x0 >= m_pixelsXMax)
		{
			return;
		}
		if (x0 < m_pixelsXMin)
		{
			x0 = m_pixelsXMin;
		}
		if (x1 > m_pixelsXMax)
		{
			x1 = m_pixelsXMax;
		}
		int num2 = y * m_pixelsWd + x0;
		if (!alphaBlend)
		{
			if (lineType == LineType.Solid)
			{
				for (int i = x0; i < x1; i++)
				{
					m_pixels[num2++] = m_pixelColor;
				}
				return;
			}
			for (int j = x0; j < x1; j++)
			{
				if (CheckForPixel())
				{
					m_pixels[num2] = m_pixelColor;
				}
				num2++;
			}
			return;
		}
		if (lineType == LineType.Solid)
		{
			for (int k = x0; k < x1; k++)
			{
				m_pixels[num2] = GetAlphaBlendedPixel(m_pixels[num2]);
				num2++;
			}
			return;
		}
		for (int l = x0; l < x1; l++)
		{
			if (CheckForPixel())
			{
				m_pixels[num2] = GetAlphaBlendedPixel(m_pixels[num2]);
			}
			num2++;
		}
	}

	public void TexEllipse(int cx, int cy, int rx, int ry)
	{
		if (rx >= ry)
		{
			int num = rx;
			int num2 = -rx;
			int num3 = (int)Mathf.Ceil((float)rx / Mathf.Sqrt(2f));
			float num4 = (float)ry / (float)rx;
			for (int i = 0; i <= num3; i++)
			{
				TexPixel(cx + i, (int)((float)cy + (float)num * num4));
				TexPixel(cx + i, (int)((float)cy - (float)num * num4));
				TexPixel(cx - i, (int)((float)cy + (float)num * num4));
				TexPixel(cx - i, (int)((float)cy - (float)num * num4));
				TexPixel(cx + num, (int)((float)cy + (float)i * num4));
				TexPixel(cx - num, (int)((float)cy + (float)i * num4));
				TexPixel(cx + num, (int)((float)cy - (float)i * num4));
				TexPixel(cx - num, (int)((float)cy - (float)i * num4));
				num2 += 2 * i + 1;
				if (num2 > 0)
				{
					num2 += 2 - 2 * num--;
				}
			}
			return;
		}
		int num5 = ry;
		int num6 = -ry;
		int num7 = (int)Mathf.Ceil((float)ry / Mathf.Sqrt(2f));
		float num8 = (float)rx / (float)ry;
		for (int j = 0; j <= num7; j++)
		{
			TexPixel((int)((float)cx + (float)j * num8), cy + num5);
			TexPixel((int)((float)cx + (float)j * num8), cy - num5);
			TexPixel((int)((float)cx - (float)j * num8), cy + num5);
			TexPixel((int)((float)cx - (float)j * num8), cy - num5);
			TexPixel((int)((float)cx + (float)num5 * num8), cy + j);
			TexPixel((int)((float)cx - (float)num5 * num8), cy + j);
			TexPixel((int)((float)cx + (float)num5 * num8), cy - j);
			TexPixel((int)((float)cx - (float)num5 * num8), cy - j);
			num6 += 2 * j + 1;
			if (num6 > 0)
			{
				num6 += 2 - 2 * num5--;
			}
		}
	}

	public void TexFillEllipse(int cx, int cy, int rx, int ry)
	{
		if (rx >= ry)
		{
			int num = rx;
			int num2 = -rx;
			int num3 = (int)Mathf.Ceil((float)rx / Mathf.Sqrt(2f));
			float num4 = (float)ry / (float)rx;
			for (int i = 0; i <= num3; i++)
			{
				TexSegmentV(cx + i, cy, (int)((float)cy + (float)num * num4));
				TexSegmentV(cx + i, cy, (int)((float)cy - (float)num * num4));
				TexSegmentV(cx - i, cy, (int)((float)cy + (float)num * num4));
				TexSegmentV(cx - i, cy, (int)((float)cy - (float)num * num4));
				TexSegmentV(cx + num, cy, (int)((float)cy + (float)i * num4));
				TexSegmentV(cx - num, cy, (int)((float)cy + (float)i * num4));
				TexSegmentV(cx + num, cy, (int)((float)cy - (float)i * num4));
				TexSegmentV(cx - num, cy, (int)((float)cy - (float)i * num4));
				num2 += 2 * i + 1;
				if (num2 > 0)
				{
					num2 += 2 - 2 * num--;
				}
			}
			return;
		}
		int num5 = ry;
		int num6 = -ry;
		int num7 = (int)Mathf.Ceil((float)ry / Mathf.Sqrt(2f));
		float num8 = (float)rx / (float)ry;
		for (int j = 0; j <= num7; j++)
		{
			TexSegmentH((int)((float)cx + (float)j * num8), cx, cy + num5);
			TexSegmentH((int)((float)cx + (float)j * num8), cx, cy - num5);
			TexSegmentH((int)((float)cx - (float)j * num8), cx, cy + num5);
			TexSegmentH((int)((float)cx - (float)j * num8), cx, cy - num5);
			TexSegmentH((int)((float)cx + (float)num5 * num8), cx, cy + j);
			TexSegmentH((int)((float)cx - (float)num5 * num8), cx, cy + j);
			TexSegmentH((int)((float)cx + (float)num5 * num8), cx, cy - j);
			TexSegmentH((int)((float)cx - (float)num5 * num8), cx, cy - j);
			num6 += 2 * j + 1;
			if (num6 > 0)
			{
				num6 += 2 - 2 * num5--;
			}
		}
	}
}
