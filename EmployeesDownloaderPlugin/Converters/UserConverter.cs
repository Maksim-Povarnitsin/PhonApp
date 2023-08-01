using EmployeesDownloaderPlugin.Objects;
using PhoneApp.Domain.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeesDownloaderPlugin.Converters
{
    public class UserConverter
    {
        public static List<EmployeesDTO> UsersToEmployees(List<UserByJson> users,List<EmployeesDTO> employees)
        {
            foreach (var user in users) 
            {
                employees.Add(new EmployeesDTO() { Name=$@"{user.FirstName} {user.MaidenName} {user.LastName}", Phone=user.Phone });
                //Было желание сделать номера телефонов по аналогии - списком, на случай, если будет несколько номеров в исходных данных,
                //но в задаче это не стояло, так что сделал того же формата, что и исходные данные
            }

            return employees;
        }
    }
}
