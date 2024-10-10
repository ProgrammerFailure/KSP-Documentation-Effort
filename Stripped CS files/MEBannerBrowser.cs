using System.Collections.Generic;
using System.IO;
using Expansions.Missions;
using Expansions.Missions.Editor;
using ns9;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MEBannerBrowser : MonoBehaviour
{
	public delegate void BannerSelectedCallback(MEBannerEntry selected, MEBannerType t);

	public delegate void BannerDeletedCallback(MEBannerEntry selected, MEBannerType t);

	public BannerSelectedCallback OnBannerSelected = delegate
	{
	};

	public BannerDeletedCallback OnBannerDeleted = delegate
	{
	};

	public Callback OnDismiss = delegate
	{
	};

	public const string uiSkinName = "FlagBrowserSkin";

	public List<MEBannerEntry> banners;

	public List<DialogGUIToggleButton> items;

	public MEBannerEntry selected;

	public UISkinDef skin;

	public PopupDialog dialog;

	public Vector2 windowSize = new Vector2(450f, 550f);

	public Vector2 bannerSize = new Vector2(430f, 240f);

	public MEBannerType currentBannerType;

	public void Setup(MEBannerType bannerType)
	{
		skin = UISkinManager.GetSkin("FlagBrowserSkin");
		currentBannerType = bannerType;
		banners = new List<MEBannerEntry>();
		LoadMissionBanners(ref banners, currentBannerType);
		LoadGameDataBanners(ref banners, currentBannerType);
		banners.Sort((MEBannerEntry x, MEBannerEntry y) => x.fileName.CompareTo(y.fileName));
		selected = null;
		dialog = CreateBrowser(bannerType);
		dialog.OnDismiss = Dismiss;
	}

	public DialogGUIToggleGroup LoadBannerButtons(Transform parent)
	{
		items = new List<DialogGUIToggleButton>();
		int i = 0;
		for (int count = banners.Count; i < count; i++)
		{
			MEBannerEntry f = banners[i];
			DialogGUIToggleButton dialogGUIToggleButton = new DialogGUIToggleButton(set: false, "", delegate
			{
				selected = f;
				if (Mouse.Left.GetDoubleClick(isDelegate: true))
				{
					Accept(selected, currentBannerType);
				}
			}, bannerSize.x, bannerSize.y);
			f.buttonReference = dialogGUIToggleButton;
			DialogGUILabel dialogGUILabel = new DialogGUILabel(banners[i].DisplayName, skin.toggle);
			dialogGUILabel.textLabelOptions = new DialogGUILabel.TextLabelOptions();
			dialogGUILabel.textLabelOptions.enableWordWrapping = false;
			dialogGUILabel.textLabelOptions.OverflowMode = TextOverflowModes.Ellipsis;
			DialogGUIVerticalLayout child = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(10, 10, 10, 10), TextAnchor.MiddleCenter, new DialogGUIImage(new Vector2(-1f, -1f), Vector2.zero, Color.white, banners[i].texture), dialogGUILabel);
			dialogGUIToggleButton.AddChild(child);
			items.Add(dialogGUIToggleButton);
			if (parent != null)
			{
				dialogGUIToggleButton.uiItem.transform.parent = parent;
			}
		}
		DialogGUIToggle[] toggles = items.ToArray();
		return new DialogGUIToggleGroup(toggles);
	}

	public void LoadGameDataBanners(ref List<MEBannerEntry> bannersList, MEBannerType bannerType)
	{
		if (!(GameDatabase.Instance != null))
		{
			return;
		}
		List<GameDatabase.TextureInfo> list = new List<GameDatabase.TextureInfo>();
		list = GameDatabase.Instance.GetAllTexturesInFolder("SquadExpansion/MakingHistory/Banners/" + bannerType.ToString() + "/");
		int count = list.Count;
		for (int i = 0; i < count; i++)
		{
			string bannerName = list[i].file.name + "." + list[i].file.fileExtension;
			MEBannerEntry duplicate = GetDuplicate(bannerName, ref bannersList);
			if (duplicate == null)
			{
				MEBannerEntry mEBannerEntry = new MEBannerEntry(bannerType);
				mEBannerEntry.isInMissionFolder = false;
				mEBannerEntry.texture = list[i].texture;
				mEBannerEntry.fileName = list[i].file.name + "." + list[i].file.fileExtension;
				bannersList.Add(mEBannerEntry);
			}
			else
			{
				duplicate.IsDuplicate = true;
			}
		}
	}

	public void LoadMissionBanners(ref List<MEBannerEntry> bannersList, MEBannerType bannerType)
	{
		if (MissionEditorLogic.Instance.EditorMission.MissionInfo == null)
		{
			return;
		}
		string path = MissionEditorLogic.Instance.EditorMission.BannersPath + bannerType.ToString() + "/";
		if (!Directory.Exists(path))
		{
			return;
		}
		string[] files = Directory.GetFiles(path);
		for (int i = 0; i < files.Length; i++)
		{
			if (MissionsUtils.GetTextureInExternalPath(files[i]) != null)
			{
				string fileName = Path.GetFileName(files[i]);
				MEBannerEntry mEBannerEntry = new MEBannerEntry(bannerType);
				mEBannerEntry.LoadFromMissionFolder(fileName, bannerType, MissionEditorLogic.Instance.EditorMission);
				bannersList.Add(mEBannerEntry);
			}
		}
	}

	public MEBannerEntry GetDuplicate(string bannerName, ref List<MEBannerEntry> bannerList)
	{
		MEBannerEntry result = null;
		for (int i = 0; i < bannerList.Count; i++)
		{
			if (bannerName == bannerList[i].fileName)
			{
				result = bannerList[i];
				break;
			}
		}
		return result;
	}

	public PopupDialog CreateBrowser(MEBannerType bannerType)
	{
		DialogGUIContentSizer dialogGUIContentSizer = new DialogGUIContentSizer(ContentSizeFitter.FitMode.PreferredSize, ContentSizeFitter.FitMode.PreferredSize, useParentSize: true);
		DialogGUIToggleGroup dialogGUIToggleGroup = LoadBannerButtons(null);
		DialogGUIVerticalLayout layout = new DialogGUIVerticalLayout(true, false, 0f, new RectOffset(0, 20, 0, 0), TextAnchor.UpperCenter, dialogGUIToggleGroup, dialogGUIContentSizer);
		DialogGUIScrollList dialogGUIScrollList = new DialogGUIScrollList(windowSize, hScroll: false, vScroll: true, layout);
		return PopupDialog.SpawnPopupDialog(Vector2.one * 0.5f, Vector2.one * 0.5f, new MultiOptionDialog("BannerBrowser", "", Localizer.Format("#autoLOC_8003107"), skin, new Rect(0.5f, 0.5f, windowSize.x, windowSize.y), dialogGUIScrollList, new DialogGUIHorizontalLayout(false, false, 8f, new RectOffset(), TextAnchor.MiddleRight, new DialogGUIButton("<color=orange>" + Localizer.Format("#autoLOC_129950") + "</color>", delegate
		{
			OnButtonDelete();
		}, () => selected != null && selected.isInMissionFolder, 90f, 30f, dismissOnSelect: false), new DialogGUIFlexibleSpace(), new DialogGUIButton(Localizer.Format("#autoLOC_190768"), delegate
		{
			Dismiss();
		}, 90f, 30f, true), new DialogGUIButton(Localizer.Format("#autoLOC_190328"), delegate
		{
			Accept(selected, bannerType);
		}, () => selected != null, 90f, 30f, dismissOnSelect: true))), persistAcrossScenes: false, skin);
	}

	public void Dismiss()
	{
		for (int i = 0; i < banners.Count; i++)
		{
			banners[i].DestroyTexture();
		}
		OnDismiss();
		dialog.Dismiss();
	}

	public void OnButtonDelete()
	{
		PopupDialog.SpawnPopupDialog(new Vector2(0.5f, 0.5f), new Vector2(0.5f, 0.5f), new MultiOptionDialog("DeleteBanner", Localizer.Format(Localizer.Format("#autoLOC_8003108")), Localizer.Format("#autoLOC_464288"), skin, new DialogGUIButton(Localizer.Format("#autoLOC_8003109"), ConfirmDeleteBanner, dismissOnSelect: true), new DialogGUIButton(Localizer.Format("#autoLOC_464291"), null, dismissOnSelect: true)), persistAcrossScenes: false, skin);
	}

	public void ConfirmDeleteBanner()
	{
		if (selected != null)
		{
			OnBannerDeleted(selected, currentBannerType);
			selected.DeleteSource();
			dialog.Dismiss();
			Setup(currentBannerType);
		}
	}

	public void Accept(MEBannerEntry selectedBanner, MEBannerType bannerType)
	{
		OnBannerSelected(selectedBanner, bannerType);
		dialog.Dismiss();
	}
}
