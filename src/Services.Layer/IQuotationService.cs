
using System.Threading.Tasks;

namespace Services.Layer
{
    public interface IQuotationService
    {

        string CurrentMoney { get; set; }
        string CurrentMoneyAcronym { get; set; }
        Task<string> Quote();

    }
}
