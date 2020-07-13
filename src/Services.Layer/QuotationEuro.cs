
using System.Threading.Tasks;

namespace Services.Layer
{
    public class QuotationEuro : IQuotationService
    {
        private string currentMoneyAcronym = "EUR"; //EUR
        private string currentMoney = "Euro";

        public QuotationEuro(
        )
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

        public async Task<string> Quote()
        {
            var data = await CallApi.Quotation(this.CurrentMoneyAcronym);

            return data;
        }
    }
}
