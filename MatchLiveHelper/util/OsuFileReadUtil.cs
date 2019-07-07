using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ShowcaseHelper.util
{
    class OsuFileReadUtil
    {
        private readonly Regex PATTERN = new Regex("(?<=[\\d*],[\\d*],\")(?:.*\\)*(.*\\.(?i)(jpg|png|jpeg))");
        public string getBgPath(string osuFilePath) {
            string text = System.IO.File.ReadAllText(@osuFilePath);
            var mc = PATTERN.Match(text);
            return mc.Success ? mc.Groups[1].Value + mc.Groups[2].Value : null;
        }
        
    }
}
