﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using BLToolkit.Reflection;

namespace BLToolkit.Data.Sql.SqlProvider
{
	using DataProvider;

	public class MySqlSqlProvider : BasicSqlProvider
	{
		public override int CommandCount(SqlQuery sqlQuery)
		{
			return sqlQuery.QueryType == QueryType.Insert && sqlQuery.Set.WithIdentity ? 2 : 1;
		}

		protected override void BuildCommand(int commandNumber, StringBuilder sb)
		{
			sb.AppendLine("SELECT LAST_INSERT_ID()");
		}

		protected override ISqlProvider CreateSqlProvider()
		{
			return new MySqlSqlProvider();
		}

		protected override string LimitFormat { get { return "LIMIT {0}"; } }

		public override bool IsNestedJoinParenthesisRequired { get { return true; } }

		protected override void BuildOffsetLimit(StringBuilder sb)
		{
			if (SqlQuery.Select.SkipValue == null)
				base.BuildOffsetLimit(sb);
			else
			{
				AppendIndent(sb).AppendFormat
				(
					SqlQuery.Select.SkipValue != null ? "LIMIT {0},{1}" : "LIMIT {1}",
					BuildExpression(new StringBuilder(), SqlQuery.Select.SkipValue),
					SqlQuery.Select.TakeValue == null?
						long.MaxValue.ToString():
						BuildExpression(new StringBuilder(), SqlQuery.Select.TakeValue).ToString()
				).AppendLine();
			}
		}

		public override ISqlExpression ConvertExpression(ISqlExpression expr)
		{
			expr = base.ConvertExpression(expr);

			if (expr is SqlBinaryExpression)
			{
				SqlBinaryExpression be = (SqlBinaryExpression)expr;

				switch (be.Operation)
				{
					case "+":
						if (be.SystemType == typeof(string))
						{
							if (be.Expr1 is SqlFunction)
							{
								SqlFunction func = (SqlFunction)be.Expr1;

								if (func.Name == "Concat")
								{
									List<ISqlExpression> list = new List<ISqlExpression>(func.Parameters);
									list.Add(be.Expr2);
									return new SqlFunction(be.SystemType, "Concat", list.ToArray());
								}
							}

							return new SqlFunction(be.SystemType, "Concat", be.Expr1, be.Expr2);
						}

						break;
				}
			}
			else if (expr is SqlFunction)
			{
				SqlFunction func = (SqlFunction) expr;

				switch (func.Name)
				{
					case "Convert" :
						Type ftype = TypeHelper.GetUnderlyingType(func.SystemType);

						if (ftype == typeof(bool))
						{
							ISqlExpression ex = AlternativeConvertToBoolean(func, 1);
							if (ex != null)
								return ex;
						}

						if ((ftype == typeof(double) || ftype == typeof(float)) && TypeHelper.GetUnderlyingType(func.Parameters[1].SystemType) == typeof(decimal))
							return func.Parameters[1];

						return new SqlExpression(func.SystemType, "Cast({0} as {1})", Precedence.Primary, FloorBeforeConvert(func), func.Parameters[0]);
				}
			}
			else if (expr is SqlExpression)
			{
				SqlExpression e = (SqlExpression)expr;

				if (e.Expr.StartsWith("Extract(DayOfYear"))
					return new SqlFunction(e.SystemType, "DayOfYear", e.Parameters);

				if (e.Expr.StartsWith("Extract(WeekDay"))
					return Inc(
						new SqlFunction(e.SystemType,
							"WeekDay",
							new SqlFunction(
								null,
								"Date_Add",
								e.Parameters[0],
								new SqlExpression(null, "interval 1 day"))));
			}

			return expr;
		}

		protected override void BuildDataType(StringBuilder sb, SqlDataType type)
		{
			switch (type.SqlDbType)
			{
				case SqlDbType.Int           :
				case SqlDbType.SmallInt      : sb.Append("Signed");        break;
				case SqlDbType.TinyInt       : sb.Append("Unsigned");      break;
				case SqlDbType.Money         : sb.Append("Decimal(19,4)"); break;
				case SqlDbType.SmallMoney    : sb.Append("Decimal(10,4)"); break;
				case SqlDbType.SmallDateTime :
				case SqlDbType.DateTime2     : sb.Append("DateTime");      break;
				case SqlDbType.Bit           : sb.Append("Boolean");       break;
				case SqlDbType.Float         :
				case SqlDbType.Real          : base.BuildDataType(sb, SqlDataType.Decimal); break;
				case SqlDbType.VarChar       :
				case SqlDbType.NVarChar      :
					sb.Append("Char");
					if (type.Length > 0)
						sb.Append('(').Append(type.Length).Append(')');
					break;
				default: base.BuildDataType(sb, type); break;
			}
		}

		protected override void BuildDeleteClause(StringBuilder sb)
		{
			AppendIndent(sb)
				.Append("DELETE ")
				.Append(Convert(GetTableAlias(SqlQuery.From.Tables[0]), ConvertType.NameToQueryTableAlias))
				.AppendLine();
		}

		protected override void BuildUpdateClause(StringBuilder sb)
		{
			base.BuildFromClause(sb);
			sb.Remove(0, 4).Insert(0, "UPDATE");
			base.BuildUpdateSet(sb);
		}

		protected override void BuildFromClause(StringBuilder sb)
		{
			if (SqlQuery.QueryType != QueryType.Update)
				base.BuildFromClause(sb);
		}

		public static char ParameterSymbol           { get; set; }
		public static bool TryConvertParameterSymbol { get; set; }

		private static string _commandParameterPrefix = "";
		public  static string  CommandParameterPrefix
		{
			get { return _commandParameterPrefix; }
			set { _commandParameterPrefix = string.IsNullOrEmpty(value) ? string.Empty : value; }
		}

		private static string _sprocParameterPrefix = "";
		public  static string  SprocParameterPrefix
		{
			get { return _sprocParameterPrefix; }
			set { _sprocParameterPrefix = string.IsNullOrEmpty(value) ? string.Empty : value; }
		}

		private static List<char> _convertParameterSymbols;
		public  static List<char>  ConvertParameterSymbols
		{
			get { return _convertParameterSymbols; }
			set { _convertParameterSymbols = value ?? new List<char>(); }
		}

		public override object Convert(object value, ConvertType convertType)
		{
			if (value == null)
				throw new ArgumentNullException("value");

			switch (convertType)
			{
				case ConvertType.NameToQueryParameter:
					return ParameterSymbol + value.ToString();

				case ConvertType.NameToCommandParameter:
					return ParameterSymbol + CommandParameterPrefix + value.ToString();

				case ConvertType.NameToSprocParameter:
					{
						string valueStr = value.ToString();
						if(string.IsNullOrEmpty(valueStr))
							throw new ArgumentException("Argument 'value' must represent parameter name.");

						if (valueStr[0] == ParameterSymbol)
							valueStr = valueStr.Substring(1);
						if (valueStr.StartsWith(SprocParameterPrefix, StringComparison.Ordinal))
							valueStr = valueStr.Substring(SprocParameterPrefix.Length);
						return ParameterSymbol + SprocParameterPrefix + valueStr;
					}

				case ConvertType.SprocParameterToName:
					if (value != null)
					{
						string str = value.ToString();
						str = (str.Length > 0 && (str[0] == ParameterSymbol || (TryConvertParameterSymbol && ConvertParameterSymbols.Contains(str[0])))) ? str.Substring(1) : str;

						if ((!string.IsNullOrEmpty(SprocParameterPrefix))
							&& str.StartsWith(SprocParameterPrefix))
							str = str.Substring(SprocParameterPrefix.Length);

						return str;
					}

					break;
			}

			return value;
		}
	}
}
