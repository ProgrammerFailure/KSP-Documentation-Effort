using System.Collections;
using ns2;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMarquee : MonoBehaviour
{
	public enum ScrollType
	{
		Left,
		Right,
		LeftThenRight,
		RightThenLeft
	}

	[SerializeField]
	public TextMeshProUGUI targetLabel;

	public Transform labelParentGO;

	public GameObject marqueeObject;

	public float marqueeWidth;

	public ContentSizeFitter labelFitter;

	public RectMask2D mask;

	public Vector3 movingTransform;

	public float leftX;

	public float rightX;

	public float leftXBound;

	public float rightXBound;

	public PointerEnterExitHandler enterExitHandler;

	public bool movingLeft;

	public ScrollType scrollMode = ScrollType.LeftThenRight;

	public bool loop;

	public float delayAfterLoop = 1f;

	public float delayBeforeLoop = 0.7f;

	public bool autoScrollOnShow = true;

	public bool stopOnMouseOff = true;

	public float startDelay = 0.4f;

	public float movingOverrun = 10f;

	public bool isMoving;

	public TextOverflowModes staticOverflowMode = TextOverflowModes.Ellipsis;

	public TextOverflowModes movingOverflowMode;

	public TextAlignmentOptions staticHorizontalAlign = TextAlignmentOptions.MidlineRight;

	public TextAlignmentOptions movingHorizontalAlign = TextAlignmentOptions.MidlineLeft;

	public bool ready;

	public bool isActive;

	public bool isMouseOver;

	public bool isRightAnchored;

	public IEnumerator marqueeMovementCoroutine;

	public float originalFontSizeMax;

	public float movingSpeed = 20f;

	public float movementAmount;

	public virtual void Awake()
	{
		if (targetLabel == null)
		{
			Object.Destroy(this);
			return;
		}
		labelParentGO = targetLabel.transform.parent;
		int siblingIndex = targetLabel.transform.GetSiblingIndex();
		marqueeObject = new GameObject("marqueeHolder");
		marqueeObject.AddComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		mask = marqueeObject.AddComponent<RectMask2D>();
		enterExitHandler = marqueeObject.AddComponent<PointerEnterExitHandler>();
		enterExitHandler.onPointerEnter.AddListener(OnPointerEnter);
		enterExitHandler.onPointerExit.AddListener(OnPointerExit);
		marqueeObject.transform.SetParent(labelParentGO);
		marqueeObject.transform.SetSiblingIndex(siblingIndex);
		marqueeObject.transform.localScale = Vector3.one;
		marqueeObject.transform.localPosition = targetLabel.transform.localPosition;
		marqueeObject.transform.localRotation = targetLabel.transform.localRotation;
		(marqueeObject.transform as RectTransform).pivot = (targetLabel.transform as RectTransform).pivot;
		(marqueeObject.transform as RectTransform).anchorMin = (targetLabel.transform as RectTransform).anchorMin;
		(marqueeObject.transform as RectTransform).anchorMax = (targetLabel.transform as RectTransform).anchorMax;
		(marqueeObject.transform as RectTransform).anchoredPosition = (targetLabel.transform as RectTransform).anchoredPosition;
		(marqueeObject.transform as RectTransform).sizeDelta = (targetLabel.transform as RectTransform).sizeDelta;
		targetLabel.transform.SetParent(marqueeObject.transform);
		labelFitter = targetLabel.gameObject.AddComponent<ContentSizeFitter>();
		labelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		isRightAnchored = (targetLabel.transform as RectTransform).anchorMin.x == 1f && (targetLabel.transform as RectTransform).anchorMax.x == 1f;
		movingTransform = targetLabel.transform.localPosition;
	}

	public virtual void Start()
	{
		StartCoroutine(Initialize());
	}

	public IEnumerator Initialize()
	{
		yield return new WaitForEndOfFrame();
		marqueeWidth = (marqueeObject.transform as RectTransform).rect.width;
		Configure();
		ready = true;
		AutoStartMarquee();
	}

	public virtual void Field_OnValueModified(object arg1)
	{
		StopMarquee();
		Configure();
		AutoStartMarquee();
	}

	public virtual void OnDestroy()
	{
		StopMarquee();
		enterExitHandler.onPointerEnter.RemoveListener(OnPointerEnter);
		enterExitHandler.onPointerExit.RemoveListener(OnPointerExit);
	}

	[ContextMenu("Reconfigure Marquee")]
	public void Configure()
	{
		originalFontSizeMax = targetLabel.fontSizeMax;
		targetLabel.fontSizeMax = targetLabel.fontSizeMin;
		labelFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
		labelFitter.enabled = true;
		_ = targetLabel.overflowMode;
		targetLabel.overflowMode = movingOverflowMode;
		targetLabel.ForceMeshUpdate();
		isActive = targetLabel.preferredWidth > marqueeWidth;
		targetLabel.fontSizeMax = originalFontSizeMax;
		targetLabel.ForceMeshUpdate();
		if (isActive)
		{
			rightX = targetLabel.preferredWidth - marqueeWidth + leftX + movingOverrun;
			targetLabel.overflowMode = staticOverflowMode;
			targetLabel.alignment = staticHorizontalAlign;
		}
		labelFitter.enabled = false;
		targetLabel.rectTransform.sizeDelta = mask.rectTransform.sizeDelta;
		movingTransform.x = leftX;
		targetLabel.transform.localPosition = movingTransform;
		targetLabel.overflowMode = staticOverflowMode;
		targetLabel.alignment = staticHorizontalAlign;
		targetLabel.ForceMeshUpdate();
	}

	public void StartMarquee()
	{
		if (!isActive)
		{
			return;
		}
		isMoving = true;
		labelFitter.enabled = true;
		targetLabel.overflowMode = movingOverflowMode;
		targetLabel.alignment = movingHorizontalAlign;
		targetLabel.ForceMeshUpdate();
		switch (scrollMode)
		{
		default:
			if (isRightAnchored)
			{
				movingTransform.x = rightX;
			}
			movingLeft = true;
			break;
		case ScrollType.Right:
		case ScrollType.RightThenLeft:
			if (isRightAnchored)
			{
				movingTransform.x = leftX;
				movingLeft = false;
			}
			else
			{
				movingTransform.x = 0f - rightX;
			}
			break;
		}
		targetLabel.transform.localPosition = movingTransform;
		StopAllCoroutines();
		marqueeMovementCoroutine = MoveMarquee();
		StartCoroutine(marqueeMovementCoroutine);
	}

	public void StopMarquee()
	{
		if (marqueeMovementCoroutine != null)
		{
			StopCoroutine(marqueeMovementCoroutine);
		}
		isMoving = false;
		labelFitter.enabled = false;
		targetLabel.rectTransform.sizeDelta = mask.rectTransform.sizeDelta;
		movingTransform.x = leftX;
		targetLabel.transform.localPosition = movingTransform;
		targetLabel.overflowMode = staticOverflowMode;
		targetLabel.alignment = staticHorizontalAlign;
		targetLabel.ForceMeshUpdate();
	}

	public void AutoStartMarquee()
	{
		if (autoScrollOnShow && isActive && !isMoving)
		{
			StartMarquee();
		}
	}

	public IEnumerator MoveMarquee()
	{
		yield return new WaitForSecondsRealtime(startDelay);
		while (isMoving)
		{
			if (!isActive || !ready)
			{
				yield return null;
			}
			if (loop)
			{
				if (isRightAnchored)
				{
					leftXBound = leftX - marqueeWidth;
					rightXBound = rightX + marqueeWidth;
				}
				else
				{
					leftXBound = 0f - rightX;
					rightXBound = 0f;
				}
			}
			else if (isRightAnchored)
			{
				leftXBound = leftX;
				rightXBound = rightX;
			}
			else
			{
				leftXBound = 0f - rightX;
				rightXBound = 0f;
			}
			movementAmount = movingSpeed * Time.unscaledDeltaTime;
			if (movingLeft)
			{
				movementAmount *= -1f;
			}
			movingTransform.x += movementAmount;
			if (movingTransform.x <= leftXBound)
			{
				if (loop && delayAfterLoop > 0f)
				{
					ScrollType scrollType = scrollMode;
					if (scrollType == ScrollType.Left || scrollType == ScrollType.RightThenLeft)
					{
						yield return new WaitForSecondsRealtime(delayAfterLoop);
						movingTransform.x = leftXBound;
						ResetMovingFlags(comingFromRight: true);
						targetLabel.transform.localPosition = movingTransform;
						yield return new WaitForSecondsRealtime(delayBeforeLoop);
					}
					yield return null;
				}
				movingTransform.x = leftXBound;
				ResetMovingFlags(comingFromRight: true);
			}
			if (movingTransform.x >= rightXBound)
			{
				if (loop && delayAfterLoop > 0f)
				{
					ScrollType scrollType = scrollMode;
					if ((uint)(scrollType - 1) <= 1u)
					{
						yield return new WaitForSecondsRealtime(delayAfterLoop);
						movingTransform.x = rightXBound;
						ResetMovingFlags(comingFromRight: false);
						targetLabel.transform.localPosition = movingTransform;
						yield return new WaitForSecondsRealtime(delayBeforeLoop);
					}
					yield return null;
				}
				movingTransform.x = rightXBound;
				ResetMovingFlags(comingFromRight: false);
			}
			targetLabel.transform.localPosition = movingTransform;
			yield return null;
		}
	}

	public void ResetMovingFlags(bool comingFromRight)
	{
		if (comingFromRight)
		{
			switch (scrollMode)
			{
			default:
				if (loop)
				{
					movingTransform.x = rightXBound;
				}
				else
				{
					StopMarquee();
				}
				break;
			case ScrollType.Right:
			case ScrollType.LeftThenRight:
				movingLeft = false;
				break;
			case ScrollType.RightThenLeft:
				if (loop)
				{
					movingLeft = false;
				}
				else
				{
					StopMarquee();
				}
				break;
			}
			return;
		}
		switch (scrollMode)
		{
		default:
			if (loop)
			{
				movingTransform.x = leftX;
			}
			else
			{
				StopMarquee();
			}
			break;
		case ScrollType.LeftThenRight:
			if (loop)
			{
				movingLeft = true;
			}
			else
			{
				StopMarquee();
			}
			break;
		case ScrollType.Left:
		case ScrollType.RightThenLeft:
			movingLeft = true;
			break;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if (isActive && ready)
		{
			if (!isMoving)
			{
				StartMarquee();
			}
			isMouseOver = true;
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if (isActive && ready)
		{
			if (stopOnMouseOff)
			{
				StopMarquee();
			}
			isMouseOver = false;
		}
	}
}
