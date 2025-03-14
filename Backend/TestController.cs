using Backend.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Backend
{
    [ApiController]
    [Route("test")]
    public class TestController : ControllerBase
    {
        private MariaDBContext dbcontext;
        public TestController(MariaDBContext mariaDBContext)
        {
            dbcontext = mariaDBContext;
        }
        [HttpGet]
        public async Task<IEnumerable<testDataModel>> Get()
        {
            return await dbcontext.Test.ToListAsync();
        }
    }
    public class testDataModel
    {
        public int? id { get; set; }
        public string name { get; set; }
    }
}
