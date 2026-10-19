namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

/// <summary>
/// Progress of a running generation, reported per table.
/// </summary>
/// <param name="Table">The table, collection or graph file being written.</param>
/// <param name="RowsWritten">Rows (documents, nodes or relationships) written to that table so far.</param>
/// <param name="BytesWritten">Bytes written to that table so far.</param>
public readonly record struct DatasetProgress(string Table, long RowsWritten, long BytesWritten);
