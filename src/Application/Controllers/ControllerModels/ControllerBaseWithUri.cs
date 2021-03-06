using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Threading.Tasks;

namespace Application.Controllers.ControllerModels
{
    public abstract class ControllerBaseWithUri<T> : ControllerBase
    {
        public abstract string CreateResourceUri(T resourceParameters, ResourceUriType type, string urlLink);
    }
}
