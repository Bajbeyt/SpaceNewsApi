using System.Dynamic;

namespace Services.Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeDataList(IEnumerable<T> entities, string fieldsString);
    ExpandoObject ShapeData(T entity, string fieldsString);
}