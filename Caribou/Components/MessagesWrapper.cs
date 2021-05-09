namespace Caribou.Components
{
    using System.Collections.Generic;

    /// <summary>
    /// A convenience wrapped around Grasshopper messages so they are easier to construct and pass around.
    /// This deliberately does not depend on the Grasshopper types so that input parsing can be unit tested.
    /// </summary>
    public class MessagesWrapper
    {
        public List<(Level, string)> Messages { get; }
        public enum Level
        {
            Warning, Remark, Error
        }

        public MessagesWrapper()
        {
            this.Messages = new List<(Level, string)>();
        }

        public void AddWarning(string msg)
        {
            this.Messages.Add((Level.Warning, msg));
        }

        public void AddRemark(string msg)
        {
            this.Messages.Add((Level.Remark, msg));
        }

        public void AddError(string msg)
        {
            this.Messages.Add((Level.Error, msg));
        }
    }
}
