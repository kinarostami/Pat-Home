using System;

namespace InventoryManagement.Domain
{
    public class BaseEntity
    {
        public BaseEntity()
        {
            CreateDate = DateTime.Now;
        }
        public long Id { get; private set; }
        public DateTime CreateDate { get; private set; }
    }
}
