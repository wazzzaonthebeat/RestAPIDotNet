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
        public static Error InvalidName => Error.Validation(
            code:"Breakfast.InvalidName",
            description:$"Your breakfast name must be atleast {Models.Breakfast.MinNameLength}"+
            $" characters long and at most {Models.Breakfast.MaxNameLength} characters long"
        );
         public static Error InvalidDescription => Error.Validation(
            code:"Breakfast.InvalidDescription",
            description:$"Your breakfast description must be atleast {Models.Breakfast.MinDescripLength}"+
            $" characters long and at most {Models.Breakfast.MaxDescripLength} characters long"
        );
    }
}