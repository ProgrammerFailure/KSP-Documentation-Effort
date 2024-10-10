using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Expansions.Missions.Editor;

public class MEGUIPanel : MonoBehaviour
{
	public enum PanelState
	{
		Normal,
		Hidden,
		Maximized
	}

	public enum PanelSizeStyle
	{
		Absolute,
		Normalized
	}

	public enum PanelAnchor
	{
		Left,
		Right,
		Top,
		Bottom,
		None
	}

	public PanelAnchor Anchor;

	public PanelSizeStyle SizeStyle;

	public RectTransform ContentRoot;

	public float minWidth;

	public float minHeight;

	public float preferredWidth;

	public float preferredHeight;

	public float maxWidth;

	public float maxHeight;

	public Toggle VisibilityToggle;

	public Toggle MaximizeToggle;

	public RectTransform MaximizeParent;

	public MEGUIPanel[] HideOnMaximize;

	public bool isDirty;

	public float currentWidth;

	public float currentHeight;

	public LayoutElement layoutElement;

	public RectTransform parent;

	public bool isMoving;

	public List<MEGUIPanel> hiddenPanels;

	public List<MEGUIPanel> childPanels;

	public PanelState State { get; set; }

	public float CurrentWidth
	{
		get
		{
			return currentWidth;
		}
		set
		{
			currentWidth = value;
			isDirty = true;
		}
	}

	public float CurrentHeight
	{
		get
		{
			return currentHeight;
		}
		set
		{
			currentHeight = value;
			isDirty = true;
		}
	}

	public void Awake()
	{
		layoutElement = GetComponent<LayoutElement>();
		parent = base.transform.parent as RectTransform;
		hiddenPanels = new List<MEGUIPanel>();
		if (SizeStyle == PanelSizeStyle.Normalized)
		{
			minWidth *= Screen.width;
			minHeight *= Screen.height;
			preferredWidth *= Screen.width;
			preferredHeight *= Screen.height;
			maxWidth *= Screen.width;
			maxHeight *= Screen.height;
		}
		CurrentWidth = preferredWidth;
		CurrentHeight = preferredHeight;
		childPanels = new List<MEGUIPanel>(GetComponentsInChildren<MEGUIPanel>());
	}

	public void Start()
	{
		if (VisibilityToggle != null)
		{
			VisibilityToggle.onValueChanged.AddListener(OnVisibilityValueChange);
		}
		if (MaximizeToggle != null)
		{
			MaximizeToggle.onValueChanged.AddListener(OnMaximizeValueChange);
		}
	}

	public void FixedUpdate()
	{
		if (isDirty)
		{
			layoutElement.minWidth = currentWidth;
			layoutElement.minHeight = currentHeight;
			isDirty = false;
		}
	}

	public void OnVisibilityValueChange(bool value)
	{
		if (value)
		{
			Hide(enableUI: true);
		}
		else
		{
			Show(enableUI: true);
		}
	}

	public void Show(bool enableUI = false)
	{
		base.gameObject.SetActive(value: true);
		if (isMoving)
		{
			return;
		}
		if (State == PanelState.Hidden)
		{
			State = PanelState.Normal;
			if (Anchor != PanelAnchor.None)
			{
				StartCoroutine(VisibilityShowAnimation());
			}
			else
			{
				base.gameObject.SetActive(value: true);
			}
		}
		if (VisibilityToggle != null)
		{
			VisibilityToggle.GetComponent<CanvasGroup>().alpha = (enableUI ? 1 : 0);
		}
	}

	public void Hide(bool enableUI = false)
	{
		if (isMoving)
		{
			return;
		}
		if (State == PanelState.Normal)
		{
			State = PanelState.Hidden;
			if (Anchor != PanelAnchor.None)
			{
				StartCoroutine(VisibilityHideAnimation());
			}
			else
			{
				base.gameObject.SetActive(value: false);
			}
		}
		if (VisibilityToggle != null)
		{
			VisibilityToggle.GetComponent<CanvasGroup>().alpha = (enableUI ? 1 : 0);
		}
	}

	public IEnumerator VisibilityHideAnimation()
	{
		isMoving = true;
		if (Anchor != 0 && Anchor != PanelAnchor.Right)
		{
			float speed2 = (float)Screen.height / 0.5f;
			while (layoutElement.minWidth > 0f)
			{
				layoutElement.minHeight = Mathf.Clamp(layoutElement.minHeight - speed2 * Time.deltaTime, 0f, Screen.height);
				yield return null;
			}
		}
		else
		{
			float speed2 = (float)Screen.width / 0.5f;
			while (!(layoutElement.minWidth <= 0f))
			{
				layoutElement.minWidth = Mathf.Clamp(layoutElement.minWidth - speed2 * Time.deltaTime, 0f, Screen.width);
				yield return null;
			}
		}
		base.gameObject.SetActive(value: false);
		RebuildPanelLayout();
		isMoving = false;
	}

	public IEnumerator VisibilityShowAnimation()
	{
		isMoving = true;
		if (Anchor != 0 && Anchor != PanelAnchor.Right)
		{
			float speed2 = (float)Screen.height / 0.5f;
			while (layoutElement.minHeight < currentHeight)
			{
				layoutElement.minHeight = Mathf.Clamp(layoutElement.minHeight + speed2 * Time.deltaTime, 0f, currentHeight);
				yield return null;
			}
		}
		else
		{
			float speed2 = (float)Screen.width / 0.5f;
			while (!(layoutElement.minWidth >= CurrentWidth))
			{
				layoutElement.minWidth = Mathf.Clamp(layoutElement.minWidth + speed2 * Time.deltaTime, 0f, CurrentWidth);
				yield return null;
			}
		}
		RebuildPanelLayout();
		isMoving = false;
	}

	public void OnMaximizeValueChange(bool value)
	{
		if (value)
		{
			Maximize();
		}
		else
		{
			Restore();
		}
	}

	public void Maximize()
	{
		if (State != 0)
		{
			return;
		}
		State = PanelState.Maximized;
		int num = HideOnMaximize.Length;
		while (num-- > 0)
		{
			hiddenPanels.Add(HideOnMaximize[num]);
			if (HideOnMaximize[num].State == PanelState.Normal)
			{
				HideOnMaximize[num].Hide();
			}
			else if (HideOnMaximize[num].VisibilityToggle != null)
			{
				HideOnMaximize[num].VisibilityToggle.GetComponent<CanvasGroup>().alpha = 0f;
			}
		}
		if (MaximizeParent != null)
		{
			MaximizeParent.gameObject.SetActive(value: true);
			base.transform.SetParent(MaximizeParent, worldPositionStays: false);
		}
	}

	public void Restore()
	{
		if (State != PanelState.Maximized)
		{
			return;
		}
		State = PanelState.Normal;
		int num = HideOnMaximize.Length;
		while (num-- > 0)
		{
			if (hiddenPanels.Contains(HideOnMaximize[num]))
			{
				HideOnMaximize[num].Show(enableUI: true);
				HideOnMaximize[num].Reset();
			}
		}
		hiddenPanels.Clear();
		if (MaximizeParent != null)
		{
			MaximizeParent.gameObject.SetActive(value: false);
			base.transform.SetParent(parent, worldPositionStays: false);
		}
	}

	public void Reset()
	{
		CurrentWidth = preferredWidth;
		CurrentHeight = preferredHeight;
		if (VisibilityToggle != null)
		{
			VisibilityToggle.isOn = false;
		}
		if (MaximizeToggle != null)
		{
			MaximizeToggle.isOn = false;
		}
	}

	public void RebuildPanelLayout()
	{
		LayoutRebuilder.MarkLayoutForRebuild(ContentRoot);
		foreach (MEGUIPanel childPanel in childPanels)
		{
			LayoutRebuilder.MarkLayoutForRebuild(childPanel.ContentRoot);
		}
	}
}
