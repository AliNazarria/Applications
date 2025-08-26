using System.ComponentModel.DataAnnotations;

namespace Applications.Infrastructure;

public class ConnectionStringsConfigOptions
{
    public const string SectionName = "ConnectionStrings";

    [MinLength(10, ErrorMessage = "Value for {0} must be greater than {1}")]
    public required string Database { get; set; }

    [MinLength(10, ErrorMessage = "Value for {0} must be greater than {1}")]
    public required string Redis { get; set; }

    [MinLength(9, ErrorMessage = "Value for {0} must be greater than {1}")]
    public required string RabbitMQ { get; set; }
}