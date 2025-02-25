using Microsoft.AspNetCore.Mvc;

namespace ProtoType.Api.Breakfasts
{
    public class FrenchToastController : ControllerBase
    {
        [HttpGet("/french-toast")]
        public ActionResult Get()
        {
            return Ok ("Delicious!");
        }
    }
}
