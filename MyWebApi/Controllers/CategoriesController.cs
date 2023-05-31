using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace MyWebApi.Controllers
{
    public class CategoriesController : ApiController
    {

        private string connectionString = "workstation id=mywebapi.mssql.somee.com;packet size=4096;user id=vitormaster_SQLLogin_1;pwd=ld4dfgwwx1;data source=mywebapi.mssql.somee.com;persist security info=False;initial catalog=mywebapi";
        private DataClasses1DataContext dc;

        public CategoriesController()
        {
            dc = new DataClasses1DataContext(connectionString);
        }


        // GET: api/Categories
        public List<Category> Get()
        {
            var list = from Category in dc.Categories select Category;

            return list.ToList();
        }

        // GET: api/Categories/HR
        [Route("api/categories/{acronym}")]
        public IHttpActionResult Get(string acronym)
        {
            var category = dc.Categories.SingleOrDefault(c => c.acronym == acronym);

            if (category != null)
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.OK, category));
            }

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Category not exist!"));
        }

        // POST : api/categories
        public IHttpActionResult Post([FromBody]Category newCategory)
        {
            Category category = dc.Categories.FirstOrDefault(c => c.acronym == newCategory.acronym);

            if (category != null) 
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.Conflict, "Category already exist!"));
            }

            dc.Categories.InsertOnSubmit(newCategory);

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

        // PUT: api/categories/HR

        public IHttpActionResult PUT([FromBody]Category newCategory)
        {
            Category category = dc.Categories.FirstOrDefault(c => c.acronym == newCategory.acronym);

            if(category == null) 
            {
                return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Category not exist!"));
            }    

            category.acronym = newCategory.acronym;     
            category.Category1 = newCategory.Category1;

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

        // Delete: api/categories/HR
        [Route("api/categories/{acronym}")]
        public IHttpActionResult Delete(string acronym) 
        { 
            Category category = dc.Categories.FirstOrDefault(c => c.acronym == acronym);   
            
            if(category != null) 
            {
                dc.Categories.DeleteOnSubmit(category);

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

            return ResponseMessage(Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Category not exist!"));

        }
    }
}
