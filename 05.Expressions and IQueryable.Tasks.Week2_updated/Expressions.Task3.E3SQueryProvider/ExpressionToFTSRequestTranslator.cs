using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Expressions.Task3.E3SQueryProvider
{
    public class ExpressionToFtsRequestTranslator : ExpressionVisitor
    {
        private const string LeftSeparator = "(";
        private const string RightSepator = ")";
        private const string AnySymbols = "*";

        private readonly StringBuilder _resultStringBuilder;

        public ExpressionToFtsRequestTranslator()
        {
            _resultStringBuilder = new StringBuilder();
        }

        public string Translate(Expression exp)
        {
            Visit(exp);

            return _resultStringBuilder.ToString();
        }

        #region protected methods

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            if (node.Method.DeclaringType == typeof(Queryable)
                && node.Method.Name == "Where")
            {
                var predicate = node.Arguments[1];
                Visit(predicate);

                return node;
            }
            
            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "Equals")
            {
                VisitBinary(Expression.Equal(node.Object, node.Arguments[0]));

                return node;
            }

            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "StartsWith")
            {
                Visit(node.Object);
                _resultStringBuilder.Append("(");
                Visit(node.Arguments[0]);
                _resultStringBuilder.Append("*)");

                return node;
            }

            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "EndsWith")
            {
                Visit(node.Object);
                _resultStringBuilder.Append("(*");
                Visit(node.Arguments[0]);
                _resultStringBuilder.Append(")");

                return node;
            }

            if (node.Method.DeclaringType == typeof(string)
                && node.Method.Name == "Contains")
            {
                Visit(node.Object);
                _resultStringBuilder.Append("(*");
                Visit(node.Arguments[0]);
                _resultStringBuilder.Append("*)");

                return node;
            }

            return base.VisitMethodCall(node);
        }

        protected override Expression VisitBinary(BinaryExpression node)
        {
            switch (node.NodeType)
            {
                case ExpressionType.Equal:
                    if (node.Left.NodeType == ExpressionType.Constant)
                    {
                        Visit(Expression.Equal(node.Right, node.Left));
                        break;
                    }

                    if (node.Right.NodeType != ExpressionType.Constant)
                        throw new NotSupportedException($"One of the operands should be constant: {node.NodeType}");

                    Visit(node.Left);
                    _resultStringBuilder.Append("(");
                    Visit(node.Right);
                    _resultStringBuilder.Append(")");
                    break;
                case ExpressionType.AndAlso:
                    Visit(node.Left);
                    _resultStringBuilder.Append("&&");
                    Visit(node.Right);
                    break;
                default:
                    throw new NotSupportedException($"Operation '{node.NodeType}' is not supported");
            };

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _resultStringBuilder.Append(node.Member.Name).Append(":");

            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            _resultStringBuilder.Append(node.Value);

            return node;
        }

        #endregion

        private void AddLeftSeparator()
        {
            AddSeparator(LeftSeparator);
        }

        private void AddRightSeparator()
        {
            AddSeparator(RightSepator);
        }

        private void AddAnySymbols()
        {
            AddSeparator(AnySymbols);
        }

        private void AddSeparator(string separator)
        {
            _resultStringBuilder.Append(separator);
        }
    }
}
