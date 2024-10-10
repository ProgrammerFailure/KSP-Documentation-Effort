using System;
using System.Collections.Generic;
using System.IO;
using Contracts.Agents;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagBrowser : MonoBehaviour
{
	[Serializable]
	public class FlagEntry
	{
		public GameDatabase.TextureInfo textureInfo;

		public string name;
	}

	public delegate void FlagSelectedCallback(FlagEntry selected);

	public List<FlagEntry> Flags;

	public List<FlagEntry> OrganizationFlags;

	public List<FlagEntry> AgencyFlags;

	public FlagEntry selected;

	public string uiSkinName = "FlagBrowserSkin";

	public UISkinDef skin;

	public PopupDialog dialog;

	public MenuNavigation menuNav;

	public Rect windowRect;

	public Vector2 scrollPos;

	public float width = 360f;

	public float height = 400f;

	public float iconSize = 64f;

	public float iconSpacing = 8f;

	public FlagSelectedCallback OnFlagSelected = delegate
	{
	};

	public Callback OnDismiss = delegate
	{
	};

	public DialogGUIToggleGroup agencyToggleGroup;

	public DialogGUIToggleGroup organizationToggleGroup;

	public DialogGUIToggleGroup generalToggleGroup;

	public List<DialogGUIToggleButton> generalItems;

	public List<DialogGUIToggleButton> agencyItems;

	public List<DialogGUIToggleButton> organizationItems;

	public bool toggleInProgress;

	public void Start()
	{
		skin = UISkinManager.GetSkin(uiSkinName);
		windowRect.width = width;
		windowRect.height = height;
		if (GameDatabase.Instance != null)
		{
			List<GameDatabase.TextureInfo> list = new List<GameDatabase.TextureInfo>();
			list = GameDatabase.Instance.GetAllTexturesInFolderType("Flags", caseInsensitive: true);
			Flags = ProcessFlagFolder(list);
			list = GameDatabase.Instance.GetAllTexturesInFolderType("FlagsOrganization", caseInsensitive: true);
			OrganizationFlags = ProcessFlagFolder(list);
			list = GameDatabase.Instance.GetAllTexturesInFolderType("FlagsAgency", caseInsensitive: true);
			AgencyFlags = ProcessFlagFolder(list);
		}
		if (AgentList.Instance != null)
		{
			int count = AgentList.Instance.Agencies.Count;
			for (int i = 0; i < count; i++)
			{
				Agent agent = AgentList.Instance.Agencies[i];
				bool flag = false;
				int count2 = Flags.Count;
				while (count2-- > 0)
				{
					if (Flags[count2].textureInfo.texture == agent.Logo)
					{
						flag = true;
					}
					if (Flags[count2].textureInfo.texture == agent.LogoScaled)
					{
						Flags.RemoveAt(count2);
					}
				}
				if (!flag)
				{
					FlagEntry flagEntry = new FlagEntry();
					flagEntry.textureInfo = GameDatabase.Instance.GetTextureInfo(agent.LogoURL);
					flagEntry.name = agent.Name;
					if (flagEntry.textureInfo != null)
					{
						Flags.Add(flagEntry);
					}
				}
			}
		}
		selected = null;
		dialog = CreateFlagBrowser();
		dialog.OnDismiss = Dismiss;
		menuNav = MenuNavigation.SpawnMenuNavigation(dialog.gameObject, Navigation.Mode.Automatic, hasText: false, limitCheck: true, SliderFocusType.Scrollbar);
	}

	public List<FlagEntry> ProcessFlagFolder(List<GameDatabase.TextureInfo> textures)
	{
		List<FlagEntry> list = new List<FlagEntry>();
		list.Clear();
		int count = textures.Count;
		for (int i = 0; i < count; i++)
		{
			GameDatabase.TextureInfo textureInfo = textures[i];
			FlagEntry flagEntry = new FlagEntry();
			flagEntry.textureInfo = textureInfo;
			flagEntry.name = Path.GetFileNameWithoutExtension(textureInfo.name);
			list.Add(flagEntry);
		}
		return list;
	}

	public PopupDialog CreateFlagBrowser()
	{
		generalItems = new List<DialogGUIToggleButton>();
		agencyItems = new List<DialogGUIToggleButton>();
		organizationItems = new List<DialogGUIToggleButton>();
		DialogGUILabel dialogGUILabel = new DialogGUILabel(Localizer.Format("#autoLOC_6002637"), skin.label);
		dialogGUILabel.height = 20f;
		agencyItems = ProcessButtons(AgencyFlags);
		DialogGUILabel dialogGUILabel2 = new DialogGUILabel(Localizer.Format("#autoLOC_6002638"), skin.label);
		dialogGUILabel2.height = 20f;
		organizationItems = ProcessButtons(OrganizationFlags);
		DialogGUILabel dialogGUILabel3 = new DialogGUILabel(Localizer.Format("#autoLOC_6002639"), skin.label);
		dialogGUILabel3.height = 20f;
		generalItems = ProcessButtons(Flags);
		DialogGUIVerticalLayout dialogGUIVerticalLayout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 11, 4, 4), TextAnchor.UpperLeft);
		dialogGUIVerticalLayout.AddChild(dialogGUILabel);
		DialogGUIToggle[] toggles = agencyItems.ToArray();
		agencyToggleGroup = new DialogGUIToggleGroup(toggles);
		toggles = organizationItems.ToArray();
		organizationToggleGroup = new DialogGUIToggleGroup(toggles);
		toggles = generalItems.ToArray();
		generalToggleGroup = new DialogGUIToggleGroup(toggles);
		dialogGUIVerticalLayout.AddChild(new DialogGUIGridLayout(new RectOffset(2, 8, 4, 4), new Vector2(iconSize, iconSize), new Vector2(4f, 4f), GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.UpperLeft, GridLayoutGroup.Constraint.FixedColumnCount, 6, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true), agencyToggleGroup));
		dialogGUIVerticalLayout.AddChild(dialogGUILabel2);
		dialogGUIVerticalLayout.AddChild(new DialogGUIGridLayout(new RectOffset(2, 8, 4, 4), new Vector2(iconSize, iconSize), new Vector2(4f, 4f), GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.UpperLeft, GridLayoutGroup.Constraint.FixedColumnCount, 6, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true), organizationToggleGroup));
		dialogGUIVerticalLayout.AddChild(dialogGUILabel3);
		dialogGUIVerticalLayout.AddChild(new DialogGUIGridLayout(new RectOffset(2, 8, 4, 4), new Vector2(iconSize, iconSize), new Vector2(4f, 4f), GridLayoutGroup.Corner.UpperLeft, GridLayoutGroup.Axis.Horizontal, TextAnchor.UpperLeft, GridLayoutGroup.Constraint.FixedColumnCount, 6, new DialogGUIContentSizer(ContentSizeFitter.FitMode.Unconstrained, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true), generalToggleGroup));
		DialogGUIScrollList dialogGUIScrollList = new DialogGUIScrollList(new Vector2(445f, 485f), hScroll: false, vScroll: true, dialogGUIVerticalLayout);
		PopupDialog result = PopupDialog.SpawnPopupDialog(Vector2.one * 0.5f, Vector2.one * 0.5f, new MultiOptionDialog("FlagBrowser", "", Localizer.Format("#autoLOC_364495"), skin, new Rect(0.5f, 0.5f, 455f, height), dialogGUIScrollList, new DialogGUIHorizontalLayout(false, false, 8f, new RectOffset(), TextAnchor.MiddleRight, new DialogGUIButton(Localizer.Format("#autoLOC_190768"), delegate
		{
			Dismiss();
		}, 90f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_190328"), delegate
		{
			Accept(selected);
		}, () => selected != null, 90f, 30f, dismissOnSelect: true))), persistAcrossScenes: false, skin);
		dialogGUIScrollList.Content.AddComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
		organizationToggleGroup.Group.allowSwitchOff = true;
		generalToggleGroup.Group.allowSwitchOff = true;
		if (agencyItems.Count > 0)
		{
			agencyItems[0].toggle.Select();
			return result;
		}
		if (organizationItems.Count > 0)
		{
			organizationItems[0].toggle.Select();
			return result;
		}
		if (generalItems.Count > 0)
		{
			generalItems[0].toggle.Select();
		}
		return result;
	}

	public List<DialogGUIToggleButton> ProcessButtons(List<FlagEntry> flags)
	{
		List<DialogGUIToggleButton> list = new List<DialogGUIToggleButton>();
		int i = 0;
		for (int count = flags.Count; i < count; i++)
		{
			FlagEntry f = flags[i];
			DialogGUIToggleButton toggle = new DialogGUIToggleButton(set: false, "", delegate
			{
			});
			DialogGUILabel dialogGUILabel = new DialogGUILabel(flags[i].name, skin.toggle);
			dialogGUILabel.textLabelOptions = new DialogGUILabel.TextLabelOptions();
			dialogGUILabel.textLabelOptions.enableWordWrapping = false;
			dialogGUILabel.textLabelOptions.OverflowMode = TextOverflowModes.Ellipsis;
			DialogGUIVerticalLayout image = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(4, 4, 8, 8), TextAnchor.MiddleCenter, new DialogGUIImage(new Vector2(-1f, -1f), Vector2.zero, Color.white, flags[i].textureInfo.texture), dialogGUILabel);
			image.children[1].OnResize = delegate
			{
				image.children[1].uiItem.GetComponent<LayoutElement>().preferredHeight = 18f;
			};
			toggle.AddChild(image);
			list.Add(toggle);
			toggle.onToggled = delegate(bool b)
			{
				if (b)
				{
					selected = f;
					if (Mouse.Left.GetDoubleClick(isDelegate: true) || menuNav.SumbmitOnSelectedToggle())
					{
						Accept(selected);
					}
				}
				Toggled(toggle, b);
			};
		}
		return list;
	}

	public void Toggled(DialogGUIToggleButton t, bool b)
	{
		if (!b || toggleInProgress)
		{
			return;
		}
		toggleInProgress = true;
		if (t.toggle.group != null)
		{
			if (t.toggle.group == agencyToggleGroup.Group)
			{
				agencyToggleGroup.Group.allowSwitchOff = false;
				organizationToggleGroup.Group.allowSwitchOff = true;
				generalToggleGroup.Group.allowSwitchOff = true;
				ToggleAllOff(organizationToggleGroup);
				ToggleAllOff(generalToggleGroup);
			}
			else if (t.toggle.group == organizationToggleGroup.Group)
			{
				agencyToggleGroup.Group.allowSwitchOff = true;
				organizationToggleGroup.Group.allowSwitchOff = false;
				generalToggleGroup.Group.allowSwitchOff = true;
				ToggleAllOff(agencyToggleGroup);
				ToggleAllOff(generalToggleGroup);
			}
			else if (t.toggle.group == generalToggleGroup.Group)
			{
				agencyToggleGroup.Group.allowSwitchOff = true;
				organizationToggleGroup.Group.allowSwitchOff = true;
				generalToggleGroup.Group.allowSwitchOff = false;
				ToggleAllOff(agencyToggleGroup);
				ToggleAllOff(organizationToggleGroup);
			}
		}
		toggleInProgress = false;
	}

	public void ToggleAllOff(DialogGUIToggleGroup group)
	{
		foreach (Toggle item in group.Group.ActiveToggles())
		{
			item.isOn = false;
		}
	}

	public void Dismiss()
	{
		OnDismiss();
		dialog.Dismiss();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	public void Accept(FlagEntry sel)
	{
		OnFlagSelected(sel);
		Debug.Log("Selected Flag " + sel.name);
		dialog.Dismiss();
		UnityEngine.Object.Destroy(base.gameObject);
	}
}
