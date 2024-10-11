using System.Collections.Generic;
using System.Runtime.CompilerServices;
using KSP.UI.Screens;
using UnityEngine;

public class ScreenMessages : MonoBehaviour
{
	public ScreenMessagesText textPrefab;

	public RectTransform lowerCenter;

	public RectTransform upperCenter;

	public RectTransform upperLeft;

	public RectTransform upperRight;

	public RectTransform kerbalEva;

	public bool useLifetimeGradient;

	public Gradient lifetimeGradient;

	public Color defaultColor;

	private List<ScreenMessage> activeMessages;

	private float[] lastPendingMessageAdded;

	private int[] pendingMessages;

	private bool[] failedAreas;

	private ScreenMessage[] firstMessage;

	private float[] waitTime;

	private float MAX_INTERVAL;

	private float MAX_WAIT;

	private bool sceneLoadInProgress;

	public static ScreenMessages Instance
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

	public List<ScreenMessage> ActiveMessages
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		set
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ScreenMessages()
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
	private void OnGameSceneLoadRequested(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnLevelLoaded(GameScenes scene)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, bool persist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration, ScreenMessageStyle style, ScreenMessage msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration, bool persist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, float duration)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, bool persist)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(string message, ScreenMessage msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static ScreenMessage PostScreenMessage(ScreenMessage msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static void RemoveMessage(ScreenMessage msg)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	protected static void SendStateMessage(string title, string message, MessageSystemButton.MessageButtonColor color, MessageSystemButton.ButtonIcons icon)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ScreenMessage PostMessage(ScreenMessage current, string msg, ScreenMessagesText textPrefab, float duration, ScreenMessageStyle style)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private ScreenMessage PostMessage(ScreenMessage current, string msg, ScreenMessagesText textPrefab, float duration, ScreenMessageStyle style, Color color)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void CreateTextInstance(ScreenMessage message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyMessages()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void DestroyMessage(ScreenMessage message)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void SetParent(Transform t, ScreenMessageStyle style)
	{
		throw null;
	}
}
