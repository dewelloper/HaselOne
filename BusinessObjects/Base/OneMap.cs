using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using DAL;

namespace BusinessObjects.Base
{
    public class OneMap
    {
        public static IMapper mapper;
        private static MapperConfiguration _config;

        public static void Config()
        {
            _config = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<Cm_CustomerRequest, CustomerRequestWrapper>();
               cfg.CreateMap<CustomerRequestWrapper, Cm_CustomerRequest>();
               cfg.CreateMap<Cm_CustomerInterviews, CustomerInterviewsWrapper>();
               cfg.CreateMap<Cm_CustomerMachineparks, MachineparkWrapper>()
                 .ForMember(c => c.CategoryName, s => s.MapFrom(c => c.Cm_MachineparkCategory == null ? "" : c.Cm_MachineparkCategory.CategoryName))
                 .ForMember(c => c.LocationName, s => s.MapFrom(c => c.Cm_CustomerLocations == null ? "" : c.Cm_CustomerLocations.Name))
                 .ForMember(c => c.MarkName, s => s.MapFrom(c => c.Cm_MachineparkMark == null ? "" : c.Cm_MachineparkMark.MarkName))
                 .ForMember(c => c.ModelName, s => s.MapFrom(c => c.Pr_MachineModel == null ? "" : c.Pr_MachineModel.Name))
                 .ForMember(c => c.CreateUserName, s => s.MapFrom(c => c.Gn_User1 == null ? "" : c.Gn_User1.Name));
               cfg.CreateMap<MachineparkWrapper, Cm_CustomerMachineparks>()
                   .ForMember(c => c.Cm_MachineparkCategory, option => option.Ignore())
                   .ForMember(c => c.Cm_CustomerLocations, option => option.Ignore())
                   .ForMember(c => c.Cm_MachineparkMark, option => option.Ignore());

               cfg.CreateMap<Cm_Customer, CustomerWrapper>();
               //.ForMember(c => c.LocationName, s => s.MapFrom(c => c.Cm_CustomerLocations.Where(p => p.Name.ToLower() == "merkez").FirstOrDefault().Address));
               //.ForMember(c => c.AuthenticatorName, s => s.MapFrom(c => c.Cm_CustomerAuthenticators.Where(k => k.Name != "").FirstOrDefault().Name));
               //.ForMember(c => c.SaleEngineeer, s => s.MapFrom(c => c.Cm_CustomerSalesmans.Where(k => k.SalesmanId > 0).FirstOrDefault().SalesmanId));

               cfg.CreateMap<Gn_Sector, SectorWrapper>();
               cfg.CreateMap<Gn_Combos, ComboWrapper>();

               cfg.CreateMap<Cm_CustomerLocations, LocationWrapper>();
               cfg.CreateMap<LocationWrapper, Cm_CustomerLocations>();
               cfg.CreateMap<Pr_MachineModel, MachineModelWrapper>();
               cfg.CreateMap<MachineModelWrapper, Pr_MachineModel>();
               cfg.CreateMap<CustomerWrapper, Cm_Customer>();

               cfg.CreateMap<SectorWrapper, Gn_Sector>();
               cfg.CreateMap<Cm_MachineparkCategory, MachineparkCategoryWrapper>();
               cfg.CreateMap<MachineparkCategoryWrapper, Cm_MachineparkCategory>();
               cfg.CreateMap<Cm_MachineparkMark, MachineparkMarkWrapper>();
               cfg.CreateMap<MachineparkMarkWrapper, Cm_MachineparkMark>();
               cfg.CreateMap<ComboWrapper, Gn_Combos>();
               cfg.CreateMap<CustomerInterviewsWrapper,Cm_CustomerInterviews >();
               cfg.CreateMap<Cm_CustomerInterviews, CustomerInterviewsWrapper>();
           });

            mapper = _config.CreateMapper();
        }

        public static MapperConfiguration GetConfig()
        {
            return _config;
        }
    }

    public class CheckDb
    {
        public List<string> Check()
        {
            var badTable = new List<string>();
            using (var db = new HASELONEEntities())
            {
                var metadata = ((IObjectContextAdapter)db).ObjectContext.MetadataWorkspace;

                var tables = metadata.GetItemCollection(DataSpace.SSpace)
                  .GetItems<EntityContainer>()
                  .Single()
                  .BaseEntitySets
                  .OfType<EntitySet>()
                  .Where(s => !s.MetadataProperties.Contains("Type")
                    || s.MetadataProperties["Type"].ToString() == "Tables");

                foreach (var table in tables)
                {
                    var tableName = table.MetadataProperties.Contains("Table")
                        && table.MetadataProperties["Table"].Value != null
                      ? table.MetadataProperties["Table"].Value.ToString()
                      : table.Name;

                    //var tableSchema = table.MetadataProperties["Schema"].Value.ToString();

                    //  Console.WriteLine(tableSchema + "." + tableName);
                    if (tableName.StartsWith("Log_"))
                    {
                        string checkResult = CheckPropeties(tableName);
                        if (checkResult.Length > 0)
                        {
                            badTable.Add(checkResult);
                        }
                    }
                }
            }

            return badTable;
        }

        public string CheckPropeties(string str)
        {
            var path = Assembly.GetAssembly(new HASELONEEntities().GetType()).Location;
            var thisAssembly = Assembly.LoadFrom(path);

            var TypeName = "DAL." + str;

            Type typeLog = thisAssembly.GetType(TypeName);
            object instance_log = Activator.CreateInstance(typeLog);

            var typeEnti = thisAssembly.GetType(TypeName.Replace("Log_", ""));
            object instance = Activator.CreateInstance(typeEnti);

            foreach (PropertyInfo propertyInfo in instance.GetType().GetProperties())
            {
                var logProperties = instance_log.GetType().GetProperty(propertyInfo.Name);
                if (logProperties == null)
                {
                    return $"{propertyInfo.Name} alani log da yok! Table: {str}";
                }

                if (propertyInfo.PropertyType.AssemblyQualifiedName != logProperties.PropertyType.AssemblyQualifiedName)
                {
                    return $"{propertyInfo.Name} uyumsuz dur";
                }
            }
            //todo: .net de isprimitive varmi ona bak
            //  AutoMapper.Mapper.Configuration
            return "";
        }
    }
}