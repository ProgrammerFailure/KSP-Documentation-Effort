using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

namespace KSP.UI;

public class ButtonSpritesMgr : MonoBehaviour
{
	[Serializable]
	public class ButtonSprites
	{
		[SerializeField]
		private Sprite[] sprites;

		public Sprite[] Sprites
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

		public Sprite Normal
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Sprite Hover
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Sprite Active
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		public Sprite Disabled
		{
			[MethodImpl(MethodImplOptions.NoInlining)]
			get
			{
				throw null;
			}
		}

		[MethodImpl(MethodImplOptions.NoInlining)]
		public ButtonSprites()
		{
			throw null;
		}
	}

	[SerializeField]
	private Image btnImg;

	[SerializeField]
	private Button btnCtrl;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public ButtonSpritesMgr()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	public void SetSpriteSet(ButtonSprites set)
	{
		throw null;
	}
}
