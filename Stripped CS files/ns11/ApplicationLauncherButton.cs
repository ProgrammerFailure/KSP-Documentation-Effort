using System;
using System.Collections;
using ns2;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns11;

public class ApplicationLauncherButton : MonoBehaviour
{
	public enum AnimatedIconType
	{
		NOTIFICATION = 1
	}

	public UIRadioButton toggleButton;

	public PointerEnterExitHandler hoverController;

	public UIListItem container;

	public RawImage sprite;

	public Animator spriteAnim;

	public Callback onTrue = delegate
	{
	};

	public Callback onFalse = delegate
	{
	};

	public Callback onHover = delegate
	{
	};

	public Callback<UIRadioButton> onHoverBtn = delegate
	{
	};

	public Callback<UIRadioButton> onHoverBtnActive = delegate
	{
	};

	public Callback onHoverOut = delegate
	{
	};

	public Callback<UIRadioButton> onHoverOutBtn = delegate
	{
	};

	public Callback onEnable = delegate
	{
	};

	public Callback onDisable = delegate
	{
	};

	public Callback onLeftClick = delegate
	{
	};

	public Callback<UIRadioButton> onLeftClickBtn = delegate
	{
	};

	public Callback onRightClick = delegate
	{
	};

	public ApplicationLauncher.AppScenes visibleInScenes = ApplicationLauncher.AppScenes.ALWAYS;

	public float animStartTime = -1f;

	public float animDuration = -1f;

	public Coroutine animCoroutine;

	public bool IsEnabled { get; set; }

	public bool IsHovering { get; set; }

	public ApplicationLauncher.AppScenes VisibleInScenes
	{
		get
		{
			return visibleInScenes;
		}
		set
		{
			visibleInScenes = value;
			ApplicationLauncher.Instance.DetermineVisibility(this);
		}
	}

	public void OnClick(PointerEventData data, UIRadioButton.State state, UIRadioButton.CallType callType)
	{
		if (data.button == PointerEventData.InputButton.Left)
		{
			onLeftClick();
			onLeftClickBtn(toggleButton);
		}
		else
		{
			onRightClick();
		}
	}

	public void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
	{
		onTrue();
	}

	public void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
	{
		onFalse();
	}

	public void OnHover(PointerEventData data)
	{
		if (IsEnabled)
		{
			IsHovering = true;
			onHover();
			onHoverBtn(toggleButton);
			onHoverBtnActive(toggleButton);
		}
	}

	public void OnHoverOut(PointerEventData data)
	{
		if (IsEnabled)
		{
			IsHovering = false;
			onHoverOut();
			onHoverOutBtn(toggleButton);
		}
	}

	public void Awake()
	{
		IsEnabled = true;
		toggleButton.onClick.AddListener(OnClick);
		toggleButton.onTrue.AddListener(OnTrue);
		toggleButton.onFalse.AddListener(OnFalse);
		hoverController.onPointerEnter.AddListener(OnHover);
		hoverController.onPointerExit.AddListener(OnHoverOut);
		GameEvents.onGameSceneLoadRequested.Add(StopAnim);
	}

	public void OnDestroy()
	{
		GameEvents.onGameSceneLoadRequested.Remove(StopAnim);
		if (base.gameObject != null)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Setup(Texture texture)
	{
		SetTexture(texture);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Texture texture)
	{
		onTrue = (Callback)Delegate.Combine(onTrue, onButtonTrue);
		onFalse = (Callback)Delegate.Combine(onFalse, onButtonFalse);
		Setup(texture);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Texture texture)
	{
		onHover = (Callback)Delegate.Combine(onHover, onButtonHover);
		onHoverOut = (Callback)Delegate.Combine(onHoverOut, onButtonHoverOut);
		Setup(onButtonTrue, onButtonFalse, texture);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Callback onButtonEnable, Callback onButtonDisable, Texture texture)
	{
		onDisable = (Callback)Delegate.Combine(onDisable, onButtonDisable);
		onEnable = (Callback)Delegate.Combine(onEnable, onButtonEnable);
		Setup(onButtonTrue, onButtonFalse, onButtonHover, onButtonHoverOut, texture);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Callback onButtonEnable, Callback onButtonDisable, ApplicationLauncher.AppScenes visibleInScenes, Texture texture)
	{
		Setup(onButtonTrue, onButtonFalse, onButtonHover, onButtonHoverOut, onButtonEnable, onButtonDisable, texture);
		VisibleInScenes = visibleInScenes;
	}

	public void Setup(Animator sprite)
	{
		SetSprite(sprite);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Animator sprite)
	{
		onTrue = (Callback)Delegate.Combine(onTrue, onButtonTrue);
		onFalse = (Callback)Delegate.Combine(onFalse, onButtonFalse);
		Setup(sprite);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Animator sprite)
	{
		onHover = (Callback)Delegate.Combine(onHover, onButtonHover);
		onHoverOut = (Callback)Delegate.Combine(onHoverOut, onButtonHoverOut);
		Setup(onButtonTrue, onButtonFalse, sprite);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Callback onButtonEnable, Callback onButtonDisable, Animator sprite)
	{
		onDisable = (Callback)Delegate.Combine(onDisable, onButtonDisable);
		onEnable = (Callback)Delegate.Combine(onEnable, onButtonEnable);
		Setup(onButtonTrue, onButtonFalse, onButtonHover, onButtonHoverOut, sprite);
	}

	public void Setup(Callback onButtonTrue, Callback onButtonFalse, Callback onButtonHover, Callback onButtonHoverOut, Callback onButtonEnable, Callback onButtonDisable, ApplicationLauncher.AppScenes visibleInScenes, Animator sprite)
	{
		Setup(onButtonTrue, onButtonFalse, onButtonHover, onButtonHoverOut, onButtonEnable, onButtonDisable, sprite);
		VisibleInScenes = visibleInScenes;
	}

	public void SetTexture(Texture texture)
	{
		sprite.texture = texture;
	}

	public void SetSprite(Animator sprite)
	{
		spriteAnim = sprite;
		this.sprite.color = Color.clear;
		spriteAnim.gameObject.transform.SetParent(this.sprite.gameObject.transform, worldPositionStays: false);
		spriteAnim.transform.localPosition = new Vector3(0f, 0f, -1f);
	}

	public void PlayAnim(AnimatedIconType animationType, float duration)
	{
		animStartTime = Time.realtimeSinceStartup;
		animDuration = duration;
		if (animCoroutine == null && base.gameObject.activeInHierarchy)
		{
			animCoroutine = StartCoroutine(AnimCoroutine(animationType));
		}
	}

	public IEnumerator AnimCoroutine(AnimatedIconType animationType)
	{
		PlayAnim(animationType);
		while (animStartTime + animDuration > Time.realtimeSinceStartup)
		{
			yield return null;
		}
		StopAnim();
	}

	public void PlayAnim(AnimatedIconType animationType)
	{
		if (spriteAnim != null && spriteAnim.isInitialized)
		{
			spriteAnim.SetTrigger(animationType.ToString().ToLower());
		}
	}

	public void StopAnim()
	{
		if (spriteAnim != null && spriteAnim.isInitialized)
		{
			spriteAnim.SetTrigger("idle");
		}
		animCoroutine = null;
	}

	public void StopAnim(GameScenes scene)
	{
		StopAnim();
	}

	public Vector3 GetAnchor()
	{
		Vector3 position = container.transform.position;
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			return new Vector3(position.x + 3f, position.y - 41f, position.z);
		}
		return new Vector3(position.x + 41f, position.y + 3f, position.z);
	}

	public Vector3 GetAnchorLocal()
	{
		Vector3 localPosition = container.transform.localPosition;
		if (ApplicationLauncher.Instance.IsPositionedAtTop)
		{
			return new Vector3(localPosition.x + 3f, localPosition.y, localPosition.z);
		}
		return new Vector3(localPosition.x + 41f, localPosition.y + 3f, localPosition.z);
	}

	public Vector3 GetAnchorUL()
	{
		Vector3[] array = new Vector3[4];
		((RectTransform)base.transform).GetWorldCorners(array);
		return array[1];
	}

	public Vector3 GetAnchorUR()
	{
		Vector3[] array = new Vector3[4];
		((RectTransform)base.transform).GetWorldCorners(array);
		return array[2];
	}

	public Vector3 GetAnchorTopRight()
	{
		return new Vector3(-41f, 0f, 0f);
	}

	public void SetTrue(bool makeCall = true)
	{
		if (makeCall)
		{
			toggleButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATION, null);
		}
		else
		{
			toggleButton.SetState(UIRadioButton.State.True, UIRadioButton.CallType.APPLICATIONSILENT, null);
		}
	}

	public void SetFalse(bool makeCall = true)
	{
		if (makeCall)
		{
			toggleButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATION, null);
		}
		else
		{
			toggleButton.SetState(UIRadioButton.State.False, UIRadioButton.CallType.APPLICATIONSILENT, null);
		}
	}

	public void Enable(bool makeCall = true)
	{
		toggleButton.Interactable = true;
		IsEnabled = true;
		if (makeCall)
		{
			onEnable();
		}
	}

	public void Disable(bool makeCall = true)
	{
		toggleButton.Interactable = false;
		IsEnabled = false;
		if (makeCall)
		{
			onDisable();
		}
	}
}
