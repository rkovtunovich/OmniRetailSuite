﻿using System;

namespace Catalog.Core.Exceptions;

public class DuplicateException : Exception
{
    public DuplicateException(string message) : base(message)
    {

    }

}
