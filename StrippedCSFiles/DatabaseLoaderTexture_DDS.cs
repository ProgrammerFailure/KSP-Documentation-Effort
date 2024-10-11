using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;
using UnityEngine;

[DatabaseLoaderAttrib(new string[] { "dds" })]
public class DatabaseLoaderTexture_DDS : DatabaseLoader<GameDatabase.TextureInfo>
{
	[CompilerGenerated]
	private sealed class _003CLoad_003Ed__0 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public DatabaseLoaderTexture_DDS _003C_003E4__this;

		public FileInfo file;

		public UrlDir.UrlFile urlFile;

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
		public _003CLoad_003Ed__0(int _003C_003E1__state)
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

	private bool isNormalMap;

	[MethodImpl(MethodImplOptions.NoInlining)]
	public DatabaseLoaderTexture_DDS()
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	[IteratorStateMachine(typeof(_003CLoad_003Ed__0))]
	public override IEnumerator Load(UrlDir.UrlFile urlFile, FileInfo file)
	{
		throw null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private Texture2D Read(string filename)
	{
		throw null;
	}
}
