﻿using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Builder
{
	using BLToolkit.Linq;
	using Data.Sql;

	class DeleteBuilder : MethodCallBuilder
	{
		protected override bool CanBuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			return methodCall.IsQueryable("Delete");
		}

		protected override IBuildContext BuildMethodCall(ExpressionBuilder builder, MethodCallExpression methodCall, BuildInfo buildInfo)
		{
			var sequence = builder.BuildSequence(new BuildInfo(buildInfo, methodCall.Arguments[0]));

			if (methodCall.Arguments.Count == 2)
				sequence = builder.BuildWhere(buildInfo.Parent, sequence, (LambdaExpression)methodCall.Arguments[1].Unwrap(), false);

			sequence.SqlQuery.QueryType = QueryType.Delete;

			return new DeleteContext(buildInfo.Parent, sequence);
		}

		class DeleteContext : SequenceContextBase
		{
			public DeleteContext(IBuildContext parent, IBuildContext sequence)
				: base(parent, sequence, null)
			{
			}

			public override void BuildQuery<T>(Query<T> query, ParameterExpression queryParameter)
			{
				query.SetNonQueryQuery();
			}

			public override Expression BuildExpression(Expression expression, int level)
			{
				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
			{
				throw new NotImplementedException();
			}

			public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
			{
				throw new NotImplementedException();
			}

			public override IBuildContext GetContext(Expression expression, int level, BuildInfo buildInfo)
			{
				throw new NotImplementedException();
			}
		}
	}
}
