﻿using System;
using System.Threading;
using System.Threading.Tasks;
using AsyncAwait.Task2.CodeReviewChallenge.Headers;
using CloudServices.Interfaces;
using Microsoft.AspNetCore.Http;

namespace AsyncAwait.Task2.CodeReviewChallenge.Middleware;

public class StatisticMiddleware
{
    private readonly RequestDelegate _next;

    private readonly IStatisticService _statisticService;

    public StatisticMiddleware(RequestDelegate next, IStatisticService statisticService)
    {
        _next = next;
        _statisticService = statisticService ?? throw new ArgumentNullException(nameof(statisticService));
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string path = context.Request.Path;

        //var staticRegTask = Task.Run(
        //    () => _statisticService.RegisterVisitAsync(path)
        //        .ConfigureAwait(false)
        //        .GetAwaiter().OnCompleted(UpdateHeaders));
        //Console.WriteLine(staticRegTask.Status); // just for debugging purposes
        await _statisticService.RegisterVisitAsync(path);

        var totalPageVisits = await _statisticService.GetVisitsCountAsync(path);
        context.Response.Headers.Add(
                CustomHttpHeaders.TotalPageVisits,
                totalPageVisits.ToString());
        //void UpdateHeaders()
        //{
        //    context.Response.Headers.Add(
        //        CustomHttpHeaders.TotalPageVisits,
        //        _statisticService.GetVisitsCountAsync(path).GetAwaiter().GetResult().ToString());
        //}

        await _next(context);
    }
}
