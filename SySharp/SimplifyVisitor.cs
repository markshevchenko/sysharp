﻿using System;
using System.Diagnostics;
using System.Linq.Expressions;

namespace SySharp
{
    public class SimplifyVisitor : ExpressionVisitor
    {
        protected internal Expression Simplify(Expression expression) => Visit(expression);

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return SimplifySum(node.Left, node.Right);

                case ExpressionType.Subtract:
                    return SimplifySubtract(node.Left, node.Right);

                case ExpressionType.Multiply:
                    return SimplifyProduct(node.Left, node.Right);

                case ExpressionType.Divide:
                    return SimplifyDivide(node.Left, node.Right);

                default:
                    return node;
            }
        }

        private Expression SimplifySum(Expression left, Expression right)
        {
            if (TryCalculate(left, right, (a, b) => a + b, out double value))
                return Expression.Constant(value);

            if (left is ConstantExpression { Value: 0.0 })
                return Simplify(right);

            if (right is ConstantExpression { Value: 0.0 })
                return Simplify(left);

            left = Simplify(left);
            right = Simplify(right);

            if (left == right)
                return Expression.Multiply(Expression.Constant(2.0), Visit(left));

            return Expression.Add(left, right);
        }

        private Expression SimplifySubtract(Expression left, Expression right)
        {
            if (TryCalculate(left, right, (a, b) => a - b, out double value))
                return Expression.Constant(value);

            if (left is ConstantExpression { Value: 0.0 })
                return Expression.Negate(Simplify(right));

            if (right is ConstantExpression { Value: 0.0 })
                return Simplify(left);

            left = Simplify(left);
            right = Simplify(right);

            if (left == right)
                return Expression.Constant(0.0);

            return Expression.Subtract(left, right);
        }

        private Expression SimplifyProduct(Expression left, Expression right)
        {
            if (TryCalculate(left, right, (a, b) => a * b, out double value))
                return Expression.Constant(value);

            if (left is ConstantExpression { Value: 0.0 })
                return Expression.Constant(0.0);

            if (right is ConstantExpression { Value: 0.0 })
                return Expression.Constant(0.0);

            if (left is ConstantExpression { Value: 1.0 })
                return Simplify(right);

            if (right is ConstantExpression { Value: 1.0 })
                return Simplify(left);

            left = Simplify(left);
            right = Simplify(right);

            return Expression.Multiply(left, right);
        }

        private Expression SimplifyDivide(Expression left, Expression right)
        {
            if (TryCalculate(left, right, (a, b) => a / b, out double value))
                return Expression.Constant(value);

            if (left is ConstantExpression { Value: 0.0 })
                return Expression.Constant(0.0);

            if (right is ConstantExpression { Value: 1.0 })
                return Simplify(left);

            left = Simplify(left);
            right = Simplify(right);

            return Expression.Divide(left, right);
        }

        private bool TryCalculate(Expression left, Expression right, Func<double, double, double> calculator, out double value)
        {
            if (TryGetValue(left, out double leftValue) && TryGetValue(right, out double rightValue))
            {
                value = calculator(leftValue, rightValue);

                return true;
            }

            value = default;
            return false;
        }

        private bool TryGetValue(Expression expression, out double value)
        {
            value = default;
            if (expression is ConstantExpression constant)
            {
                if (constant.Type == typeof(double) || constant.Type == typeof(int))
                {
                    Debug.Assert(constant.Value != null);
                    value = (double)constant.Value;

                    return true;
                }
            }

            return false;
        }
    }
}