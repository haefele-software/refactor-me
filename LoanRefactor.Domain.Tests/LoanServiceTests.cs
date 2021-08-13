using FluentAssertions;
using LoanRefactor.Persistence.Models;
using System;
using System.Linq;
using Xunit;

namespace LoanRefactor.Domain.Tests
{
    public class LoanServiceTests
    {
        [Fact]
        public void MakeLoadRepayment_should_return_error_message_when_loan_does_not_exist()
        {
            var loanService = new LoanService();

            var response = loanService.MakeLoadRepayment(123, new Repayment());

            response.Should().Be("Error: loan does not exist");
        }

        [Fact]
        public void Loan_should_be_fully_paid_on_final_payment()
        {
            var loanService = new LoanService();
            var loan = loanService.CreateLoan(100M, DateTime.Now.AddDays(30));

            var repayment = new Repayment()
            {
                Amount = 50,
                DatePaid = DateTime.Now
            };

            var finalRepayment = new Repayment()
            {
                Amount = 50,
                DatePaid = DateTime.Now
            };

            loanService.MakeLoadRepayment(loan.Id, repayment);

            var response = loanService.MakeLoadRepayment(loan.Id, finalRepayment);

            response.Should().Be("Success: Fully paid");
            loan.AmountRemaining.Should().Be(0M);
            loan.AmountRepaid.Should().Be(100M);
            loan.NextRepaymentDueDate.Should().Be(null);
            loan.IsFullyPaid.Should().BeTrue();
            loan.IsInAdvance.Should().BeTrue();
            loan.IsInArrears.Should().BeFalse();
        }

        [Fact]
        public void Loan_should_be_in_arrears_when_payment_received_less_than_amount_due()
        {
            var loanService = new LoanService();
            var loan = loanService.CreateLoan(100M, DateTime.Now.AddDays(-1));

            var repayment = new Repayment()
            {
                Amount = 10,
                DatePaid = DateTime.Now
            };

            var response = loanService.MakeLoadRepayment(loan.Id, repayment);
            
            var nextRepaymentDate = loan.ProjectedRepayments.First(projection => projection.ExpectedAmountRemaining > loan.AmountRemaining).RepaymentDate;

            response.Should().Be("Success: loan repayment recieved");
            loan.AmountRemaining.Should().Be(90M);
            loan.AmountRepaid.Should().Be(10M);
            loan.NextRepaymentDueDate.Should().Be(nextRepaymentDate);
            loan.IsFullyPaid.Should().BeFalse();
            loan.IsInAdvance.Should().BeFalse();
            loan.IsInArrears.Should().BeTrue();
        }

        [Fact]
        public void MakeLoadRepayment_should_fail_when_amount_repaid_is_greater_than_amount_remaining()
        {
            var loanService = new LoanService();
            var loan = loanService.CreateLoan(100M, DateTime.Now.AddDays(-1));

            var repayment = new Repayment()
            {
                Amount = 20,
                DatePaid = DateTime.Now
            };

            var overpayment = new Repayment()
            {
                Amount = 90,
                DatePaid = DateTime.Now
            };

            loanService.MakeLoadRepayment(loan.Id, repayment);

            var response = loanService.MakeLoadRepayment(loan.Id, overpayment);
           
            var nextRepaymentDate = loan.ProjectedRepayments.First(projection => projection.ExpectedAmountRemaining > loan.AmountRemaining).RepaymentDate;

            response.Should().Be("Error: the amount repaid is more than the amount remaining.");
            loan.AmountRemaining.Should().Be(80M);
            loan.AmountRepaid.Should().Be(20M);
            loan.NextRepaymentDueDate.Should().Be(nextRepaymentDate);
            loan.IsFullyPaid.Should().BeFalse();
            loan.IsInAdvance.Should().BeFalse();
            loan.IsInArrears.Should().BeFalse();
        }

        [Fact]
        public void MakeLoadRepayment_should_fail_when_loan_is_already_fully_paid()
        {
            var loanService = new LoanService();
            var loan = loanService.CreateLoan(100M, DateTime.Now.AddDays(30));

            var repayment = new Repayment()
            {
                Amount = 100,
                DatePaid = DateTime.Now
            };

            var overpayment = new Repayment()
            {
                Amount = 10,
                DatePaid = DateTime.Now
            };

            loanService.MakeLoadRepayment(loan.Id, repayment);

            var response = loanService.MakeLoadRepayment(loan.Id, overpayment);

            response.Should().Be("Error: Loan already fully paid");
            loan.AmountRemaining.Should().Be(0M);
            loan.AmountRepaid.Should().Be(100M);
            loan.NextRepaymentDueDate.Should().Be(null);
            loan.IsFullyPaid.Should().BeTrue();
            loan.IsInAdvance.Should().BeTrue();
            loan.IsInArrears.Should().BeFalse();
        }
    }
}
