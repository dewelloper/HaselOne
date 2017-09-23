using BusinessObjects;
using DAL;
using HaselOne.App_Start;
using HaselOne.Services.Interfaces;
using HaselOne.Services.Services;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using Microsoft.Ajax.Utilities;

namespace HaselOne
{

    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ScriptService]
    public class HaselGridService : System.Web.Services.WebService
    {
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public int GetUser()
        {
            return CurrentUser.CurrentUserId;
        }

        HASELONEEntities context = new HASELONEEntities();
        [WebMethod(EnableSession = true)]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        public dynamic GetCustomersLast202(string customerStatus)
        {
            
            List<Cm_Customer> custs = new List<Cm_Customer>();
            if (customerStatus == "1")  //   1-Tüm Cariler
            {
                custs = context.Cm_Customer.ToList().OrderByDescending(k => k.Id).ToList();
            }
            if (customerStatus == "2") //   2-Onaysız Cariler
            {
                custs = context.Cm_Customer.Where(m => m.StatusId == 0).ToList().OrderByDescending(k => k.Id).ToList();
            }

            if (customerStatus == "3")  //   3-Tamamlanmayan Cariler 
            {
                List<int> unLocatedCustomerIds = context.Cm_Customer.Where(k => !context.Cm_CustomerLocations.Any(f => f.CustomerId == k.Id))
                  .Select(k => k.Id).ToList();
                List<int> unAuthCustomerIds = context.Cm_Customer.Where(k => !context.Cm_CustomerAuthenticators.Any(f => f.CustomerId == k.Id))
              .Select(k => k.Id).ToList();
                List<int> unSalesmanCustomerIds = context.Cm_Customer.Where(k => !context.Cm_CustomerSalesmans.Any(f => f.CustomerId == k.Id))
              .Select(k => k.Id).ToList();
                List<int> unMachineparkCustomerIds = context.Cm_Customer.Where(k => !context.Cm_CustomerMachineparks.Any(f => f.CustomerId == k.Id))
              .Select(k => k.Id).ToList();
                unLocatedCustomerIds.AddRange(unAuthCustomerIds);
                unLocatedCustomerIds.AddRange(unSalesmanCustomerIds);
                unLocatedCustomerIds.AddRange(unMachineparkCustomerIds);
                custs = context.Cm_Customer.Where(k => unLocatedCustomerIds.Contains(k.Id)).ToList();
            }

            if (customerStatus == "4") //4-Lokasyonu Olmayan Cariler
            {
                List<int> all = context.Cm_Customer.Where(k => !context.Cm_CustomerLocations.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                custs = context.Cm_Customer.Where(k => all.Contains(k.Id)).ToList();
            }

            if (customerStatus == "5") //5-Makinesi  Olmayan Cariler
            {
                List<int> all = context.Cm_Customer.Where(k => !context.Cm_CustomerMachineparks.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                custs = context.Cm_Customer.Where(k => all.Contains(k.Id)).ToList();
            }

            if (customerStatus == "6")//6-Satıcısı Olmayan Cariler
            {
                List<int> all = context.Cm_Customer.Where(k => !context.Cm_CustomerSalesmans.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                custs = context.Cm_Customer.Where(k => all.Contains(k.Id)).ToList();
            }

            if (customerStatus == "7") //7-Yetkilisi Olmayan Cariler
            {
                List<int> all = context.Cm_Customer.Where(k => !context.Cm_CustomerAuthenticators.Any(f => f.CustomerId == k.Id)).Select(k => k.Id).ToList();
                custs = context.Cm_Customer.Where(k => all.Contains(k.Id)).ToList();
            }

            custs = custs.Where(m => m.IsDeleted != true).OrderByDescending(m => m.Id).ToList();

            List<CustomerWrapper> cr = new List<CustomerWrapper>();
            foreach (Cm_Customer c in custs)
            {
                Gn_User createUser = new Gn_User();
                Gn_User modifiedUser = new Gn_User();
                if (c.CreatorId != null)
                {
                    createUser = GetListUser().FirstOrDefault(m => m.Id == c.CreatorId.Value);

                }
                if (c.ModifierId != null)
                {
                    modifiedUser = GetListUser().FirstOrDefault(m => m.Id == c.ModifierId.Value);
                }
                cr.Add(new CustomerWrapper
                {
                    Id = c.Id,
                    Name = c.Name,
                    NetsisHaselCode = c.NetsisHaselCode,
                    NetsisRentliftCode = c.NetsisRentliftCode,
                    TaxNumber = c.TaxNumber,
                    Web = c.Web,
                    SectorId = c.SectorId,
                    StatusId = c.StatusId,
                    CreatorName = createUser != null ? createUser.Name + " " + createUser.Surname : "",
                    CreateDate = c.CreateDate,
                    ModifiedName = modifiedUser != null ? modifiedUser.Name + " " + modifiedUser.Surname : "",
                    ModifiedDate = c.ModifyDate,

                });

            }
            return cr.ToList();
            return null;

        }

        List<Gn_User> listUser = new List<Gn_User>();
        public List<Gn_User> GetListUser()
        {
            if (listUser.Count == 0)
            {
                listUser = context.Gn_User.ToList();
            }

            return listUser;

        }

    }
}
