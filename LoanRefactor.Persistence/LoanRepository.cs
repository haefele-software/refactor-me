using LoanRefactor.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanRefactor.Persistence
{
    public class LoanRepository
    {
        private List<Loan> loans = new List<Loan>();

        public void Add(Loan loan)
        {
            loans.Add(loan);
        }

        public Loan Get(int loanId)
        {
            return loans.FirstOrDefault(l => l.Id == loanId);
        }

        internal void Save(Loan loan)
        {
            loan.Id = new Random().Next();
            loans.Add(loan);
        }
    }
}
