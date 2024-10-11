using System.Runtime.CompilerServices;
using CommNet;
using KSP.UI.TooltipTypes;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI.Screens.Flight;

public class TelemetryUpdate : MonoBehaviour
{
	public CommNetUIModeButton modeButton;

	public Sprite NOSIG;

	public Sprite NOEP;

	public Sprite BLK;

	public Sprite AUP;

	public Sprite ADN;

	public Sprite EP0;

	public Sprite EP1;

	public Sprite EP2;

	public Sprite CK1;

	public Sprite CK2;

	public Sprite CK3;

	public Sprite CP1;

	public Sprite CP2;

	public Sprite CP3;

	public Sprite SS0;

	public Sprite SS1;

	public Sprite SS2;

	public Sprite SS3;

	public Sprite SS4;

	public Image arrow_icon;

	public Image firstHop_icon;

	public Image lastHop_icon;

	public Image control_icon;

	public Image signal_icon;

	public TooltipController_Text firstHop_tooltip;

	public TooltipController_Text arrow_tooltip;

	public TooltipController_Text lastHop_tooltip;

	public TooltipController_Text control_tooltip;

	public TooltipController_SignalStrength signal_tooltip;

	private static string cacheAutoLOC_461342;

	private static string cacheAutoLOC_461346;

	private static string cacheAutoLOC_461350;

	private static string cacheAutoLOC_461354;

	private static string cacheAutoLOC_461358;

	private static string cacheAutoLOC_461362;

	private static string cacheAutoLOC_461366;

	private static string cacheAutoLOC_461381;

	private static string cacheAutoLOC_461382;

	private static string cacheAutoLOC_461402;

	private static string cacheAutoLOC_461407;

	private static string cacheAutoLOC_461418;

	private static string cacheAutoLOC_461422;

	private static string cacheAutoLOC_461426;

	private static string cacheAutoLOC_461436;

	private static string cacheAutoLOC_461442;

	private static string cacheAutoLOC_461446;

	public static TelemetryUpdate Instance
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		protected set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public TelemetryUpdate()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Start()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ClearGui()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetIcon(Image image, Sprite sprite, bool disableOnBlank = true)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void SetEVAUI(Vessel v)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void ShowModeButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected virtual void HideModeButton()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal static void CacheLocalStrings()
	{
		throw null;
	}
}
