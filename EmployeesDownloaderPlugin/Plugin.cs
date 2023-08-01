using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using EmployeesDownloaderPlugin.Converters;
using EmployeesDownloaderPlugin.Objects;
using PhoneApp.Domain.Attributes;
using PhoneApp.Domain.DTO;
using PhoneApp.Domain.Interfaces;

namespace EmployeesDownloaderPlugin
{
    [Author(Name = "Maksim Povarnitsin")]
    public class Plugin : IPluggable
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public IEnumerable<DataTransferObject> Run(IEnumerable<DataTransferObject> args)
        {
            logger.Info("Downloading employees");
            var employeesList = args.Cast<EmployeesDTO>().ToList();
            UsersArrayByJson users = new UsersArrayByJson();

            try
            {

                logger.Info("Looking source");
                var url = EmployeesDownloaderPlugin.Properties.Resources.Url;
                logger.Info($@"Source found: {url}");

                using (var client = new HttpClient())
                {
                    logger.Info("Start HttpClient");

                    client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite);

                    logger.Info("Sending request");
                    var task = Task.Run(() => client.GetAsync(url));
                    task.Wait();
                    HttpResponseMessage response = task.Result;
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        logger.Info("Request has been sent");

                        string employeesData = response.Content.ReadAsStringAsync().Result;

                        users = Newtonsoft.Json.JsonConvert.DeserializeObject<UsersArrayByJson>(employeesData);
                    }
                    else
                    {
                        logger.Error($@"Sending request failed. Status code: {response.StatusCode}");
                    }
                    logger.Info("End HttpClient");
                }
                employeesList = UserConverter.UsersToEmployees(users.Users, employeesList);
                logger.Info($"Downloaded {employeesList.Count()} employees");
            }
            catch (Exception ex) 
            {
                logger.Error(ex.Message.ToString());
            }
            return employeesList.Cast<DataTransferObject>();
        }
    }
}
