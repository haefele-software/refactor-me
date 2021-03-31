using System;
using Xunit;

namespace ProcessDelivery.Tests
{
    public class ReturnBookTests
    {
        [Fact]
        public void ShouldReturn_LowRisk_When_NoReturnHistoryExistsAndReturnedOnDueDateThisTime()
        {
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = null,
                LastReturnedDate = null,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate);

            Assert.Equal("LowRisk: first time being returned and returned on time", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_NoReturnHistoryExistsAndReturnedLateThisTime()
        {
            var currentDueDate = DateTime.Now.AddDays(-1);
            var book = new Book()
            {
                LastDueDate = null,
                LastReturnedDate = null,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(1));

            Assert.Equal("MediumRisk: first time being returned and returned late", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_NoReturnHistoryExistsAndReturnedEarlyThisTime()
        {
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = null,
                LastReturnedDate = null,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(-1));

            Assert.Equal("LowRisk: first time being returned and returned early", result);
        }



        [Fact]
        public void ShouldReturn_LowRisk_When_BookWasReturnedOnDueDateLastTimeAndOnDueDateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-1);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate);

            Assert.Equal("LowRisk: returned on due date last 2 times", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_BookWasReturnedOnDueDateLastTimeButWasLateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-1);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(1));

            Assert.Equal("MediumRisk: returned on due date last time but late this time", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_BookWasReturnedOnDueDateLastTimeButWasEarlyThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-1);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate,

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(-1));

            Assert.Equal("MediumRisk: returned on due date last time but early this time", result);
        }





        [Fact]
        public void ShouldReturn_MediumRisk_When_BookWasLateLastTimeButOnDueDateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-1);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate);

            Assert.Equal("MediumRisk: returned late last time but on due date this time", result);
        }
        
        [Fact]
        public void ShouldReturn_HighRisk_When_BookWasReturnedLateLastTimeAndLateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-2);
            var currentDueDate = DateTime.Now.AddDays(-1);
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(1));

            Assert.Equal("HighRisk: returned late last time and late this time", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_BookWasLateLastTimeButEarlyThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-2);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(-1));

            Assert.Equal("MediumRisk: returned late last time but early this time", result);
        }


        [Fact]
        public void ShouldReturn_LowRisk_When_BookWasReturnedEarlyLastTimeAndOnDueDateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-1);
            var currentDueDate = DateTime.Now;
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(-1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate);

            Assert.Equal("LowRisk: returned on early last time and on due date this time", result);
        }

        [Fact]
        public void ShouldReturn_MediumRisk_When_BookWasReturnedEarlyLastTimeButWasLateThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-2);
            var currentDueDate = DateTime.Now.AddDays(-1);
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(-1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(1));

            Assert.Equal("MediumRisk: returned early last time but late this time", result);
        }

        [Fact]
        public void ShouldReturn_LowRisk_When_BookWasReturnedEarlyLastTimeAndEarlyThisTime()
        {
            var lastDueDate = DateTime.Now.AddDays(-2);
            var currentDueDate = DateTime.Now.AddDays(-1);
            var book = new Book()
            {
                LastDueDate = lastDueDate,
                LastReturnedDate = lastDueDate.AddDays(-1),

                CurrentDueDate = currentDueDate,
            };
            var libraryManager = new LibraryManager();

            var result = libraryManager.ReturnBook(book, currentDueDate.AddDays(-1));

            Assert.Equal("LowRisk: returned early last time and early this time", result);
        }

    }
}
