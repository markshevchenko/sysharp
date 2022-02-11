using System;
using System.Linq.Expressions;
using Xunit;

namespace SySharp.Tests
{
    public class SymbolicTests
    {
        [Fact]
        public void Derivative_WithSquareX_Returns2X()
        {
            Expression<Func<double, double>> f = x => x * x;

            var derivative = Symbolic.Derivative(f);

            Assert.Equal("x => (x * x)", f.ToString());
            Assert.Equal("x => ((x * 1) + (1 * x))", derivative.ToString());
        }
    }
}
