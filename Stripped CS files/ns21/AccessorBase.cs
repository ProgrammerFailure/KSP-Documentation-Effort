using System;
using System.Reflection;
using UnityEngine;

namespace ns21;

public abstract class AccessorBase
{
	public object obj;

	public MemberInfo member;

	public abstract object Value { get; set; }

	public AccessorBase(object obj, MemberInfo member)
	{
		this.obj = obj;
		this.member = member;
	}

	public static bool IsIdentifier(char ch, bool numok)
	{
		switch (ch)
		{
		case '_':
			return true;
		case 'A':
		case 'B':
		case 'C':
		case 'D':
		case 'E':
		case 'F':
		case 'G':
		case 'H':
		case 'I':
		case 'J':
		case 'K':
		case 'L':
		case 'M':
		case 'N':
		case 'O':
		case 'P':
		case 'Q':
		case 'R':
		case 'S':
		case 'T':
		case 'U':
		case 'V':
		case 'W':
		case 'X':
		case 'Y':
		case 'Z':
			return true;
		default:
			if (ch >= 'a' && ch <= 'z')
			{
				return true;
			}
			if (numok && ch >= '0' && ch <= '9')
			{
				return true;
			}
			return false;
		}
	}

	public static bool IsDigit(char ch)
	{
		if (ch >= '0' && ch <= '9')
		{
			return true;
		}
		return false;
	}

	public static AccessorBase Create(Type objType, object obj, string memberExpr)
	{
		Debug.LogFormat("[AccessorBase] Create {0} {1} {2}", objType, obj, memberExpr);
		if (string.IsNullOrEmpty(memberExpr))
		{
			return null;
		}
		BindingFlags bindingFlags = BindingFlags.Public;
		bindingFlags = ((obj != null) ? (bindingFlags | BindingFlags.Instance) : (bindingFlags | BindingFlags.Static));
		AccessorBase accessorBase = null;
		char c = memberExpr[0];
		int length = memberExpr.Length;
		int num = 0;
		if (c == '[' && length > 2)
		{
			while (++num < length)
			{
				c = memberExpr[num];
				if (!IsDigit(c))
				{
					break;
				}
			}
			if (c != ']')
			{
				Debug.LogFormat("[AccessorBase] Create: bad index: {0}", memberExpr);
				return null;
			}
			string text = memberExpr.Substring(1, num++ - 1);
			if (!int.TryParse(text, out var result))
			{
				Debug.LogFormat("[AccessorBase] Create: bad index: {0}", text);
				return null;
			}
			PropertyInfo property = objType.GetProperty("Item", bindingFlags);
			if (property == null)
			{
				Debug.LogFormat("[AccessorBase] Create: no indexer: {0}", "Item");
				return null;
			}
			ParameterInfo[] indexParameters = property.GetIndexParameters();
			if (indexParameters.Length != 1 || !(indexParameters[0].ParameterType == typeof(int)))
			{
				object[] args = indexParameters;
				Debug.LogFormat("[AccessorBase] Create: bad indexer: {0}", args);
				return null;
			}
			accessorBase = new PropertyAccessor(obj, property, new object[1] { result });
		}
		else
		{
			if (!IsIdentifier(c, numok: false))
			{
				Debug.LogFormat("[AccessorBase] Create: bad memberExpr: {0}", memberExpr);
				return null;
			}
			while (++num < length)
			{
				c = memberExpr[num];
				if (!IsIdentifier(c, numok: true))
				{
					break;
				}
			}
			string text2 = memberExpr.Substring(0, num);
			MemberInfo[] array = objType.GetMember(text2, bindingFlags);
			if (array == null || array.Length != 1)
			{
				object[] args = array;
				Debug.LogFormat("[AccessorBase] Create: too many members: {0}", args);
				return null;
			}
			if (array[0] is PropertyInfo)
			{
				accessorBase = new PropertyAccessor(obj, array[0], null);
			}
			else
			{
				if (!(array[0] is FieldInfo))
				{
					Debug.LogFormat("[AccessorBase] Create: member not field or property: {0}", text2);
					return null;
				}
				accessorBase = new FieldAccessor(obj, array[0]);
			}
		}
		memberExpr = memberExpr.Substring(num);
		length = memberExpr.Length;
		if (length > 0)
		{
			obj = accessorBase.Value;
			if (memberExpr[0] == '[')
			{
				return Create(obj.GetType(), obj, memberExpr);
			}
			if (memberExpr[0] == '.')
			{
				return Create(obj.GetType(), obj, memberExpr.Substring(1));
			}
			Debug.LogFormat("[AccessorBase] Create: bad extended memberExpr: {0}", memberExpr);
			return null;
		}
		return accessorBase;
	}
}
