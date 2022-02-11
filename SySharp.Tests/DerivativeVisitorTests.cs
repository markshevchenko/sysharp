using System;
using System.Linq.Expressions;
using Xunit;

namespace SySharp.Tests
{
    public class DerivativeVisitorTests
    {
        private readonly DerivativeVisitor _derivativeVisitor = new();

        [Fact]
        public void D_WithConstant_ReturnsConstant0()
        {
            var derivative = _derivativeVisitor.D(Expression.Constant(2.72));

            Assert.True(derivative is ConstantExpression { Value: 0.0 });
        }

        [Fact]
        public void D_WithBoundVariable_ReturnsConstant1()
        {
            Expression<Func<double, double>> f = x => x;

            var derivative = _derivativeVisitor.D(f.Body);

            Assert.True(derivative is ConstantExpression { Value: 1.0 });
        }

        [Fact]
        public void D_WithFreeVariable_ReturnsConstant0()
        {
            double y = 3.13;
            Expression<Func<double, double>> f = x => y;

            var derivative = _derivativeVisitor.D(f.Body);

            Assert.True(derivative is ConstantExpression { Value: 0.0 });
        }

        [Fact]
        public void D_WithSum_ReturnsSum()
        {
            Expression<Func<double, double>> f = x => x + 12;

            var derivative = _derivativeVisitor.D(f.Body);

            Assert.True(derivative is BinaryExpression { NodeType: ExpressionType.Add });
        }

        [Fact]
        public void D_WithProduct_ReturnsSumOfProducts()
        {
            Expression<Func<double, double>> f = x => x * 12;

            var derivative = _derivativeVisitor.D(f.Body);

            Assert.True(derivative is BinaryExpression
            {
                NodeType: ExpressionType.Add,
                Left: BinaryExpression { NodeType: ExpressionType.Multiply },
                Right: BinaryExpression { NodeType: ExpressionType.Multiply }
            });
        }

        [Fact]
        public void D_WithXPow3_Returns3MulXPow2()
        {
            Expression<Func<double, double>> f = x => Math.Pow(x, 3);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(3 * Pow(x, (3 - 1)))", actual);
        }

        [Fact]
        public void D_With3PowX_Returns3PowXMulLog3()
        {
            Expression<Func<double, double>> f = x => Math.Pow(3, x);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(Pow(3, x) * Log(3))", actual);
        }

        [Fact]
        public void D_WithSinX_Returns1MulCosX()
        {
            Expression<Func<double, double>> f = x => Math.Sin(x);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(1 * Cos(x))", actual);
        }

        [Fact]
        public void D_WithCosX_Returns1MulNegSinX()
        {
            Expression<Func<double, double>> f = x => Math.Cos(x);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(1 * -Sin(x))", actual);
        }

        [Fact]
        public void D_WithTanX_Returns1DivCos2X()
        {
            Expression<Func<double, double>> f = x => Math.Tan(x);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(1 / Pow(Cos(x), 2))", actual);
        }

        [Fact]
        public void D_WithLogX_Returns1DivX()
        {
            Expression<Func<double, double>> f = x => Math.Log(x);

            var actual = _derivativeVisitor.D(f.Body).ToString();

            Assert.Equal("(1 / x)", actual);
        }
    }
}
