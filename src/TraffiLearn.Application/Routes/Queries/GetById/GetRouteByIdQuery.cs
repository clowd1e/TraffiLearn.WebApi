﻿using MediatR;
using TraffiLearn.Application.Routes.DTO;
using TraffiLearn.SharedKernel.Shared;

namespace TraffiLearn.Application.Routes.Queries.GetById
{
    public sealed record GetRouteByIdQuery(
        Guid? RouteId) : IRequest<Result<RouteResponse>>;
}
