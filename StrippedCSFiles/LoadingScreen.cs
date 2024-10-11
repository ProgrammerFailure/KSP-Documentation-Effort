using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using KSP.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
	[Serializable]
	public class LoadingScreenState
	{
		public UnityEngine.Object[] screens;

		public float fadeInTime;

		public float displayTime;

		public float fadeOutTime;

		public string[] tips;

		public float tipTime;

		[HideInInspector]
		public Texture2D activeScreen;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public LoadingScreenState()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CLoadSystems_003Ed__11 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadingScreen _003C_003E4__this;

		private int _003CcurrentIndex_003E5__2;

		private float _003CstartTime_003E5__3;

		private LoadingSystem _003Ccurrent_003E5__4;

		private float _003CloadStep_003E5__5;

		private float _003CtotalWeight_003E5__6;

		private float _003CloadDelta_003E5__7;

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
		public _003CLoadSystems_003Ed__11(int _003C_003E1__state)
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
	private sealed class _003CUpdateLoadingScreen_003Ed__27 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadingScreen _003C_003E4__this;

		private int _003CcurrentIndex_003E5__2;

		private LoadingScreenState _003Ccurrent_003E5__3;

		private int _003CiSuccess_003E5__4;

		private int _003Ci_003E5__5;

		private UnityWebRequest _003Cuwr_003E5__6;

		private int _003CisDoneRepeat_003E5__7;

		private float _003CtipTime_003E5__8;

		private float _003CendTime_003E5__9;

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
		public _003CUpdateLoadingScreen_003Ed__27(int _003C_003E1__state)
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
		private void _003C_003Em__Finally1()
		{
			throw null;
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw null;
		}
	}

	[CompilerGenerated]
	private sealed class _003CFadeOut_003Ed__29 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadingScreenState current;

		public LoadingScreen _003C_003E4__this;

		private float _003Calpha_003E5__2;

		private float _003CdAlpha_003E5__3;

		private float _003CendTime_003E5__4;

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
		public _003CFadeOut_003Ed__29(int _003C_003E1__state)
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
	private sealed class _003CFadeIn_003Ed__30 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public LoadingScreenState current;

		public LoadingScreen _003C_003E4__this;

		private float _003Calpha_003E5__2;

		private float _003CdAlpha_003E5__3;

		private float _003CendTime_003E5__4;

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
		public _003CFadeIn_003Ed__30(int _003C_003E1__state)
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

	public static GameScenes loadScene;

	public static float minFrameTime;

	public List<LoadingSystem> loaders;

	[SerializeField]
	private List<LoadingScreenState> screens;

	private AspectRatioFitter aspectFitter;

	private GameObject screenInstance;

	[SerializeField]
	[Tooltip("The index of the list inside screens that contains the randomized loading screens. This list is the one that will have user screens added to it")]
	private int loadingScreensListIndex;

	private string userScreensPath;

	private bool addedCustomScreens;

	private string[] customScreenExtensions;

	[SerializeField]
	[Tooltip("How many frames to wait for isDone from the uwr if the request is finished, but the content is not done yet")]
	private int loadingScreenIsDoneRetryLimit;

	private string[] filePaths;

	private FileInfo fileInfo;

	[SerializeField]
	private ProgressBar scrollBar;

	[SerializeField]
	private TextMeshProUGUI scrollBarText;

	[SerializeField]
	private TextMeshProUGUI scrollBarTextMasked;

	[SerializeField]
	private RawImage screenImage;

	[SerializeField]
	private CanvasGroup screenMask;

	[SerializeField]
	private TextMeshProUGUI tipsText;

	public static LoadingScreen Instance
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

	public List<LoadingScreenState> Screens
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
	public LoadingScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	static LoadingScreen()
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
	public static void StartLoadingScreens()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void OnDestroy()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoadSystems_003Ed__11))]
	private IEnumerator LoadSystems()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void UpdateProgressBar(string title, float fraction)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CUpdateLoadingScreen_003Ed__27))]
	private IEnumerator UpdateLoadingScreen()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetTip(LoadingScreenState current)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFadeOut_003Ed__29))]
	private IEnumerator FadeOut(LoadingScreenState current)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CFadeIn_003Ed__30))]
	private IEnumerator FadeIn(LoadingScreenState current)
	{
		throw null;
	}
}
