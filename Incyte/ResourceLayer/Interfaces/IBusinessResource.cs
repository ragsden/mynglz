using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;

namespace Incyte.Interfaces
{
    public delegate object Converter(IDataReader dr, object dataObject);

    public interface IBusinessResource
    {
        object BusinessCreateLight(List<BusinessLight> businesses, Converter converter, object dataObject);
        
        //T BusinessGetByID<T>(int businessID,SourceType sourceType);

        string BusinessExternalGet (string query);

    }

}
