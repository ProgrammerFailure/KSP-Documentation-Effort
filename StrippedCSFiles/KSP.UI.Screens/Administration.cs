using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Strategies;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class Administration : MonoBehaviour
{
	public class StrategyWrapper
	{
		public Strategy strategy
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public StrategyListItem stratListIcon
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public UIRadioButton button
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			get
			{
				throw null;
			}
			[MethodImpl(MethodImplOptions.NoInlining)]
			[CompilerGenerated]
			private set
			{
				throw null;
			}
		}

		public UIRadioButton ButtonInUse
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public StrategyWrapper(Strategy strategy, StrategyListItem stratListIcon)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public StrategyWrapper(Strategy strategy, UIRadioButton button)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnTrue(PointerEventData data, UIRadioButton.CallType callType)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnFalse(PointerEventData data, UIRadioButton.CallType callType)
		{
			throw null;
		}
	}

	public static Administration Instance;

	public UIStateButton btnAcceptCancel;

	public Slider sliderCommitment;

	public RectTransform panelCommitmentSlider;

	public RawImage selectedStrategyImage;

	public TextMeshProUGUI selectedStrategyTitle;

	public TextMeshProUGUI selectedStrategyDescription;

	public UIStatePanel textPanel;

	public UIList scrollListStrategies;

	public UIList scrollListKerbals;

	public UIList scrollListActive;

	public UIListItem prefabStratListContainer;

	public UIListItem prefabStratListItem;

	public UIListItem prefabKerbalItem;

	public UIListItem prefabActiveStrat;

	public Texture defaultIcon;

	public TextMeshProUGUI activeStratCount;

	private GameObject avatarLighting;

	private PopupDialog strategyConfirmationDialog;

	private int activeStrategyCount;

	private int maxActiveStrategies;

	private float maxStrategyCommitLevel;

	public StrategyWrapper SelectedWrapper
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		private set
		{
			throw null;
		}
	}

	public int ActiveStrategyCount
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public int MaxActiveStrategies
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public float MaxStrategyCommitLevel
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public Administration()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateStrategiesList(List<DepartmentConfig> departments)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddStrategiesListItem(UIList itemList, List<Strategy> strategies)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddKerbalListItem(DepartmentConfig dep)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStrategyCount()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateActiveStratList()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private UIListItem CreateActiveStratItem(Strategy strategy, out StrategyWrapper wrapper)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private StrategyWrapper AddActiveStratItem(Strategy strategy)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSelectedStrategy(StrategyWrapper wrapper)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnselectStrategy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetStrategyDescription(Texture image, string title, string description, string effects, string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStrategyDescription(string title, string description, string effects, string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private string GetStrategyDescription(string description, string effects, string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void BtnInputAccept(string state)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAcceptConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnCancelConfirm()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPopupDismiss()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RedrawPanels()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnSliderCommitmentValueChanged(float value)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetEffectSliderValue(float value, float min, float max)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateStrategyStats()
	{
		throw null;
	}
}
