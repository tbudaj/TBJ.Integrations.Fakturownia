using TBJ.Integrations.Fakturownia;
using TBJ.Integrations.Fakturownia.Configuration;
using TBJ.Integrations.Fakturownia.Interfaces;
using TBJ.Integrations.Fakturownia.Models.Invoices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddFakturownia(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "TBJ.Integrations.Fakturownia WebApi",
        Version = "v1",
        Description = "Przykładowe API demonstrujące użycie pakietu TBJ.Integrations.Fakturownia."
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

/// <summary>Pobiera listę faktur z domyślnego konta.</summary>
app.MapGet("/api/invoices", async (IFakturowniaClient fakturownia, int? page) =>
{
    var invoices = await fakturownia.Invoices.GetInvoicesAsync(
        new InvoiceListFilter { Page = page ?? 1 });
    return Results.Ok(invoices);
})
.WithName("GetInvoices")
.WithOpenApi();

/// <summary>Pobiera pojedynczą fakturę po identyfikatorze.</summary>
app.MapGet("/api/invoices/{id}", async (IFakturowniaClient fakturownia, long id) =>
{
    var invoice = await fakturownia.Invoices.GetInvoiceAsync(id);
    return Results.Ok(invoice);
})
.WithName("GetInvoiceById")
.WithOpenApi();

/// <summary>Pobiera listę kontrahentów.</summary>
app.MapGet("/api/clients", async (IFakturowniaClient fakturownia, int? page) =>
{
    var clients = await fakturownia.Clients.GetClientsAsync(page: page ?? 1);
    return Results.Ok(clients);
})
.WithName("GetClients")
.WithOpenApi();

app.Run();
