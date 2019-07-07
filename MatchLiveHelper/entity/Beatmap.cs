using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowcaseHelper.entity
{
    class Beatmap
    {
        private string setId;
        private string title;
        private string mapper;
        private string difficulty;

        public string SetId { get => setId; set => setId = value; }
        public string Mapper { get => mapper; set => mapper = value; }
        public string Title { get => title; set => title = value; }
        public string Difficulty { get => difficulty; set => difficulty = value; }
    }
}
