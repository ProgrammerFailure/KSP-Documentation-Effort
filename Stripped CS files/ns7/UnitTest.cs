using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace ns7;

public abstract class UnitTest
{
	public List<TestState> Results = new List<TestState>();

	public UnitTest()
	{
	}

	public virtual void TestStartUp()
	{
	}

	public virtual void TestTearDown()
	{
	}

	public void assertEquals(string testname, object value, object shouldbe)
	{
		string text = "TEST " + testname + ": ";
		bool flag;
		if (flag = value != null)
		{
			switch (shouldbe.GetType().Name)
			{
			default:
				flag = shouldbe.Equals(value);
				break;
			case "Vector3d":
			{
				Vector3d vector3d = (Vector3d)value;
				Vector3d vector3d2 = (Vector3d)shouldbe;
				flag = value != null && Math.Abs(vector3d.x - vector3d2.x) < 0.0001 && Math.Abs(vector3d.y - vector3d2.y) < 0.0001 && Math.Abs(vector3d.z - vector3d2.z) < 0.0001;
				break;
			}
			case "Single":
				flag = Math.Abs((float)value - (float)shouldbe) < 0.0001f;
				break;
			case "Double":
				flag = Math.Abs((double)value - (double)shouldbe) < 0.0001;
				break;
			}
		}
		if (!flag)
		{
			string text2 = "null";
			if (value != null)
			{
				text2 = value.ToString();
			}
			throw new Exception(text + "FAIL!  Value was: \"" + text2 + "\"");
		}
		PDebug.Log(text + "PASS!");
	}

	public IEnumerable<TestState> PerformTest()
	{
		try
		{
			Results.Clear();
			TestStartUp();
			MethodInfo[] methods = GetType().GetMethods();
			foreach (MethodInfo methodInfo in methods)
			{
				object[] customAttributes = methodInfo.GetCustomAttributes(inherit: false);
				TestInfo testInfo = null;
				for (int j = 0; j < customAttributes.Length; j++)
				{
					if (customAttributes[j].GetType().Equals(typeof(TestInfo)))
					{
						testInfo = (TestInfo)customAttributes[j];
						break;
					}
				}
				if (testInfo == null)
				{
					continue;
				}
				TestState testState = new TestState();
				testState.Info = testInfo;
				if (!methodInfo.ReturnType.Equals(typeof(void)))
				{
					testState.Succeeded = false;
					testState.Reason = "Method " + methodInfo.Name + " cannot be run:  Return type != void";
					testState.Details = "";
					Results.Add(testState);
					continue;
				}
				if (methodInfo.GetParameters().Length != 0)
				{
					testState.Succeeded = false;
					testState.Reason = "Method " + methodInfo.Name + " cannot be run:  Must not have arguments.";
					testState.Details = "";
					Results.Add(testState);
					continue;
				}
				try
				{
					methodInfo.Invoke(this, new object[0]);
					testState.Succeeded = true;
					testState.Reason = "PASS";
					testState.Details = "";
					Results.Add(testState);
				}
				catch (Exception ex)
				{
					testState.Succeeded = false;
					testState.Reason = "Method " + methodInfo.Name + " raised an exception";
					testState.Details = ex.ToString();
					Results.Add(testState);
				}
			}
		}
		catch (Exception ex2)
		{
			MonoBehaviour.print(GetType().FullName + " FAILED: " + ex2.ToString());
		}
		return Results;
	}
}
