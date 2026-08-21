using Azure.Storage.Queues;
using Microsoft.Extensions.Azure;
using StorageAccounts.Mvc.Services;

var builder = WebApplication.CreateBuilder(args);

var storageConnString = builder.Configuration["StorageAccountConnString"];
builder.Services.AddAzureClients(c =>
{
    c.AddBlobServiceClient(storageConnString);
    c.AddTableServiceClient(storageConnString);
    c.AddQueueServiceClient(storageConnString).ConfigureOptions(o =>
    {
        o.MessageEncoding = QueueMessageEncoding.Base64;
    });
});

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ITableStorageService, TableStorageService>();
builder.Services.AddScoped<IBlobStorageService, BlobStorageService>();
builder.Services.AddScoped<IQueueService, QueueService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
