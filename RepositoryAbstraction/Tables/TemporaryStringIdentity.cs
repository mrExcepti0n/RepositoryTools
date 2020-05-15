using BaseEntities.Interfaces;

namespace RepositoryAbstraction.Tables
{
    public class TemporaryStringIdentity : IIdentityEntity<string>
    {
        protected TemporaryStringIdentity()
        {

        }

        public TemporaryStringIdentity(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
