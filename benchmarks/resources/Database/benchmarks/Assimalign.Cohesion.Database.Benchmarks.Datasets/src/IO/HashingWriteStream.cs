using System;
using System.IO;
using System.Security.Cryptography;

namespace Assimalign.Cohesion.Database.Benchmarks.Datasets;

internal sealed class HashingWriteStream : Stream
{
    private readonly FileStream file;
    private readonly IncrementalHash hash = IncrementalHash.CreateHash(HashAlgorithmName.SHA256);
    public long Bytes { get; private set; }

    public HashingWriteStream(string path) => file = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None, 65536, FileOptions.SequentialScan);
    public string CompleteHash() { Flush(); return Convert.ToHexStringLower(hash.GetHashAndReset()); }
    public override void Write(byte[] buffer, int offset, int count) => Write(buffer.AsSpan(offset, count));
    public override void Write(ReadOnlySpan<byte> buffer) { file.Write(buffer); hash.AppendData(buffer); Bytes += buffer.Length; }
    public override void WriteByte(byte value) { Span<byte> one = stackalloc byte[1]; one[0] = value; Write(one); }
    public override void Flush() => file.Flush();
    protected override void Dispose(bool disposing) { if (disposing) { file.Dispose(); hash.Dispose(); } base.Dispose(disposing); }
    public override bool CanRead => false;
    public override bool CanSeek => false;
    public override bool CanWrite => true;
    public override long Length => Bytes;
    public override long Position { get => Bytes; set => throw new NotSupportedException(); }
    public override int Read(byte[] buffer, int offset, int count) => throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin) => throw new NotSupportedException();
    public override void SetLength(long value) => throw new NotSupportedException();
}
