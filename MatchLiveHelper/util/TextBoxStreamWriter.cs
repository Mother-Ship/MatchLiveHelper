using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MatchLiveHelper.util
{
    class TextBoxStreamWriter : TextWriter
    {
        TextBox _output = null;

        public TextBoxStreamWriter(TextBox output)
        {
            _output = output;
        }


        public override void WriteLine(string value) {
            base.Write(value);
            _output.Dispatcher.Invoke(new Action(()=>_output.AppendText(DateTime.Now.ToString() + " " + value.ToString() + "\n")));
          
        }

        public override Encoding Encoding {
            get { return System.Text.Encoding.UTF8; }
        }
    }
}
