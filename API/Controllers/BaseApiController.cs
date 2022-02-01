using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    // bu tüm API'ların temeli olucak
    [ApiController]
    // yol localhost kısmından sonra /classAdını alıcak
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        
    }
}