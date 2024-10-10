using UnityEngine;

namespace ns2;

[RequireComponent(typeof(RectTransform))]
public class UIRectScaler : MonoBehaviour
{
	public enum ScaleType
	{
		UIScale,
		DivUIScale,
		OverFlowProtect
	}

	public RectTransform rt;

	[SerializeField]
	public ScaleType scaleType = ScaleType.DivUIScale;

	[SerializeField]
	public float scale = 1f;

	public bool setInAwake;

	public bool setInStart = true;

	[ContextMenu("Set")]
	public void Set()
	{
		Set(scaleType);
	}

	public void Set(ScaleType scaleType)
	{
		this.scaleType = scaleType;
		switch (scaleType)
		{
		case ScaleType.UIScale:
			SetGameSettings();
			break;
		case ScaleType.DivUIScale:
			SetGameSettingsReciprocal();
			break;
		case ScaleType.OverFlowProtect:
			OverFlowProtector();
			break;
		}
	}

	public void Set(float scale)
	{
		this.scale = scale;
		Set(scaleType);
	}

	public void Awake()
	{
		rt = GetComponent<RectTransform>();
		if (setInAwake)
		{
			Set();
		}
	}

	public void Start()
	{
		GameEvents.OnGameSettingsApplied.Add(Set);
		if (setInStart)
		{
			Set();
		}
	}

	public void OnDestroy()
	{
		GameEvents.OnGameSettingsApplied.Remove(Set);
	}

	public void SetGameSettings()
	{
		rt.localScale = Vector3.one * (scale * GameSettings.UI_SCALE);
	}

	public void SetGameSettingsReciprocal()
	{
		rt.localScale = Vector3.one * (scale / GameSettings.UI_SCALE);
	}

	public void OverFlowProtector()
	{
		float num = Screen.width;
		float num2 = Screen.height;
		DragPanel component = GetComponent<DragPanel>();
		float num3 = 0f;
		num3 = ((!(component != null)) ? 60f : ((float)component.edgeOffset));
		num -= num3;
		num2 -= num3;
		float num4 = rt.sizeDelta.x * GameSettings.UI_SCALE;
		float num5 = rt.sizeDelta.y * GameSettings.UI_SCALE;
		float num6 = 0f;
		float num7 = 0f;
		if (num5 > num2 || num4 > num)
		{
			num6 = num / num4;
			num7 = num2 / num5;
			rt.localScale = Vector3.one * Mathf.Min(num6, num7);
		}
	}

	public void Update()
	{
	}
}
