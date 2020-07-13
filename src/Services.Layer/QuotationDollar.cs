
using System.Threading.Tasks;

namespace Services.Layer
{
    public class QuotationDollar : IQuotationService
    {

        private string currentMoneyAcronym = "USD"; //USD
        private string currentMoney = "Dollar"; //Dollar

        public QuotationDollar()
        {
        }

        public string CurrentMoney
        {
            get
            {
                return this.currentMoney;
            }
            set
            {
                this.currentMoney = value;
            }
        }
        public string CurrentMoneyAcronym
        {
            get
            {
                return this.currentMoneyAcronym;
            }
            set
            {
                this.currentMoneyAcronym = value;
            }
        }

        public Task<string> Quote()
        {
            return this.Quotation();
        }

        private async Task<string> Quotation()
        {
            var data = await CallApi.Quotation(this.CurrentMoneyAcronym);
            return data;
        }
    }
}
