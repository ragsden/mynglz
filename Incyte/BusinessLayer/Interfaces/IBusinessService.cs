using System;
using Incyte.Entities;
using System.Collections.Generic;

namespace Incyte.Interfaces
{
    public interface IBusinessService
    {
        //Business BusinessCreate(Business business);

        List<Incyte.Entities.Business> BusinessCreate(List<Business> business);
        
        //Business BusinessGetByID(int businessID);

        List<Incyte.Entities.Business> BusinessGet(Location location);

        List<Incyte.Entities.Business> BusinessGet(List<Entities.Business> businesses);

    }
}
