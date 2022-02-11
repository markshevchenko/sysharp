using System;
using System.Linq.Expressions;

namespace SySharp
{
    public class DerivativeVisitor : ExpressionVisitor
    {
        internal Expression D(Expression expression) => Visit(expression);

        protected override Expression VisitConstant(ConstantExpression node)
        {
            return Expression.Constant(0.0);
        }

        protected override Expression VisitParameter(ParameterExpression node)
        {
            return Expression.Constant(1.0);
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            return Expression.Constant(0.0);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Add:
                    return Expression.Add(D(node.Left), D(node.Right));

                case ExpressionType.Multiply:
                    var addend = Expression.Multiply(node.Left, D(node.Right));
                    var augend = Expression.Multiply(D(node.Left), node.Right);
                    return Expression.Add(addend, augend);

                default:
                    throw new InvalidOperationException("Binary operator is not applicable for derivative");
            }
        }
    }
}
