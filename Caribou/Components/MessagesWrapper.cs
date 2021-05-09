namespace Caribou.Components
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Grasshopper.Kernel;

    /// <summary>
    /// A convenience wrapped around Grasshopper messages so they are easier to construct and pass around
    /// </summary>
    public class MessagesWrapper
    {
        public List<(GH_RuntimeMessageLevel, string)> Messages;
        public MessagesWrapper()
        {
            this.Messages = new List<(GH_RuntimeMessageLevel, string)>();
        }

        public void AddWarning(string msg)
        {
            this.Messages.Add((GH_RuntimeMessageLevel.Warning, msg));
        }

        public void AddRemark(string msg)
        {
            this.Messages.Add((GH_RuntimeMessageLevel.Remark, msg));
        }

        public void AddError(string msg)
        {
            this.Messages.Add((GH_RuntimeMessageLevel.Error, msg));
        }
    }
}
