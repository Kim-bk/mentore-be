using DAL.Entities;
using Mentore.Models.Base;

namespace API.Model.Entities
{
    public class EntityField : BaseEntity
    {
        public string FieldTypeId { get; set; }
        public string TableName { get; set; }
        public string TableId { get; set; }
        public virtual Field Field { get; set; }
    }
}
