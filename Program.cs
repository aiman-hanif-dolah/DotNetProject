using Microsoft.EntityFrameworkCore;
using DotNetProject.Data;
using DotNetProject.Services;
using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Firestore;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore.Diagnostics;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// Configure Entity Framework Core
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
{
    if (builder.Environment.IsProduction())
    {
        options.UseSqlite(connectionString);
    }
    else
    {
        options.UseSqlServer(connectionString);
    }
    options.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
});

builder.Services.Configure<FirebaseOptions>(builder.Configuration.GetSection("Firebase"));
builder.Services.AddSingleton(sp =>
{
    var options = sp.GetRequiredService<IOptions<FirebaseOptions>>().Value;
    if (string.IsNullOrWhiteSpace(options.ProjectId))
    {
        throw new InvalidOperationException("Firebase project ID is missing.");
    }

    GoogleCredential credential;
    if (!string.IsNullOrWhiteSpace(options.ServiceAccountPath) && File.Exists(options.ServiceAccountPath))
    {
        Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", options.ServiceAccountPath);
        credential = GoogleCredential.FromFile(options.ServiceAccountPath);
    }
    else
    {
        credential = GoogleCredential.GetApplicationDefault();
    }

    FirebaseApp app;
    try
    {
        app = FirebaseApp.GetInstance("FirestoreSync");
    }
    catch (InvalidOperationException)
    {
        app = FirebaseApp.Create(new AppOptions
        {
            Credential = credential,
            ProjectId = options.ProjectId
        }, "FirestoreSync");
    }

    var firestoreClient = new Google.Cloud.Firestore.V1.FirestoreClientBuilder
    {
        Credential = credential
    }.Build();

    return FirestoreDb.Create(options.ProjectId, firestoreClient);
});
builder.Services.AddHostedService<FirestoreSyncService>();

var app = builder.Build();

if (!app.Environment.IsProduction() && !string.IsNullOrWhiteSpace(connectionString))
{
    DbSchemaUpdater.EnsureAuditColumns(connectionString);
}

// Apply pending migrations and seed data on startup
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    dbContext.Database.EnsureCreated();
    if (!app.Environment.IsProduction())
    {
        CsvImporter.ImportAll(dbContext, app.Environment.ContentRootPath);
    }
}

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
