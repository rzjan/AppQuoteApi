
using System.Threading.Tasks;

namespace Services.Layer
{
    public class QuotationReal : IQuotationService
    {
        private string currentMoneyAcronym = "BRL"; //BRL
        private string currentMoney = "Real";
        public QuotationReal(
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
