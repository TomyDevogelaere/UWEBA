using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;

namespace FilterLab.Filters;

public class ProfileAttribute: ActionFilterAttribute
{
    private Stopwatch? actionTimer;
    private Stopwatch? resultTimer;

    //public override void OnActionExecuting(ActionExecutingContext context)
    //{
    //    actionTimer = Stopwatch.StartNew();
    //}

    //public override void OnActionExecuted(ActionExecutedContext context)
    ////{
    ////    actionTimer?.Stop();
    ////    if (context.Exception is null && !context.Canceled)
    ////    {
    ////        Debug.WriteLine($"Action {context.ActionDescriptor.DisplayName} " +
    ////            $"elapsed time = {actionTimer?.Elapsed.TotalMilliseconds} ms");
    ////    }
    ////}
    ////public override void OnResultExecuting(ResultExecutingContext context)
    ////{
    ////    resultTimer = Stopwatch.StartNew();
    ////}

    //public override void OnResultExecuted(ResultExecutedContext context)
    //{
    //    resultTimer?.Stop();
    //    if (context.Exception is null && !context.Canceled)
    //    {
    //        Debug.WriteLine($"Result {context.ActionDescriptor.DisplayName} " +
    //            $"method elapsed time = {resultTimer?.Elapsed.TotalMilliseconds} ms");
    //    }
    //}
    public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
       actionTimer = Stopwatch.StartNew();
        var actionExecutedContext = await next();
        actionTimer?.Stop();
        if (actionExecutedContext.Exception == null && !actionExecutedContext.Canceled)
        {
          Debug.WriteLine($"Action {context.ActionDescriptor.DisplayName} elapsed time: {actionTimer?.Elapsed.TotalMilliseconds}");
        }
    }
    public override async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
         resultTimer = Stopwatch.StartNew();
        var actionExecutedContext = await next();
        resultTimer?.Stop();
        if (actionExecutedContext.Exception == null && !actionExecutedContext.Canceled)
        {
            Debug.WriteLine($"Action {context.ActionDescriptor.DisplayName} elapsed time: {actionTimer?.Elapsed.TotalMilliseconds}");
        }
    }
}
