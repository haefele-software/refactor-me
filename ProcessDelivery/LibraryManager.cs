using System;

namespace ProcessDelivery
{
    public class LibraryManager
    {
        public string ReturnBook(Book book, DateTime dateReturned)
        {
            if (book.LastReturnedDate != null && book.LastDueDate != null)
            {
                if (book.LastDueDate == book.LastReturnedDate)
                {
                    if (book.CurrentDueDate == dateReturned)
                    {
                        return "LowRisk: returned on due date last 2 times";
                    }
                    else if (book.CurrentDueDate < dateReturned)
                    {
                        return "MediumRisk: returned on due date last time but late this time";
                    }
                    else
                    {
                        return "MediumRisk: returned on due date last time but early this time";
                    }
                }
                else
                {
                    if (book.LastDueDate < book.LastReturnedDate)
                    {
                        if (book.CurrentDueDate == dateReturned)
                        {
                            return "MediumRisk: returned late last time but on due date this time";
                        }
                        else if (book.CurrentDueDate < dateReturned)
                        {
                            return "HighRisk: returned late last time and late this time";
                        }
                        else
                        {
                            return "MediumRisk: returned late last time but early this time";
                        }
                    }
                    else
                    {
                        if (book.CurrentDueDate == dateReturned)
                        {
                            return "LowRisk: returned on early last time and on due date this time";
                        }
                        else if (book.CurrentDueDate < dateReturned)
                        {
                            return "MediumRisk: returned early last time but late this time";
                        }
                        else
                        {
                            return "LowRisk: returned early last time and early this time";
                        }
                    }
                }
            }
            else
            {
                if (book.CurrentDueDate == dateReturned)
                {
                    return "LowRisk: first time being returned and returned on time";
                }
                else if (book.CurrentDueDate < dateReturned)
                {
                    return "MediumRisk: first time being returned and returned late";
                }

                return "LowRisk: first time being returned and returned early";
            }
        }
    }
}
