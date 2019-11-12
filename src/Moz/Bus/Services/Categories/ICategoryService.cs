using System.Collections.Generic;
using Moz.Biz.Dtos.Categories;

namespace Moz.Biz.Services.Categories
{
    public interface ICategoryService
    {
        QueryChildrenByParentIdResponse QueryChildrenByParentId(QueryChildrenByParentIdRequest request);
        List<long> GetChildrenIdsByParentId(long? parentId, bool includeSelfId = true);
        CreateResponse Create(CreateRequest request);
        UpdateResponse Update(UpdateRequest request);
        DeleteResponse Delete(DeleteRequest request);
        GetDetailByIdResponse GetDetailById(GetDetailByIdRequest request);
        QueryResponse Query(QueryRequest request);
        SetOrderIndexResponse SetOrderIndex(SetOrderIndexRequest request);

        string GetParentNameByChildId(long childId, long rootId);
    }
    
}