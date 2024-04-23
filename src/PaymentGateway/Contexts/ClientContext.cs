namespace PaymentGateway.Contexts;

internal sealed class ClientContext(IHttpContextAccessor httpContextAccessor) : IClientContext
{
    public Guid ClientId => (Guid)httpContextAccessor.HttpContext!.Items["ClientId"]!;
}
