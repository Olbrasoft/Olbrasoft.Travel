using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Olbrasoft.Travel.DAL;
using Olbrasoft.Travel.DTO;

namespace Olbrasoft.Travel.BLL
{
    public class SubClassesFacade : TravelFacade<SubClass>, ISubClassesFacade
    {
        protected new readonly ISubClassesRepository Repository;
        private IDictionary<string, int> _subClassesOfRegionsAsReverseDictionary;

        public SubClassesFacade(ISubClassesRepository repository) : base(repository)
        {
            Repository = repository;
        }

        public SubClass Get(string name, params Expression<Func<SubClass, object>>[] includeProperties)
        {
            return Repository.Find(subClass => subClass.Name == name, includeProperties);
        }

        public IDictionary<string, int> SubClassesAsReverseDictionary(bool clearFacadeCache = false)
        {
            if (_subClassesOfRegionsAsReverseDictionary == null || clearFacadeCache)
            {
                _subClassesOfRegionsAsReverseDictionary =
                    Repository.GetAll(t => new { t.Name, t.Id }).ToDictionary(key => key.Name, val => val.Id);
            }

            return _subClassesOfRegionsAsReverseDictionary;
        }

        public void Save(SubClass[] subClasses)
        {
            var storedNamesOfSubClasses = new HashSet<string>(Repository.GetAll(s => s.Name));
            var subClassesToSave = new HashSet<SubClass>();

            foreach (var subClass in subClasses)
            {
                if (!storedNamesOfSubClasses.Contains(subClass.Name))
                {
                    subClassesToSave.Add(subClass);
                }
            }

            Repository.Add(subClassesToSave);
        }
    }
}