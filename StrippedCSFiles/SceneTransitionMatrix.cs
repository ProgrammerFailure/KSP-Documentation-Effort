using System;
using System.Runtime.CompilerServices;

[Serializable]
public class SceneTransitionMatrix
{
	[Serializable]
	public class SceneTransitions
	{
		public bool[] To;

		public bool this[GameScenes scn]
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public SceneTransitions()
		{
			throw null;
		}
	}

	public SceneTransitions[] From;

	public SceneTransitions this[GameScenes scn]
	{
		[MethodImpl(MethodImplOptions.NoInlining)]
		get
		{
			throw null;
		}
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public SceneTransitionMatrix()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public bool GetTransitionValue(GameScenes from, GameScenes to)
	{
		throw null;
	}
}
