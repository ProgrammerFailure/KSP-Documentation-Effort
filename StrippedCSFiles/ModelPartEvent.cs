using System.Runtime.CompilerServices;
using UnityEngine;

public class ModelPartEvent : MonoBehaviour
{
	public class MPE
	{
		public string eventStart;

		public string eventEnd;

		[MethodImpl(MethodImplOptions.NoInlining)]
		public MPE()
		{
			throw null;
		}
	}

	public Part part;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ModelPartEvent()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void Awake()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void RecieveEvent(string eventName)
	{
		throw null;
	}
}
