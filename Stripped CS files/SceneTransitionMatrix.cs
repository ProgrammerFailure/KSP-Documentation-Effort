using System;

[Serializable]
public class SceneTransitionMatrix
{
	[Serializable]
	public class SceneTransitions
	{
		public bool[] To = new bool[30];

		public bool this[GameScenes scn] => To[(int)scn];
	}

	public SceneTransitions[] From = new SceneTransitions[30];

	public SceneTransitions this[GameScenes scn] => From[(int)scn];

	public bool GetTransitionValue(GameScenes from, GameScenes to)
	{
		return this[from][to];
	}
}
