﻿using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Data.Sql;

	public class PathThroughContext : SequenceContextBase
	{
		public PathThroughContext(IParseContext sequence, LambdaExpression lambda)
			: base(sequence, lambda)
		{
		}

		public PathThroughContext(IParseContext sequence, LambdaExpression lambda, SqlQuery sqlQuery)
			: base(sequence, lambda)
		{
			SqlQuery = sqlQuery;
		}

		public override Expression BuildExpression(Expression expression, int level)
		{
			throw new InvalidOperationException();
		}

		public override SqlInfo[] ConvertToSql(Expression expression, int level, ConvertFlags flags)
		{
			if (level == 0)
			{
				switch (flags)
				{
					case ConvertFlags.Field :
					case ConvertFlags.Key   :
					case ConvertFlags.All   :
						{
							var root = expression.GetRootObject();

							if (root.NodeType == ExpressionType.Parameter)
							{
								var ctx = Parser.GetContext(this, root);

								if (ctx != null)
								{
									if (ctx != this)
										return ctx.ConvertToSql(expression, 0, flags);

									return root == expression ?
										Sequence.ConvertToSql(null,       0,         flags) :
										Sequence.ConvertToSql(expression, level + 1, flags);
								}
							}

							break;
						}
				}

				throw new NotImplementedException();
			}

			throw new InvalidOperationException();

			//return Array<ISqlExpression>.Empty;
		}

		public override SqlInfo[] ConvertToIndex(Expression expression, int level, ConvertFlags flags)
		{
			throw new InvalidOperationException();
		}

		public override bool IsExpression(Expression expression, int level, RequestFor requestFlag)
		{
			switch (requestFlag)
			{
				case RequestFor.Root        : return expression == Lambda.Parameters[0];

				case RequestFor.Association :
				case RequestFor.Object       :
				case RequestFor.Field       :
				case RequestFor.Expression  :
					{
						var levelExpression = expression.GetLevelExpression(level);

						return levelExpression == expression ?
							Sequence.IsExpression(null,       0,         requestFlag) :
							Sequence.IsExpression(expression, level + 1, requestFlag);
					}
			}

			return false;
		}

		public override IParseContext GetContext(Expression expression, int level, SqlQuery currentSql)
		{
			return Sequence.GetContext(expression, level + 1, currentSql);
		}
	}
}
