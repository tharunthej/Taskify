using Microsoft.EntityFrameworkCore;
using Taskify.Data;
using Taskify.Services.Services;
using Taskify.Services.Interfaces;
using Taskify.MappingProfiles;

var builder = WebApplication.CreateBuilder(args);

// Register AutoMapper by specifying one of mapping profile types.
// This will scan the assembly that contains ProjectProfile for all profiles.
builder.Services.AddAutoMapper(typeof(ProjectProfile));

// Add services to the container
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IProjectMemberService, ProjectMemberService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAttachmentService, AttachmentService>();
builder.Services.AddScoped<ICommentService, CommentService>();

// Configure the database context
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

// Configure the HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();

app.Run();