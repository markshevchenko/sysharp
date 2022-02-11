using System;
using System.Linq.Expressions;
using Xunit;

namespace SySharp.Tests
{
    public class SymbolicTests
    {
        [Fact]
        public void Derivative_WithXMulX_ReturnsXMul1Add1MulX()
        {
            var derivative = Symbolic.Derivative(x => x * x);

            Assert.Equal("x => ((x * 1) + (1 * x))", derivative.ToString());
        }

        [Fact]
        public void Simplify_WithReturnsXMul1Add1MulX_Returns2X()
        {
            Expression<Func<double, double>> f = x => (x * 1) + (1 * x);

            var simple = Symbolic.Simplify(f.Body);

            Assert.Equal("(2 * x)", simple.ToString());
        }
    }
}
