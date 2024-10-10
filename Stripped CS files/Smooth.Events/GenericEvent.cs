using Smooth.Delegates;

namespace Smooth.Events;

public struct GenericEvent
{
	public event DelegateAction Handle;

	public void Raise()
	{
		this.Handle?.Invoke();
	}
}
public struct GenericEvent<T1>
{
	public event DelegateAction<T1> Handle;

	public void Raise(T1 t1)
	{
		this.Handle?.Invoke(t1);
	}
}
public struct GenericEvent<T1, T2>
{
	public event DelegateAction<T1, T2> Handle;

	public void Raise(T1 t1, T2 t2)
	{
		this.Handle?.Invoke(t1, t2);
	}
}
public struct GenericEvent<T1, T2, T3>
{
	public event DelegateAction<T1, T2, T3> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3)
	{
		this.Handle?.Invoke(t1, t2, t3);
	}
}
public struct GenericEvent<T1, T2, T3, T4>
{
	public event DelegateAction<T1, T2, T3, T4> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4)
	{
		this.Handle?.Invoke(t1, t2, t3, t4);
	}
}
public struct GenericEvent<T1, T2, T3, T4, T5>
{
	public event DelegateAction<T1, T2, T3, T4, T5> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5)
	{
		this.Handle?.Invoke(t1, t2, t3, t4, t5);
	}
}
public struct GenericEvent<T1, T2, T3, T4, T5, T6>
{
	public event DelegateAction<T1, T2, T3, T4, T5, T6> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6)
	{
		this.Handle?.Invoke(t1, t2, t3, t4, t5, t6);
	}
}
public struct GenericEvent<T1, T2, T3, T4, T5, T6, T7>
{
	public event DelegateAction<T1, T2, T3, T4, T5, T6, T7> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7)
	{
		this.Handle?.Invoke(t1, t2, t3, t4, t5, t6, t7);
	}
}
public struct GenericEvent<T1, T2, T3, T4, T5, T6, T7, T8>
{
	public event DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8)
	{
		this.Handle?.Invoke(t1, t2, t3, t4, t5, t6, t7, t8);
	}
}
public struct GenericEvent<T1, T2, T3, T4, T5, T6, T7, T8, T9>
{
	public event DelegateAction<T1, T2, T3, T4, T5, T6, T7, T8, T9> Handle;

	public void Raise(T1 t1, T2 t2, T3 t3, T4 t4, T5 t5, T6 t6, T7 t7, T8 t8, T9 t9)
	{
		this.Handle?.Invoke(t1, t2, t3, t4, t5, t6, t7, t8, t9);
	}
}
