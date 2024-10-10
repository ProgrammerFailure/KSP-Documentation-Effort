using UnityEngine;

namespace ns11;

public class ProtoStageIconInfo
{
	public ProtoStageIcon protoIcon;

	public StageIconInfoBox infoBoxRef;

	public Color msgBoxTextColor;

	public Color msgBoxBgColor;

	public string msg;

	public string pBarCaption;

	public Color pBarMainColor;

	public Color pBarBgColor;

	public float pBarValue;

	public ProtoStageIcon ProtoIcon => protoIcon;

	public ProtoStageIconInfo(ProtoStageIcon icon)
	{
		protoIcon = icon;
		msgBoxTextColor = Color.white;
		msgBoxBgColor = Color.grey;
		pBarMainColor = Color.white;
		pBarBgColor = Color.grey;
		msg = "";
		pBarCaption = "";
		pBarValue = 0.5f;
	}

	public void StartInfoBox(StageIconInfoBox box)
	{
		if (!(box == null))
		{
			if (infoBoxRef == null)
			{
				infoBoxRef = box;
			}
			infoBoxRef.SetMsgTextColor(msgBoxTextColor);
			infoBoxRef.SetMsgBgColor(msgBoxBgColor);
			infoBoxRef.SetMessage(msg);
			infoBoxRef.SetProgressBarColor(pBarMainColor);
			infoBoxRef.SetProgressBarBgColor(pBarBgColor);
			infoBoxRef.SetValue(pBarValue);
			infoBoxRef.SetCaption(pBarCaption);
		}
	}

	public void SetLength(float l)
	{
	}

	public void SetMsgTextColor(Color c)
	{
		msgBoxTextColor = c;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetMsgTextColor(c);
		}
	}

	public void SetMsgBgColor(Color c)
	{
		msgBoxBgColor = c;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetMsgBgColor(c);
		}
	}

	public void SetProgressBarColor(Color c)
	{
		pBarMainColor = c;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetProgressBarColor(c);
		}
	}

	public void SetProgressBarBgColor(Color c)
	{
		pBarBgColor = c;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetProgressBarBgColor(c);
		}
	}

	public void SetMessage(string m)
	{
		msg = m;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetMessage(msg);
		}
	}

	public void SetValue(float value)
	{
		pBarValue = value;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetValue(pBarValue);
		}
	}

	public void SetValue(float value, float min, float max)
	{
		SetValue(Mathf.InverseLerp(min, max, value));
	}

	public void SetCaption(string cap)
	{
		pBarCaption = cap;
		if ((bool)infoBoxRef)
		{
			infoBoxRef.SetCaption(cap);
		}
	}
}
