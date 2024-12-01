﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace TestAssignmentLunaEdge.Core.Interfaces
{
    public interface IJwtService
    {
        string GenerateToken(Guid userid);
        ClaimsPrincipal? ValidateToken(string token);
    }
}