using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace Services.Layer
{
    public static class QuotationBuilderFactor
    {
        private static List<IQuotationService> quotationTypeCurrency = new List<IQuotationService>();

        public static IQuotationService GetQuote(string currency)
        {
            if (quotationTypeCurrency.Count == 0)
            {
                quotationTypeCurrency = Assembly.GetExecutingAssembly()
                                                .GetTypes()
                                                .Where(type => typeof(IQuotationService).IsAssignableFrom(type) && type.IsClass)
                                                .Select(type => Activator.CreateInstance(type))
                                                .Cast<IQuotationService>()
                                                .ToList();
            }
            return quotationTypeCurrency.Where(c => c.CurrentMoney == currency).FirstOrDefault();

        }
    }
}
