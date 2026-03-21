using System.Text.Json;
using DotNetBuildingBlocks.Guards;
using DotNetBuildingBlocks.Primitives.Errors;
using DotNetBuildingBlocks.Primitives.Paging;
using DotNetBuildingBlocks.Primitives.Results;
using DotNetBuildingBlocks.Serialization;
using DotNetBuildingBlocks.Time;

namespace DotNetBuildingBlocks.ConsoleDemo
{
    /// <summary>
    /// Helper Class
    /// </summary>
    public static class HelperClass
    {
        /// <summary>
        /// RunAbstractionsExample
        /// </summary>
        public static void RunAbstractionsExample()
        {
            WriteSection("1. Abstractions");

            Customer customer = new(
                Guid.NewGuid(),
                "John Doe",
                "John@example.com");

            Console.WriteLine($"Customer Id: {customer.Id}");
            Console.WriteLine($"Customer Name: {customer.Name}");
            Console.WriteLine($"Customer Email: {customer.Email}");
        }

        /// <summary>
        /// RunPrimitivesExample
        /// </summary>
        public static void RunPrimitivesExample()
        {
            WriteSection("2. Primitives");

            Error error = new(
                Code: "customer.email.invalid",
                Message: "Customer email is invalid.");

            Result successResult = Result.Success();
            Result<CustomerSummary> customerSuccess = Result.Success(
                new CustomerSummary(Guid.NewGuid(), "John Doe"));
            Result failureResult = Result.Failure(error);

            Console.WriteLine($"Success Result IsSuccess: {successResult.IsSuccess}");
            Console.WriteLine($"Generic Success Result IsSuccess: {customerSuccess.IsSuccess}");
            Console.WriteLine($"Failure Result IsFailure: {failureResult.IsFailure}");
            Console.WriteLine($"Failure Error Code: {failureResult.Error.Code}");
            Console.WriteLine($"Failure Error Message: {failureResult.Error.Message}");

            PagedRequest pageRequest = new(1, 2);

            IReadOnlyCollection<CustomerSummary> customers =
            [
                new CustomerSummary(Guid.NewGuid(), "John Doe"),
                new CustomerSummary(Guid.NewGuid(), "John Smith"),
            ];

            PagedResult<CustomerSummary> pageResult = new(
                Items: customers,
                PageNumber: pageRequest.PageNumber,
                PageSize: pageRequest.PageSize,
                TotalCount: 10);

            Console.WriteLine($"Requested Page: {pageRequest.PageNumber}");
            Console.WriteLine($"Requested Page Size: {pageRequest.PageSize}");
            Console.WriteLine($"Returned Items: {pageResult.Items.Count}");
            Console.WriteLine($"Total Count: {pageResult.TotalCount}");
        }

        /// <summary>
        /// RunGuardsExample
        /// </summary>
        public static void RunGuardsExample()
        {
            WriteSection("3. Guards");

            string validName = Guard.AgainstNullOrWhiteSpace("John");
            int validPageSize = Guard.AgainstZeroOrNegative(10);
            Guid validId = Guard.AgainstEmpty(Guid.NewGuid());

            Console.WriteLine($"Validated Name: {validName}");
            Console.WriteLine($"Validated Page Size: {validPageSize}");
            Console.WriteLine($"Validated Guid: {validId}");

            try
            {
                Guard.AgainstNullOrWhiteSpace("   ");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Expected guard exception: {ex.Message}");
            }
        }

        /// <summary>
        /// RunTimeExample
        /// </summary>
        public static void RunTimeExample()
        {
            WriteSection("4. Time");

            SystemClock clock = new();

            DateTime utcNow = clock.UtcNow;
            DateTimeOffset utcNowOffset = clock.UtcNowOffset;
            DateOnly todayUtc = clock.TodayUtc;

            Console.WriteLine($"UtcNow: {utcNow:O}");
            Console.WriteLine($"UtcNowOffset: {utcNowOffset:O}");
            Console.WriteLine($"TodayUtc: {todayUtc:yyyy-MM-dd}");

            DateTime validatedUtc = TimeGuard.AgainstNonUtc(utcNow, nameof(utcNow));
            DateTimeOffset validatedUtcOffset = TimeGuard.AgainstNonUtc(utcNowOffset, nameof(utcNowOffset));

            Console.WriteLine($"Validated UTC DateTime Kind: {validatedUtc.Kind}");
            Console.WriteLine($"Validated UTC Offset: {validatedUtcOffset.Offset}");
        }

        /// <summary>
        /// RunSerializationExample
        /// </summary>
        public static void RunSerializationExample()
        {
            WriteSection("5. Serialization");

            DemoResponse response = new(
                CorrelationId: Guid.NewGuid(),
                CustomerName: "John Doe",
                Status: DemoStatus.Completed,
                OptionalNote: null);

            JsonSerializerOptions options = JsonSerializerConfiguration.CreateDefault();
            string json = JsonSerializer.Serialize(response, options);

            Console.WriteLine("Serialized JSON:");
            Console.WriteLine(json);

            const string inputJson = """
                             {
                               "correlationId": "11111111-1111-1111-1111-111111111111",
                               "customerName": "John Doe",
                               "status": "completed"
                             }
                             """;

            DemoResponse? deserialized = JsonSerializer.Deserialize<DemoResponse>(
                inputJson,
                JsonSerializerConfiguration.Default);

            Console.WriteLine("Deserialized object:");
            Console.WriteLine($"CorrelationId: {deserialized?.CorrelationId}");
            Console.WriteLine($"CustomerName: {deserialized?.CustomerName}");
            Console.WriteLine($"Status: {deserialized?.Status}");
            Console.WriteLine($"OptionalNote: {deserialized?.OptionalNote ?? "<null>"}");
        }

        static void WriteSection(string title)
        {
            Console.WriteLine();
            Console.WriteLine(title);
            Console.WriteLine(new string('-', title.Length));
        }
    }
}
