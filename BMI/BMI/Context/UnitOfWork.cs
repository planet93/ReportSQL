using BMI.Context.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Context
{
    public class UnitOfWork : IDisposable
    {
        private bool _disposed;
        private readonly DataContext _db;
        private Repository<ClassifierType> _classifierType;
        private Repository<Classifier> _classifier;
        private Repository<Estimate> _estimate;

        public UnitOfWork(DataContext db = null)
        {
            _db = db ?? new DataContext();
        }

        public Repository<ClassifierType> ClassifierTypes => _classifierType ?? (_classifierType = new Repository<ClassifierType>(_db));
        public Repository<Classifier> Classifiers => _classifier ?? (_classifier = new Repository<Classifier>(_db));
        public Repository<Estimate> Estimate => _estimate ?? (_estimate = new Repository<Estimate>(_db));

        #region Dispose
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {

                _classifierType?.Dispose();
                _classifier?.Dispose();
                _estimate?.Dispose();
            }

            _disposed = true;
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        ~UnitOfWork()
        {
            Dispose(false);
        }
        #endregion

    }
}