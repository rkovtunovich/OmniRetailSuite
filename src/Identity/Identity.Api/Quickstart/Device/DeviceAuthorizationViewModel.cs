﻿// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using Identity.Api.Quickstart.Consent;

namespace Identity.Api.Quickstart.Device;

public class DeviceAuthorizationViewModel : ConsentViewModel
{
    public string UserCode { get; set; } = null!;

    public bool ConfirmUserCode { get; set; }
}
