using Repository.Core;
using Repository.EF.UnitOfWork;
using System.ComponentModel;
using System;

namespace BLL.Base
{

    [DataObject(true)]
    public abstract class BLBase
    {
        private IUnitOfWork _UnitOfWork;
        protected IUnitOfWork UnitOfWork
        {
            get
            {
                if (_UnitOfWork == null)
                {
                    _UnitOfWork = new EFUnitOfWork();
                }

                return _UnitOfWork;
            }
        }
        
    }
}
