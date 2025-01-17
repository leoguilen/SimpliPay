﻿global using Confluent.Kafka;
global using Dapper;
global using FluentValidation;
global using MassTransit;
global using Npgsql;

global using HealthChecks.ApplicationStatus.DependencyInjection;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.AspNetCore.Diagnostics.HealthChecks;
global using Microsoft.AspNetCore.Mvc;
global using Microsoft.Extensions.Diagnostics.HealthChecks;
global using Microsoft.OpenApi.Models;
global using Microsoft.Extensions.Caching.Memory;

global using System.Data;
global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;
global using System.Net.Mime;
global using System.Text.Json;
global using System.Globalization;
global using System.Threading.RateLimiting;

global using PaymentGateway.Features.Payments.Events;
global using PaymentGateway.Features.Payments.Repositories;
global using PaymentGateway.Features.Payments.Repositories.Impl;
global using PaymentGateway.Contexts;
global using PaymentGateway.Features.Payments.Enums;
global using PaymentGateway.Features.Payments.Endpoints;
global using PaymentGateway.Features.Payments.Endpoints.Contracts.Requests;
global using PaymentGateway.Features.Payments.Endpoints.Contracts.Responses;
global using PaymentGateway.Features.Payments.Endpoints.Contracts.Validators;
global using PaymentGateway.Features.Payments.Models;
global using PaymentGateway.Features.Payments.Services;
global using PaymentGateway.Features.Payments.Services.Impl;
global using PaymentGateway.Extensions;
global using PaymentGateway.Middlewares;
global using PaymentGateway.Utils;
global using PaymentGateway.Features.Balances.Enums;
global using PaymentGateway.Features.Balances.Models;
global using PaymentGateway.Features.Balances.Repositories;
global using PaymentGateway.Features.Balances.Repositories.Impl;
global using PaymentGateway.Features.Balances.Services;
global using PaymentGateway.Features.Balances.Services.Impl;
global using PaymentGateway.Features.Balances.Endpoints.Contracts.Responses;
global using ApplicationBuilderExtensions = PaymentGateway.Extensions.ApplicationBuilderExtensions;

