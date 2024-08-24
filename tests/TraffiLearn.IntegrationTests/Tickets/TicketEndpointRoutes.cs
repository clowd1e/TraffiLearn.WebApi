﻿namespace TraffiLearn.IntegrationTests.Tickets
{
    internal static class TicketEndpointRoutes
    {
        public static readonly string CreateTicketRoute =
            "api/tickets";

        public static readonly string GetAllTicketsRoute =
            "api/tickets";

        public static string GetTicketQuestionsRoute(Guid ticketId) =>
            $"api/tickets/{ticketId}/questions";
    }
}
