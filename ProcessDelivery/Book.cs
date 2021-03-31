using System;

namespace ProcessDelivery
{
    public class Book
    {
        public DateTime? LastReturnedDate { get; set; }
        public DateTime? LastDueDate { get; set; }
        public DateTime CurrentDueDate { get; set; }
    }
}