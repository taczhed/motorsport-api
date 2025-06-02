namespace MotorsportApi.Web.Middleware
{
    public class RequestCountingMiddleware
    {
        private static int _requestCount = 0;
        private readonly RequestDelegate _next;

        public RequestCountingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            Interlocked.Increment(ref _requestCount);

            //Console.WriteLine($"Total requests: {_requestCount}");

            await _next(context);
        }

        public static int GetRequestCount() => _requestCount;
    }
}