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
    }
}
