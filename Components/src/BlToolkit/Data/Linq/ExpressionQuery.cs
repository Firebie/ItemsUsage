﻿using System;
using System.Linq.Expressions;

namespace BLToolkit.Data.Linq
{
	class ExpressionQuery<T> : Table<T>
	{
		public ExpressionQuery(IDataContextInfo dataContext, Expression expression)
			: base(dataContext, expression)
		{
		}

		public new string SqlText
		{
			get { return base.SqlText; }
		}

#if OVERRIDETOSTRING

		public override string ToString()
		{
			return base.SqlText;
		}

#endif
	}
}
