namespace ACLI;

public class DummyStream : Stream
{
    public override bool CanRead => true;
    public override bool CanSeek => true;
    public override bool CanWrite => true;
    public override bool CanTimeout => true;
    public override long Length => 0;
    
    public override long Position
    {
        get => 0;
        set {}
    }
    public override int ReadTimeout
    {
        get => 0;
        set {}
    }
    public override int WriteTimeout
    {
        get => 0;
        set {}
    }
    
    public override void Flush() {}
    public override int Read(byte[] buffer, int offset, int count) => 0;
    public override long Seek(long offset, SeekOrigin origin) => 0;
    public override void SetLength(long value) {}
    public override void Write(byte[] buffer, int offset, int count) {}
    public override void WriteByte(byte value) {}
    public override void CopyTo(Stream destination, int bufferSize) {}
}