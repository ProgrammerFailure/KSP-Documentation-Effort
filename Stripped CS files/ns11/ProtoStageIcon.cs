using System;
using System.Collections.Generic;
using UnityEngine;

namespace ns11;

[Serializable]
public class ProtoStageIcon
{
	[SerializeField]
	public Part part;

	[SerializeField]
	public StageIcon stageIcon;

	public Color iconColor = Color.white;

	public Color backgroundColor = Color.white;

	public Color borderColor = Color.white;

	public DefaultIcons iconType = DefaultIcons.MYSTERY_PART;

	public string customIconFilename = "";

	public int customIconX;

	public int customIconY;

	public bool blinkBorder;

	public float blinkInterval = 1f;

	public bool frozen;

	public bool highlighted;

	public Vector3 homePos;

	public List<ProtoStageIconInfo> infoBoxes;

	public Callback onStageIconDestroy = delegate
	{
	};

	public Part Part => part;

	public StageIcon StageIcon => stageIcon;

	public bool Highlighted => highlighted;

	public ProtoStageIcon(Part part)
	{
		this.part = part;
	}

	public StageIcon CreateIcon(bool alertStagingSequencer = true)
	{
		if (!(part == null) && !string.IsNullOrEmpty(part.stagingIcon))
		{
			if (HighLogic.LoadedSceneIsFlight && (part.State == PartStates.DEAD || part.vessel != FlightGlobals.ActiveVessel))
			{
				return null;
			}
			if (this.stageIcon != null)
			{
				return this.stageIcon;
			}
			StageIcon stageIcon = StageManager.CreateIcon(this, alertStagingSequencer);
			if (infoBoxes == null)
			{
				infoBoxes = new List<ProtoStageIconInfo>(StageIcon.maxInfoBoxes);
			}
			if (stageIcon == null)
			{
				return null;
			}
			this.stageIcon = stageIcon;
			if (iconType == DefaultIcons.CUSTOM && !string.IsNullOrEmpty(customIconFilename))
			{
				this.stageIcon.SetIcon(customIconFilename, customIconX, customIconY);
			}
			else
			{
				stageIcon.SetIcon(iconType);
			}
			stageIcon.SetIconColor(iconColor);
			stageIcon.SetBackgroundColor(backgroundColor);
			stageIcon.Highlight(highlighted, highlightReferencedPart: true);
			stageIcon.BlinkBorder(blinkInterval);
			int count = infoBoxes.Count;
			for (int i = 0; i < count; i++)
			{
				infoBoxes[i].StartInfoBox(stageIcon.DisplayInfo());
			}
			return this.stageIcon;
		}
		return null;
	}

	public void RemoveIcon(bool alertStagingSequencer = true)
	{
		if (!(stageIcon == null))
		{
			StageManager.RemoveIcon(stageIcon, destroyIcon: true, removeSelection: true, alertStagingSequencer);
			ClearInfoBoxes();
			infoBoxes = null;
			stageIcon = null;
			onStageIconDestroy();
		}
	}

	public void DisableIcon()
	{
		if (!(stageIcon == null))
		{
			StageManager.DisableIcon(stageIcon);
		}
	}

	public void SetIcon(DefaultIcons icon)
	{
		iconType = icon;
		if ((bool)stageIcon)
		{
			stageIcon.SetIcon(icon);
		}
	}

	public void SetIcon(string file, int x, int y)
	{
		iconType = DefaultIcons.CUSTOM;
		if (stageIcon != null)
		{
			stageIcon.SetIcon(file, x, y);
		}
	}

	public void SetIconColor(Color c)
	{
		iconColor = c;
		if ((bool)stageIcon)
		{
			stageIcon.SetIconColor(iconColor);
		}
	}

	public void SetBorderColor(Color c)
	{
		borderColor = c;
		if ((bool)stageIcon)
		{
			stageIcon.SetBorderColor(borderColor);
		}
	}

	public void SetBackgroundColor(Color c)
	{
		backgroundColor = c;
		if ((bool)stageIcon)
		{
			stageIcon.SetBackgroundColor(backgroundColor);
		}
	}

	public void BlinkBorder(float interval)
	{
		blinkBorder = true;
		blinkInterval = interval;
		if ((bool)stageIcon)
		{
			stageIcon.BlinkBorder(blinkInterval);
		}
	}

	public void Highlight(bool highlightState)
	{
		if (highlighted != highlightState)
		{
			highlighted = highlightState;
			if ((bool)stageIcon)
			{
				stageIcon.Highlight(highlighted, highlightReferencedPart: true);
			}
		}
	}

	public void Freeze()
	{
		frozen = true;
		if ((bool)stageIcon)
		{
			stageIcon.Freeze();
		}
	}

	public void Unfreeze()
	{
		frozen = false;
		if ((bool)stageIcon)
		{
			stageIcon.Unfreeze();
		}
	}

	public ProtoStageIconInfo DisplayInfo()
	{
		ProtoStageIconInfo protoStageIconInfo = null;
		if ((bool)stageIcon)
		{
			protoStageIconInfo = new ProtoStageIconInfo(this);
			infoBoxes.Add(protoStageIconInfo);
			protoStageIconInfo.StartInfoBox(stageIcon.DisplayInfo());
		}
		return protoStageIconInfo;
	}

	public void RemoveInfo(ProtoStageIconInfo iBox)
	{
		if ((bool)stageIcon)
		{
			infoBoxes.Remove(iBox);
			if ((bool)iBox.infoBoxRef && (bool)stageIcon)
			{
				stageIcon.RemoveInfo(iBox.infoBoxRef);
			}
		}
	}

	public void ClearInfoBoxes()
	{
		if (infoBoxes != null)
		{
			infoBoxes.Clear();
		}
		if ((bool)stageIcon)
		{
			stageIcon.ClearInfoBoxes();
		}
	}
}
