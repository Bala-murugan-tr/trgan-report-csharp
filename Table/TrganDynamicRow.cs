using System;
using System.Collections.Generic;
using System.Dynamic;

namespace TrganReport.Table;

/// <summary>
/// Represents a dynamic row in a <c>TrganTable</c>, allowing flexible assignment of column values
/// using dynamic property syntax (e.g., <c>row.ValueOf.Name = "Alice"</c>).
/// </summary>
public class TrganDynamicRow(List<string> columns) : DynamicObject {
    /// <summary>
    /// Stores the values assigned to each column in the row.
    /// </summary>
    private readonly Dictionary<string, string> _rowValues = [];

    /// <summary>
    /// Use to set or get column values for the current row instance.
    ///<para>
    /// <example>
    /// <code>
    ///var row = trganTable.AddEntry();
    ///row.ValueOf.Name = "Alice";              // Set value
    ///var name = row.ValueOf.Name;         // Get value
    /// </code>
    /// </example></para>
    /// </summary>
    public dynamic ValueOf => this;

    /// <summary>
    /// Overridden method to support dynamic property assignment.
    /// </summary>
    /// <remarks>
    /// This method is part of the internal dynamic infrastructure and is not intended for direct use.
    /// Prefer using <see cref="ValueOf"/>, to assign values to columns dynamically.
    /// </remarks>
    /// <param name="binder">The dynamic property being set.</param>
    /// <param name="value">The value to assign.</param>
    /// <returns><c>true</c> if the assignment was successful.</returns>
    /// <exception cref="ArgumentException">
    /// Thrown if the column name is not defined.
    /// </exception>
    public override bool TrySetMember(SetMemberBinder binder, object value) {
        string name = binder.Name;
        if (!columns.Contains(name))
            throw new ArgumentException($"Column '{name}' is not defined.");
        _rowValues[name] = value?.ToString() ?? "";
        return true;
    }

    /// <summary>
    /// Overridden method to support dynamic property retrieval.
    /// </summary>
    /// <remarks>
    /// This method is part of the internal dynamic infrastructure and is not intended for direct use.
    /// Prefer using <see cref="ValueOf"/> to retrieve column values.
    /// </remarks>
    /// <param name="binder">The dynamic property being accessed.</param>
    /// <param name="result">The retrieved value, if available.</param>
    /// <returns><c>true</c> always, even if the column is not defined.</returns>
    public override bool TryGetMember(GetMemberBinder binder, out object result) {
        string name = binder.Name;
        result = _rowValues.TryGetValue(name, out string val) ? val : null;
        return true;
    }

    /// <summary>
    /// Returns a dictionary of all column values for this row.
    /// Any columns not explicitly set are filled with empty strings.
    /// </summary>
    /// <returns>A dictionary mapping column names to their assigned values.</returns>
    internal Dictionary<string, string> GetValues() {
        foreach (string col in columns) {
            if (!_rowValues.ContainsKey(col))
                _rowValues[col] = "";
        }
        return new Dictionary<string, string>(_rowValues);
    }
}