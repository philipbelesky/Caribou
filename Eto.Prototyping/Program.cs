using Eto.Drawing;
using Eto.Forms;
using System;
using Caribou.Forms;
using Caribou.Components;

namespace Eto.Prototyping
{
    class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            new Application().Run(new MyForm());
        }
    }

    public class MyForm : Form
    {
        public MyForm()
        {
            Title = "My Cross-Platform App";
            ClientSize = new Size(200, 200);
            Content = new Label { Text = "Hello World!" };
        }
    }
}
