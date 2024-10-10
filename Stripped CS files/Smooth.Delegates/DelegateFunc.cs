namespace Smooth.Delegates;

public delegate T DelegateFunc<out T>();
public delegate T DelegateFunc<in T1, out T>(T1 _1);
public delegate T DelegateFunc<in T1, in T2, out T>(T1 _1, T2 _2);
public delegate T DelegateFunc<in T1, in T2, in T3, out T>(T1 _1, T2 _2, T3 _3);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, out T>(T1 _1, T2 _2, T3 _3, T4 _4);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, in T5, out T>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, in T5, in T6, out T>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, out T>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, out T>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8);
public delegate T DelegateFunc<in T1, in T2, in T3, in T4, in T5, in T6, in T7, in T8, in T9, out T>(T1 _1, T2 _2, T3 _3, T4 _4, T5 _5, T6 _6, T7 _7, T8 _8, T9 _9);
