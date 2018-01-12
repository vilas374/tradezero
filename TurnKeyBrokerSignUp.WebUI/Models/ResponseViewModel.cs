using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TurnKeyBrokerSignUp.WebUI.Models
{
    public class ResponseViewModel
    {
        /// <summary>
        ///     Gets or sets the identifier.
        /// </summary>
        /// <value>
        ///     The identifier.
        /// </value>
        public long Id { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether this instance is success.
        /// </summary>
        /// <value>
        ///     <c>true</c> if this instance is success; otherwise, <c>false</c>.
        /// </value>
        public bool IsSuccess { get; set; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>
        ///     The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        ///     Gets or sets the content.
        /// </summary>
        /// <value>
        ///     The content.
        /// </value>
        public object Content { get; set; }
        //public bool IsExist { get; set; }
    }
}