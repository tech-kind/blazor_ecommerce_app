using System;

namespace BlazorECommerceApp.Client.EventArgs
{
    public class ReviewAverageCalculatedEventArgs
    {
        public decimal ReviewAverage { get; set; }

        public int ReviewCount { get; set; }
    }
}
