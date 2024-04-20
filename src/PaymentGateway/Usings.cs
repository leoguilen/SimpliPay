global using Microsoft.AspNetCore.Mvc;
global using Microsoft.AspNetCore.Diagnostics;
global using Microsoft.OpenApi.Models;

global using System.Diagnostics;
global using System.Diagnostics.CodeAnalysis;

global using PaymentGateway.Enums;
global using PaymentGateway.Endpoints.Payments;
global using PaymentGateway.Endpoints.Payments.Contracts.Requests;
global using PaymentGateway.Endpoints.Payments.Contracts.Validators;
global using PaymentGateway.Extensions;
global using PaymentGateway.Middlewares;
global using ApplicationBuilderExtensions = PaymentGateway.Extensions.ApplicationBuilderExtensions;

global using FluentValidation;
