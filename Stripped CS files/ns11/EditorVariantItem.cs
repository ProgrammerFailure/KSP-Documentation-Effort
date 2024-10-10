using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ns11;

public class EditorVariantItem : MonoBehaviour
{
	public TextMeshProUGUI title;

	public TextMeshProUGUI description;

	public TextMeshProUGUI partcount;

	public Image imagePrimaryColor;

	public Image imageSecomdaryColor;

	public Button btnSetVariantTheme;

	public Button btnApplyToVessel;

	public AvailableVariantTheme variantTheme;

	public void Awake()
	{
		btnSetVariantTheme.onClick.AddListener(MouseInput_SetTheme);
		btnApplyToVessel.onClick.AddListener(MouseInput_ApplyTheme);
	}

	public void OnDestroy()
	{
		btnSetVariantTheme.onClick.RemoveListener(MouseInput_SetTheme);
		btnApplyToVessel.onClick.RemoveListener(MouseInput_ApplyTheme);
	}

	public void Create(EditorPartList partList, AvailableVariantTheme variant)
	{
		variantTheme = variant;
		title.text = variant.displayName;
		partcount.text = variant.parts.Count.ToString();
		description.text = variant.description;
		SetColor(variant.primaryColor, variant.secondaryColor);
	}

	public void SetColor(string hexPrimaryColor, string hexSecondaryColor)
	{
		Color color = Color.white;
		ColorUtility.TryParseHtmlString(hexPrimaryColor, out color);
		Color color2;
		Color color3 = ((!ColorUtility.TryParseHtmlString(hexSecondaryColor, out color2)) ? color : color2);
		imagePrimaryColor.color = color;
		imageSecomdaryColor.color = color3;
	}

	public void MouseInput_SetTheme()
	{
		for (int i = 0; i < variantTheme.parts.Count; i++)
		{
			AvailablePart availablePart = variantTheme.parts[i];
			bool flag = false;
			for (int j = 0; j < availablePart.Variants.Count; j++)
			{
				if (availablePart.Variants[j].themeName == variantTheme.name)
				{
					availablePart.variant = availablePart.Variants[j];
					GameEvents.onEditorDefaultVariantChanged.Fire(availablePart, availablePart.variant);
					flag = true;
				}
			}
			if (!flag)
			{
				Debug.LogError("This shouldn't be possible unless the variants have changed since the loading");
			}
		}
	}

	public void MouseInput_ApplyTheme()
	{
		EditorLogic fetch = EditorLogic.fetch;
		if (fetch == null || ((fetch.ship == null) | (fetch.ship.parts == null)))
		{
			Debug.Log("No EditorLogic ship so bypassing.");
		}
		for (int i = 0; i < fetch.ship.parts.Count; i++)
		{
			ModulePartVariants variants = fetch.ship.parts[i].variants;
			if (variants != null && variants.HasVariantTheme(variantTheme.name))
			{
				variants.SetVariantTheme(variantTheme.name);
			}
		}
	}
}
