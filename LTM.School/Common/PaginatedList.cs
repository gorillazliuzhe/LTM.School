using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LTM.School.Common
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; set; }

        public int TotalPages { get; set; }

        public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;

            TotalPages = (int)Math.Ceiling(count / (decimal)pageSize);

            this.AddRange(items);
        }
        /// <summary>
        /// 判断是否有上一页
        /// </summary>
        public bool HasPreViousPage => (PageIndex > 1);
        //public bool HasPreViousPage
        //{
        //    get
        //    {
        //        return (PageIndex > 1);
        //    }
        //}

        /// <summary>
        /// 判断是否下一页
        /// </summary>
        public bool HasNextPage => (PageIndex < TotalPages);

        /// <summary>
        /// 创建分页
        /// </summary>
        /// <param name="source">实体信息</param>
        /// <param name="pageIndex">当前页</param>
        /// <param name="pageSize">页容量</param>
        /// <returns></returns>
        public static async Task<PaginatedList<T>> CreatepagingAsync(IQueryable<T> source,int pageIndex,int pageSize)
        {
            var count = await source.CountAsync();

            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();

            var dtos = new PaginatedList<T>(items, count, pageIndex, pageSize);

            return dtos;
            
        }
    }
}
