# SySharp

**SySharp** is simple library to symbolic calcualtions to demonstrate the power of
expressions trees in C#.

The library can produce derivative function and simplify expressions.

## Demo

To start the experiments you can clone the repository into your projects' directory:

```bash
c:
cd \Projects
git clone https://github.com/markshevchenko/sysharp
cd sysharp
dotnet build -c Release
```

Open "C# Interactive" window in Visual Studio. Please note that **csi.exe** hasn't
been suported the *core mode*, so you need Visual Studio.

Type these commands:

```
#reset core
#r "System.Linq.Expressions"
#r "System.Runtime"
#r "System.Linq"
#r "C:\Projects\sysharp\SySharp\bin\Release\net5.0\SySharp.dll"
using SySharp;
```

Now you can experiment with the library. Let's' find out what is the derivative
of the function x<sup>2</sup>:

```
Symbolic.Derivative(x => x * x)
```

The answer is `x => ((x * 1) + (1 * x))`. To see it clearly we need
append the `.ToString()` to the expression:

```
Symbolic.Derivative(x => x * x).ToString()
```

It's too complicated, isn't it?

We can simplify expressions with the help of method `Simplify()`:

```
Symbolic.Derivative(x => x * x).Simplify().ToString()
```

Now the anwer is `x => (2 * x)`. Much better.

At the end we can calculate the value of derivative function:

```
var d = (Func<double, double>)Symbolic.Derivative(x => Math.Sin(Math.Pow(x, 3))).Compile();
d(3.0)
```

The value of (sin *x*<sup>3</sup>)' (3.0) will be -7.887747835813577.