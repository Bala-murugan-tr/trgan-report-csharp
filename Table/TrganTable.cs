using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TrganReport.Exceptions;

namespace TrganReport.Table;
/// <summary>
/// Creates a new table with defined number of columns for storing execution data.
/// </summary>
/// <param name="maxColumn">Maximum number of columns to be filled</param>
public class TrganTable(int maxColumn) {
    private readonly int columnLimit = maxColumn;

    /// <summary>
    /// List of column names in the table.
    /// </summary>
    internal List<string> Columns { get; } = new List<string>(maxColumn);

    /// <summary>
    /// List of finalized rows, each row is a dictionary of column name and value.
    /// </summary>
    internal List<Dictionary<string, string>> Rows { get; } = [];

    private readonly ConcurrentQueue<TrganDynamicRow> Entries = new();

    /// <summary>
    /// Adds a column to the table.
    /// </summary>
    /// <param name="columnName">Name of the column to add.</param>
    /// <exception cref="InvalidOperationException">
    /// Thrown if the column limit is reached.
    /// </exception>
    public void AddColumn(string columnName) {
        if (Columns.Count >= columnLimit)
            throw new TrganTableException($"Given number of columns are already created, cannot add more than {columnLimit} columns.");
        if (!Columns.Contains(columnName))
            Columns.Add(columnName);
    }

    /// <summary>
    /// Creates a new row entry in the table.
    /// </summary>
    /// <returns>A row instance to set column values.</returns>
    public TrganDynamicRow AddEntry() {
        if (Columns.Count == 0)
            throw new TrganTableException("No columns defined. Call AddColumn() before adding entries.");
        TrganDynamicRow entry = new(Columns);
        Entries.Enqueue(entry);
        return entry;
    }

    /// <summary>
    /// Finalizes all row entries and adds them to the table.
    /// </summary>
    internal void FinalizeEntries() {
        while (Entries.TryDequeue(out TrganDynamicRow entry)) {
            Rows.Add(entry.GetValues());
        }
    }

    /// <summary>
    /// Returns the finalized table as a list of rows.
    /// Each row is a dictionary of column name and value.
    /// </summary>
    public IReadOnlyList<Dictionary<string, string>> GetTable() {
        return Rows.AsReadOnly();
    }
}


