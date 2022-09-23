using ErrorOr;

namespace BuberBreakfast.ServiceErrors;

public static class Errors
{
    public static class Breakfast
    {
        public static Error NotFound => Error.NotFound(
            code:"Breakfast.NotFound",
            description:"Your breakfast was not found"
        );
    }
}