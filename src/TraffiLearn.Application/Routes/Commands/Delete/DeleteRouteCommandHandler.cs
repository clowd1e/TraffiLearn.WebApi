﻿using MediatR;
using TraffiLearn.Application.Abstractions.Data;
using TraffiLearn.Application.Abstractions.Storage;
using TraffiLearn.Domain.Aggregates.Routes;
using TraffiLearn.Domain.Aggregates.Routes.Errors;
using TraffiLearn.Domain.Aggregates.Routes.ValueObjects;
using TraffiLearn.Domain.Shared;

namespace TraffiLearn.Application.Routes.Commands.Delete
{
    internal sealed class DeleteRouteCommandHandler
        : IRequestHandler<DeleteRouteCommand, Result>
    {
        private readonly IRouteRepository _routeRepository;
        private readonly IImageService _imageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteRouteCommandHandler(
            IRouteRepository routeRepository, 
            IImageService imageService,
            IUnitOfWork unitOfWork)
        {
            _routeRepository = routeRepository;
            _imageService = imageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result> Handle(
            DeleteRouteCommand request, 
            CancellationToken cancellationToken)
        {
            var routeId = new RouteId(request.RouteId.Value);

            var route = await _routeRepository.GetByIdAsync(routeId, cancellationToken);

            if (route is null)
            {
                return RouteErrors.NotFound;
            }

            await _routeRepository.DeleteAsync(route);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            if (route.ImageUri is not null)
            {
                await _imageService.DeleteAsync(
                    imageUri: route.ImageUri,
                    cancellationToken);
            }
            
            return Result.Success();
        }
    }
}