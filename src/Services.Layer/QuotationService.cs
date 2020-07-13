
using System.Threading.Tasks;

namespace Services.Layer
{
    public class QuotationService
    {

        protected IQuotationService _quote;

        public QuotationService(IQuotationService quote)
        {
            this._quote = quote;
        }

        public Task<string> Quote()
        {
            return this._quote.Quote();
        }
    }
}
