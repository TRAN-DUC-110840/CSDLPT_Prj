using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CSDLPT_API.Entities;
using CSDLPT_API.Context;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<CSDLPT_API.Context.MyDbContext>(options =>
options.UseSqlServer("Server=26.148.184.54;User Id=sa;Password=anhduc9A@5;Database=CSDLPT;Trust Server Certificate=True;"));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🟢 Thêm cấu hình CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// 🟢 Cấu hình Authentication JWT
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtSecret = jwtSection["Secret"] ?? "dev-secret-change-me-please-very-long";
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services
	.AddAuthentication(options =>
	{
		options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
		options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
	})
	.AddJwtBearer(options =>
	{
		options.RequireHttpsMetadata = false;
		options.SaveToken = true;
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidIssuer = jwtSection["Issuer"],
			ValidAudience = jwtSection["Audience"],
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = key,
			ClockSkew = TimeSpan.Zero
		};
	});

var app = builder.Build();

// 🟢 Bật CORS ngay sau khi app được build
app.UseCors("AllowAll");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// 🟢 Seed tài khoản admin nếu chưa có
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<MyDbContext>();
	// Tạo bảng Users nếu chưa tồn tại (không ảnh hưởng dữ liệu hiện có)
	var createUsersTableSql = @"
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
BEGIN
	CREATE TABLE [dbo].[Users](
		[Id] INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
		[Username] NVARCHAR(100) NOT NULL,
		[PasswordHash] NVARCHAR(255) NOT NULL,
		[Role] INT NOT NULL,
		[IsApproved] BIT NOT NULL CONSTRAINT DF_Users_IsApproved DEFAULT(1)
	);
	CREATE UNIQUE INDEX [IX_Users_Username] ON [dbo].[Users]([Username]);
END
";
	db.Database.ExecuteSqlRaw(createUsersTableSql);
	// Đảm bảo cột IsApproved tồn tại nếu bảng đã có từ trước
	var addIsApprovedColumnSql = @"
IF EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Users]') AND type in (N'U'))
AND NOT EXISTS (
	SELECT * FROM sys.columns 
	WHERE Name = N'IsApproved' AND Object_ID = Object_ID(N'[dbo].[Users]')
)
BEGIN
	ALTER TABLE [dbo].[Users] ADD [IsApproved] BIT NOT NULL CONSTRAINT DF_Users_IsApproved DEFAULT(1);
	-- Giữ nguyên giá trị mặc định là 1 (đã duyệt) cho các user hiện có
END
";
	db.Database.ExecuteSqlRaw(addIsApprovedColumnSql);
	var adminUsername = "admin@example.com";
	if (!db.Users.Any(u => u.Username == adminUsername))
	{
		db.Users.Add(new User
		{
			Username = adminUsername,
			PasswordHash = BCrypt.Net.BCrypt.HashPassword("anhduc9A@5"),
			Role = Role.Admin
		});
		db.SaveChanges();
	}
}

app.Run();
