using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.Editor;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

[RequireComponent(typeof(Button), typeof(PointerEnterExitHandler))]
public class EditorPartIcon : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003CWaitAndTakePartSnapshot_003Ed__81 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public EditorPartIcon _003C_003E4__this;

		public StoredPart sPart;

		public string thumbnailPath;

		public int variantIdx;

		private string _003CfullFileName_003E5__2;

		private int _003Cframes_003E5__3;

		private Texture2D _003Ctex2_003E5__4;

		object IEnumerator<object>.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		object IEnumerator.Current
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			[DebuggerHidden]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		public _003CWaitAndTakePartSnapshot_003Ed__81(int _003C_003E1__state)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool MoveNext()
		{
			throw null;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	public Color experimentalPartColor;

	public Button btnSpawnPart;

	public Button btnRemove;

	public Button btnAdd;

	public Button btnSwapTexture;

	public Button btnPlacePart;

	public TextMeshProUGUI stackAmountText;

	public RawImage inventoryItemThumbnail;

	public Slider stackSlider;

	[SerializeField]
	private float placeOffsetWhenStack;

	public bool inInventory;

	private int inventorySlotIndex;

	private PointerEnterExitHandler hoverHandler;

	private EditorPartList partList;

	private AvailablePart availablePart;

	public float iconSize;

	public float iconOverScale;

	public float iconOverSpin;

	public int variantIndex;

	private bool checkedExperimental;

	[SerializeField]
	private bool isCargoPart;

	[SerializeField]
	private bool isDeployablePart;

	public bool isFlag;

	public FlagDecalBackground flagDecalBackground;

	public Material[] materials;

	public Color missionRequiredPartColor;

	private bool checkedMissionRequired;

	public Callback<EditorPartIcon> PlacePartCallback;

	public PartIcon partIcon;

	private SpriteState buttonState;

	public bool isPart;

	public bool isEmptySlot;

	public PartUpgradeHandler.Upgrade upgrade;

	private UIPartActionInventorySlot UIPAIS;

	private WaitForEndOfFrame wfoef;

	private string thumbnailPath;

	public Image highlightImage;

	private RectTransform btnPlacePartRect;

	private Vector2 placeButtonPosition;

	private float placePositionXInitial;

	private bool changingDefaultVariant;

	private bool mouseOver;

	private bool stillFocused;

	private Vector3 partScale;

	private float partRotation;

	private Quaternion startRot;

	public AvailablePart AvailPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsCargoPart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsDeployablePart
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public AvailablePart partInfo
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public Transform PartIcon
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool HasIconOrThumbnail
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool MouseOver
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool StillFocused
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool Focused
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool isGrey
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

	public string greyoutToolTipMessage
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

	[MethodImpl(MethodImplOptions.NoInlining)]
	public EditorPartIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorPartEvent(ConstructionEventType evt, Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnInventoryPartOnMouseChanged(Part p)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(EditorPartList partList, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(EditorPartList partList, AvailablePart part, float iconSize, float iconOverScale, float iconOverSpin, Callback<EditorPartIcon> placePartCallback, bool btnPlacePartActive)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Create(EditorPartList partList, AvailablePart part, StoredPart sPart, float iconSize, float iconOverScale, float iconOverSpin, Callback<EditorPartIcon> placePartCallback, bool btnPlacePartActive, bool skipVariants, PartVariant variant, bool useImageThumbnail, bool inInventory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetEmptySlot()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void UpdateStackUI(bool isStackable, int currentStackedAmount, int maxStackedAmount)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_SpawnPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableDeleteButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableDeleteButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_Delete()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void EnableAddButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void DisableAddButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_Add()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInput_PlacePart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ToggleVariant()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnEditorDefaultVariantChanged(AvailablePart ap, PartVariant variant)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetFlagBrowserTexture(string flagURL)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool VariantsAvailable()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public PartVariant GetCurrentVariant()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InstantiatePartIcon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material[] CreateMaterialArray(GameObject gameObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Material[] CreateMaterialArray(GameObject gameObject, bool includeInactiveRenderers)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void CleanUpMaterials(GameObject gameObject)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadPartThumbnail(int variantIdx, StoredPart sPart = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CWaitAndTakePartSnapshot_003Ed__81))]
	internal IEnumerator WaitAndTakePartSnapshot(string thumbnailPath, int variantIdx, StoredPart sPart)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_PointerEnter(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void MouseInput_PointerExit(PointerEventData data)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Highlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Unhighlight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetGrey(string why)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UnsetGrey()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPartColor(GameObject partIcon, Color color, AvailablePart part = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPartColor(GameObject partIcon, Color color, bool isFlag, AvailablePart part = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPartColor(GameObject partIcon, Color color, bool processUnlit, bool isFlag, AvailablePart part = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void SetPartColor(Renderer[] renderers, Color color, bool processUnlit, bool isFlag, AvailablePart part = null)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private static bool SetMatchingIconPrefabMaterial(AvailablePart part, Material material)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckExperimental()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CheckMissionRequired()
	{
		throw null;
	}
}
