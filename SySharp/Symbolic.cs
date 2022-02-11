using System;
using System.Linq.Expressions;

namespace SySharp
{
    public static class Symbolic
    {
        private static readonly DerivativeVisitor _derivativeVisitor = new ();
        private static readonly SimplifyVisitor _simplifyVisitor = new ();

        public static Expression Derivative(Expression<Func<double, double>> f) =>
            Expression.Lambda(_derivativeVisitor.D(f.Body), f.Parameters);

        public static Expression Simplify(Expression expression) => _simplifyVisitor.Visit(expression);
    }
}
