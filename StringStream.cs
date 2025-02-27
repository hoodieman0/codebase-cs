using System.Text;

namespace Codebase;

/// <summary>
/// Creates a stream from a string.
/// </summary>
class StringStream : MemoryStream {
    /// <summary>
    /// Creates a memory stream from a given string.
    /// </summary>
    /// <param name="val">The string to read.</param>
    public StringStream(string val) : base(Encoding.Default.GetBytes(val)) { }

    /// <summary>
    /// Get the next character from the stream.
    /// </summary>
    /// <returns>The next character from the stream.</returns>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public char ReadChar(){
        if (Position == Length) throw new IndexOutOfRangeException();

        return (char) ReadByte();
    }

    /// <summary>
    /// Read the stream until a space is reached or stream length exceeded.
    /// </summary>
    /// <returns>A string of the characters from the stream, not including the space.</returns>
    /// <remarks>The stream's position is set to the space. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadWord() => ReadUntilDelimiter(' ');

    /// <summary>
    /// Read the stream until a new line is reached or stream length exceeded.
    /// </summary>
    /// <returns>A string of the characters from the stream, not including the \n.</returns>
    /// <remarks>The stream's position is set to the \n. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadNewline() => ReadUntilDelimiter('\n');
    
    /// <summary>
    /// Read the stream until the delimiter is reached or stream length exceeded.
    /// </summary>
    /// <param name="delimiters">The characters to end the read.</param>
    /// <returns>A string of the characters from the stream, not including the delimiter.</returns>
    /// <remarks>The stream's position is set to the found delimiter. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadUntilDelimiter(params char[] delimiters){
        if (Position == Length) throw new IndexOutOfRangeException();

        int temp = ReadByte();
        string result = "";
        while(temp != -1 && !delimiters.Contains((char) temp) ){
            result += (char)temp;
            temp = ReadByte();
        }

        return result;
    }

    /// <summary>
    /// Read the stream until the delimiter is reached or stream length exceeded.
    /// </summary>
    /// <param name="delimiters">The strings to end the read.</param>
    /// <returns>A string of the characters from the stream, not including the delimiter.</returns>
    /// <remarks>The stream's position is set to the found delimiter. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadUntilDelimiter(params string[] delimiters){
        if (Position == Length) throw new IndexOutOfRangeException();

        int temp = ReadByte();
        string result = "";
        while(temp != -1 && !delimiters.Any(dlm => result.EndsWith(dlm))){
            result += (char)temp;
            temp = ReadByte();
        }
        if (temp == -1) return result;
        else result.Replace(delimiters.First(dlm => result.EndsWith(dlm)), "");

        return result;
    }

    /// <summary>
    /// Read the stream when the delimiter is reached or stream length exceeded.
    /// </summary>
    /// <param name="delimiters">The characters to end the read.</param>
    /// <returns>A string of the characters from the stream, including the delimiter.</returns>
    /// <remarks>The stream's position is set to the found delimiter. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadThroughDelimiter(params char[] delimiters){
        if (Position == Length) throw new IndexOutOfRangeException();
        int temp = ReadByte();
        string result = "";
        while(temp != -1 && !delimiters.Contains((char) temp) ){
            result += (char)temp;
            temp = ReadByte();
        }
        if (temp != -1) result += (char) temp;
        return result;
    }

    /// <summary>
    /// Read the stream until the delimiter is reached or stream length exceeded.
    /// </summary>
    /// <param name="delimiters">The strings to end the read.</param>
    /// <returns>A string of the characters from the stream, including the delimiter.</returns>
    /// <remarks>The stream's position is set to the found delimiter. The next read will not include it.</remarks>
    /// <exception cref="IndexOutOfRangeException"></exception>
    public string ReadThroughDelimiter(params string[] delimiters){
        if (Position == Length) throw new IndexOutOfRangeException();

        int temp = ReadByte();
        string result = "";
        while(temp != -1 && !delimiters.Any(dlm => result.EndsWith(dlm))){
            result += (char)temp;
            temp = ReadByte();
        }
        if (temp != -1) result += (char) temp;

        return result;
    }
}