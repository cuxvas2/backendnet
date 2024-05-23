using Microsoft.EntityFrameworkCore;
using backendnet.Data;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//Agregar soporte para MySQL
var connectionString = builder.Configuration.GetConnectionString("DataContext");
builder.Services.AddDbContext<DataContext>(options => {
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
});

//Agregar soporte para CORS
builder.Services.AddCors(options =>{
    options.AddDefaultPolicy(
        policy => {
            policy.WithOrigins("http://localhost:3001", "http://localhost:8080")
                .AllowAnyHeader()
                .WithMethods("GET", "POST", "PUT", "DELETE");
        });
});

//Funcionalidad a los controladores
builder.Services.AddControllers();
//Agrega la funcionalidad para la documentacion de la API
builder.Services.AddSwaggerGen();

//Construye la aplicacion web
var app = builder.Build();

//Mostraremos la documentacion solo en ambiente de desarrollo
if (app.Environment.IsDevelopment()){
    app.UseSwagger();
    app.UseSwaggerUI();
}

//Utiliza rutas oara los endpoints de los controladores
app.UseRouting();
//Usa Cors con la policy definida anteriormente
app.UseCors();
//Establece el uso de rutas sin especificaruna por default
app.MapControllers();

app.Run();
