using System;
using System.Collections.Generic;

namespace LoanRefactor.Persistence.Models
{
    public class Loan
    {
        private LoanRepository loanRepository;

        public Loan(LoanRepository loanRepository)
        {
            this.loanRepository = loanRepository;
        }

        public int Id { get; set; }

        public List<( DateTime RepaymentDate, decimal ExpectedAmountRemaining)> ProjectedRepayments { get; set; }

        public decimal AmountRemaining { get; set; }

        public decimal AmountRepaid { get; set; }

        public DateTime? NextRepaymentDueDate { get; set; }

        public bool IsInArrears { get; set; }
        public bool IsInAdvance { get; set; }
        public bool IsFullyPaid { get; set; }

        public List<Repayment> Repayments { get; set; }

        public void Save()
        {
            loanRepository.Save(this);
        }
    }
}
