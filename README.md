# SySharp

**SySharp** is simple library to symbolic calcualtions to demonstrate the power of
expressions trees in C#.

The library can produce derivative function and simplify expressions.

## Demo

To start the experiments you can clone the repository into your projects' directory:

```bash
cd $home/source
git clone https://github.com/markshevchenko/sysharp
cd sysharp
dotnet build -c Release
```

Open "C# Interactive" window in Visual Studio. Please note that **csi.exe** hasn't
been supported the *core mode*, so you need Visual Studio.

Type these commands:

```c#
#reset core
#r "source/sysharp/SySharp/bin/Release/net5.0/SySharp.dll"
using SySharp;
```

Don't forget to check the path. **source/sysharp/SySharp/bin/Release/net5.0** will be valid,
if you have cloned the repository to the **$home/source** directory.

Now we can experiment with the library. Let's' find out the derivative
of the function x<sup>2</sup>:

```c#
Symbolic.Derivative(x => x * x)
```

The answer is `x => ((x * 1) + (1 * x))`. To see it clearly we need
call the `ToString()` method:

```c#
Symbolic.Derivative(x => x * x).ToString()
```

It's too complicated, isn't it?

We can simplify expressions with the help of the method `Simplify()`:

```c#
Symbolic.Derivative(x => x * x).Simplify().ToString()
```

Now the anwer is `x => (2 * x)`. Much better.

At the end we can calculate the value of derivative function:

```c#
var d = (Func<double, double>)Symbolic.Derivative(x => Math.Sin(Math.Pow(x, 3))).Compile();
d(3)
```

The value of the function (sin *x*<sup>3</sup>)' at *x* = 3 will be equal to -7.887747835813577.