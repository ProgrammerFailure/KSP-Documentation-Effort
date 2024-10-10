using System.Collections.Generic;
using UnityEngine;

namespace FinePrint.Utilities;

public class SpriteMap
{
	public readonly IDictionary<string, Sprite> _sprites = new Dictionary<string, Sprite>();

	public Sprite this[string key]
	{
		get
		{
			if (_sprites.ContainsKey(key))
			{
				return _sprites[key];
			}
			Texture2D texture2D = SystemUtilities.LoadTexture(key);
			_sprites[key] = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
			return _sprites[key];
		}
	}
}
