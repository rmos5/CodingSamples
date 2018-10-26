using System;
using System.Linq;

namespace TaskManagerDemo.Data
{
    public static class DataExtensions
    {
        public const int PageSizeMax = 200;

        public const int PageSizeDefault = 12;

        public static void UpdateModified(this TaskInfo taskInfo, ApplicationDbContext database, string userName)
        {
            ModifyInfo modifyInfo = new ModifyInfo { Modified = DateTime.UtcNow, Modifier = userName ?? "Not authenticated" };
            taskInfo.ModifyInfos.Add(modifyInfo);
            database.Update(taskInfo);
        }


        public static bool HasChanges(this TaskInfo item, ApplicationDbContext database)
        {
            //hack: ef change tracking is obviously poor
            var entry = database.Entry(item);
            var modified = entry.Properties.Where(o => o.IsModified && o.Metadata.Name != "Timestamp");
            return modified.Count() > 0;
        }

        public static IQueryable<TaskInfo> ApplyPaging(this IQueryable<TaskInfo> result, int pageNum, int pageSize = PageSizeDefault)
        {
            pageSize = pageSize > PageSizeMax ? PageSizeMax : pageSize <= 0 ? PageSizeDefault : pageSize;

            pageNum = pageNum - 1;

            if (pageNum > 0)
                result = result.Skip((int)(pageSize * pageNum));

            result = result.Take((int)pageSize);

            return result;
        }

        public static IQueryable<TaskInfo> ApplyNameFilter(this IQueryable<TaskInfo> result, string value, bool contains)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                if (contains)
                    result = result.Where(o => o.Name.Contains(value));
                else
                    result = result.Where(o => o.Name.StartsWith(value));
            }

            return result;
        }
    }
}
