﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.

namespace Identity.Api.Quickstart.Account;

public class ExternalProvider
{
    public string DisplayName { get; set; } = null!;

    public string AuthenticationScheme { get; set; } = null!;
}
