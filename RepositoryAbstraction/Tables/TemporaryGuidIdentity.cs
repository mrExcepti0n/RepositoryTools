using BaseEntities.Interfaces;
using System;

namespace RepositoryAbstraction.Tables
{
    public class TemporaryGuidIdentity : IIdentityEntity<Guid>
    {
        protected TemporaryGuidIdentity()
        {

        }

        public TemporaryGuidIdentity(Guid id)
        {
            Id = id;
        }


        public Guid Id { get; set; }
    }
}
