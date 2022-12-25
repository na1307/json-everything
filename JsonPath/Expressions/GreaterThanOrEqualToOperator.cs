﻿using System;
using System.Text.Json.Nodes;
using Json.More;

namespace Json.Path.Expressions;

internal class GreaterThanOrEqualToOperator : IBinaryComparativeOperator
{
	public int Precedence => 10;

	public bool Evaluate(JsonNode? left, JsonNode? right)
	{
		left = left.TryGetSingleValue();
		right = right.TryGetSingleValue();

		if (left.IsEquivalentTo(right)) return true;

		if (left is not JsonValue lValue ||
		    right is not JsonValue rValue)
			return false;

		if (left.TryGetValue(out string? leftString) &&
		    left.TryGetValue(out string? rightString))
			return string.Compare(leftString, rightString, StringComparison.Ordinal) == 1;

		return lValue.GetNumber() > rValue.GetNumber();
	}

	public override string ToString()
	{
		return ">=";
	}
}