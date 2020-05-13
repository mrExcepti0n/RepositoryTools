using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryEF.UnitTests.Model
{
    public class Agreement : ITable<int>
    {
        public int Id { get; set; }
        public bool IsDeleted { get; set; }
        public string ChangedBy { get; set; }
        public DateTime? ChangeDate { get; set; }
      

        public string Number { get; set; }
    }
}
