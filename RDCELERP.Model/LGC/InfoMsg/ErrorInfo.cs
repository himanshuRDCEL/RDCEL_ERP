namespace RDCELERP.Model.InfoMessage
{
    /// <summary>
    /// Represents application error in response object
    /// </summary>
    public class ErrorInfo
    {
        public ErrorInfo() : this(string.Empty, string.Empty)
        { }

        public ErrorInfo(string errorMessage) : this("", errorMessage)
        { }

        public ErrorInfo(string key, string errorMessage)
        {
            Key = key;
            ErrorMessage = errorMessage;
        }

        public string Key { get; set; }

        public string ErrorMessage { get; set; }

        public override string ToString()
        {
            return string.Format("{0}. Key: '{1}', ErrorMessage: '{2}'", base.ToString(), Key, ErrorMessage);
        }
    }
}
