using System.Dynamic;
using System.Reflection;
using Services.Contracts;

namespace Services;

public class DataShaper<T> : IDataShaper<T> where T : class
    {
        public PropertyInfo[] properties { get; set; }
        // Tiplerin property'lerini tutacak bir dizi
        public DataShaper() 
        {
            // Çekilen propertylerin public ve yenilenebilir olması
            properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

        }
        
        private IEnumerable <PropertyInfo> GetRequiredProperties(string fieldsString)
        {// Verilen fieldsString'e göre gerekli property'leri bulan metot
            var requiredFields = new List<PropertyInfo>();
            if (!string.IsNullOrWhiteSpace(fieldsString)) {
                //removeEmptyEntries boş olanları fields nesnesinden kaldırıyor
                var fields = fieldsString.Split(',', StringSplitOptions.RemoveEmptyEntries); 
                foreach ( var field in fields)
                {//Nesnenin adına eşit olanı yakala, Trim boşluk kontrolü, StringCmp büyük küçük fark etmeksizin
                    var property = properties.FirstOrDefault(x => x.Name.Equals(field.Trim(), 
                        StringComparison.InvariantCultureIgnoreCase));
                    if (property is null) continue; //nullsa foreach'a devam et, diğer propu oku
                    requiredFields.Add(property); //null değilse gelen parametreyi ekle

                }
            } else requiredFields = properties.ToList();
            
            return requiredFields;
        }
        
        private ExpandoObject FetchDataForEntity(T entity, IEnumerable<PropertyInfo> requiredProperties)
        {// Verilen entity ve gerekli property'ler kullanılarak ExpandoObject oluşturan metot
            var shapeObject = new ExpandoObject();
            // ExpandoObject, dinamik olarak özellik eklememizi sağlayan bir sınıftır.
            foreach( var property in requiredProperties)
            {
                var objectPropertyValue = property.GetValue(entity);
                // Entity'den ilgili property'nin değerini al
                shapeObject.TryAdd(property.Name, objectPropertyValue);
                // ExpandoObject'e property adı ve değeri ekleniyor
            }
            return shapeObject;
        }

        private IEnumerable<ExpandoObject> FetchData(IEnumerable<T> entities, IEnumerable<PropertyInfo> requiredProperties)
        {// Verilen entity listesi ve gerekli property'ler kullanılarak ExpandoObject listesi oluşturan metot
            var shapeData = new List<ExpandoObject>();
            foreach( var entity in entities)
            {
                var shapeObject = FetchDataForEntity(entity, requiredProperties);
                // Her bir entity için ExpandoObject oluşturuluyor
                shapeData.Add(shapeObject);
            }
            return shapeData;
        }
        public ExpandoObject ShapeData(T entity, string fieldsString)
        {//Tek bir entity'nin ExpandoObject'ini oluşturur
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchDataForEntity(entity, requiredProperties);
            // Entity ve gerekli property'ler kullanılarak ExpandoObject oluştur
        }

        public IEnumerable<ExpandoObject> ShapeDataList(IEnumerable<T> entities, string fieldsString)
        {//Entity listesinin ExpandoObject listesini oluşturur
            var requiredProperties = GetRequiredProperties(fieldsString);
            return FetchData(entities, requiredProperties);
        }
    }