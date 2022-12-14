using Microsoft.AspNetCore.Mvc;
using BuberBreakfast.Contracts.Breakfast;
using BuberBreakfast.Models;
using BuberBreakfast.Services.Breakfasts;
using ErrorOr;
using BuberBreakfast.ServiceErrors;

namespace BuberBreakfast.Controllers;

public class BreakfastsController : ApiController
{

    private readonly IBreakfastService _breakfastService;

    public BreakfastsController(IBreakfastService breakfastService)
    {
        _breakfastService = breakfastService;
    }

    [HttpPost]
    public IActionResult CreateBreakfast(CreateBreakfastRequest request)
    {
        ErrorOr <Breakfast> requestBreakfastResult =  Breakfast.Create(
            request.Name,
            request.Description,
            request.StartDateTime,
            request.EndDateTime,
            request.Savory,
            request.Sweet
        );
        if (requestBreakfastResult.IsError)
            return Problem(requestBreakfastResult.Errors);
        var breakfast = requestBreakfastResult.Value;
        ErrorOr<Created> createdBreakfastResult = _breakfastService.CreateBreakfast(breakfast);
         return createdBreakfastResult.Match(
            created => CreatedAtGetBreakfast(breakfast),
            errors => Problem(errors)
        );
    }


    [HttpGet("{id:guid}")]
    public IActionResult GetBreakfast(Guid id)
    {
        ErrorOr<Breakfast> getBreakfastResult = _breakfastService.GetBreakfast(id);
        return getBreakfastResult.Match(
            breakfast => Ok(MapBreakfastResponse(breakfast)),
            errors => Problem(errors)
        );
    }

   

    [HttpPut("{id:guid}")]
    public IActionResult UpsertBreakfast(Guid id, UpsertBreakfastRequest request)
    {

        ErrorOr<Breakfast> requestBreakfastResult =  Breakfast.From(id,request);

       if (requestBreakfastResult.IsError)
        return Problem(requestBreakfastResult.Errors);
        var breakfast = requestBreakfastResult.Value;
       ErrorOr<UpsertedBreakfast> upsertedBreakfastResult = _breakfastService.UpsertBreakfast(breakfast);

        return upsertedBreakfastResult.Match(
            upserted => upserted.isNewlyCreated ? CreatedAtGetBreakfast(breakfast) : NoContent(),
            errors => Problem(errors)
        );
    }

    [HttpDelete("{id:guid}")]
    public IActionResult DeleteBreakfast(Guid id)
    {
       ErrorOr<Deleted> deletedBreakfastResult = _breakfastService.DeleteBreakfast(id);
       return deletedBreakfastResult.Match(
        deleted=> NoContent(),
        errors => Problem(errors)
       );
    }

     private static BreakfastResponse MapBreakfastResponse(Breakfast breakfast)
    {
        return new BreakfastResponse(
           breakfast.Id,
           breakfast.Name,
           breakfast.Description,
           breakfast.StartDateTime,
           breakfast.EndDateTime,
           breakfast.LastModifiedDateTime,
           breakfast.Savory,
           breakfast.Sweet
       );
    }
    
    private CreatedAtActionResult CreatedAtGetBreakfast(Breakfast breakfast)
    {
        return CreatedAtAction(
            actionName: nameof(GetBreakfast),
            routeValues: new { id = breakfast.Id },
            value: MapBreakfastResponse(breakfast));
    }
}