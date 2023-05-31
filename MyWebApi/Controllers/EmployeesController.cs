using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MyWebApi.Controllers
{
    public class EmployeesController : ApiController
    {

        private string connectionString = "workstation id=mywebapi.mssql.somee.com;packet size=4096;user id=vitormaster_SQLLogin_1;pwd=ld4dfgwwx1;data source=mywebapi.mssql.somee.com;persist security info=False;initial catalog=mywebapi";
        private DataClasses1DataContext dc;

        public EmployeesController()
        {
            dc = new DataClasses1DataContext(connectionString);
        }

        // GET: api/Employees
        public List<Employee> Get()
        {
            var list = from Employee in dc.Employees orderby Employee.Name select Employee;

            return list.ToList();
        }

        // GET: api/Employees/2
        public IHttpActionResult Get(int id)
        {
            var employee = dc.Employees.SingleOrDefault(e => e.Id == id);

            if (employee != null)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.OK, employee));
            }

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Employee not exist!"));
        }

        // POST : api/Employees
        public IHttpActionResult Post([FromBody] Employee newEmployee)
        {
            Employee employee = dc.Employees.FirstOrDefault(e => e.Id == newEmployee.Id);

            if (employee != null)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.Conflict, "Employee already exist!"));
            }

            Category category = dc.Categories.FirstOrDefault(c => c.acronym == newEmployee.Category);

            if(category == null)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Category not exist!"));
            }

            dc.Employees.InsertOnSubmit(newEmployee);

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.ServiceUnavailable, e));
            }

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.OK));

        }

        // PUT: api/Employees/2

        public IHttpActionResult PUT([FromBody] Employee editedEmployee) 
        { 
            Employee employee = dc.Employees.FirstOrDefault(e => e.Id == editedEmployee.Id);    

            if (employee == null) 
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Employee not exist!"));
            }

            Category category = dc.Categories.FirstOrDefault(c => c.acronym == editedEmployee.Category);     

            if(category == null)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Category not exist!"));
            }

            employee.Name = editedEmployee.Name;
            employee.Category = editedEmployee.Category;

            try
            {
                dc.SubmitChanges();
            }
            catch (Exception e)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.ServiceUnavailable, e));
            }

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.OK));

        }   

        // DELETE: api/Employees/2

        public IHttpActionResult Delete(int id)
        {
            Employee employee = dc.Employees.FirstOrDefault(e => e.Id == id);   

            if (employee != null) 
            {
                dc.Employees.DeleteOnSubmit(employee);

                try
                {
                    dc.SubmitChanges();
                }
                catch (Exception e)
                {
                    return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.ServiceUnavailable, e));
                }

                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.OK));
            }

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Employee not exist!"));

        }

    }
}
