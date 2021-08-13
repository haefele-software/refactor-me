using LoanRefactor.Persistence;
using LoanRefactor.Persistence.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace LoanRefactor.Domain
{
    public class LoanService
    {
        private LoanRepository loanRepository = new LoanRepository();

        public Loan CreateLoan(decimal amount, DateTime loanStartDate)
        {
            var loan = new Loan(loanRepository);

            loan.ProjectedRepayments = new List<(DateTime, decimal)>
            {
                (loanStartDate, amount),
                (loanStartDate.AddDays(30), amount - (amount * 0.2M)),
                (loanStartDate.AddDays(60), amount - (amount * 0.4M)),
                (loanStartDate.AddDays(90), amount - (amount * 0.6M)),
                (loanStartDate.AddDays(120), amount - (amount * 0.8M)),
                (loanStartDate.AddDays(150), 0)
            };
            loan.AmountRemaining = amount;
            loan.AmountRepaid = 0;
            loan.IsFullyPaid = false;
            loan.IsInAdvance = false;
            loan.IsInArrears = false;
            loan.NextRepaymentDueDate = loanStartDate.AddDays(30);
            loan.Repayments = new List<Repayment>();
            
            loan.Save();

            return loan;
        }

        public string MakeLoadRepayment(int loanId, Repayment repayment)
        {
            var loan = loanRepository.Get(loanId);

            var response = string.Empty;

            if (loan == null)
            {
                response = "Error: loan does not exist";
            }
            else
            {
                if (loan.AmountRemaining > 0)
                {
                    if (repayment.Amount > 0 && repayment.Amount <= loan.AmountRemaining)
                    {
                        loan.Repayments.Add(repayment);

                        if (repayment.Amount == loan.AmountRemaining)
                        {
                            loan.AmountRemaining = 0;
                            loan.AmountRepaid += repayment.Amount;
                            loan.NextRepaymentDueDate = null;
                            response = "Success: Fully paid";
                        }
                        else if (repayment.Amount < loan.AmountRemaining)
                        {

                            loan.AmountRemaining -= repayment.Amount;
                            loan.AmountRepaid += repayment.Amount;
                            loan.IsFullyPaid = false;
                            loan.NextRepaymentDueDate = loan.ProjectedRepayments.First(projection => projection.ExpectedAmountRemaining > loan.AmountRemaining).RepaymentDate;
                            response = "Success: loan repayment recieved";
                        }

                        var expectedRepaymentStatus = loan.ProjectedRepayments.First(projection => projection.RepaymentDate >= DateTime.Now);

                        if (loan.AmountRemaining > expectedRepaymentStatus.ExpectedAmountRemaining  )
                        {
                            loan.IsInArrears = true;
                            loan.IsInAdvance = false;
                        }
                        else if (loan.AmountRemaining < expectedRepaymentStatus.ExpectedAmountRemaining )
                        {
                            loan.IsInAdvance = true;
                            loan.IsInArrears = false;
                        }
                        else
                        {
                            loan.IsInAdvance = false;
                            loan.IsInArrears = false;
                        }

                        if (loan.AmountRemaining == 0)
                        {
                            loan.IsFullyPaid = true;
                        }
                    }
                    else if (repayment.Amount > loan.AmountRemaining)
                    {
                        response = "Error: the amount repaid is more than the amount remaining.";
                    }
                }
                else
                {
                    response = "Error: Loan already fully paid";
                }

                loan.Save();
            }

            return response;
        }
    }
}
