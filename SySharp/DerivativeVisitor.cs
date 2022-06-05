using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace SySharp
{
    public class DerivativeVisitor : ExpressionVisitor
    {
        private static readonly ConstantExpression _zero = Expression.Constant(0.0);
        private static readonly ConstantExpression _one = Expression.Constant(1.0);
        private static readonly ConstantExpression _two = Expression.Constant(2.0);
        private static readonly Type[] _double1 = new[] { typeof(double) };
        private static readonly Type[] _double2 = new[] { typeof(double), typeof(double) };
        private static readonly MethodInfo _pow = typeof(Math).GetMethod(nameof(Math.Pow), _double2)!;
        private static readonly MethodInfo _sin = typeof(Math).GetMethod(nameof(Math.Sin), _double1)!;
        private static readonly MethodInfo _cos = typeof(Math).GetMethod(nameof(Math.Cos), _double1)!;
        private static readonly MethodInfo _tan = typeof(Math).GetMethod(nameof(Math.Tan), _double1)!;
        private static readonly MethodInfo _log = typeof(Math).GetMethod(nameof(Math.Log), _double1)!;

        internal Expression D(Expression expression) => Visit(expression);

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return _zero;
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return _one;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return _zero;
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return Expression.Add(D(node.Left), D(node.Right));

                case ExpressionType.Subtract:
                    return Expression.Subtract(D(node.Left), D(node.Right));

                case ExpressionType.Multiply:
                    var addend = Expression.Multiply(node.Left, D(node.Right));
                    var augend = Expression.Multiply(D(node.Left), node.Right);
                    return Expression.Add(addend, augend);

                case ExpressionType.Divide:
                    var minuend = Expression.Multiply(D(node.Left), node.Right);
                    var subtrahend = Expression.Multiply(node.Left, D(node.Right));
                    var difference = Expression.Subtract(minuend, subtrahend);
                    var divider = Expression.Call(_pow, node.Right, _two);
                    return Expression.Divide(difference, divider);

                default:
                    throw new InvalidOperationException("Binary operator is not applicable for derivative");
            }
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var arguments = node.Arguments.ToArray();

            if (node.Method == _pow)
            {
                Debug.Assert(arguments.Length == 2, "Math.Pow should have two arguments");
                var @base = arguments[0];
                var exponent = arguments[1];

                if (@base is ParameterExpression)
                {
                    var multiplier = exponent;
                    var newExponent = Expression.Subtract(exponent, _one);
                    var multiplicant = Expression.Call(_pow, @base, newExponent);
                    return Expression.Multiply(multiplier, multiplicant);
                }
                else if (exponent is ParameterExpression)
                {
                    var multiplicant = Expression.Call(_log, @base);
                    return Expression.Multiply(node, multiplicant);
                }
                else
                    throw new InvalidOperationException("Too complicated power");
            }
            else if (node.Method == _sin)
            {
                Debug.Assert(arguments.Length == 1, "Math.Sin should have one argument");
                var derivate = Expression.Call(_cos, arguments[0]);
                return Expression.Multiply(D(arguments[0]), derivate);
            }
            else if (node.Method == _cos)
            {
                Debug.Assert(arguments.Length == 1, "Math.Cos should have one argument");
                var derivate = Expression.Negate(Expression.Call(_sin, arguments[0]));
                return Expression.Multiply(D(arguments[0]), derivate);
            }
            else if (node.Method == _tan)
            {
                Debug.Assert(arguments.Length == 1, "Math.Tan should have one argument");
                var dividor = Expression.Call(_pow, Expression.Call(_cos, arguments[0]), _two);
                return Expression.Divide(D(arguments[0]), dividor);
            }
            else if (node.Method == _log)
            {
                Debug.Assert(arguments.Length == 1, "Math.Log should have one argument");
                return Expression.Divide(D(arguments[0]), arguments[0]);
            }
            else
                throw new InvalidOperationException("Unrecognized function");
        }
    }
}
