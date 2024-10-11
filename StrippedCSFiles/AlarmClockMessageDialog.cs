using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AlarmClockMessageDialog : MonoBehaviour
{
	[Header("UI Components")]
	[SerializeField]
	private TextMeshProUGUI textHeader;

	[SerializeField]
	private TextMeshProUGUI textDate;

	[SerializeField]
	private TextMeshProUGUI textDescription;

	[SerializeField]
	private TextMeshProUGUI textWarpState;

	[SerializeField]
	private Button deleteOnClose;

	[SerializeField]
	private Button jumpButton;

	[SerializeField]
	private Button closeButton;

	private AlarmTypeBase alarm;

	private Callback onClose;

	private bool isClosing;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockMessageDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AlarmClockMessageDialog Spawn(AlarmTypeBase alarm, Callback onClose = null)
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
	private void OnDeleteOnClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnJump()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnClose()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void CloseDialog()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnAlarmRemoved(uint alarmID)
	{
		throw null;
	}
}
