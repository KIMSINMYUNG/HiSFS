using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HiSFS.Api.Shared.Models
{
    public class 공통정보 : ModelBase
    {
        public bool 사용유무 { get; set; } = true;
        public bool 삭제유무 { get; set; } = false;
        public string 상세JSON { get; set; }
    }

    public static class 공통쿼리확장
    {
        public static IQueryable<TSource> Where_미삭제_사용<TSource>(this IQueryable<TSource> source)
            where TSource : 공통정보
        {
            return source.Where(x => x.삭제유무 != true && x.사용유무 == true);
        }

        public static IQueryable<TSource> Where_미삭제<TSource>(this IQueryable<TSource> source)
            where TSource : 공통정보
        {
            return source.Where(x => x.삭제유무 != true);
        }

        public static IQueryable<TSource> Where_사용<TSource>(this IQueryable<TSource> source)
            where TSource : 공통정보
        {
            return source.Where(x => x.사용유무 == true);
        }

        public static IQueryable<TSource> Order_등록최신<TSource>(this IQueryable<TSource> source)
            where TSource : 공통정보
        {
            return source.OrderByDescending(x => x.CreateTime);
        }
    }

    //public class Condition
    //{

    //}

    //public enum ConditionKind
    //{
    //    Code,
    //    Name,
    //    Param1,
    //    Param2,
    //    Param3
    //}
}
