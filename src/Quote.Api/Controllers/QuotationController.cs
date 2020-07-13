
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Services.Layer;
using System;
using System.Threading.Tasks;


namespace Quote.Api.Controllers
{

    
    [Authorize]   
    [ApiVersion("1.0")]
    [ApiController]
    [Produces("application/json")]  
    [Route("quotation")]

    public class QuotationController : Controller
    {

        /// <summary>
        /// Consulta la cotizacion de la moneda a api externa
        /// </summary>
        /// <param name="currency">
        /// Parámetro = currency: "string"
        /// </param>
        /// <returns></returns>
        [HttpGet("{currency}")]
        public async Task<IActionResult> Quotation(string currency)
        {

            if (currency != null)
            {
                QuotationService quotation = null;
                IQuotationService iquotation = QuotationBuilderFactor.GetQuote(currency);
                quotation = new QuotationService(iquotation);
                var json = await quotation.Quote();

                JObject rss = JObject.Parse(json);

                JObject result = (JObject)rss["result"];

                result.Property("updated").Remove();
                result.Property("target").Remove();
                result.Property("value").Remove();
                result.Property("quantity").Remove();

                return Ok(result);
            }

            throw new NotImplementedException("This currency is not supported");
        }

    }
}