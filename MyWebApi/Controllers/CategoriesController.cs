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
        /// <summary>
        /// Get categories list
        /// </summary>
        /// <returns>List fo all Categories</returns>
        public List<Category> Get()
        {
            var list = from Category in dc.Categories select Category;

            return list.ToList();
        }

        // GET: api/Categories/HR
        /// <summary>
        /// Get a category Detail by acronym
        /// </summary>
        /// <param name="acronym">acronym</param>
        /// <returns>All details of especific Category</returns>
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
        /// <summary>
        /// Regist a new category
        /// </summary>
        /// <param name="newCategory">Category</param>
        /// <returns></returns>
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
        /// <summary>
        /// Edit category details
        /// </summary>
        /// <param name="newCategory">Category</param>
        /// <returns></returns>

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
        /// <summary>
        /// Delete category by acronym
        /// </summary>
        /// <param name="acronym">acronym</param>
        /// <returns></returns>
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
