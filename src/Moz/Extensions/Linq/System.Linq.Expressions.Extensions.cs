using System.Collections.Generic;
using System.Reflection;

// ReSharper disable CheckNamespace
// ReSharper disable MemberCanBePrivate.Global

namespace System.Linq.Expressions
{
    public static class ExpressionsExtensions
    {
        public static Expression<T> Compose<T>(this Expression<T> first, Expression<T> second,
            Func<Expression, Expression, Expression> merge)
        {
            // build parameter map (from parameters of second to parameters of first)
            var map = first.Parameters.Select((f, i) => new {f, s = second.Parameters[i]})
                .ToDictionary(p => p.s, p => p.f);

            // replace parameters in the second lambda expression with parameters from the first
            var secondBody = ParameterRebinder.ReplaceParameters(map, second.Body);

            // apply composition of lambda expression bodies to parameters from the first expression 
            return Expression.Lambda<T>(merge(first.Body, secondBody), first.Parameters);
        }

        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.And);
        }

        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> first,
            Expression<Func<T, bool>> second)
        {
            return first.Compose(second, Expression.Or);
        }

        public static Expression<Func<T, decimal>> Add<T>(this Expression<Func<T, decimal>> first,
            Expression<Func<T, decimal>> second)
        {
            return first.Compose(second, Expression.Add);
        }

        public static Expression<Func<T, decimal>> Subtract<T>(this Expression<Func<T, decimal>> first,
            Expression<Func<T, decimal>> second)
        {
            return first.Compose(second, Expression.Subtract);
        }

        public static Expression<Func<T, decimal>> Multiply<T>(this Expression<Func<T, decimal>> first,
            Expression<Func<T, decimal>> second)
        {
            return first.Compose(second, Expression.Multiply);
        }

        public static Expression<Func<T, decimal>> Divide<T>(this Expression<Func<T, decimal>> first,
            Expression<Func<T, decimal>> second)
        {
            return first.Compose(second, Expression.Divide);
        }

        public static string GetPropName<T, TPropType>(this Expression<Func<T, TPropType>> keySelector)
        {
            var member = keySelector.Body as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    keySelector));

            var propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    keySelector));

            var key = typeof(T).Name + "." + propInfo.Name;
            return key;
        }
    }

    public class ParameterRebinder : ExpressionVisitor
    {
        private readonly Dictionary<ParameterExpression, ParameterExpression> map;

        public ParameterRebinder(Dictionary<ParameterExpression, ParameterExpression> map)
        {
            this.map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
        }

        public static Expression ReplaceParameters(Dictionary<ParameterExpression, ParameterExpression> map,
            Expression exp)
        {
            return new ParameterRebinder(map).Visit(exp);
        }

        protected override Expression VisitParameter(ParameterExpression p)
        {
            ParameterExpression replacement;
            if (map.TryGetValue(p, out replacement)) p = replacement;
            return base.VisitParameter(p);
        }
    }

    public static class NameReaderExtensions
    {
        private static readonly string expressionCannotBeNullMessage = "The expression cannot be null.";
        private static readonly string invalidExpressionMessage = "Invalid expression.";

        public static string GetMemberName<T>(this Expression<Func<T, object>> expression)
        {
            return GetMemberName(expression.Body);
        }

        public static List<string> GetMemberNames<T>
            (params Expression<Func<T, object>>[] expressions)
        {
            var memberNames = new List<string>();
            foreach (var cExpression in expressions) memberNames.Add(GetMemberName(cExpression.Body));

            return memberNames;
        }

        public static string GetMemberName<T>(Expression<Action<T>> expression)
        {
            return GetMemberName(expression.Body);
        }

        private static string GetMemberName(Expression expression)
        {
            if (expression == null) throw new ArgumentException(expressionCannotBeNullMessage);

            if (expression is MemberExpression)
            {
                // Reference type property or field
                var memberExpression = (MemberExpression) expression;
                return memberExpression.Member.Name;
            }

            if (expression is MethodCallExpression)
            {
                // Reference type method
                var methodCallExpression = (MethodCallExpression) expression;
                return methodCallExpression.Method.Name;
            }

            if (expression is UnaryExpression)
            {
                // Property, field of method returning value type
                var unaryExpression = (UnaryExpression) expression;
                return GetMemberName(unaryExpression);
            }

            throw new ArgumentException(invalidExpressionMessage);
        }

        private static string GetMemberName(UnaryExpression unaryExpression)
        {
            if (unaryExpression.Operand is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression) unaryExpression.Operand;
                return methodExpression.Method.Name;
            }

            return ((MemberExpression) unaryExpression.Operand).Member.Name;
        }
    }
}