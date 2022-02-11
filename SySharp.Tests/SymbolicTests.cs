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
            var derivative = Symbolic.Derivative(x => x * x);

            Assert.Equal("x => ((x * 1) + (1 * x))", derivative.ToString());
        }
    }
}
