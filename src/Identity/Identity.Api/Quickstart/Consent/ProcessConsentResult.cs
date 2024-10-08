﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;

namespace Identity.Api.Quickstart.Consent;

public class ProcessConsentResult
{
    public bool IsRedirect => RedirectUri != null;

    public string RedirectUri { get; set; } = null!;

    public Client Client { get; set; } = null!;

    public bool ShowView => ViewModel != null;

    public ConsentViewModel ViewModel { get; set; } = null!;

    public bool HasValidationError => ValidationError != null;

    public string ValidationError { get; set; } = null!;
}
