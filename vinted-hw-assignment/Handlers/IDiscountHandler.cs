using vinted_hw_assignment.Context;
using vinted_hw_assignment.Models;

namespace vinted_hw_assignment.Handlers;

//used to implement different classes that handle discount logic
//helps with flexible design
public interface IDiscountHandler
{
    Transaction ApplyDiscount(Transaction transaction, DiscountContext context);
}