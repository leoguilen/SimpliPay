global using Xunit;
global using Xunit.Abstractions;

global using Microsoft.AspNetCore.Hosting;
global using Microsoft.AspNetCore.Mvc.Testing;
global using Microsoft.AspNetCore.TestHost;

global using System.Net;
global using System.Net.Http.Json;

global using AutoFixture;

global using FluentAssertions;

global using PaymentGateway.Features.Payments.Endpoints.Contracts.Requests;
global using PaymentGateway.Integration.Test.Fixtures;

global using Microsoft.Extensions.Configuration;

global using System.Text;

global using Testcontainers.Kafka;
global using Testcontainers.PostgreSql;

global using Bogus;

global using PaymentGateway.Enums;