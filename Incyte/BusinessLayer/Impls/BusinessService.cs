using System;
using System.Configuration;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using Incyte.Interfaces;
using Incyte.Resource;
using Incyte.Entities;
using System.Linq;
using System.Data;
using System.Data.SqlClient;

namespace Incyte.Services
{
    public class BusinessService : IBusinessService
    {
        string url = string.Empty;
        string sConn = string.Empty;
        IBusinessResource businessResource = null;

        public BusinessService()
        {
            if (string.IsNullOrEmpty(this.url))
                this.url = ConfigurationManager.ConnectionStrings[Constants.BusinessURL].ConnectionString;

            if (string.IsNullOrEmpty(this.sConn))
                this.sConn = System.Configuration.ConfigurationManager.ConnectionStrings[Constants.LocalAppDBCOnnection].ConnectionString;


            if (this.businessResource == null)
                this.businessResource = new BusinessResource(this.url, this.sConn);
          
        }

        public BusinessService(string url, IBusinessResource businessResource)
        {
            this.url = url;
            this.businessResource = businessResource;
        }

        public List<Entities.Business> BusinessGet(Entities.Location location)
        {
            try
            {
                string query = string.Empty;

                System.Collections.IDictionaryEnumerator ienum = location.SetValues.GetEnumerator();
                query = BuildQueryString(ienum);

                string json = businessResource.BusinessExternalGet(query);

                RootObject root = JsonConvert.DeserializeObject<RootObject>(json);

                BusinessCreate(root.Businesses);

                return root.Businesses;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Incyte.Entities.Business> BusinessGet(List<Entities.Business> businesses)
        {
            try
            {
                return BusinessCreate(businesses);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<Entities.Business> BusinessCreate(List<Entities.Business> businesses)
        {
            List<Entities.Business> bss = businessResource.BusinessCreateLight(businesses.Select(x => new BusinessLight(x.ExternalID,"1")).ToList(), this.BusinessConverter, businesses) as List<Entities.Business>;

            return businesses;
        }

        private object BusinessConverter(IDataReader dr, object dataObject)
        {
            List<Entities.Business> businesses = dataObject as List<Entities.Business>;
            string[] keys = { "BusinessId", "CheckinCount" };
            Business business = businesses.FirstOrDefault(x => x.ExternalID == dr["ExternalID"].ToString());
            List<string> cols = new List<string>();
            
            for (int i = 0; i < dr.FieldCount; i++)
            {
                cols.Add(dr.GetName(i));
            }

            if (business != null)
            {
                keys.ToList().ForEach(key =>
                    {
                        if(cols.Contains(key))
                        {
                            switch (key)
                            {
                                case "BusinessId":
                                    business.BusinessId = int.Parse(dr[key].ToString());
                                    break;
                                case "CheckinCount":
                                    business.CheckinCount = int.Parse(dr[key].ToString());
                                    break;
                                default:
                                    break;
                            }
                        }
                    });
                business.BusinessId = int.Parse(dr["BusinessId"].ToString());
                business.CheckinCount = int.Parse(dr["GenderMCheckIns"].ToString()) + int.Parse(dr["GenderFCheckIns"].ToString());
            }
            return dataObject;
        }

        private string BuildQueryString(System.Collections.IDictionaryEnumerator ienum)
        {
            string query = string.Empty;
            Hashtable hsh = new Hashtable();
            string qname = string.Empty;
            string val = string.Empty;

            while (ienum.MoveNext())
            {
                qname = (ienum.Value as LocationQueryString).QueryName;
                val  = (ienum.Value as LocationQueryString).Name;
                if (hsh.ContainsKey(qname))
                {
                    hsh[qname] = hsh[qname] + " " + val;
                }
                else
                {
                    hsh[qname] = val;
                }
            }
            System.Collections.IDictionaryEnumerator qenum = hsh.GetEnumerator();

            while (qenum.MoveNext())
            {
                query = query + string.Format("&{0}={1}", qenum.Key.ToString(), qenum.Value.ToString());
            }
            return query;
        }

    }
}
