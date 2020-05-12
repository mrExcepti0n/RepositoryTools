using BaseEntities.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryAbstraction
{
    public abstract class BaseRepository
    {
        protected BaseRepository(IIdentityProvider identityProvider)
        {
            IdentityProvider = identityProvider;
        }

        protected IIdentityProvider IdentityProvider;



        protected string User => IdentityProvider.User;


        protected void ApplayUpdateDate<T>(IEnumerable<T> entityCollection) where T : class
        {
            if (typeof(IChangeDate).IsAssignableFrom(typeof(T)))
            {
                ApplayUpdateDate(entityCollection.Cast<IChangeDate>());
            }
        }

        protected void ApplayUpdateDate(IEnumerable<IChangeDate> entityCollection)
        {
            var dateTimeNow = DateTime.Now;
            foreach (var element in entityCollection)
            {
                element.ChangeDate = dateTimeNow;
            }
        }

        protected void ApplayWorkerChangers<T>(IEnumerable<T> entityCollection) where T : class
        {
            if (typeof(IChangedBy).IsAssignableFrom(typeof(T)))
            {
                ApplayWorkerChangers(entityCollection.Cast<IChangedBy>());
            }
        }

        protected void ApplayWorkerChangers(IEnumerable<IChangedBy> entityCollection)
        {
            foreach (IChangedBy element in entityCollection)
            {
                element.ChangedBy = User;
            }
        }
    }
}
