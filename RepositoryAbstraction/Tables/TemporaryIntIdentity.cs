using BaseEntities.Interfaces;

namespace RepositoryAbstraction.Tables
{
    public class TemporaryIntIdentity : IIdentityEntity<int>
    {
        protected TemporaryIntIdentity()
        {

        }

        public TemporaryIntIdentity(int id)
        {
            Id = id;
        }
        public int Id { get; set; }
    }
}
