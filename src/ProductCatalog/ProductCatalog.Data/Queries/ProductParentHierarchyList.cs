using Npgsql;

namespace ProductCatalog.Data.Queries;

public static class ProductParentHierarchyList
{
    public static IQueryable<ProductParent?> GetParentHierarchy(this ProductDbContext dbContext, Guid parentId)
    {
        var parentIdParameter = new NpgsqlParameter("parentId", parentId);

        FormattableString queryString = $@"WITH RECURSIVE parents AS (
            SELECT id as id, parent_id, is_deleted
            FROM item_parents
            WHERE id = {parentIdParameter}
            UNION ALL
            SELECT pp.id, pp.parent_id, pp.is_deleted
            FROM item_parents pp
            INNER JOIN parents pt ON pp.parent_id = pt.id
        )
        SELECT id, parent_id, is_deleted FROM parents";

        var hierarchyQuery = dbContext.ItemParents.FromSql(queryString);

        return hierarchyQuery;
    }
}
