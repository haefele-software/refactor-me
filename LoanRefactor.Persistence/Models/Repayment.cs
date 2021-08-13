using System;

namespace LoanRefactor.Persistence.Models
{
    public class Repayment
    {
        public decimal Amount { get; set; }

        public DateTime DatePaid { get; set; }
    }
}