﻿using System;
using System.Data;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	class CountParser : MethodCallParser
	{
		protected override bool CanParseMethodCall(ExpressionParser parser, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			return methodCall.IsQueryable("Count", "LongCount");
		}

		protected override IParseContext ParseMethodCall(ExpressionParser parser, IParseContext parent, MethodCallExpression methodCall, SqlQuery sqlQuery)
		{
			var sequence = parser.ParseSequence(parent, methodCall.Arguments[0], sqlQuery);

			if (sequence.SqlQuery.Select.IsDistinct || sequence.SqlQuery.Select.TakeValue != null || sequence.SqlQuery.Select.SkipValue != null)
			{
				sequence.ConvertToIndex(null, 0, ConvertFlags.Key);
				sequence = new SubQueryContext(sequence);
			}

			if (sequence.SqlQuery.OrderBy.Items.Count > 0)
			{
				if (sequence.SqlQuery.Select.TakeValue == null && sequence.SqlQuery.Select.SkipValue == null)
					sequence.SqlQuery.OrderBy.Items.Clear();
				else
					sequence = new SubQueryContext(sequence);
			}

			if (methodCall.Arguments.Count == 2)
			{
				var condition = (LambdaExpression)methodCall.Arguments[1].Unwrap();

				sequence = parser.ParseWhere(sequence, condition, true);
				sequence.SetAlias(condition.Parameters[0].Name);
			}

			var context = new CountConext(sequence, methodCall.Method.ReturnType);

			context.FieldIndex = context.SqlQuery.Select.Add(SqlFunction.CreateCount(methodCall.Method.ReturnType, context.SqlQuery), "cnt");

			return context;
		}

		class CountConext : SequenceContextBase
		{
			public CountConext(IParseContext sequence, Type returnType)
				: base(sequence, null)
			{
				_returnType = returnType;
			}

			private  SqlInfo[] _index;
			readonly Type      _returnType;

			public int FieldIndex;

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				var expr = Expression.Convert(Parser.BuildSql(_returnType, FieldIndex), typeof(object));

				var mapper = Expression.Lambda<Func<QueryContext,IDataContext,IDataReader,Expression,object[],object>>(
					expr, new []
					{
						ExpressionParser.ContextParam,
						ExpressionParser.DataContextParam,
						ExpressionParser.DataReaderParam,
						ExpressionParser.ExpressionParam,
						ExpressionParser.ParametersParam,
					});

				query.SetElementQuery(mapper.Compile());
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				return Parser.BuildSql(_returnType, ConvertToIndex(expression, level, ConvertFlags.Field)[0].Index);
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field : return new[] { new SqlInfo { Sql = SqlQuery } };
				}

				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
						return _index ?? (_index = new[] { new SqlInfo { Index = Parent.SqlQuery.Select.Add(SqlQuery) } });
				}

				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				switch (requestFlag)
				{
					case RequestFor.Expression : return true;
				}

				return false;
			}

			public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
			{
				return Sequence.GetContext(expression, level, currentSql);
			}
		}
	}
}
