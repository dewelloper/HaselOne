using BusinessObjects;
using HaselOne.Domain.UnitOfWork;
using System.Text;

namespace HaselOne.Services.Services
{
    public abstract class ReportServiceBase
    {
        protected readonly IUnitOfWork _uow;

        public ReportServiceBase(UnitOfWork uow)
        {
            _uow = uow;
        }

        protected virtual string GetTotalCountQuery(IFilter filter)
        {
            if (string.IsNullOrEmpty(filter.TableName))
                throw new System.Exception("Tablo adı belirlitmemiş.");

            var columnString = "*";
            if (filter.Columns.Length > 0)
                columnString = string.Join(",", filter.Columns);

            var sb = new StringBuilder();

            sb.AppendLine($@" SELECT COUNT({columnString})
                              FROM {filter.TableName}");

            if (filter.FunctionParams.Length > 0)
                sb.Append($@"({string.Join(",", filter.FunctionParams)})");

            if (!string.IsNullOrEmpty(filter.WhereCondition))
                sb.Append($@"WHERE {filter.WhereCondition}");

            return sb.ToString();
        }

        protected virtual string GetPagedQuery(IFilter filter)
        {
            if (string.IsNullOrEmpty(filter.TableName))
                throw new System.Exception("Tablo adı belirlitmemiş.");

            if (string.IsNullOrEmpty(filter.OrderByCondition))
                throw new System.Exception("Sayfalanmış sorgu için sıralama(Order By) koşulu gerekir.");

            var columnString = "*";
            if (filter.Columns.Length > 0)
                columnString = string.Join(",", filter.Columns);

            var sb = new StringBuilder();
            sb.Append($@"DECLARE @Page INT = {filter.PageNo}
                         DECLARE @PageSize INT = {filter.PageSize}

                        DECLARE @Columns VARCHAR(MAX)

                        IF OBJECT_ID('tempdb..#temp') IS NOT NULL DROP TABLE #temp");

            sb.AppendLine($@"SELECT ROW_NUMBER() OVER(
                                ORDER BY {filter.OrderByCondition}
                                ) AS RowNumber
                            , {columnString}
                        INTO #temp
                        FROM {filter.TableName}");

            if (filter.FunctionParams.Length > 0)
                sb.Append($@"({string.Join(",", filter.FunctionParams)})");

            if (!string.IsNullOrEmpty(filter.WhereCondition))
                sb.Append($@"WHERE {filter.WhereCondition}");

            if (filter.Columns.Length == 0)
                sb.AppendLine($@"SELECT @Columns = SubString((
                                 SELECT ', ' + QUOTENAME([Name])

                                 FROM sys.columns

                                 WHERE object_id = object_id('{filter.TableName}')

                                 FOR XML PATH('')
			                     ), 3, 1000)");
            else
                sb.AppendLine($@"SET @Columns = {columnString}");

            sb.AppendLine(@"DECLARE @From INT= (@Page - 1) * @Pagesize
                            DECLARE @To INT = (@Page - 1) * @Pagesize + @Pagesize");

            sb.AppendLine($@"EXEC('SELECT ' + @Columns + ' FROM #temp WHERE RowNumber BETWEEN ' + @From + ' AND ' + @To)");

            return sb.ToString();
        }
    }
}