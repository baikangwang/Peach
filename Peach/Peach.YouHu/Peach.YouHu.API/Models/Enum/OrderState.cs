namespace Peah.YouHu.API.Models
{
    public enum OrderState
    {
        Ready = 0,
        Dealing = 1,
        Rejected = 2,
        Dealt = 3,
        Paying = 4,
        Paid = 5,
        InProgress = 6,
        Arrived = 7,
        Consigned = 8
    }
}