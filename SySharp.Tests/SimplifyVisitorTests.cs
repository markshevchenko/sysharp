using System;
using System.Linq.Expressions;
using Xunit;

namespace SySharp.Tests
{
    public class SimplifyVisitorTests
    {
        private readonly SimplifyVisitor _simplifyVisitor = new();

        [Fact]
        public void Simplify_With2Plus3_Return5()
        {
            Expression<Func<double, double>> f = x => 2 + 3;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("5", actual);
        }

        [Fact]
        public void Simplify_WithXPlus0_ReturnX()
        {
            Expression<Func<double, double>> f = x => x + 0;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_With0PlusX_ReturnX()
        {
            Expression<Func<double, double>> f = x => 0 + x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_WithXPlusX_Return2MulX()
        {
            Expression<Func<double, double>> f = x => x + x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("(2 * x)", actual);
        }

        [Fact]
        public void Simplify_With3Minus2_Returns1()
        {
            Expression<Func<double, double>> f = x => 3 - 2;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("1", actual);
        }

        [Fact]
        public void Simplify_WithXMinus0_ReturnX()
        {
            Expression<Func<double, double>> f = x => x - 0;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_With0MinusX_ReturnNegX()
        {
            Expression<Func<double, double>> f = x => 0 - x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("-x", actual);
        }

        [Fact]
        public void Simplify_WithXMinusX_Return0()
        {
            Expression<Func<double, double>> f = x => x - x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("0", actual);
        }

        [Fact]
        public void Simplify_With2Mul3_Return6()
        {
            Expression<Func<double, double>> f = x => 2 * 3;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("6", actual);
        }

        [Fact]
        public void Simplify_WithXMul1_ReturnX()
        {
            Expression<Func<double, double>> f = x => x * 1;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_WithXMul0_Return0()
        {
            Expression<Func<double, double>> f = x => x * 0;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("0", actual);
        }

        [Fact]
        public void Simplify_With1MulX_ReturnX()
        {
            Expression<Func<double, double>> f = x => 1 * x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_With0MulX_Return0()
        {
            Expression<Func<double, double>> f = x => 0 * x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("0", actual);
        }

        [Fact]
        public void Simplify_With6Div2_Return3()
        {
            Expression<Func<double, double>> f = x => 6 / 2;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("3", actual);
        }

        [Fact]
        public void Simplify_WithXDiv1_ReturnX()
        {
            Expression<Func<double, double>> f = x => x / 1;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("x", actual);
        }

        [Fact]
        public void Simplify_With0DivX_Return0()
        {
            Expression<Func<double, double>> f = x => 0 / x;

            var actual = _simplifyVisitor.Simplify(f.Body).ToString();

            Assert.Equal("0", actual);
        }
    }
}
