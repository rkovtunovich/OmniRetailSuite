using Catalog.Data;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Data.Queries;

public static class ItemParentsQuery
{
    //public static IQueryable<ItemParentTree> ItemParentsRec(this CatalogContext dbContext)
    //{
    //    FormattableString queryString = $@"WITH RECURSIVE item_tree AS (
    //                        -- Base case: select the root nodes (where parent_id IS NULL)
    //                        SELECT id, name, parent_id, is_deleted, ARRAY[]::integer[] as child_ids
    //                        FROM item_parents
    //                        WHERE parent_id IS NULL

    //                        UNION ALL

    //                        -- Recursive case: select the child nodes
    //                        SELECT c.id, c.name, c.parent_id, c.is_deleted, p.child_ids || p.id
    //                        FROM item_parents c
    //                        JOIN item_tree p ON c.parent_id = p.id
    //                    )
    //                    SELECT * FROM item_tree;";

    //    var result = dbContext.Database.SqlQuery<ItemParentTree>(queryString);

    //    return result;
    //}
}
