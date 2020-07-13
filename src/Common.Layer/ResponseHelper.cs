

namespace Common.Layer
{
    public abstract class ResponseBaseHelper
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
    }

    public class ResponseHelper<T> : ResponseBaseHelper
    {
        public T Result { get; set; }
    }

    public class Result
    {
        public string Updated { get; set; }
        public string Source { get; set; } //Currency
        public string Target { get; set; } //Price
        public string Value { get; set; } //Price
        public string Quantity { get; set; }
        public string Amount { get; set; }

    }
}
