﻿using System;
using System.Linq.Expressions;
using System.Reflection;

namespace BLToolkit.Data.Linq
{
	class LambdaInfo
	{
		public LambdaInfo(Expression b, params ParameterExpression[] parms)
		{
			Body       = b;
			Parameters = parms;
		}

		public Expression            Body;
		public ParameterExpression[] Parameters;
		public MethodInfo            MethodInfo;
	}
}
