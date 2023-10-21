namespace SocketClientServerLib.Helpers
{
    public class Logger
    {
        private readonly Action<string> _write;
        private readonly Action<string> _writeLine;
        private readonly Action<string> _writeLineError;
        public Logger(
            Action<string> write,
            Action<string> writeLine,
            Action<string> writelineError)
        {
            _write = write;
            _writeLine = writeLine;
            _writeLineError = writelineError;
        }
        public void Write(string message) => _write(message);
        public void Writeline(string message) => _writeLine(message);
        public void WritelineError(string message) => _writeLineError(message);
    }
}
