using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneApp.Domain;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace EmployeesLoaderPlugin
{

    [Author(Name = "Ivan Petrov")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Loading employees");
            var employeesList = args.Cast<EmployeesDTO>().ToList();
            int oldCount = employeesList.Count;

            var employeesNewDataList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<EmployeesDTO>>(EmployeesLoaderPlugin.Properties.Resources.EmployeesJson);

            foreach ( var employeeNewData in employeesNewDataList )
            {
                employeesList.Add( employeeNewData );
            }

            logger.Info($"Loaded {employeesList.Count()-oldCount}; Total employees {employeesList.Count()}");

            return employeesList.Cast<DataTransferObject>();
        }
    }
}
