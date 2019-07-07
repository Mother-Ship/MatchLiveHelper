using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShowcaseHelper.entity
{
    class MapPoolSet
    {
        private List<MapPool> pool;
        private string poolName;

        public string PoolName { get => poolName; set => poolName = value; }
        public List<MapPool> Pool { get => pool; set => pool = value; }
    }
}
