using System.Collections.Generic;
using ns2;
using UnityEngine;

public class SuitCombos : MonoBehaviour
{
	[SerializeField]
	public List<SuitCombo> defaultCombos;

	[SerializeField]
	public List<SuitCombo> stockCombos;

	public ConfigNode[] nodeNewCombos;

	[SerializeField]
	public List<SuitCombo> extraCombos;

	[SerializeField]
	public HelmetSuitPickerWindow helmetSuitPickerWindowPrefab;

	public HelmetSuitPickerWindow helmetSuitPickerWindow;

	public List<SuitCombo> DefaultCombos => defaultCombos;

	public List<SuitCombo> StockCombos => stockCombos;

	public List<SuitCombo> ExtraCombos => extraCombos;

	public void Start()
	{
		if (helmetSuitPickerWindow == null)
		{
			helmetSuitPickerWindow = CreateHelmetSuitPickerWindow();
		}
		GameEvents.OnGameDatabaseLoaded.Add(GetSuitCombos);
	}

	public void OnDestroy()
	{
		GameEvents.OnGameDatabaseLoaded.Remove(GetSuitCombos);
	}

	public void GetSuitCombos()
	{
		nodeNewCombos = new List<ConfigNode>().ToArray();
		extraCombos = new List<SuitCombo>();
		if (nodeNewCombos.Length != 0)
		{
			return;
		}
		nodeNewCombos = GameDatabase.Instance.GetConfigNodes("SUITCOMBOS");
		Debug.Log(nodeNewCombos);
		List<ConfigNode> list = new List<ConfigNode>();
		for (int i = 0; i < nodeNewCombos.Length; i++)
		{
			ConfigNode[] nodes = nodeNewCombos[i].GetNodes("SUITCOMBO");
			for (int j = 0; j < nodes.Length; j++)
			{
				list.Add(nodes[j]);
			}
		}
		for (int k = 0; k < list.Count; k++)
		{
			SuitCombo suitCombo = new SuitCombo();
			for (int l = 0; l < list[k].values.Count; l++)
			{
				switch (list[k].values[l].name)
				{
				case "sprite":
					suitCombo.sprite = list[k].values[l].value;
					break;
				case "gender":
					suitCombo.gender = list[k].values[l].value;
					break;
				case "name":
					suitCombo.name = list[k].values[l].value;
					break;
				case "suitTexture":
					suitCombo.suitTexture = list[k].values[l].value;
					break;
				case "primaryColor":
					suitCombo.primaryColor = list[k].values[l].value;
					break;
				case "normalTexture":
					suitCombo.normalTexture = list[k].values[l].value;
					break;
				case "secondaryColor":
					suitCombo.secondaryColor = list[k].values[l].value;
					break;
				case "suitType":
					suitCombo.suitType = list[k].values[l].value;
					break;
				case "displayName":
					suitCombo.displayName = list[k].values[l].value;
					break;
				}
			}
			if (ValidateCombo(suitCombo))
			{
				extraCombos.Add(suitCombo);
			}
		}
	}

	public bool ValidateCombo(SuitCombo suitCombo)
	{
		if (!(suitCombo.displayName == "") && !(suitCombo.suitType == "") && !(suitCombo.gender == "") && !(suitCombo.name == ""))
		{
			Debug.Log("Suit combo " + suitCombo.displayName + "/" + suitCombo.name + " validated.");
			return true;
		}
		Debug.LogError("Suit combo invalid data: \nDisplayName: " + suitCombo.displayName + "\nSuitType: " + suitCombo.suitType + "\nGender: " + suitCombo.gender + "\nName: " + suitCombo.name + "\nSuitTexture: " + suitCombo.suitTexture + "\nNormalTexture: " + suitCombo.normalTexture + "\nSprite: " + suitCombo.sprite + "\nPrimaryColor: " + suitCombo.primaryColor + "\nSecondaryColor: " + suitCombo.secondaryColor + "\nreview your SUITCOMBOS.cfg files.");
		return false;
	}

	public HelmetSuitPickerWindow CreateHelmetSuitPickerWindow()
	{
		HelmetSuitPickerWindow obj = Object.Instantiate(helmetSuitPickerWindowPrefab);
		obj.suitCombos = this;
		RectTransform obj2 = obj.transform as RectTransform;
		obj2.SetParent(UIMasterController.Instance.dialogCanvas.transform, worldPositionStays: false);
		obj2.localPosition = new Vector3((float)Screen.width * -0.05f * GameSettings.UI_SCALE, (float)(-Screen.height) * 0.1f * GameSettings.UI_SCALE, 80f);
		obj2.localScale = Vector3.one;
		obj.gameObject.SetActive(value: false);
		return obj;
	}

	public Texture GetDefaultTexture(ProtoCrewMember crew, SuitCombo.TextureTarget textureTarget, Material partMaterial, SuitCombo.MaterialProperty materialProperty)
	{
		Vector2 scaleUpdate = new Vector2(1f, 1f);
		UpdateTextureScale(partMaterial, scaleUpdate, materialProperty);
		for (int i = 0; i < defaultCombos.Count; i++)
		{
			if (crew.gender.ToString() == defaultCombos[i].gender && crew.suit.ToString() == defaultCombos[i].suitType)
			{
				switch (textureTarget)
				{
				case SuitCombo.TextureTarget.Helmet:
					Debug.LogWarning("Custom helmet texture has not found, assigning default texture.");
					return defaultCombos[i].defaultSuitTexture;
				case SuitCombo.TextureTarget.Body:
					Debug.LogWarning("Custom body texture has not found, assigning default texture.");
					return defaultCombos[i].defaultSuitTexture;
				case SuitCombo.TextureTarget.Normal:
					Debug.LogWarning("Custom normal texture has not found, assigning default normal texture.");
					return defaultCombos[i].defaultNormalTexture;
				}
			}
		}
		Debug.LogWarning("Custom texture not found.");
		return null;
	}

	public void UpdateTextureScale(Material suitMaterial, Vector2 scaleUpdate, SuitCombo.MaterialProperty materialProperty)
	{
		switch (materialProperty)
		{
		case SuitCombo.MaterialProperty.BumpMap:
			suitMaterial.SetTextureScale("_BumpMap", scaleUpdate);
			break;
		case SuitCombo.MaterialProperty.MainTex:
			suitMaterial.mainTextureScale = scaleUpdate;
			break;
		case SuitCombo.MaterialProperty.All:
			suitMaterial.mainTextureScale = scaleUpdate;
			suitMaterial.SetTextureScale("_BumpMap", scaleUpdate);
			break;
		}
	}

	public SuitCombo GetCombo(string comboId)
	{
		int num = 0;
		while (true)
		{
			if (num < stockCombos.Count)
			{
				if (stockCombos[num].name == comboId)
				{
					break;
				}
				num++;
				continue;
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < extraCombos.Count)
				{
					if (extraCombos[num2].name == comboId)
					{
						break;
					}
					num2++;
					continue;
				}
				return null;
			}
			return extraCombos[num2];
		}
		return stockCombos[num];
	}
}
