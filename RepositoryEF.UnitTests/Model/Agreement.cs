using BaseEntities.Interfaces;
using System;

namespace RepositoryEF.UnitTests.Model
{
    public class Agreement : ITable<int>
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangeDate { get; set; }
      

        public string Number { get; set; }

        public DateTime Date { get; set; }
    }
}
