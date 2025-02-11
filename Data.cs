
namespace BankApp
{
    public class Data : BaseClass
    {
        public string FirstName{ get; set;} = default!;
        public string MiddleName{ get; set;} = null!;
        public string LastName{get; set;} = default!;
        public string DateOfBirth{get; set;} = default!;
        public string Username{get; set;} = default!;
        public int Pin{get; set;} = default!;
        public decimal MoneyInBank{get; set;} = default!;
        public string AccountNumber{get; set;} = default!;

    }
}