#reset core
#r "source/sysharp/SySharp/bin/Release/net5.0/SySharp.dll"
using SySharp;

Symbolic.Derivative(x => x * x).ToString()
Symbolic.Derivative(x => x * x).Simplify().ToString()

var d = (Func<double, double>)Symbolic.Derivative(x => Math.Sin(Math.Pow(x, 3))).Compile();
d(3)
