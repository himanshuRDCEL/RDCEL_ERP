using System.Collections.Generic;
using System.Net;

namespace RDCELERP.Model.InfoMessage
{
    /// <summary>
    /// Represents result of an action.
    /// </summary>
    public class ExecutionResult
    {
        private bool? _success;

        #region Constructors

        public ExecutionResult() : this((ExecutionResult)null)
        { }

        /// <summary>
        /// Initialize execution result with one error
        /// </summary>
       

        /// <summary>
        /// Initialize execution result with one information message
        /// </summary>
        public ExecutionResult(InfoMessage message) : this(new[] { message })
        { }

        /// <summary>
        /// Initialize execution result with error list
        /// </summary>
        

        /// <summary>
        /// Initialize execution result with information message list
        /// </summary>
        public ExecutionResult(IEnumerable<InfoMessage> messages) : this((ExecutionResult)null)
        {
            foreach (InfoMessage message in messages)
            {
                Details.Add(message);
            }
        }

        /// <summary>
        /// Main constructor
        /// </summary>
        public ExecutionResult(ExecutionResult result)
        {
            if (result != null)
            {
                
                
                Details = new List<InfoMessage>(result.Details);
            }
            else
            {
                
                Details = new List<InfoMessage>();
            }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Indicates if result is successful.
        /// </summary>
        

        /// <summary>
        /// Errors collection
        /// </summary>
       

        /// <summary>
        /// Info messages collection
        /// </summary>
        public IList<InfoMessage> Details { get;  }
        public HttpStatusCode StatusCode { get; set; }

        #endregion
    }

    /// <summary>
    /// Represents result of an action that returns any value
    /// </summary>
    /// <typeparam name="T">Type of value to be returned with action</typeparam>
    public class ExecutionResult<T> : ExecutionResult
    {
        public ExecutionResult() : this((ExecutionResult)null)
        { }

        public ExecutionResult(T result) : this((ExecutionResult)null)
        {
            Value = result;
        }

        public ExecutionResult(T result, InfoMessage message) : this((ExecutionResult)null)
        {
            Value = result;
            Details.Add(message);
        }

        public ExecutionResult(ExecutionResult result) : base(result)
        {
            if (result is ExecutionResult<T> r) // make sure result is not null and cast to ExecutionResult
            {
                Value = r.Value;
            }
        }

        public ExecutionResult(InfoMessage message) : this(new[] { message })
        { }

        public ExecutionResult(IEnumerable<InfoMessage> messages) : this((ExecutionResult)null)
        {
            foreach (InfoMessage message in messages)
            {
                Details.Add(message);
            }
        }

        public T Value { get; set; }
    }
}
