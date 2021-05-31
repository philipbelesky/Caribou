namespace Caribou.Components
{
    using System.Collections.Generic;

    /// <summary>
    /// A convenience wrapped around Grasshopper messages so they are easier to construct and pass around.
    /// This deliberately does not depend on the Grasshopper types so that input parsing can be unit tested.
    /// </summary>
    public class MessagesWrapper
    {
        public List<Dictionary<Level, string>> Messages { get; }

        public enum Level
        {
            Warning,
            Remark,
            Error
        }

        public MessagesWrapper()
        {
            this.Messages = new List<Dictionary<Level, string>>();
        }

        public void AddWarning(string msg)
        {
            var item = new Dictionary<Level, string> {
                { Level.Warning, msg }
            };
            this.Messages.Add(item);
        }

        public void AddRemark(string msg)
        {
            var item = new Dictionary<Level, string> {
                { Level.Remark, msg }
            };
            this.Messages.Add(item);
        }

        public void AddError(string msg)
        {
            var item = new Dictionary<Level, string> {
                { Level.Error, msg }
            };
            this.Messages.Add(item);
        }
    }
}
