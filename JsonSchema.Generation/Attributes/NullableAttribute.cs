﻿using System;

namespace Json.Schema.Generation;

/// <summary>
/// Overrides the <see cref="SchemaGeneratorConfiguration.Nullability"/> option and either
/// adds or removes `null` in the `type` keyword.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field |
				AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface,
	AllowMultiple = true)]
public class NullableAttribute : ConditionalAttribute, IAttributeHandler
{
	/// <summary>
	/// Gets whether `null` should be included in the `type` keyword.
	/// </summary>
	public bool IsNullable { get; }

	/// <summary>
	/// Creates a new <see cref="NullableAttribute"/> instance.
	/// </summary>
	/// <param name="isNullable">Whether `null` should be included in the `type` keyword.</param>
	public NullableAttribute(bool isNullable)
	{
		IsNullable = isNullable;
	}

	void IAttributeHandler.AddConstraints(SchemaGenerationContextBase context, Attribute attribute)
	{
	}
}