using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace ns2;

public abstract class TooltipController : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ITooltipController
{
	public Tooltip prefabType;

	public Selectable selectableBase;

	public bool RequireInteractable = true;

	public Tooltip TooltipPrefabInstance { get; set; }

	public RectTransform TooltipPrefabInstanceTransform { get; set; }

	public Tooltip TooltipPrefabType
	{
		get
		{
			if (prefabType == null)
			{
				FieldInfo[] fields = GetType().GetFields();
				int num = fields.Length;
				while (num-- > 0)
				{
					FieldInfo fieldInfo = fields[num];
					if (!fieldInfo.IsStatic && fieldInfo.FieldType.IsSubclassOf(typeof(Tooltip)))
					{
						Tooltip tooltip = fieldInfo.GetValue(this) as Tooltip;
						if (tooltip != null)
						{
							prefabType = tooltip;
						}
						break;
					}
				}
			}
			return prefabType;
		}
		set
		{
			prefabType = value;
		}
	}

	string ITooltipController.name
	{
		get
		{
			return base.name;
		}
		set
		{
			base.name = value;
		}
	}

	public TooltipController()
	{
	}

	public virtual void Awake()
	{
		selectableBase = base.gameObject.GetComponent<Selectable>();
		GameEvents.OnAppFocus.Add(OnAppFocus);
	}

	public virtual void OnDestroy()
	{
		GameEvents.OnAppFocus.Remove(OnAppFocus);
		if (UIMasterController.Instance != null)
		{
			UIMasterController.Instance.DespawnTooltip(this);
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		if (UIMasterController.Instance != null && (selectableBase == null || (selectableBase != null && (selectableBase.interactable || !RequireInteractable))))
		{
			UIMasterController.Instance.SpawnTooltip(this);
		}
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		OnDestroy();
	}

	public virtual void OnAppFocus(bool focus)
	{
		if (!focus)
		{
			OnDestroy();
		}
	}

	public virtual void OnDisable()
	{
		OnDestroy();
	}

	public virtual bool OnTooltipAboutToSpawn()
	{
		return true;
	}

	public virtual void OnTooltipSpawned(Tooltip instance)
	{
	}

	public virtual bool OnTooltipAboutToDespawn()
	{
		return true;
	}

	public virtual void OnTooltipDespawned(Tooltip instance)
	{
	}

	public virtual bool OnTooltipUpdate(Tooltip instance)
	{
		return true;
	}
}
