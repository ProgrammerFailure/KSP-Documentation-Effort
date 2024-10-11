using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using KSP.UI.Screens.PartListCategories;
using RUI.Icons.Selectable;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace KSP.UI.Screens;

public class PartCategorizer : BasePartCategorizer
{
	public enum ButtonType
	{
		FILTER,
		CATEGORY,
		SUBCATEGORY
	}

	public enum SelectionGroup
	{
		FILTER,
		CATEGORY
	}

	public class Category
	{
		public ButtonType buttonType;

		public EditorPartList.State displayType;

		public PartCategorizerButton button;

		public PartCategorizerButton buttonPlus;

		public List<Category> subcategories;

		public int subCategoryGroup;

		public static int subCategoryGroupIndex;

		private Category parent;

		public EditorPartListFilter<AvailablePart> exclusionFilter;

		public EditorPartListFilter<ShipTemplate> exclusionFilterSubassembly;

		public List<string> availableParts;

		public List<string> shipTemplates;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, EditorPartListFilter<AvailablePart> exclusionFilter, bool last = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Category(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, EditorPartListFilter<AvailablePart> exclusionFilter, bool addItem)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, EditorPartListFilter<ShipTemplate> exclusionFilter, bool last = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Category(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, EditorPartListFilter<ShipTemplate> exclusionFilter, bool addItem)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public Category(ButtonType buttonType, EditorPartList.State displayType, string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, bool last = false)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		static Category()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void Setup(ButtonType buttonType, EditorPartList.State displayType, PartCategorizerButton button, bool addItem)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddSubassembly(string template, bool compile, bool exclude, bool refresh)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemoveSubassembly(string template, bool compile, bool exclude, bool refresh)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CompileExclusionFilterSubassembly(bool exclude = true, bool sendRefresh = true)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddPart(AvailablePart part)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddPart(string partName, bool compile)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RemovePart(AvailablePart part)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void CompileExclusionFilter(bool sendRefresh = true)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddCustomCategorySubassembly()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AcceptNewSubcategorySubassembly()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void AddSubcategory(Category subcategory)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AddCustomCategory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void AcceptNewSubcategory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void EditCategory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DeleteCategory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void DeleteSubcategory()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool AcceptNewSubcategoryCriteria(string categoryName, out string reason)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool EditCategoryCriteria(string categoryName, out string reason)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool EditSubCategoryCriteria(string categoryName, out string reason)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool DeleteCategoryCriteria(out string reason)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private bool DeleteSubcategoryCriteria(out string reason)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnClick(PointerEventData eventData, UIRadioButton.State state, UIRadioButton.CallType callType)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnTrue(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnTrueFILTER(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnTrueCATEGORY()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnTrueSUB(Category cat)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void FlipAllFilterButtons(UIRadioButton exceptionBtn)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void FlipAllCategoryButtons(UIRadioButton exceptionBtn)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void RebuildSubcategoryButtons()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void InsertSubcategoryButtons()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private int UpdateSubcategoryStates(Category cat, int startIndex, bool insertItems)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnFalse(UIRadioButton button, UIRadioButton.CallType callType, PointerEventData data)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnFalseFilterOrCategory(Category cat)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void RemoveSubcategoryButtons(Category cat)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		private void OnFalseFromSubcategory(PointerEventData data, UIRadioButton.CallType callType)
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public void OnFalseSUB(Category cat)
		{
			throw null;
		}
	}

	public class PopupData
	{
		public string popupName;

		public string categoryName;

		public string categorydisplayName;

		public Icon icon;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public PopupData(string popupName, string categoryName, string categorydisplayName, Icon icon)
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CStart_003Ed__86 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PartCategorizer _003C_003E4__this;

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
		public _003CStart_003Ed__86(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CSetInitialState_003Ed__95 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PartCategorizer _003C_003E4__this;

		private int _003Ci_003E5__2;

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
		public _003CSetInitialState_003Ed__95(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CSearchRoutine_003Ed__99 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PartCategorizer _003C_003E4__this;

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
		public _003CSearchRoutine_003Ed__99(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CShowPopup_003Ed__149 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool yield;

		public PartCategorizer _003C_003E4__this;

		public PopupData popupData;

		public Callback onAccept;

		public PartCategorizerPopup.CriteriaAccept onAcceptCriteria;

		public Callback onDelete;

		public PartCategorizerPopup.CriteriaDelete onDeleteCriteria;

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
		public _003CShowPopup_003Ed__149(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CShowPopupAddPart_003Ed__152 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public bool yield;

		public PartCategorizer _003C_003E4__this;

		public string partName;

		public string partTitle;

		public Callback<string, Category> onAccept;

		public PartCategorizerPopupAddPart.CriteriaAccept onAcceptCriteria;

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
		public _003CShowPopupAddPart_003Ed__152(int _003C_003E1__state)
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

	[CompilerGenerated]
	private sealed class _003CUpdateDaemon_003Ed__156 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public PartCategorizer _003C_003E4__this;

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
		public _003CUpdateDaemon_003Ed__156(int _003C_003E1__state)
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

	public static PartCategorizer Instance;

	public static bool Ready;

	public UIList scrollListMain;

	public PartCategorizerButton buttonPrefab;

	public EditorPartList editorPartList;

	public TextMeshProUGUI labelCategoryName;

	public Button arrowRight;

	public Button arrowLeft;

	public PartCategorizerPopup popupPrefab;

	private PartCategorizerPopup popup;

	public PartCategorizerPopupAddPart popupAddPartPrefab;

	private PartCategorizerPopupAddPart popupAddPart;

	public RectTransform popupArea;

	public TechTier[] TechTierCategories;

	public BulkheadProfile[] BulkheadProfiles;

	public UIRadioButton btnSearch;

	private Category SelectedCategory;

	private bool refreshRequested;

	private bool subcategoryButtonRebuildRequested;

	private EditorPartList.State displayType;

	private bool searching;

	public List<Category> filters;

	public List<Category> categories;

	private Category subassemblies;

	private Category subassembliesAll;

	private Category variants;

	private new EditorPartListFilter<AvailablePart> filterPods;

	private new EditorPartListFilter<AvailablePart> filterEngine;

	private new EditorPartListFilter<AvailablePart> filterFuelTank;

	private new EditorPartListFilter<AvailablePart> filterControl;

	private new EditorPartListFilter<AvailablePart> filterStructural;

	private new EditorPartListFilter<AvailablePart> filterCoupling;

	private new EditorPartListFilter<AvailablePart> filterPayload;

	private new EditorPartListFilter<AvailablePart> filterAero;

	private new EditorPartListFilter<AvailablePart> filterGround;

	private new EditorPartListFilter<AvailablePart> filterThermal;

	private new EditorPartListFilter<AvailablePart> filterElectrical;

	private new EditorPartListFilter<AvailablePart> filterCommunication;

	private new EditorPartListFilter<AvailablePart> filterScience;

	private new EditorPartListFilter<AvailablePart> filterCargo;

	private new EditorPartListFilter<AvailablePart> filterRobotics;

	private new EditorPartListFilter<AvailablePart> filterUtility;

	internal Category filterFunction;

	public Category subcategoryFunctionPods;

	public Category subcategoryFunctionEngine;

	public Category subcategoryFunctionFuelTank;

	public Category subcategoryFunctionControl;

	public Category subcategoryFunctionStructural;

	public Category subcategoryFunctionCoupling;

	public Category subcategoryFunctionPayload;

	public Category subcategoryFunctionAero;

	public Category subcategoryFunctionGround;

	public Category subcategoryFunctionThermal;

	public Category subcategoryFunctionElectrical;

	public Category subcategoryFunctionCommunication;

	public Category subcategoryFunctionScience;

	public Category subcategoryFunctionCargo;

	public Category subcategoryFunctionRobotics;

	public Category subcategoryFunctionUtility;

	public static bool alwaysShowCargoTab;

	private EditorPartListFilter<AvailablePart> filter_LeftBar;

	private EditorPartListFilter<ShipTemplate> filterSubassemblies;

	private EditorPartListFilter<ShipTemplate> filterNothingSubassembly;

	private EditorPartListFilter<ShipTemplate> filterEverythingSubassembly;

	private EditorPartListFilter<AvailablePart> filterCustomCategory;

	public EditorPartListFilter<AvailablePart> filterGenericNothing;

	[NonSerialized]
	public Color colorFilterModule;

	[NonSerialized]
	public Color colorFilterResource;

	[NonSerialized]
	public Color colorFilterManufacturer;

	[NonSerialized]
	public Color colorFilterTech;

	[NonSerialized]
	public Color colorFilterProfile;

	[NonSerialized]
	public Color colorSubassembly;

	[NonSerialized]
	public Color colorVariants;

	[NonSerialized]
	public Color colorCategory;

	private SelectionGroup SelectedGroup;

	private Transform partCategorizerButtonRepository;

	private static ConfigNode[] configNodes;

	private static ConfigNode[] configNodesSubassembly;

	private PopupData currentPopupData;

	private bool updateDaemonRunning;

	public bool HasCustomCategories
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	public bool IsTransitioning
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
	public PartCategorizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static PartCategorizer()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CStart_003Ed__86))]
	private IEnumerator Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Setup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateCrossSectionFilters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateModuleFilters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateResourceFilters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateManufacturerFilters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateTechTierFilters()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSetInitialState_003Ed__95))]
	private IEnumerator SetInitialState()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void FocusSearchField()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SearchField_OnEndEdit(string s)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SearchField_OnClick(PointerEventData eventData)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CSearchRoutine_003Ed__99))]
	protected override IEnumerator SearchRoutine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SearchStop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected override void SearchFilterResult(EditorPartListFilter<AvailablePart> filter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionPods()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionFuelTank()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionEngine()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionControl()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionStructural()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionCoupling()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionPayload()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionAero()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionGround()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionThermal()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionElectrical()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionCommunication()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionScience()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionCargo()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionRobotics()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void SetPanel_FunctionUtility()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void ForceShowPanel(Category cat, bool ignoreActive = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static PartCategorizerButton InstantiatePartCategorizerButton(string categoryName, string categorydisplayName, Icon icon, Color colorButton, Color colorIcon, bool last = false)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private PartCategorizerButton InstantiatePartCategorizerButtonPlus(string categoryName, string categorydisplayName, Icon icon, Color colorButton, Color colorIcon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Category AddCustomFilter(string filterName, string filterdisplayName, Icon icon, Color colorButton)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static Category AddCustomSubcategoryFilter(Category mainFilter, string subFilterName, string subFilterdisplayName, Icon icon, Func<AvailablePart, bool> exclusionFilter)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void UpdateCategoryNameLabel()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void updateArrowStates()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputArrowLeft()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void MouseInputArrowRight()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSimpleMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetAdvancedMode()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadCustomPartCategories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void LoadCustomSubassemblyCategories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveCustomPartCategories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SaveCustomSubassemblyCategories()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AddPartToCategory(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemovePartFromCategory(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddSubassemblyToSelectedCategory(string st)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RemoveSubassemblyFromSelectedCategory(string st, bool refresh)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool SelectedSubassemblyIsCustom()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void AddPartToCustomCategoryViaPopup(AvailablePart part)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnPlusButtonClick()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void AcceptNewCategory()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool AcceptCriteria(string categoryName, out string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Category AddCustomCategory(string categoryName, string categorydisplayName, Icon icon, Color color, Color colorIcon, bool addStdSubcategory)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowPopup(PopupData popupData, Callback onAccept, PartCategorizerPopup.CriteriaAccept onAcceptCriteria, Callback onDelete, PartCategorizerPopup.CriteriaDelete onDeleteCriteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InstantiatePopup()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CShowPopup_003Ed__149))]
	private IEnumerator ShowPopup(PopupData popupData, Callback onAccept, PartCategorizerPopup.CriteriaAccept onAcceptCriteria, Callback onDelete, PartCategorizerPopup.CriteriaDelete onDeleteCriteria, bool yield)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void ShowPopupAddPart(string partName, string partTitle, Callback<string, Category> onAccept, PartCategorizerPopupAddPart.CriteriaAccept onAcceptCriteria)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void InstantiatePopupAddPart()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CShowPopupAddPart_003Ed__152))]
	private IEnumerator ShowPopupAddPart(string partName, string partTitle, Callback<string, Category> onAccept, PartCategorizerPopupAddPart.CriteriaAccept onAcceptCriteria, bool yield)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAcceptPopupAddPart(string partName, Category category)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private bool AcceptCriteriaPopupAddPart(string partName, Category category, out string reason)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateDaemon_003Ed__156))]
	private IEnumerator UpdateDaemon()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void PrintManufacturersUsed()
	{
		throw null;
	}
}
