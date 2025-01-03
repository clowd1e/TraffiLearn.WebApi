﻿using TraffiLearn.Domain.Routes.RouteDescriptions;
using TraffiLearn.Domain.Routes.RouteNumbers;
using TraffiLearn.Domain.ServiceCenters;
using TraffiLearn.SharedKernel.Primitives;
using TraffiLearn.SharedKernel.Shared;
using TraffiLearn.SharedKernel.ValueObjects.ImageUris;

namespace TraffiLearn.Domain.Routes
{
    public sealed class Route : AggregateRoot<RouteId>
    {
        private RouteNumber _routeNumber;

        private Route()
            : base(new(Guid.Empty))
        { }

        private Route(
            RouteId id,
            RouteNumber routeNumber,
            RouteDescription? description,
            ImageUri? imageUri) : base(id)
        {
            RouteNumber = routeNumber;
            Description = description;
            ImageUri = imageUri;
        }

        public RouteNumber RouteNumber
        {
            get
            {
                return _routeNumber;
            }
            private set
            {
                ArgumentNullException.ThrowIfNull(value, "Route number cannot be null.");

                _routeNumber = value;
            }
        }

        public ImageUri? ImageUri { get; private set; }

        public RouteDescription? Description { get; private set; }

        public ServiceCenter? ServiceCenter { get; private set; } = default;

        public Result SetServiceCenter(ServiceCenter serviceCenter)
        {
            ArgumentNullException.ThrowIfNull(serviceCenter, nameof(serviceCenter));

            ServiceCenter = serviceCenter;

            return Result.Success();
        }

        public Result Update(
            RouteNumber routeNumber,
            RouteDescription? routeDescription,
            ImageUri? imageUri)
        {
            RouteNumber = routeNumber;
            Description = routeDescription;
            ImageUri = imageUri;

            return Result.Success();
        }

        public void SetImageUri(ImageUri? imageUri)
        {
            ImageUri = imageUri;
        }

        public static Result<Route> Create(
            RouteId routeId,
            RouteNumber routeNumber,
            RouteDescription? routeDescription,
            ImageUri? imageUri)
        {
            return new Route(routeId, routeNumber, routeDescription, imageUri);
        }
    }
}
