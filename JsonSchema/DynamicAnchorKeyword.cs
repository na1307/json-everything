﻿using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Json.Schema;

/// <summary>
/// Handles `$dynamicAnchor`.
/// </summary>
[SchemaPriority(long.MinValue + 3)]
[SchemaKeyword(Name)]
[SchemaSpecVersion(SpecVersion.Draft202012)]
[SchemaSpecVersion(SpecVersion.DraftNext)]
[Vocabulary(Vocabularies.Core202012Id)]
[Vocabulary(Vocabularies.CoreNextId)]
[JsonConverter(typeof(DynamicAnchorKeywordJsonConverter))]
public class DynamicAnchorKeyword : IJsonSchemaKeyword
{
	/// <summary>
	/// The JSON name of the keyword.
	/// </summary>
	public const string Name = "$dynamicAnchor";

	/// <summary>
	/// Gets the anchor value.
	/// </summary>
	public string Value { get; }

	/// <summary>
	/// Creates a new <see cref="DynamicAnchorKeyword"/>.
	/// </summary>
	/// <param name="value">The anchor value.</param>
	public DynamicAnchorKeyword(string value)
	{
		Value = value ?? throw new ArgumentNullException(nameof(value));
	}

	/// <summary>
	/// Builds a constraint object for a keyword.
	/// </summary>
	/// <param name="schemaConstraint">The <see cref="SchemaConstraint"/> for the schema object that houses this keyword.</param>
	/// <param name="localConstraints">
	/// The set of other <see cref="KeywordConstraint"/>s that have been processed prior to this one.
	/// Will contain the constraints for keyword dependencies.
	/// </param>
	/// <param name="context">The <see cref="EvaluationContext"/>.</param>
	/// <returns>A constraint object.</returns>
	public KeywordConstraint GetConstraint(SchemaConstraint schemaConstraint,
		IReadOnlyList<KeywordConstraint> localConstraints,
		EvaluationContext context)
	{
		return KeywordConstraint.Skip;
	}
}

internal class DynamicAnchorKeywordJsonConverter : JsonConverter<DynamicAnchorKeyword>
{
	public override DynamicAnchorKeyword Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException("Expected string");

		var uriString = reader.GetString()!;
		if (!AnchorKeyword.AnchorPattern.IsMatch(uriString))
			throw new JsonException("Expected anchor format");

		return new DynamicAnchorKeyword(uriString);
	}
	public override void Write(Utf8JsonWriter writer, DynamicAnchorKeyword value, JsonSerializerOptions options)
	{
		writer.WriteString(DynamicAnchorKeyword.Name, value.Value);
	}
}