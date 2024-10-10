using System;
using System.Collections.Generic;
using EdyCommonTools;
using UnityEngine;
using UnityEngine.Serialization;

namespace VehiclePhysics;

public class DataLogger
{
	[Serializable]
	public class DisplaySettings
	{
		public int x = 10;

		public int y = 10;

		public int width = 1024;

		public int height = 512;

		public Color backgroundColor = GColor.Alpha(Color.black, 0.95f);

		[FormerlySerializedAs("backgroundAlpha")]
		public float chartAlpha = 1f;

		public float textAlpha = 1f;
	}

	[Serializable]
	public class GridSettings
	{
		public enum Type
		{
			Full,
			Vertical,
			Horizontal
		}

		public bool show = true;

		public Type type;

		public Vector2 resolution = new Vector2(1f, 1f);

		public Color color = GColor.green * 0.3f;

		public int autoHidePixels = 1;

		public bool CheckVisibility(TextureCanvas canvas)
		{
			if (show && canvas.GetPixelWidth(resolution.x) >= autoHidePixels)
			{
				return canvas.GetPixelHeight(resolution.y) >= autoHidePixels;
			}
			return false;
		}

		public void Draw(TextureCanvas canvas)
		{
			if (CheckVisibility(canvas))
			{
				canvas.color = color;
				if (type == Type.Vertical)
				{
					canvas.VerticalGrid(resolution.x);
				}
				else if (type == Type.Horizontal)
				{
					canvas.HorizontalGrid(resolution.y);
				}
				else
				{
					canvas.Grid(resolution.x, resolution.y);
				}
			}
		}

		public void DrawDots(TextureCanvas canvas)
		{
			if (CheckVisibility(canvas))
			{
				canvas.color = color;
				canvas.DotGrid(resolution.x, resolution.y);
			}
		}
	}

	public class Channel
	{
		public string name = "";

		public float origin;

		public float scale = 1f;

		public Color color = Color.white;

		public bool alphaBlend;

		public bool show = true;

		public bool showSegmentBegin;

		public bool showSegmentEnd;

		public float captionPositionY = 1f;

		public string valueFormat = "0.00";

		public int m_channelId;

		public DataRecorder m_recorder;

		public Channel(DataRecorder recorder)
		{
			m_channelId = recorder.NewFloatChannel();
			m_recorder = recorder;
		}

		public void Write(float value)
		{
			m_recorder.WriteFloatValue(m_channelId, value);
		}

		public float Read()
		{
			return m_recorder.ReadFloatValue(m_channelId);
		}
	}

	public delegate void PlotAction(Channel channel, bool hasValue1, float value1, float t1, bool hasValue2, float value2, float t2);

	public float minViewportWidth = 0.2f;

	public float maxViewPortWidth = 2100f;

	public float minViewportHeight = 0.6f;

	public float topLimit = 12f;

	public float bottomLimit = -0.5f;

	public int rightMarginOnRecord = 80;

	public float horizontalZoomCenter = 0.5f;

	public float verticalZoomCenter = 0.5f;

	public bool autoRefresh = true;

	public GridSettings primaryGrid = new GridSettings();

	public GridSettings secondaryGrid = new GridSettings();

	public GridSettings dotGrid = new GridSettings();

	public Color channelOriginColor = GColor.gray * 0.3f;

	public int channelCaptionMargin = 4;

	public DataRecorder m_dataRecorder;

	public TextureCanvas m_canvas;

	public DisplaySettings m_displaySettings;

	public int m_displayX;

	public int m_displayY;

	public List<Channel> m_channels = new List<Channel>();

	public float m_bufferTime;

	public float m_deltaTime;

	public bool m_dirty = true;

	public Rect rect
	{
		get
		{
			return m_canvas.rect;
		}
		set
		{
			m_canvas.rect = ClampView(value);
			m_dirty = true;
		}
	}

	public DataLogger(float bufferTime, float deltaTime, DisplaySettings textureSettings)
	{
		m_dataRecorder = new DataRecorder(bufferTime, deltaTime);
		m_canvas = new TextureCanvas(textureSettings.width, textureSettings.height);
		m_bufferTime = bufferTime;
		m_deltaTime = deltaTime;
		m_displaySettings = textureSettings;
		primaryGrid.color = GColor.green * 0.1f;
		primaryGrid.type = GridSettings.Type.Vertical;
		primaryGrid.autoHidePixels = 10;
		secondaryGrid.color = GColor.green * 0.4f;
		secondaryGrid.resolution = new Vector2(10f, 10f);
		secondaryGrid.type = GridSettings.Type.Vertical;
		dotGrid.color = GColor.gray * 0.2f;
		dotGrid.resolution = new Vector2(0.1f, 0.1f);
		dotGrid.autoHidePixels = 10;
	}

	public void ConfigureDisplay(DisplaySettings textureSettings)
	{
		m_displaySettings = textureSettings;
		if (m_displaySettings.width != m_canvas.width || m_displaySettings.height != m_canvas.height)
		{
			ReleaseTexture();
			m_canvas = new TextureCanvas(m_displaySettings.width, m_displaySettings.height, m_canvas.rect);
			Refresh(force: true);
		}
	}

	public void ReleaseTexture()
	{
		m_canvas.DestroyTexture();
	}

	public void GUIDrawGraphic()
	{
		if (autoRefresh)
		{
			Refresh();
		}
		m_displayX = ((m_displaySettings.x < 0) ? (Screen.width + m_displaySettings.x - m_displaySettings.width + 1) : m_displaySettings.x);
		m_displayY = ((m_displaySettings.y < 0) ? (Screen.height + m_displaySettings.y - m_displaySettings.height + 1) : m_displaySettings.y);
		m_canvas.GUIDraw(m_displayX, m_displayY);
	}

	public void GUIDrawLabels(GUIStyle style)
	{
		Color color = GColor.Alpha(style.normal.textColor, m_displaySettings.textAlpha);
		TextAnchor alignment = style.alignment;
		style.normal.textColor = color * 0.9f;
		style.alignment = TextAnchor.UpperLeft;
		int verticalPos = m_displaySettings.height - channelCaptionMargin - style.fontSize;
		if (m_canvas.GetPixelWidth(secondaryGrid.resolution.x) > 40)
		{
			m_canvas.HorizontalAction(delegate(float x)
			{
				GUILabel(m_canvas.GetPixelX(x) + channelCaptionMargin, verticalPos, x.ToString("0"), style);
			}, secondaryGrid.resolution.x);
		}
		else
		{
			m_canvas.HorizontalAction(delegate(float x)
			{
				GUILabel(m_canvas.GetPixelX(x), verticalPos, x.ToString("0"), style);
			}, secondaryGrid.resolution.x * 10f);
		}
		if (m_canvas.GetPixelWidth(primaryGrid.resolution.x) > 40)
		{
			m_canvas.HorizontalAction(delegate(float x)
			{
				GUILabel(m_canvas.GetPixelX(x) + channelCaptionMargin, verticalPos, x.ToString("0"), style);
			}, primaryGrid.resolution.x);
		}
		m_dataRecorder.MoveReadToEnd();
		int i = 0;
		for (int count = m_channels.Count; i < count; i++)
		{
			Channel channel = m_channels[i];
			int pixelY = m_canvas.GetPixelY(channel.origin);
			if (pixelY > 0 && pixelY < m_displaySettings.height - style.fontSize - 2)
			{
				style.normal.textColor = GColor.Alpha(Color.Lerp(channel.color, color, 0.5f), channel.color.a * color.a);
				style.alignment = TextAnchor.UpperLeft;
				GUILabelOnTexture(channelCaptionMargin, -pixelY - (int)((float)style.fontSize * 1.6f * channel.captionPositionY), channel.name, style);
				style.alignment = TextAnchor.UpperRight;
				float f = channel.Read();
				if (float.IsNaN(f))
				{
					GUILabelOnTexture(channelCaptionMargin, -pixelY - (int)((float)style.fontSize * 1.6f * channel.captionPositionY), channel.valueFormat.Replace("0"[0], "-"[0]), style);
				}
				else
				{
					GUILabelOnTexture(channelCaptionMargin, -pixelY - (int)((float)style.fontSize * 1.6f * channel.captionPositionY), f.ToString(channel.valueFormat), style);
				}
			}
		}
		style.normal.textColor = color;
		style.alignment = alignment;
	}

	public void GUILabel(int x, int y, string label, GUIStyle style)
	{
		if (x >= 0 && x <= m_displaySettings.width - 30)
		{
			GUI.Label(new Rect(x + m_displayX, y + m_displayY, 1f, 1f), label, style);
		}
	}

	public void GUILabelOnTexture(int x, int y, string label, GUIStyle style)
	{
		Rect position = default(Rect);
		position.x = m_displayX + x;
		if (x < 0)
		{
			position.x += m_displaySettings.width;
		}
		position.y = m_displayY + y;
		if (y < 0)
		{
			position.y += m_displaySettings.height;
		}
		position.xMax = m_displayX + m_displaySettings.width - channelCaptionMargin;
		position.yMax = m_displayY + m_displaySettings.height - channelCaptionMargin;
		GUI.Label(position, label, style);
	}

	public Rect ClampView(Rect rc)
	{
		if (rc.height > topLimit - bottomLimit)
		{
			rc.height = topLimit - bottomLimit;
		}
		if (rc.y < bottomLimit)
		{
			rc.y = bottomLimit;
		}
		if (rc.y > topLimit - rc.height)
		{
			rc.y = topLimit - rc.height;
		}
		float num = (float)m_dataRecorder.GetFirstFrame() * m_deltaTime;
		float num2 = (float)m_dataRecorder.GetLastFrame() * m_deltaTime;
		float num3 = Mathf.Max(num2 - num + 2f, 60f);
		if (rc.width < minViewportWidth)
		{
			rc.width = minViewportWidth;
		}
		if (rc.width > num3)
		{
			rc.width = num3;
		}
		float num4 = num - m_canvas.Pixels2CanvasX(20);
		float num5 = num2 - m_canvas.Pixels2CanvasX(100);
		if (num5 < 0f)
		{
			num5 = 0f;
		}
		if (rc.x < num4)
		{
			rc.x = num4;
		}
		if (rc.x > num5)
		{
			rc.x = num5;
		}
		return rc;
	}

	public void MovePosition(float offsetX, float offsetY)
	{
		if (offsetX != 0f || offsetY != 0f)
		{
			Rect rc = m_canvas.rect;
			rc.x += offsetX * rc.width;
			rc.y += offsetY * rc.height;
			m_canvas.rect = ClampView(rc);
			m_dirty = true;
		}
	}

	public void HorizontalZoom(float rate)
	{
		if (rate != 0f)
		{
			Rect rc = m_canvas.rect;
			float num = rc.width * rate;
			rc.xMin += num * horizontalZoomCenter;
			rc.xMax -= num * (1f - horizontalZoomCenter);
			m_canvas.rect = ClampView(rc);
			m_dirty = true;
		}
	}

	public void VerticalZoom(float rate)
	{
		if (rate != 0f)
		{
			Rect rc = m_canvas.rect;
			float num = rc.height * rate;
			rc.yMin += num * verticalZoomCenter;
			rc.yMax -= num * (1f - verticalZoomCenter);
			m_canvas.rect = ClampView(rc);
			m_dirty = true;
		}
	}

	public void NextFrame()
	{
		m_dataRecorder.NextWriteFrame();
		float num = (float)m_dataRecorder.GetWriteFrame() * m_deltaTime;
		float num2 = m_canvas.rect.xMax - m_canvas.Pixels2CanvasX(rightMarginOnRecord);
		if (num > num2)
		{
			Rect rect = m_canvas.rect;
			rect.x += num - num2;
			m_canvas.rect = rect;
		}
		m_dirty = true;
	}

	public Channel NewChannel(string name, Color color, float origin = 0f)
	{
		Channel channel = new Channel(m_dataRecorder);
		m_channels.Add(channel);
		channel.name = name;
		channel.color = color;
		channel.origin = origin;
		return channel;
	}

	public int FrameCount()
	{
		return m_dataRecorder.GetWriteFrame();
	}

	public void ResetData()
	{
		m_dataRecorder.ResetData();
		Rect rc = m_canvas.rect;
		rc.x = 0f;
		m_canvas.rect = ClampView(rc);
		m_dirty = true;
	}

	public void Restart()
	{
		m_channels.Clear();
		m_dataRecorder = new DataRecorder(m_bufferTime, m_deltaTime);
		m_dirty = true;
	}

	public void Refresh(bool force = false)
	{
		if (m_dirty || force)
		{
			m_canvas.color = m_displaySettings.backgroundColor;
			m_canvas.alpha = m_displaySettings.backgroundColor.a * m_displaySettings.chartAlpha;
			m_canvas.alphaBlend = false;
			m_canvas.Clear();
			m_canvas.alpha = m_displaySettings.chartAlpha;
			dotGrid.DrawDots(m_canvas);
			primaryGrid.Draw(m_canvas);
			secondaryGrid.Draw(m_canvas);
			m_canvas.color = Color.white;
			m_canvas.VerticalLine(0f);
			m_canvas.color = channelOriginColor;
			int i = 0;
			for (int count = m_channels.Count; i < count; i++)
			{
				m_canvas.HorizontalLine(m_channels[i].origin);
			}
			PlotChannels(PlotLines);
			if (m_canvas.GetPixelWidth(m_deltaTime) > 5)
			{
				PlotChannels(PlotDots);
			}
			m_dirty = false;
		}
	}

	public void PlotChannels(PlotAction plotAction)
	{
		m_dataRecorder.GetReadPointers(m_canvas.rect.xMin, m_canvas.rect.xMax, out var fromFrame, out var frameCount);
		int num = Mathf.FloorToInt(m_canvas.Pixels2CanvasX(1) / m_deltaTime);
		int i = 0;
		for (int count = m_channels.Count; i < count; i++)
		{
			Channel channel = m_channels[i];
			m_canvas.color = channel.color;
			m_canvas.alphaBlend = channel.alphaBlend;
			m_dataRecorder.MoveReadToFrame(fromFrame);
			float num2 = (float)fromFrame * m_deltaTime;
			float num3 = channel.Read() * channel.scale + channel.origin;
			bool flag = !float.IsNaN(num3);
			float num4 = num2;
			if (num < 2)
			{
				for (int j = 1; j < frameCount; j++)
				{
					m_dataRecorder.NextReadFrame();
					num4 += m_deltaTime;
					float num5 = channel.Read() * channel.scale + channel.origin;
					bool flag2 = !float.IsNaN(num5);
					plotAction(channel, flag, num3, num2, flag2, num5, num4);
					num2 = num4;
					num3 = num5;
					flag = flag2;
				}
				continue;
			}
			float num6 = 0f;
			float num7 = num3;
			bool flag3 = flag;
			for (int k = 1; k < frameCount; k++)
			{
				m_dataRecorder.NextReadFrame();
				num4 += m_deltaTime;
				float num5 = channel.Read() * channel.scale + channel.origin;
				bool flag2;
				if (flag2 = !float.IsNaN(num5))
				{
					if (flag3)
					{
						float num8 = Mathf.Abs(num5 - num3);
						if (num8 > num6)
						{
							num6 = num8;
							num7 = num5;
						}
					}
					else
					{
						num6 = 0f;
						num7 = num5;
						flag3 = true;
					}
				}
				if ((fromFrame + k) % num == 0 || k == frameCount - 1)
				{
					plotAction(channel, flag, num3, num2, flag3, num7, num4);
					num2 = num4;
					num3 = num7;
					flag = flag3;
					num6 = 0f;
					flag3 = false;
				}
			}
		}
	}

	public bool BoundsCheck(float bound0, float bound1, float value0, float value1)
	{
		if (bound0 > bound1)
		{
			float num = bound0;
			bound0 = bound1;
			bound1 = num;
		}
		if (value0 >= bound0 && value0 <= bound1 && value1 >= bound0)
		{
			return value1 <= bound1;
		}
		return false;
	}

	public void PlotLines(Channel channel, bool hasValue1, float value1, float t1, bool hasValue2, float value2, float t2)
	{
		if (hasValue1 && hasValue2)
		{
			m_canvas.Line(t1, value1, t2, value2);
		}
		else
		{
			if (!(hasValue1 || hasValue2))
			{
				return;
			}
			if (hasValue1)
			{
				if (channel.showSegmentEnd)
				{
					m_canvas.Line(t1, value1, t2, channel.origin);
				}
			}
			else if (channel.showSegmentBegin)
			{
				m_canvas.Line(t1, channel.origin, t2, value2);
			}
		}
	}

	public void PlotDots(Channel channel, bool hasValue1, float value1, float t1, bool hasValue2, float value2, float t2)
	{
		if (hasValue1)
		{
			m_canvas.Dot(t1, value1);
		}
		if (hasValue2)
		{
			m_canvas.Dot(t2, value2);
		}
	}
}
