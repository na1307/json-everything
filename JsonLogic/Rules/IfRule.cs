﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace Json.Logic.Rules;

/// <summary>
/// Handles the `if` and `?:` operations.
/// </summary>
[Operator("if")]
[Operator("?:")]
public class IfRule : Rule
{
	private readonly List<Rule> _components;

	internal IfRule(params Rule[] components)
	{
		_components = new List<Rule>(components);
	}

	/// <summary>
	/// Applies the rule to the input data.
	/// </summary>
	/// <param name="data">The input data.</param>
	/// <param name="contextData">
	///     Optional secondary data.  Used by a few operators to pass a secondary
	///     data context to inner operators.
	/// </param>
	/// <returns>The result of the rule.</returns>
	public override JsonNode? Apply(JsonNode? data, JsonNode? contextData = null)
	{
		bool condition;
		switch (_components.Count)
		{
			case 0:
				return null;
			case 1:
				return _components[0].Apply(data, contextData);
			case 2:
				condition = _components[0].Apply(data, contextData).IsTruthy();
				var thenResult = _components[1];

				return condition
					? thenResult.Apply(data, contextData)
					: null;
			default:
				var currentCondition = _components[0];
				var currentTrueResult = _components[1];
				var elseIndex = 2;

				while (currentCondition != null)
				{
					condition = currentCondition.Apply(data, contextData).IsTruthy();

					if (condition)
						return currentTrueResult.Apply(data, contextData);

					if (elseIndex == _components.Count) return null;

					currentCondition = _components[elseIndex++];

					if (elseIndex >= _components.Count)
						return currentCondition.Apply(data, contextData);

					currentTrueResult = _components[elseIndex++];
				}
				break;
		}

		throw new NotImplementedException("Something went wrong. This shouldn't happen.");
	}
}

internal class IfRuleJsonConverter : JsonConverter<IfRule>
{
	public override IfRule? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var parameters = JsonSerializer.Deserialize<Rule[]>(ref reader, options);

		if (parameters == null) return new IfRule();

		return new IfRule(parameters);
	}

	public override void Write(Utf8JsonWriter writer, IfRule value, JsonSerializerOptions options)
	{
		throw new NotImplementedException();
	}
}
