namespace Caribou.Components
{
    using System.Collections.Generic;

    /// <summary>
    /// A convenience wrapped around Grasshopper messages so they are easier to construct and pass around.
    /// This deliberately does not depend on the Grasshopper types so that input parsing can be unit tested.
    /// </summary>
    public class Message
    {
        public Level level;
        public string text;

        public enum Level
        {
            Warning,
            Remark,
            Error
        }

        public Message(string msg, Level type)
        {
            this.text = msg;
            this.level = type;
        }
    }
}
