using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace BaseEntities
{
    public class BaseEntity<TKey> : ITable<TKey>
    {
        public bool IsDeleted { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangeDate { get; set; }
        public TKey Id { get; set; }
    }
}
