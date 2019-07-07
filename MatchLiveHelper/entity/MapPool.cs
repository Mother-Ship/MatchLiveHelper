using System.Collections.Generic;

namespace ShowcaseHelper.entity
{
    class MapPool
    {
        private string type;
        private List<Beatmap> map;

        public string Type { get => type; set => type = value; }
        public List<Beatmap> Map { get => map; set => map = value; }
    }
}
