﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;

namespace BLToolkit.Data.Linq.Parser
{
	using BLToolkit.Linq;
	using Common;
	using Data.Sql;
	using Mapping;
	using Reflection;

	partial class ExpressionParser
	{
		#region Parse Where

		public IParseContext ParseWhere(IParseContext sequence, LambdaExpression condition, bool checkForSubQuery)
		{
			var makeHaving = false;

			var  ctx  = new PathThroughContext(sequence, condition);
			var  expr = condition.Body.Unwrap();

			if (checkForSubQuery && CheckSubQueryForWhere(ctx, expr, out makeHaving))
			{
				sequence = new SubQueryContext(sequence);
				ctx      = new PathThroughContext(sequence, condition);
			}

			ParseSearchCondition(
				ctx,
				expr,
				makeHaving ?
					ctx.SqlQuery.Having.SearchCondition.Conditions :
					ctx.SqlQuery.Where. SearchCondition.Conditions);

			return sequence;
		}

		bool CheckSubQueryForWhere(IParseContext context, Expression expression, out bool makeHaving)
		{
			//var checkParameter = true; //context.IsExpression(expression, 0, RequestFor.ScalarExpression);
			var makeSubQuery   = false;
			var isHaving       = false;
			var isWhere        = false;

			expression.Find(expr =>
			{
				if (IsSubQuery(context, expr))
					return isWhere = true;

				var stopWalking = false;

				switch (expr.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)expr;

							if (TypeHelper.IsNullableValueMember(ma.Member))
								break;

							if (ConvertMember(ma.Member) == null)
							{
								var ctx = GetContext(context, expr);

								if (ctx != null)
								{
									if (ctx.IsExpression(expr, 0, RequestFor.Expression))
										makeSubQuery = true;
									stopWalking = true;
								}
							}

							isWhere = true;

							break;
						}

					case ExpressionType.Call:
						{
							var e = (MethodCallExpression)expr;

							if (e.Method.DeclaringType == typeof(Enumerable) && e.Method.Name != "Contains")
								return isHaving = true;

							isWhere = true;

							break;
						}

					case ExpressionType.Parameter:
						{
							var ctx = GetContext(context, expr);

							if (ctx != null)
							{
								if (ctx.IsExpression(expr, 0, RequestFor.Expression))
									makeSubQuery = true;
								stopWalking = true;
							}

							isWhere = true;

							break;
						}
				}

				return stopWalking;
			});

			makeHaving = isHaving && !isWhere;
			return makeSubQuery || isHaving && isWhere;
		}

		#endregion

		#region ParseTake

		public void ParseTake(IParseContext context, ISqlExpression expr)
		{
			var sql = context.SqlQuery;

			sql.Select.Take(expr);

			SqlProvider.SqlQuery = sql;

			if (sql.Select.SkipValue != null && SqlProvider.IsTakeSupported && !SqlProvider.IsSkipSupported)
			{
				if (context.SqlQuery.Select.SkipValue is SqlParameter && sql.Select.TakeValue is SqlValue)
				{
					var skip = (SqlParameter)sql.Select.SkipValue;
					var parm = (SqlParameter)sql.Select.SkipValue.Clone(new Dictionary<ICloneableElement,ICloneableElement>(), _ => true);

					parm.SetTakeConverter((int)((SqlValue)sql.Select.TakeValue).Value);

					sql.Select.Take(parm);

					var ep = (from pm in CurrentSqlParameters where pm.SqlParameter == skip select pm).First();

					ep = new ParameterAccessor
					{
						Expression   = ep.Expression,
						Accessor     = ep.Accessor,
						SqlParameter = parm
					};

					CurrentSqlParameters.Add(ep);
				}
				else
					sql.Select.Take(Convert(
						context,
						new SqlBinaryExpression(typeof(int), sql.Select.SkipValue, "+", sql.Select.TakeValue, Precedence.Additive)));
			}

			if (!SqlProvider.TakeAcceptsParameter)
			{
				var p = sql.Select.TakeValue as SqlParameter;

				if (p != null)
					p.IsQueryParameter = false;
			}
		}

		#endregion

		#region ParseSubQuery

		IParseContext GetSubQuery(IParseContext context, Expression expr)
		{
			SubQueryParsingCounter++;
			var sequence = ParseSequence(context, expr, new SqlQuery { ParentSql = context.SqlQuery });
			SubQueryParsingCounter--;

			return sequence;
		}

		ISqlExpression ParseSubQuery(IParseContext context, Expression expression)
		{
			if (expression.NodeType == ExpressionType.MemberAccess)
			{
				var ma = (MemberExpression)expression;

				if (ma.Expression != null)
				{
					switch (ma.Expression.NodeType)
					{
						case ExpressionType.Call         :
						case ExpressionType.MemberAccess :
						case ExpressionType.Parameter    :
							{
								var ctx = GetSubQuery(context, ma.Expression);

								var ex  = expression.Convert(e => e == ma.Expression ? Expression.Constant(null, ma.Expression.Type) : e);
								var sql = ctx.ConvertToSql(ex, 0, ConvertFlags.Field);

								if (sql.Length != 1)
									throw new NotImplementedException();

								ctx.SqlQuery.Select.Add(sql[0].Sql);

								var idx = context.SqlQuery.Select.Add(ctx.SqlQuery);

								return context.SqlQuery.Select.Columns[idx];
							}
					}
				}
			}

			var sequence = GetSubQuery(context, expression);

			/*
			if (expr.NodeType == ExpressionType.Call)
			{
				var call = (MethodCallExpression)expr;

				if (call.Method.Name == "Any" && (call.Method.DeclaringType == typeof(Queryable) || call.Method.DeclaringType == typeof(Enumerable)))
					return ((SqlQuery.Predicate.FuncLike) result.Where.SearchCondition.Conditions[0].Predicate).Function;
			}
			*/

			return sequence.SqlQuery;
		}

		#endregion

		#region IsSubquery

		bool IsSubQuery(IParseContext context, Expression expression)
		{
			return IsSubQuery(context, expression, false);
		}

		bool IsSubQuery(IParseContext context, Expression expression, bool ignoreMembers)
		{
			/////if (queries.Length > 0 &&
			/////	queries[0] is QuerySource.SubQuerySourceColumn &&
			/////	((QuerySource.SubQuerySourceColumn)queries[0]).SourceColumn is QuerySource.GroupBy)
			/////	return false;

			switch (expression.NodeType)
			{
				case ExpressionType.Call:
					{
						var call = (MethodCallExpression)expression;

						if (call.IsQueryable())
						{
							var arg = call.Arguments[0];

							if (arg.NodeType == ExpressionType.Call)
								return IsSubQuery(context, arg);

							if (IsSubQuerySource(context, arg))
								return true;
						}

						/*
						if (call.Method.DeclaringType == typeof(Queryable) || call.Method.DeclaringType == typeof(Enumerable))
						{
							var arg = call.Arguments[0];

							if (arg.NodeType == ExpressionType.Call)
								return IsSubQuery(context, arg);

							// *
							var qs = queries;

							if (queries.Length > 0 && queries[0].GetType() == typeof(QuerySource.Expr))
								qs = new[] { queries[0].BaseQuery }.Concat(queries).ToArray();

							if (IsSubQuerySource(arg, qs))
								return true;
							* /
						}
						*/

						//if (IsIEnumerableType(expression))
						//	return !CanBeCompiled(expression);
					}

					break;

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expression;

						if (IsSubQueryMember(expression) && IsSubQuerySource(context, ma.Expression))
							return !CanBeCompiled(expression);

						if (!ignoreMembers && IsIEnumerableType(expression))
							return !CanBeCompiled(expression);

						if (ma.Expression != null)
							return IsSubQuery(context, ma.Expression, true);

						break;
					}
			}

			return false;
		}

		bool IsSubQuerySource(IParseContext context, Expression expr)
		{
			if (expr == null)
				return false;

			var ctx = GetContext(context, expr);

			if (ctx != null)
			{
				//while (source is QuerySource.SubQuerySourceColumn)
				//	source = ((QuerySource.SubQuerySourceColumn)source).SourceColumn;

				if (ctx.IsExpression(expr, 0, RequestFor.Object))
					return true;
			}

			while (expr != null && expr.NodeType == ExpressionType.MemberAccess)
				expr = ((MemberExpression)expr).Expression;

			return expr != null && expr.NodeType == ExpressionType.Constant;
		}

		static bool IsSubQueryMember(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.Call :
					break;

				case ExpressionType.MemberAccess :
					break;
			}

			return false;
		}

		static bool IsIEnumerableType(Expression expr)
		{
			var type = expr.Type;

			var res  = (type.IsClass || type.IsInterface)
				&& type != typeof(string)
				&& type != typeof(byte[])
				&& TypeHelper.IsSameOrParent(typeof(IEnumerable), type);

			if (res && expr.NodeType == ExpressionType.MemberAccess)
				res = TypeHelper.GetAttributes(type, typeof(IgnoreIEnumerableAttribute)).Length == 0;

			return res;
		}

		#endregion

		#region ParseExpression

		public SqlInfo[] ParseExpressions(IParseContext context, Expression expression, ConvertFlags queryConvertFlag)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.New :
					{
						var expr = (NewExpression)expression;

// ReSharper disable ConditionIsAlwaysTrueOrFalse
// ReSharper disable HeuristicUnreachableCode
						if (expr.Members == null)
							return Array<SqlInfo>.Empty;
// ReSharper restore HeuristicUnreachableCode
// ReSharper restore ConditionIsAlwaysTrueOrFalse

						return expr.Arguments
							.Select((arg,i) =>
							{
								var sql = ParseExpression(context, arg);
								var mi  = expr.Members[i];

								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return new SqlInfo { Sql = sql, Member = mi };
							})
							.ToArray();
					}

				case ExpressionType.MemberInit :
					{
						var expr = (MemberInitExpression)expression;

						return expr.Bindings
							.Where(b => b is MemberAssignment)
							.Cast<MemberAssignment>()
							.Select(a =>
							{
								var sql = ParseExpression(context, a.Expression);
								var mi  = a.Member;

								if (mi is MethodInfo)
									mi = TypeHelper.GetPropertyByMethod((MethodInfo)mi);

								return new SqlInfo { Sql = sql, Member = mi };
							})
							.ToArray();
					}
			}

			var ctx = GetContext(context, expression);

			if (ctx != null)
			{
				if (ctx.IsExpression(expression, 0, RequestFor.Object))
					return ctx.ConvertToSql(expression, 0, queryConvertFlag);
			}

			return new[] { new SqlInfo { Sql = ParseExpression(context, expression) } };
		}

		public ISqlExpression ParseExpression(IParseContext context, Expression expression)
		{
			/*
			var qlen = queries.Length;

			if (expression.NodeType == ExpressionType.Parameter && qlen == 1 && queries[0] is QuerySource.Scalar)
			{
				var ma = (QuerySource.Scalar)queries[0];
				return ParseExpression(ma.Lambda, ma.Lambda.Body, ma.Sources);
			}
			*/

			if (CanBeConstant(expression))
				return BuildConstant(expression);

			if (CanBeCompiled(expression))
				return BuildParameter(expression).SqlParameter;

			if (IsSubQuery(context, expression))
				return ParseSubQuery(context, expression);

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
				case ExpressionType.OrElse:
				case ExpressionType.Not:
				case ExpressionType.Equal:
				case ExpressionType.NotEqual:
				case ExpressionType.GreaterThan:
				case ExpressionType.GreaterThanOrEqual:
				case ExpressionType.LessThan:
				case ExpressionType.LessThanOrEqual:
					{
						var condition = new SqlQuery.SearchCondition();
						ParseSearchCondition(context, expression, condition.Conditions);
						return condition;
					}

				case ExpressionType.Add:
				case ExpressionType.AddChecked:
				case ExpressionType.And:
				case ExpressionType.Divide:
				case ExpressionType.ExclusiveOr:
				case ExpressionType.Modulo:
				case ExpressionType.Multiply:
				case ExpressionType.MultiplyChecked:
				case ExpressionType.Or:
				case ExpressionType.Power:
				case ExpressionType.Subtract:
				case ExpressionType.SubtractChecked:
				case ExpressionType.Coalesce:
					{
						var e = (BinaryExpression)expression;
						var l = ParseExpression(context, e.Left);
						var r = ParseExpression(context, e.Right);
						var t = e.Type;

						switch (expression.NodeType)
						{
							case ExpressionType.Add             :
							case ExpressionType.AddChecked      : return Convert(context, new SqlBinaryExpression(t, l, "+", r, Precedence.Additive));
							case ExpressionType.And             : return Convert(context, new SqlBinaryExpression(t, l, "&", r, Precedence.Bitwise));
							case ExpressionType.Divide          : return Convert(context, new SqlBinaryExpression(t, l, "/", r, Precedence.Multiplicative));
							case ExpressionType.ExclusiveOr     : return Convert(context, new SqlBinaryExpression(t, l, "^", r, Precedence.Bitwise));
							case ExpressionType.Modulo          : return Convert(context, new SqlBinaryExpression(t, l, "%", r, Precedence.Multiplicative));
							case ExpressionType.Multiply:
							case ExpressionType.MultiplyChecked : return Convert(context, new SqlBinaryExpression(t, l, "*", r, Precedence.Multiplicative));
							case ExpressionType.Or              : return Convert(context, new SqlBinaryExpression(t, l, "|", r, Precedence.Bitwise));
							case ExpressionType.Power           : return Convert(context, new SqlFunction(t, "Power", l, r));
							case ExpressionType.Subtract        :
							case ExpressionType.SubtractChecked : return Convert(context, new SqlBinaryExpression(t, l, "-", r, Precedence.Subtraction));
							case ExpressionType.Coalesce        :
								{
									if (r is SqlFunction)
									{
										var c = (SqlFunction)r;

										if (c.Name == "Coalesce")
										{
											var parms = new ISqlExpression[c.Parameters.Length + 1];

											parms[0] = l;
											c.Parameters.CopyTo(parms, 1);

											return Convert(context, new SqlFunction(t, "Coalesce", parms));
										}
									}

									return Convert(context, new SqlFunction(t, "Coalesce", l, r));
								}
						}

						break;
					}

				case ExpressionType.UnaryPlus:
				case ExpressionType.Negate:
				case ExpressionType.NegateChecked:
					{
						var e = (UnaryExpression)expression;
						var o = ParseExpression(context, e.Operand);
						var t = e.Type;

						switch (expression.NodeType)
						{
							case ExpressionType.UnaryPlus     : return o;
							case ExpressionType.Negate        :
							case ExpressionType.NegateChecked :
								return Convert(context, new SqlBinaryExpression(t, new SqlValue(-1), "*", o, Precedence.Multiplicative));
						}

						break;
					}

				case ExpressionType.Convert        :
				case ExpressionType.ConvertChecked :
					{
						var e = (UnaryExpression)expression;
						var o = ParseExpression(context, e.Operand);

						if (e.Method == null && e.IsLifted)
							return o;

						if (e.Operand.Type.IsEnum && Enum.GetUnderlyingType(e.Operand.Type) == e.Type)
							return o;

						return Convert(
							context,
							new SqlFunction(e.Type, "$Convert$", SqlDataType.GetDataType(e.Type), SqlDataType.GetDataType(e.Operand.Type), o));
					}

				case ExpressionType.Conditional   :
					{
						var e = (ConditionalExpression)expression;
						var s = ParseExpression(context, e.Test);
						var t = ParseExpression(context, e.IfTrue);
						var f = ParseExpression(context, e.IfFalse);

						if (f is SqlFunction)
						{
							var c = (SqlFunction)f;

							if (c.Name == "CASE")
							{
								var parms = new ISqlExpression[c.Parameters.Length + 2];

								parms[0] = s;
								parms[1] = t;
								c.Parameters.CopyTo(parms, 2);

								return Convert(context, new SqlFunction(e.Type, "CASE", parms));
							}
						}

						return Convert(context, new SqlFunction(e.Type, "CASE", s, t, f));
					}

				case ExpressionType.MemberAccess:
					{
						var ma = (MemberExpression)expression;
						var l  = ConvertMember(ma.Member);

						if (l != null)
						{
							var ef  = l.Body.Unwrap();
							var pie = ef.Convert(wpi => wpi.NodeType == ExpressionType.Parameter ? ma.Expression : wpi);

							return ParseExpression(context, pie);
						}

						var attr = GetFunctionAttribute(ma.Member);

						if (attr != null)
							return Convert(context, attr.GetExpression(ma.Member));

						if (TypeHelper.IsNullableValueMember(ma.Member))
							return ParseExpression(context, ma.Expression);

						var de = ParseTimeSpanMember(context, ma);

						if (de != null)
							return de;

						var ctx = GetContext(context, expression);

						if (ctx != null)
						{
							var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

							switch (sql.Length)
							{
								case 0  : break;
								case 1  : return sql[0].Sql;
								default : throw new InvalidOperationException();
							}
						}

						break;
					}

				case ExpressionType.Parameter:
					{
						var ctx = GetContext(context, expression);

						if (ctx != null)
						{
							var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

							switch (sql.Length)
							{
								case 0  : break;
								case 1  : return sql[0].Sql;
								default : throw new InvalidOperationException();
							}
						}

						break;

						/*
						var sql = context.ConvertToSql(expression, 0, ConvertFlags.None).ToList();

						if (sql.Count != 0)
						{
							if (sql.Count == 1)
								return sql[0];

							throw new InvalidOperationException();
						}

						break;
						*/
					}

				case ExpressionType.Call:
					{
						var e = (MethodCallExpression)expression;

						if (e.Method.DeclaringType == typeof(Enumerable))
						{
							var ctx = GetContext(context, expression);

							if (ctx != null)
							{
								var sql = ctx.ConvertToSql(expression, 0, ConvertFlags.Field);

								if (sql.Length != 1)
									throw new InvalidOperationException();

								return sql[0].Sql;
							}

							return ParseEnumerable(context, e);
						}

						var cm = ConvertMethod(e);
						if (cm != null)
							return ParseExpression(context, cm);

						var attr = GetFunctionAttribute(e.Method);

						if (attr != null)
						{
							var parms = new List<ISqlExpression>();

							if (e.Object != null)
								parms.Add(ParseExpression(context, e.Object));

							parms.AddRange(e.Arguments.Select(t => ParseExpression(context, t)));

							return Convert(context, attr.GetExpression(e.Method, parms.ToArray()));
						}

						break;
					}

				case ExpressionType.New:
					{
						var pie = ConvertNew((NewExpression)expression);

						if (pie != null)
							return ParseExpression(context, pie);

						break;
					}

				case ExpressionType.Invoke:
					{
						var pi = (InvocationExpression)expression;
						var ex = pi.Expression;

						if (ex.NodeType == ExpressionType.Quote)
							ex = ((UnaryExpression)ex).Operand;

						//if (ex.NodeType == ExpressionType.MemberAccess)
						//	return ParseExpression(lambda, ex, queries);

						if (ex.NodeType == ExpressionType.Lambda)
						{
							var l   = (LambdaExpression)ex;
							var dic = new Dictionary<Expression,Expression>();

							for (var i = 0; i < l.Parameters.Count; i++)
								dic.Add(l.Parameters[i], pi.Arguments[i]);

							var pie = l.Body.Convert(wpi =>
							{
								Expression ppi;
								return dic.TryGetValue(wpi, out ppi) ? ppi : wpi;
							});

							return ParseExpression(context, pie);
						}

						break;
					}
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", expression);
		}

		#endregion

		#region IsServerSideOnly

		bool IsServerSideOnly(Expression expr)
		{
			switch (expr.NodeType)
			{
				case ExpressionType.MemberAccess:
					{
						var ex = (MemberExpression)expr;
						var l  = ConvertMember(ex.Member);

						if (l != null)
							return IsServerSideOnly(l.Body.Unwrap());

						var attr = GetFunctionAttribute(ex.Member);
						return attr != null && attr.ServerSideOnly;
					}

				case ExpressionType.Call:
					{
						var e = (MethodCallExpression)expr;

						if (e.Method.DeclaringType == typeof(Enumerable))
						{
							switch (e.Method.Name)
							{
								case "Count"   :
								case "Average" :
								case "Min"     :
								case "Max"     :
								case "Sum"     : return IsQueryMember(e.Arguments[0]);
							}
						}
						else if (e.Method.DeclaringType == typeof(Queryable))
						{
							switch (e.Method.Name)
							{
								case "Any"      :
								case "All"      :
								case "Contains" : return true;
							}
						}
						else
						{
							var l = ConvertMember(e.Method);

							if (l != null)
								return l.Body.Unwrap().Find(IsServerSideOnly) != null;

							var attr = GetFunctionAttribute(e.Method);
							return attr != null && attr.ServerSideOnly;
						}

						break;
					}
			}

			return false;
		}

		static bool IsQueryMember(Expression expr)
		{
			if (expr != null) switch (expr.NodeType)
			{
				case ExpressionType.Parameter    : return true;
				case ExpressionType.MemberAccess : return IsQueryMember(((MemberExpression)expr).Expression);
				case ExpressionType.Call         :
					{
						var call = (MethodCallExpression)expr;

						if (call.Method.DeclaringType == typeof(Queryable))
							return true;

						if (call.Method.DeclaringType == typeof(Enumerable) && call.Arguments.Count > 0)
							return IsQueryMember(call.Arguments[0]);

						return IsQueryMember(call.Object);
					}
			}

			return false;
		}

		#endregion

		#region CanBeConstant

		bool CanBeConstant(Expression expr)
		{
			return null == expr.Find(ex =>
			{
				if (ex is BinaryExpression || ex is UnaryExpression || ex.NodeType == ExpressionType.Convert)
					return false;

				switch (ex.NodeType)
				{
					case ExpressionType.Constant:
						{
							var c = (ConstantExpression)ex;

							if (c.Value == null || ExpressionHelper.IsConstant(ex.Type))
								return false;

							break;
						}

					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)ex;

							if (ExpressionHelper.IsConstant(ma.Member.DeclaringType) || TypeHelper.IsNullableValueMember(ma.Member))
								return false;

							break;
						}

					case ExpressionType.Call:
						{
							var mc = (MethodCallExpression)ex;

							if (ExpressionHelper.IsConstant(mc.Method.DeclaringType) || mc.Method.DeclaringType == typeof(object))
								return false;

							var attr = GetFunctionAttribute(mc.Method);

							if (attr != null && !attr.ServerSideOnly)
								return false;

							break;
						}
				}

				return true;
			});
		}

		#endregion

		#region CanBeCompiled

		bool CanBeCompiled(Expression expr)
		{
			return null == expr.Find(ex =>
			{
				if (IsServerSideOnly(ex))
					return true;

				switch (ex.NodeType)
				{
					case ExpressionType.Parameter :
						return ex != ParametersParam;

					case ExpressionType.MemberAccess :
						{
							var attr = GetFunctionAttribute(((MemberExpression)ex).Member);
							return attr != null && attr.ServerSideOnly;
						}

					case ExpressionType.Call :
						{
							var attr = GetFunctionAttribute(((MethodCallExpression)ex).Method);
							return attr != null && attr.ServerSideOnly;
						}
				}

				return false;
			});
		}

		#endregion

		#region Build Constant

		readonly Dictionary<Expression,SqlValue> _constants = new Dictionary<Expression,SqlValue>();

		SqlValue BuildConstant(Expression expr)
		{
			SqlValue value;

			if (_constants.TryGetValue(expr, out value))
				return value;

			var lambda = Expression.Lambda<Func<object>>(Expression.Convert(expr, typeof(object)));

			var v = lambda.Compile()();

			if (v != null && v.GetType().IsEnum)
			{
				var attrs = v.GetType().GetCustomAttributes(typeof(SqlEnumAttribute), true);

				v = Map.EnumToValue(v, attrs.Length == 0);
			}

			value = new SqlValue(v);

			_constants.Add(expr, value);

			return value;
		}

		#endregion

		#region Build Parameter

		readonly Dictionary<Expression,ParameterAccessor> _parameters   = new Dictionary<Expression, ParameterAccessor>();
		readonly HashSet<Expression>                      _asParameters = new HashSet<Expression>();

		ParameterAccessor BuildParameter(Expression expr)
		{
			ParameterAccessor p;

			if (_parameters.TryGetValue(expr, out p))
				return p;

			string name = null;

			var newExpr = ReplaceParameter(_expressionAccessors, expr, nm => name = nm);
			var mapper  = Expression.Lambda<Func<Expression,object[],object>>(
				Expression.Convert(newExpr, typeof(object)),
				new [] { ExpressionParam, ParametersParam });

			p = new ParameterAccessor
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, name, null)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p;
		}

		Expression ReplaceParameter(IDictionary<Expression,Expression> expressionAccessors, Expression expression, Action<string> setName)
		{
			return expression.Convert(expr =>
			{
				if (expr.NodeType == ExpressionType.Constant)
				{
					var c = (ConstantExpression)expr;

					if (!ExpressionHelper.IsConstant(expr.Type) || _asParameters.Contains(c))
					{
						var val = expressionAccessors[expr];

						expr = Expression.Convert(val, expr.Type);

						if (expression.NodeType == ExpressionType.MemberAccess)
						{
							var ma = (MemberExpression)expression;
							setName(ma.Member.Name);
						}
					}
				}

				return expr;
			});
		}

		#endregion

		#region ParseEnumerable

		ISqlExpression ParseEnumerable(IParseContext context, MethodCallExpression expression)
		{
			return ParseSubQuery(context, expression);

			//throw new NotImplementedException();

			/*
			QueryField field = queries.Length == 1 && queries[0] is QuerySource.GroupBy ? queries[0] : null;

			if (field == null)
				field = GetField(lambda, expression.Arguments[0], queries);

			while (field is QuerySource.SubQuerySourceColumn)
				field = ((QuerySource.SubQuerySourceColumn)field).SourceColumn;

			if (field is QuerySource.GroupJoin && expression.Method.Name == "Count")
			{
				var ex = ((QuerySource.GroupJoin)field).Counter.GetExpressions(this)[0];
				return ex;
			}

			if (!(field is QuerySource.GroupBy))
				throw new LinqException("'{0}' cannot be converted to SQL.", expression);

			var groupBy = (QuerySource.GroupBy)field;
			var expr    = ParseEnumerable(expression, groupBy);

			if (queries.Length == 1 && queries[0] is QuerySource.SubQuery)
			{
				var subQuery  = (QuerySource.SubQuery)queries[0];
				var column    = groupBy.FindField(new QueryField.ExprColumn(groupBy, expr, null));
				var subColumn = subQuery.EnsureField(column);

				expr = subColumn.GetExpressions(this)[0];
			}

			return expr;
			*/
		}

		#endregion

		#region Predicate Parser

		ISqlPredicate ParsePredicate(IParseContext context, Expression expression)
		{
			switch (expression.NodeType)
			{
				case ExpressionType.Equal              :
				case ExpressionType.NotEqual           :
				case ExpressionType.GreaterThan        :
				case ExpressionType.GreaterThanOrEqual :
				case ExpressionType.LessThan           :
				case ExpressionType.LessThanOrEqual    :
					{
						var e = (BinaryExpression)expression;
						return ParseCompare(context, expression.NodeType, e.Left, e.Right);
					}

				case ExpressionType.Call               :
					{
						var e = (MethodCallExpression)expression;

						ISqlPredicate predicate = null;

						if (e.Method.Name == "Equals" && e.Object != null && e.Arguments.Count == 1)
							return ParseCompare(context, ExpressionType.Equal, e.Object, e.Arguments[0]);

						if (e.Method.DeclaringType == typeof(string))
						{
							switch (e.Method.Name)
							{
								case "Contains"   : predicate = ParseLikePredicate(context, e, "%", "%"); break;
								case "StartsWith" : predicate = ParseLikePredicate(context, e, "",  "%"); break;
								case "EndsWith"   : predicate = ParseLikePredicate(context, e, "%", "");  break;
							}
						}
						else if (e.Method.DeclaringType == typeof(Enumerable))
						{
							switch (e.Method.Name)
							{
								case "Contains" :
									predicate = ParseInPredicate(context, e);
									break;
							}
						}
						else if (TypeHelper.IsSameOrParent(typeof(IList), e.Method.DeclaringType))
						{
							switch (e.Method.Name)
							{
								case "Contains" :
									predicate = ParseInPredicate(context, e);
									break;
							}
						}
#if !SILVERLIGHT
						else if (e.Method == ReflectionHelper.Functions.String.Like11) predicate = ParseLikePredicate(context, e);
						else if (e.Method == ReflectionHelper.Functions.String.Like12) predicate = ParseLikePredicate(context, e);
#endif
						else if (e.Method == ReflectionHelper.Functions.String.Like21) predicate = ParseLikePredicate(context, e);
						else if (e.Method == ReflectionHelper.Functions.String.Like22) predicate = ParseLikePredicate(context, e);

						if (predicate != null)
							return Convert(context, predicate);

						break;
					}

				case ExpressionType.Conditional:
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							ParseExpression(context, expression),
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(true)));

				case ExpressionType.MemberAccess:
					{
						var e = expression as MemberExpression;

						if (e.Member.Name == "HasValue" && 
							e.Member.DeclaringType.IsGenericType && 
							e.Member.DeclaringType.GetGenericTypeDefinition() == typeof(Nullable<>))
						{
							var expr = ParseExpression(context, e.Expression);
							return Convert(context, new SqlQuery.Predicate.IsNull(expr, true));
						}

						break;
					}

				case ExpressionType.TypeIs:
					{
						throw new NotImplementedException();

						/*
						var e     = expression as TypeBinaryExpression;
						var table = GetSource(lambda, e.Expression, queries) as QuerySource.Table;

						if (table != null && table.InheritanceMapping.Count > 0)
							return MakeIsPredicate(table, e.TypeOperand);

						break;
						*/
					}
			}

			var ex = ParseExpression(context, expression);

			if (SqlExpression.NeedsEqual(ex))
				return Convert(context, new SqlQuery.Predicate.ExprExpr(ex, SqlQuery.Predicate.Operator.Equal, new SqlValue(true)));

			return Convert(context, new SqlQuery.Predicate.Expr(ex));
		}

		#region ParseCompare

		ISqlPredicate ParseCompare(IParseContext context, ExpressionType nodeType, Expression left, Expression right)
		{
			if (left.NodeType == ExpressionType.Convert && left.Type == typeof(int) && right.NodeType == ExpressionType.Constant)
			{
				var conv = (UnaryExpression)left;

				if (conv.Operand.Type == typeof(char))
				{
					left  = conv.Operand;
					right = Expression.Constant(ConvertTo<char>.From(((ConstantExpression)right).Value));
				}
			}

			if (right.NodeType == ExpressionType.Convert && right.Type == typeof(int) && left.NodeType == ExpressionType.Constant)
			{
				var conv = (UnaryExpression)right;

				if (conv.Operand.Type == typeof(char))
				{
					right = conv.Operand;
					left  = Expression.Constant(ConvertTo<char>.From(((ConstantExpression)left).Value));
				}
			}

			switch (nodeType)
			{
				case ExpressionType.Equal    :
				case ExpressionType.NotEqual :

					var p = ParseObjectComparison(nodeType, context, left, context, right);
					if (p != null)
						return p;

					p = ParseObjectNullComparison(context, left, right, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					p = ParseObjectNullComparison(context, right, left, nodeType == ExpressionType.Equal);
					if (p != null)
						return p;

					if (left.NodeType == ExpressionType.New || right.NodeType == ExpressionType.New)
					{
						p = ParseNewObjectComparison(context, nodeType, left, right);
						if (p != null)
							return p;
					}

					break;
			}

			SqlQuery.Predicate.Operator op;

			switch (nodeType)
			{
				case ExpressionType.Equal             : op = SqlQuery.Predicate.Operator.Equal;          break;
				case ExpressionType.NotEqual          : op = SqlQuery.Predicate.Operator.NotEqual;       break;
				case ExpressionType.GreaterThan       : op = SqlQuery.Predicate.Operator.Greater;        break;
				case ExpressionType.GreaterThanOrEqual: op = SqlQuery.Predicate.Operator.GreaterOrEqual; break;
				case ExpressionType.LessThan          : op = SqlQuery.Predicate.Operator.Less;           break;
				case ExpressionType.LessThanOrEqual   : op = SqlQuery.Predicate.Operator.LessOrEqual;    break;
				default: throw new InvalidOperationException();
			}

			if (left.NodeType == ExpressionType.Convert || right.NodeType == ExpressionType.Convert)
			{
				var p = ParseEnumConversion(context, left, op, right);
				if (p != null)
					return p;
			}

			var l = ParseExpression(context, left);
			var r = ParseExpression(context, right);

			switch (nodeType)
			{
				case ExpressionType.Equal   :
				case ExpressionType.NotEqual:

					if (!context.SqlQuery.ParameterDependent && (l is SqlParameter || r is SqlParameter) && l.CanBeNull() && r.CanBeNull())
						context.SqlQuery.ParameterDependent = true;

					break;
			}

			if (l is SqlQuery.SearchCondition)
				l = Convert(context, new SqlFunction(typeof(bool), "CASE", l, new SqlValue(true), new SqlValue(false)));
			//l = Convert(new SqlFunction("CASE",
			//	l, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)l) }), new SqlValue(false),
			//	new SqlValue(false)));

			if (r is SqlQuery.SearchCondition)
				r = Convert(context, new SqlFunction(typeof(bool), "CASE", r, new SqlValue(true), new SqlValue(false)));
			//r = Convert(new SqlFunction("CASE",
			//	r, new SqlValue(true),
			//	new SqlQuery.SearchCondition(new[] { new SqlQuery.Condition(true, (SqlQuery.SearchCondition)r) }), new SqlValue(false),
			//	new SqlValue(false)));

			return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
		}

		#endregion

		#region ParseEnumConversion

		ISqlPredicate ParseEnumConversion(IParseContext context, Expression left, SqlQuery.Predicate.Operator op, Expression right)
		{
			UnaryExpression conv;
			Expression      value;

			if (left.NodeType == ExpressionType.Convert)
			{
				conv  = (UnaryExpression)left;
				value = right;
			}
			else
			{
				conv  = (UnaryExpression)right;
				value = left;
			}

			var operand = conv.Operand;
			var type    = operand.Type;

			if (!type.IsEnum)
				return null;

			var dic = new Dictionary<object, object>();

			var nullValue = MappingSchema.GetNullValue(type);

			if (nullValue != null)
				dic.Add(nullValue, null);

			var mapValues = MappingSchema.GetMapValues(type);

			if (mapValues != null)
				foreach (var mv in mapValues)
					if (!dic.ContainsKey(mv.OrigValue))
						dic.Add(mv.OrigValue, mv.MapValues[0]);

			if (dic.Count == 0)
				return null;

			switch (value.NodeType)
			{
				case ExpressionType.Constant:
					{
						var    origValue = Enum.Parse(type, Enum.GetName(type, ((ConstantExpression)value).Value), false);
						object mapValue;

						if (!dic.TryGetValue(origValue, out mapValue))
							return null;

						ISqlExpression l, r;

						if (left.NodeType == ExpressionType.Convert)
						{
							l = ParseExpression(context, operand);
							r = new SqlValue(mapValue);
						}
						else
						{
							r = ParseExpression(context, operand);
							l = new SqlValue(mapValue);
						}

						return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
					}

				case ExpressionType.Convert:
					{
						value = ((UnaryExpression)value).Operand;

						var l = ParseExpression(context, operand);
						var r = ParseExpression(context, value);

						if (l is SqlParameter) ((SqlParameter)l).SetEnumConverter(type, MappingSchema);
						if (r is SqlParameter) ((SqlParameter)r).SetEnumConverter(type, MappingSchema);

						return Convert(context, new SqlQuery.Predicate.ExprExpr(l, op, r));
					}
			}

			return null;
		}

		#endregion

		#region ParseObjectNullComparison

		ISqlPredicate ParseObjectNullComparison(IParseContext context, Expression left, Expression right, bool isEqual)
		{
			if (right.NodeType == ExpressionType.Constant && ((ConstantExpression)right).Value == null)
			{
				if (left.NodeType == ExpressionType.MemberAccess || left.NodeType == ExpressionType.Parameter)
				{
					var ctx = GetContext(context, left);

					if (ctx.IsExpression(left, 0, RequestFor.Object) ||
						left.NodeType == ExpressionType.Parameter && ctx.IsExpression(left, 0, RequestFor.Field))
					{
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
					}

					/*
					var field = GetField(lambda, left, queries);

					if (field is QuerySource.GroupJoin)
					{
						var join = (QuerySource.GroupJoin)field;
						var expr = join.CheckNullField.GetExpressions(this)[0];

						return Convert(context, new SqlQuery.Predicate.IsNull(expr, !isEqual));
					}

					if (field is QuerySource || field == null && left.NodeType == ExpressionType.Parameter)
						return new SqlQuery.Predicate.Expr(new SqlValue(!isEqual));
					*/
				}
			}

			return null;
		}

		#endregion

		#region ParseObjectComparison

		public ISqlPredicate ParseObjectComparison(
			ExpressionType nodeType,
			IParseContext  leftContext,
			Expression     left,
			IParseContext  rightContext,
			Expression     right)
		{
			var qsl = GetContext(leftContext,  left);
			var qsr = GetContext(rightContext, right);

			var sl = qsl != null && qsl.IsExpression(left,  0, RequestFor.Object);
			var sr = qsr != null && qsr.IsExpression(right, 0, RequestFor.Object);

			if (sl == false && sr == false)
				return null;

			if (sl == false)
			{
				var r = right;
				right = left;
				left  = r;

				var c = rightContext;
				rightContext = leftContext;
				leftContext  = c;

				var q = qsr;
				//qsr = qsl;
				qsl = q;

				//sl = true;
				sr = false;
			}

			var isNull = right is ConstantExpression && ((ConstantExpression)right).Value == null;
			var lcols  = qsl.ConvertToSql(left, 0, ConvertFlags.Key);

			if (lcols.Length == 0)
				return null;

			var condition = new SqlQuery.SearchCondition();

			foreach (var lcol in lcols)
			{
				if (lcol.Member == null)
					throw new InvalidOperationException();

				ISqlExpression rcol = null;

				if (sr)
					rcol = rightContext.ConvertToSql(Expression.MakeMemberAccess(right, lcol.Member), 0, ConvertFlags.Field).Single().Sql;

				var rex =
					isNull ?
						new SqlValue(right.Type, null) :
						sr ?
							rcol :
							GetParameter(right, lcol.Member);

				var predicate = Convert(leftContext, new SqlQuery.Predicate.ExprExpr(
					lcol.Sql,
					nodeType == ExpressionType.Equal ? SqlQuery.Predicate.Operator.Equal : SqlQuery.Predicate.Operator.NotEqual,
					rex));

				condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			if (nodeType == ExpressionType.NotEqual)
				foreach (var c in condition.Conditions)
					c.IsOr = true;

			return condition;
		}

		ISqlPredicate ParseNewObjectComparison(IParseContext context, ExpressionType nodeType, Expression left, Expression right)
		{
			left  = FindExpression(left);
			right = FindExpression(right);

			var condition = new SqlQuery.SearchCondition();

			if (left.NodeType != ExpressionType.New)
			{
				var temp = left;
				left  = right;
				right = temp;
			}

			var newRight = right as NewExpression;
			var newExpr  = (NewExpression)left;

			for (var i = 0; i < newExpr.Arguments.Count; i++)
			{
				var lex = ParseExpression(context, newExpr.Arguments[i]);
				var rex =
					newRight != null ?
						ParseExpression(context, newRight.Arguments[i]) :
						GetParameter(right, newExpr.Members[i]);

				var predicate = Convert(context,
					new SqlQuery.Predicate.ExprExpr(
						lex,
						nodeType == ExpressionType.Equal ? SqlQuery.Predicate.Operator.Equal : SqlQuery.Predicate.Operator.NotEqual,
						rex));

				condition.Conditions.Add(new SqlQuery.Condition(false, predicate));
			}

			if (nodeType == ExpressionType.NotEqual)
				foreach (var c in condition.Conditions)
					c.IsOr = true;

			return condition;
		}

		ISqlExpression GetParameter(Expression ex, MemberInfo member)
		{
			if (member is MethodInfo)
				member = TypeHelper.GetPropertyByMethod((MethodInfo)member);

			var par    = ReplaceParameter(_expressionAccessors, ex, _ => {});
			var expr   = Expression.MakeMemberAccess(par.Type == typeof(object) ? Expression.Convert(par, member.DeclaringType) : par, member);
			var mapper = Expression.Lambda<Func<Expression,object[],object>>(
				Expression.Convert(expr, typeof(object)),
				new [] { ExpressionParam, ParametersParam });

			var p = new ParameterAccessor
			{
				Expression   = expr,
				Accessor     = mapper.Compile(),
				SqlParameter = new SqlParameter(expr.Type, member.Name, null)
			};

			_parameters.Add(expr, p);
			CurrentSqlParameters.Add(p);

			return p.SqlParameter;
		}

		static Expression FindExpression(Expression expr)
		{
			var ret = expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.Convert      :
						{
							var e = (UnaryExpression)expr;

							return
								e.Operand.NodeType == ExpressionType.ArrayIndex &&
								((BinaryExpression)e.Operand).Left == ParametersParam;
						}

					case ExpressionType.MemberAccess :
					case ExpressionType.New          :
						return true;
				}

				return false;
			});

			if (ret == null)
				throw new NotImplementedException();

			return ret;
		}

		#endregion

		#region ParseInPredicate

		private ISqlPredicate ParseInPredicate(IParseContext context, MethodCallExpression expression)
		{
			var e        = expression;
			var argIndex = e.Object != null ? 0 : 1;

			var expr = ParseExpression(context, e.Arguments[argIndex]);
			var arr  = e.Object ?? e.Arguments[0];

			switch (arr.NodeType)
			{
				case ExpressionType.NewArrayInit :
					{
						var newArr = (NewArrayExpression)arr;

						if (newArr.Expressions.Count == 0)
							return new SqlQuery.Predicate.Expr(new SqlValue(false));

						var exprs  = new ISqlExpression[newArr.Expressions.Count];

						for (var i = 0; i < newArr.Expressions.Count; i++)
							exprs[i] = ParseExpression(context, newArr.Expressions[i]);

						return new SqlQuery.Predicate.InList(expr, false, exprs);
					}

				default :

					if (CanBeCompiled(arr))
					{
						var p = BuildParameter(arr).SqlParameter;
						p.IsQueryParameter = false;
						return new SqlQuery.Predicate.InList(expr, false, p);
					}

					break;
			}

			throw new LinqException("'{0}' cannot be converted to SQL.", expression);
		}

		#endregion

		#region LIKE predicate

		ISqlPredicate ParseLikePredicate(IParseContext context, MethodCallExpression expression, string start, string end)
		{
			throw new NotImplementedException();

			/*
			var e = expression;
			var o = ParseExpression(context, e.Object);
			var a = ParseExpression(context, e.Arguments[0]);

			if (a is SqlValue)
			{
				var value = ((SqlValue)a).Value;

				if (value == null)
					throw new LinqException("NULL cannot be used as a LIKE predicate parameter.");

				return value.ToString().IndexOfAny(new[] { '%', '_' }) < 0?
					new SqlQuery.Predicate.Like(o, false, new SqlValue(start + value + end), null):
					new SqlQuery.Predicate.Like(o, false, new SqlValue(start + EscapeLikeText(value.ToString()) + end), new SqlValue('~'));
			}

			if (a is SqlParameter)
			{
				var p  = (SqlParameter)a;
				var ep = (from pm in CurrentSqlParameters where pm.SqlParameter == p select pm).First();

				ep = new Query<T>.Parameter
				{
					Expression   = ep.Expression,
					Accessor     = ep.Accessor,
					SqlParameter = new SqlParameter(ep.Expression.Type, p.Name, p.Value, GetLikeEscaper(start, end))
				};

				_parameters.Add(e, ep);
				CurrentSqlParameters.Add(ep);

				return new SqlQuery.Predicate.Like(o, false, ep.SqlParameter, new SqlValue('~'));
			}

			return null;
			*/
		}

		private ISqlPredicate ParseLikePredicate(IParseContext context, MethodCallExpression expression)
		{
			var e  = expression;
			var a1 = ParseExpression(context, e.Arguments[0]);
			var a2 = ParseExpression(context, e.Arguments[1]);

			ISqlExpression a3 = null;

			if (e.Arguments.Count == 3)
				a3 = ParseExpression(context, e.Arguments[2]);

			return new SqlQuery.Predicate.Like(a1, false, a2, a3);
		}

		static string EscapeLikeText(string text)
		{
			if (text.IndexOfAny(new[] { '%', '_' }) < 0)
				return text;

			var builder = new StringBuilder(text.Length);

			foreach (var ch in text)
			{
				switch (ch)
				{
					case '%':
					case '_':
					case '~':
						builder.Append('~');
						break;
				}

				builder.Append(ch);
			}

			return builder.ToString();
		}

		static Converter<object,object> GetLikeEscaper(string start, string end)
		{
			return value => value == null? null: start + EscapeLikeText(value.ToString()) + end;
		}

		#endregion

		#region MakeIsPredicate

		ISqlPredicate MakeIsPredicate(IParseContext context, QuerySource.Table table, Type typeOperand)
		{
			if (typeOperand == table.ObjectType && table.InheritanceMapping.Count(m => m.Type == typeOperand) == 0)
				return Convert(context, new SqlQuery.Predicate.Expr(new SqlValue(true)));

			var mapping = table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => m.m.Type == typeOperand && !m.m.IsDefault).ToList();

			switch (mapping.Count)
			{
				case 0:
					{
						var cond = new SqlQuery.SearchCondition();

						foreach (var m in table.InheritanceMapping.Select((m,i) => new { m, i }).Where(m => !m.m.IsDefault))
						{
							cond.Conditions.Add(
								new SqlQuery.Condition(
									false, 
									Convert(context,
										new SqlQuery.Predicate.ExprExpr(
											table.Columns[table.InheritanceDiscriminators[m.i]].Field,
											SqlQuery.Predicate.Operator.NotEqual,
											new SqlValue(m.m.Code)))));
						}

						return cond;
					}

				case 1:
					return Convert(context,
						new SqlQuery.Predicate.ExprExpr(
							table.Columns[table.InheritanceDiscriminators[mapping[0].i]].Field,
							SqlQuery.Predicate.Operator.Equal,
							new SqlValue(mapping[0].m.Code)));

				default:
					{
						var cond = new SqlQuery.SearchCondition();

						foreach (var m in mapping)
						{
							cond.Conditions.Add(
								new SqlQuery.Condition(
									false,
									Convert(context,
										new SqlQuery.Predicate.ExprExpr(
											table.Columns[table.InheritanceDiscriminators[m.i]].Field,
											SqlQuery.Predicate.Operator.Equal,
											new SqlValue(m.m.Code))),
									true));
						}

						return cond;
					}
			}
		}

		#endregion

		#endregion

		#region Search Condition Parser

		void ParseSearchCondition(IParseContext context, Expression expression, List<SqlQuery.Condition> conditions)
		{
			if (IsSubQuery(context, expression))
			{
				var cond = ParseConditionSubQuery(context, expression);

				if (cond != null)
				{
					conditions.Add(cond);
					return;
				}
			}

			switch (expression.NodeType)
			{
				case ExpressionType.AndAlso:
					{
						var e = (BinaryExpression)expression;

						ParseSearchCondition(context, e.Left,  conditions);
						ParseSearchCondition(context, e.Right, conditions);

						break;
					}

				case ExpressionType.OrElse:
					{
						var e           = (BinaryExpression)expression;
						var orCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(context, e.Left,  orCondition.Conditions);
						orCondition.Conditions[orCondition.Conditions.Count - 1].IsOr = true;
						ParseSearchCondition(context, e.Right, orCondition.Conditions);

						conditions.Add(new SqlQuery.Condition(false, orCondition));

						break;
					}

				case ExpressionType.Not:
					{
						var e            = expression as UnaryExpression;
						var notCondition = new SqlQuery.SearchCondition();

						ParseSearchCondition(context, e.Operand, notCondition.Conditions);

						if (notCondition.Conditions.Count == 1 && notCondition.Conditions[0].Predicate is SqlQuery.Predicate.NotExpr)
						{
							var p = notCondition.Conditions[0].Predicate as SqlQuery.Predicate.NotExpr;
							p.IsNot = !p.IsNot;
							conditions.Add(notCondition.Conditions[0]);
						}
						else
							conditions.Add(new SqlQuery.Condition(true, notCondition));

						break;
					}

				default:
					var predicate = ParsePredicate(context, expression);

					if (predicate is SqlQuery.Predicate.Expr)
					{
						var expr = ((SqlQuery.Predicate.Expr)predicate).Expr1;

						if (expr.ElementType == QueryElementType.SearchCondition)
						{
							var sc = (SqlQuery.SearchCondition)expr;

							if (sc.Conditions.Count == 1)
							{
								conditions.Add(sc.Conditions[0]);
								break;
							}
						}
					}

					conditions.Add(new SqlQuery.Condition(false, predicate));

					break;
			}
		}

		#region ParseConditionSubQuery

		SqlQuery.Condition ParseConditionSubQuery(IParseContext context, Expression expr)
		{
			if (expr.NodeType != ExpressionType.Call)
				return null;

			var call = (MethodCallExpression)expr;

			if (!call.IsQueryable())
				return null;

			SqlQuery.Condition       cond = null;
			Func<SqlQuery.Condition> func = null;

			switch (call.Method.Name)
			{
				case "Any" :

					if (call.Arguments.Count == 1)
						func = () => ParseAnyCondition(SetType.Any, call, null, null);
					else if (call.Arguments.Count == 2)
						func = () => ParseAnyCondition(SetType.Any, call, null/*call.Arguments[1]*/, null);
					else
						return null;
					break;

				case "All" :

					if (call.Arguments.Count == 2)
						func = () => ParseAnyCondition(SetType.Any, call, null/*call.Arguments[1]*/, null);
					else
						return null;
					break;

				case "Contains":

					if (call.Method.DeclaringType == typeof(Queryable))
					{
						throw new NotImplementedException();
						/*
						Expression s = null;
						call.IsQueryableMethod2("Contains", seq => s = seq, ex =>
						{
							func = () =>
							{
								var param  = Expression.Parameter(ex.Type, ex.NodeType == ExpressionType.Parameter ? ((ParameterExpression)ex).Name : "t");
								var lambda = new LambdaInfo(Expression.Equal(param, ex), param);
								return ParseAnyCondition(SetType.In, s, lambda, ex);
							};
							return true;
						});
						*/
					}

					break;
			}

			if (func != null)
			{
				SubQueryParsingCounter++;
				cond = func();
				SubQueryParsingCounter--;
			}

			return cond;
		}

		#endregion

		#endregion

		#region ParseAny

		enum SetType { Any, All, In }

		SqlQuery.Condition ParseAnyCondition(SetType setType, Expression expr, LambdaInfo lambda, Expression inExpr)
		{
			throw new NotImplementedException();

			/*
			var sql = CurrentSql;
			var cs  = _convertSource;

			CurrentSql = new SqlQuery();

			var associationList = new Dictionary<QuerySource,QuerySource>();

			_convertSource = (s,l) =>
			{
				var t = s as QuerySource.Table;

				if (t != null && t.ParentAssociation != null)
				{
					if (ParentQueries.Count > 0)
					{
						foreach (var parentQuery in ParentQueries)
						{
							var parent = parentQuery.Parent;

							while (parent is QuerySource.SubQuerySourceColumn)
								parent = ((QuerySource.SubQuerySourceColumn)parent).SourceColumn;

							if (parent.Find(t.ParentAssociation))
							{
								var orig = t;
								t = CreateTable(new SqlQuery(), l);

								associationList.Add(t, orig);

								var csql = CurrentSql.From.Tables.Count == 0 ? t.SqlQuery : CurrentSql;

								foreach (var c in orig.ParentAssociationJoin.Condition.Conditions)
								{
									var predicate = (SqlQuery.Predicate.ExprExpr)c.Predicate;
									csql.Where
										.Expr(predicate.Expr1)
										.Equal
										.Field(t.Columns[((SqlField)predicate.Expr2).Name].Field);
								}

								s = t;

								break;
							}
						}
					}
				}
				else
					s = cs(s, l);

				return s;
			};

			var query = ParseSequence(expr)[0];
			var any   = CurrentSql;

			_convertSource = cs;

			if (lambda != null)
			{
				if (setType == SetType.All)
				{
					var e  = Expression.Not(lambda.Body);
					//var pi = new NotParseInfo(e, lambda.Body);

					lambda = new LambdaInfo(e, lambda.Parameters);
				}

				if (inExpr == null || query.Fields.Count != 1)
					ParseWhere(lambda, query);
			}

			any.ParentSql = sql;
			CurrentSql    = sql;

			SqlQuery.Condition cond;

			if (inExpr != null && query.Fields.Count == 1)
			{
				query.Select(this);
				var ex = ParseExpression(inExpr);
				cond = new SqlQuery.Condition(false, new SqlQuery.Predicate.InSubQuery(ex, false, any));
			}
			else
				cond = new SqlQuery.Condition(setType == SetType.All, new SqlQuery.Predicate.FuncLike(SqlFunction.CreateExists(any)));

			ParsingTracer.DecIndentLevel();
			return cond;

			*/
		}

		#endregion

		#region CanBeTranslatedToSql

		bool CanBeTranslatedToSql(IParseContext context, Expression expr, bool canBeCompiled)
		{
			return null == expr.Find(pi =>
			{
				switch (pi.NodeType)
				{
					case ExpressionType.MemberAccess:
						{
							var ma = (MemberExpression)pi;
							var l  = ConvertMember(ma.Member);

							if (l != null)
								return !CanBeTranslatedToSql(context, l.Body.Unwrap(), canBeCompiled);

							var attr = GetFunctionAttribute(ma.Member);

							if (attr == null && !TypeHelper.IsNullableValueMember(ma.Member))
								goto case ExpressionType.Parameter;

							break;
						}

					case ExpressionType.Parameter:
						{
							if (canBeCompiled && GetContext(context, pi) == null)
								return !CanBeCompiled(pi);
							break;
						}

					case ExpressionType.Call:
						{
							var e = pi as MethodCallExpression;

							if (e.Method.DeclaringType != typeof(Enumerable))
							{
								var cm = ConvertMethod(e);

								if (cm != null)
									return !CanBeTranslatedToSql(context, cm, canBeCompiled);

								var attr = GetFunctionAttribute(e.Method);

								if (attr == null && canBeCompiled)
									return !CanBeCompiled(pi);
							}

							break;
						}

					case ExpressionType.New: return true;
				}

				return false;
			});
		}

		#endregion

		#region Helpers

		public IParseContext GetContext(IParseContext current, Expression expression)
		{
			var root = expression.GetRootObject();

			for (var ctx = current; current != null; current = current.Parent)
				if (ctx.IsExpression(root, 0, RequestFor.Root))
					return ctx;

			return null;
		}

		SqlFunctionAttribute GetFunctionAttribute(ICustomAttributeProvider member)
		{
			var attrs = member.GetCustomAttributes(typeof(SqlFunctionAttribute), true);

			if (attrs.Length == 0)
				return null;

			SqlFunctionAttribute attr = null;

			foreach (SqlFunctionAttribute a in attrs)
			{
				if (a.SqlProvider == SqlProvider.Name)
				{
					attr = a;
					break;
				}

				if (a.SqlProvider == null)
					attr = a;
			}

			return attr;
		}

		TableFunctionAttribute GetTableFunctionAttribute(ICustomAttributeProvider member)
		{
			var attrs = member.GetCustomAttributes(typeof(TableFunctionAttribute), true);

			if (attrs.Length == 0)
				return null;

			TableFunctionAttribute attr = null;

			foreach (TableFunctionAttribute a in attrs)
			{
				if (a.SqlProvider == SqlProvider.Name)
				{
					attr = a;
					break;
				}

				if (a.SqlProvider == null)
					attr = a;
			}

			return attr;
		}

		LambdaExpression ConvertMember(MemberInfo mi)
		{
			var lambda = SqlProvider.ConvertMember(mi);

			if (lambda == null)
			{
				var attrs = mi.GetCustomAttributes(typeof(MethodExpressionAttribute), true);

				if (attrs.Length == 0)
					return null;

				MethodExpressionAttribute attr = null;

				foreach (MethodExpressionAttribute a in attrs)
				{
					if (a.SqlProvider == SqlProvider.Name)
					{
						attr = a;
						break;
					}

					if (a.SqlProvider == null)
						attr = a;
				}

				if (attr != null)
				{
					var call = Expression.Lambda<Func<LambdaExpression>>(
						Expression.Convert(Expression.Call(mi.DeclaringType, attr.MethodName, Array<Type>.Empty), typeof(LambdaExpression)));

					lambda = call.Compile()();
				}
			}

			return lambda;
		}

		public ISqlExpression Convert(IParseContext context, ISqlExpression expr)
		{
			SqlProvider.SqlQuery = context.SqlQuery;
			return SqlProvider.ConvertExpression(expr);
		}

		public ISqlPredicate Convert(IParseContext context, ISqlPredicate predicate)
		{
			SqlProvider.SqlQuery = context.SqlQuery;
			return SqlProvider.ConvertPredicate(predicate);
		}

		public ISqlExpression ParseTimeSpanMember(IParseContext context, MemberExpression expression)
		{
			if (expression.Member.DeclaringType == typeof(TimeSpan))
			{
				switch (expression.Expression.NodeType)
				{
					case ExpressionType.Subtract       :
					case ExpressionType.SubtractChecked:

						Sql.DateParts datePart;

						switch (expression.Member.Name)
						{
							case "TotalMilliseconds" : datePart = Sql.DateParts.Millisecond; break;
							case "TotalSeconds"      : datePart = Sql.DateParts.Second;      break;
							case "TotalMinutes"      : datePart = Sql.DateParts.Minute;      break;
							case "TotalHours"        : datePart = Sql.DateParts.Hour;        break;
							case "TotalDays"         : datePart = Sql.DateParts.Day;         break;
							default                  : return null;
						}

						var e = (BinaryExpression)expression.Expression;

						return new SqlFunction(
							typeof(int),
							"DateDiff",
							new SqlValue(datePart),
							ParseExpression(context, e.Right),
							ParseExpression(context, e.Left));
				}
			}

			return null;
		}

		Expression ConvertNew(NewExpression pi)
		{
			var lambda = ConvertMember(pi.Constructor);

			if (lambda != null)
			{
				var ef    = lambda.Body.Unwrap();
				var parms = new Dictionary<string,int>(lambda.Parameters.Count);
				var pn    = 0;

				foreach (var p in lambda.Parameters)
					parms.Add(p.Name, pn++);

				return ef.Convert(wpi =>
				{
					if (wpi.NodeType == ExpressionType.Parameter)
					{
						var pe   = (ParameterExpression)wpi;
						var n    = parms[pe.Name];
						return pi.Arguments[n];
					}

					return wpi;
				});
			}

			return null;
		}

		Expression ConvertMethod(MethodCallExpression pi)
		{
			var l = ConvertMember(pi.Method);

			if (l == null)
				return null;

			var ef    = l.Body.Unwrap();
			var parms = new Dictionary<string,int>(l.Parameters.Count);
			var pn    = pi.Method.IsStatic ? 0 : -1;

			foreach (var p in l.Parameters)
				parms.Add(p.Name, pn++);

			var pie = ef.Convert(wpi =>
			{
				if (wpi.NodeType == ExpressionType.Parameter)
				{
					int n;
					if (parms.TryGetValue(((ParameterExpression)wpi).Name, out n))
						return n < 0 ? pi.Object : pi.Arguments[n];
				}

				return wpi;
			});

			return pie;
		}

		#endregion
	}
}
