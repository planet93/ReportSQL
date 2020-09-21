using BMI.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BMI.Services
{
    public class BaseService
    {
        protected readonly UnitOfWork mDb;

        public BaseService(UnitOfWork db = null)
        {
            mDb = db ?? new UnitOfWork();
        }
    }
}