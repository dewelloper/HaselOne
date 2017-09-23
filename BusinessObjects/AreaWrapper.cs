using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public class AreaWrapper
    {
        public AreaWrapper()
        {
        }

        public AreaWrapper(int id, string areaName, Nullable<int> mainAreaId)
        {
            Id = id;
            AreaName = areaName;
            MainAreaId = mainAreaId;
        }

        public int Id { get; set; }
        public string AreaName { get; set; }
        public Nullable<int> MainAreaId { get; set; }
    }

    public class Ex
    {


        public static Func<TData, TData> CreateNewStatement<TData>(string fields)
        {
            // input parameter "o"
            var xParameter = Expression.Parameter(typeof(TData), "o");

            // new statement "new Data()"
            var xNew = Expression.New(typeof(TData));

            // create initializers
            var bindings = fields.Split(',').Select(o => o.Trim())
                .Select(o =>
                    {

                        // property "Field1"
                        var mi = typeof(TData).GetProperty(o);

                        // original value "o.Field1"
                        var xOriginal = Expression.Property(xParameter, mi);

                        // set value "Field1 = o.Field1"
                        return Expression.Bind(mi, xOriginal);
                    }
                );

            // initialization "new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var xInit = Expression.MemberInit(xNew, bindings);

            // expression "o => new Data { Field1 = o.Field1, Field2 = o.Field2 }"
            var lambda = Expression.Lambda<Func<TData, TData>>(xInit, xParameter);

            // compile to Func<Data, Data>
            return lambda.Compile();
        }
    }
}