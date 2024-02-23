using Infrastructure.Messaging.Kafka.Configuration;
using Infrastructure.Messaging.Kafka.Configuration.Settings;
using Infrastructure.Serialization.JsonText.Configuration;
using Retail.Api.Configuration;
using Retail.Data;
using Retail.Data.Config;
using Steeltoe.Discovery.Client;

var builder = WebApplication.CreateBuilder(args);
builder.AddLogging();

builder.Services.Configure<KafkaSettings>(builder.Configuration.GetRequiredSection(KafkaSettings.SectionName));
var kafkaSettings = builder.Configuration.GetSection(KafkaSettings.SectionName).Get<KafkaSettings>();

// Add services to the container.

builder.Services.AddControllers(options =>
{
    options.Conventions.Add(new DtoBasedRouteConvention());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwagger();

builder.Services.AddDiscoveryClient(builder.Configuration);
builder.Services.AddRetailDataInfrastructure(builder.Configuration);
builder.Services.AddAppServices();
builder.Services.AddMessaging(kafkaSettings ?? throw new NullReferenceException(nameof(kafkaSettings)));
builder.Services.AddJsonTextSerialization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Retail API V1"));
}

await RetailDbContextSeed.SeedRetailDb(app);

//app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();

app.Run();
