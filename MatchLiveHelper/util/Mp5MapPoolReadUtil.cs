using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using ShowcaseHelper.entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace MatchLiveHelper.util
{
    class Mp5MapPoolReadUtil
    {
        private static readonly Regex BEATMAP_NAME_REGEX = new Regex(@"\s*(?<setId>\d*?)\s(?<title>.*?)\s\[(?<difficulty>.*?)]\s//\s(?<mapper>.*)");


        public static MapPoolSet Read(string fileName)
        {
            ISheet sheet = null;
            var set = new MapPoolSet();
            var result = new List<MapPool>();
            set.Pool = result;
            IWorkbook workbook = null;

            try
            {
                var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
                if (fileName.IndexOf(".xlsx") > 0)
                {// 2007版本
                    workbook = new XSSFWorkbook(fs);
                }
                else if (fileName.IndexOf(".xls") > 0)
                { // 2003版本
                    workbook = new HSSFWorkbook(fs);
                }

                sheet = workbook.GetSheetAt(0);

                if (sheet != null)
                {

                    int startRow = sheet.FirstRowNum;
                    int rowCount = sheet.LastRowNum;
                    var head = sheet.GetRow(startRow);
                    set.PoolName = head.GetCell(head.FirstCellNum).ToString();
                    for (int i = startRow + 1; i <= rowCount; ++i)
                    {
                        var row = sheet.GetRow(i);
                        if (row == null || row.GetCell(row.FirstCellNum).ToString().Equals("")) continue;

                        if (!row.GetCell(row.FirstCellNum).ToString().StartsWith("http"))
                        {
                            var pool = new MapPool
                            {
                                Map = new List<Beatmap>(),
                                Type = row.GetCell(row.FirstCellNum).ToString()
                            };
                            result.Add(pool);
                        }
                        else
                        {
                            var raw = row.GetCell(row.FirstCellNum + 1).ToString();
                            var mc = BEATMAP_NAME_REGEX.Match(raw);
                            if (mc.Success)
                            {
                                var link = row.GetCell(row.FirstCellNum).ToString();
                                var bid = link.Substring(link.LastIndexOf("/"));
                                var pool = result[result.Count - 1];
                                var maps = pool.Map;
                                var map = new Beatmap
                                {
                                    SetId = (mc.Groups["setId"].Value),
                                    Title = (mc.Groups["title"].Value),
                                    Difficulty = (mc.Groups["difficulty"].Value),
                                    Mapper = (mc.Groups["mapper"].Value),
                                    BeatmapId = bid
                                };
                                maps.Add(map);
                            }
                        }
                    }
                }
                return set;
            }
            catch (Exception ex)
            {
                Console.WriteLine("读取图池失败：" + ex.Message);
                return null;
            }
        }
    }
}
