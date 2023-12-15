namespace BackOffice.Application.Helpers;

public static class RetailUrlHelper
{
    private readonly static RetailResource _retailResource = new();

    public static string GetAllCashiers()
    {
        return $"{_retailResource.BaseAddress}cashiers";
    }

    public static string GetCashier(Guid cashierId)
    {
        return $"{_retailResource.BaseAddress}cashiers/{cashierId}";
    }

    public static string AddCashier()
    {
        return $"{_retailResource.BaseAddress}cashiers";
    }

    public static string UpdateCashier()
    {
        return $"{_retailResource.BaseAddress}cashiers";
    }

    public static string DeleteCashier(Guid cashierId, bool isSoftDeleting)
    {
        return $"{_retailResource.BaseAddress}cashiers/{cashierId}?useSoftDeleting={isSoftDeleting}";
    }
}
