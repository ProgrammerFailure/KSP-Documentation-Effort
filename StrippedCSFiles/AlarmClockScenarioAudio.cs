using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class AlarmClockScenarioAudio : MonoBehaviour
{
	internal delegate void AudioEventArgs(AlarmClockScenarioAudio sender, AudioClip clip);

	public class AudioClipMap
	{
		private readonly IDictionary<string, AudioClip> _clips;

		public AudioClip this[string key]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public AudioClipMap()
		{
			throw null;
		}
	}

	private AudioClipMap alarmSounds;

	private AudioSource audioSource;

	public const string soundBaseUrl = "Squad/Alarms/Sounds/";

	[CompilerGenerated]
	private AudioEventArgs onPlayFinished;

	[CompilerGenerated]
	private AudioEventArgs onPlayStarted;

	private bool playing;

	private int repeatCounter;

	private int repeatLimit;

	public AudioClipMap AlarmSounds
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal bool isPlaying
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	internal event AudioEventArgs _001E
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	internal event AudioEventArgs _0003
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		add
		{
			throw null;
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		[CompilerGenerated]
		remove
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public AlarmClockScenarioAudio()
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
	private void InitializeSoundController()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public List<string> GetStockAlarmSounds()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool PlaySound(string soundURL, int repeats = 1)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal void Stop()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	internal bool isClipPlaying(AudioClip clip)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void Update()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public static AudioClip LoadAudioClip(string soundName)
	{
		throw null;
	}
}
