// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Identity.Api.Quickstart.Diagnostics;

[SecurityHeaders]
[Authorize]
public class DiagnosticsController : Controller
{
    public async Task<IActionResult> Index()
    {
        if (HttpContext is null)
            throw new ArgumentNullException(nameof(HttpContext));

        var localIpAddress = HttpContext.Connection?.LocalIpAddress?.ToString();
        var localAddresses = new List<string> { "127.0.0.1", "::1" };
        if (localIpAddress is not null)
            localAddresses.Add(localIpAddress);

        var remoteIpAddress = HttpContext.Connection?.LocalIpAddress?.ToString();
        if (remoteIpAddress is null)
            return NotFound();

        if (!localAddresses.Contains(remoteIpAddress))
            return NotFound();

        var model = new DiagnosticsViewModel(await HttpContext.AuthenticateAsync());

        return View(model);
    }
}
