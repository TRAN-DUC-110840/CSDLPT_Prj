## Cách chạy FE và BE

```bash
docker compose up -d
```

FE : http://localhost:5000

BE : http://localhost:5057/api/{endpoint}


```cs
// Add services to the container.

// ===================== Đây là cấu Hình Server gốc =====================

builder.Services.AddDbContext<CSDLPT_API.Context.MyDbContext>(options =>
options.UseSqlServer("Server=26.148.184.54,1433;User Id=sa;Password=anhduc9A@5;Database=CSDLPT_2;Trust Server Certificate=True;"));

builder.Services.AddDbContext<CSDLPT_API.Context.K1DBContext>(options =>
	options.UseSqlServer("Server=26.90.78.94;User Id=sa;Password=123;Database=CSDLPT_2_K1;Trust Server Certificate=True;"));


builder.Services.AddDbContext<CSDLPT_API.Context.K2DBContext>(options =>
	options.UseSqlServer("Server=26.156.200.86,1433;User Id=sa;Password=123;Database=K2_CSDLPT_2;Trust Server Certificate=True;"));

// ===========================================================================================================================================


// ===================== Đây là cấu Hình SERVER BACKUP =====================

// builder.Services.AddDbContext<CSDLPT_API.Context.MyDbContext>(options =>
// options.UseSqlServer("Server=26.124.72.154,1433;User Id=sa;Password=anhduc9A@5;Database=CSDLPT_2;Trust Server Certificate=True;"));

// builder.Services.AddDbContext<CSDLPT_API.Context.K1DBContext>(options =>
// 	options.UseSqlServer("Server=26.90.78.94;User Id=sa;Password=123456;Database=CSDLPT_2_K1_Backup;Trust Server Certificate=True;"));


// builder.Services.AddDbContext<CSDLPT_API.Context.K2DBContext>(options =>
// 	options.UseSqlServer("Server=26.156.200.86,1433;User Id=sa;Password=123;Database=K2_CSDLPT_2_Backup;Trust Server Certificate=True;"));

// ===========================================================================================================================================


```

Lưu ý nha : Nếu 1 trong 3 server ở trên bị lỗi thì sẽ tiến hành note lại  như này và mở cái ở dưới

```cs
// Add services to the container.

// ===================== Đây là cấu Hình Server gốc =====================

// builder.Services.AddDbContext<CSDLPT_API.Context.MyDbContext>(options =>
// options.UseSqlServer("Server=26.148.184.54,1433;User Id=sa;Password=anhduc9A@5;Database=CSDLPT_2;Trust Server Certificate=True;"));

// builder.Services.AddDbContext<CSDLPT_API.Context.K1DBContext>(options =>
// 	options.UseSqlServer("Server=26.90.78.94;User Id=sa;Password=123;Database=CSDLPT_2_K1;Trust Server Certificate=True;"));


// builder.Services.AddDbContext<CSDLPT_API.Context.K2DBContext>(options =>
// 	options.UseSqlServer("Server=26.156.200.86,1433;User Id=sa;Password=123;Database=K2_CSDLPT_2;Trust Server Certificate=True;"));

// ===========================================================================================================================================


// ===================== Đây là cấu Hình SERVER BACKUP =====================

builder.Services.AddDbContext<CSDLPT_API.Context.MyDbContext>(options =>
options.UseSqlServer("Server=26.124.72.154,1433;User Id=sa;Password=anhduc9A@5;Database=CSDLPT_2;Trust Server Certificate=True;"));

builder.Services.AddDbContext<CSDLPT_API.Context.K1DBContext>(options =>
	options.UseSqlServer("Server=26.90.78.94;User Id=sa;Password=123456;Database=CSDLPT_2_K1_Backup;Trust Server Certificate=True;"));


builder.Services.AddDbContext<CSDLPT_API.Context.K2DBContext>(options =>
	options.UseSqlServer("Server=26.156.200.86,1433;User Id=sa;Password=123;Database=K2_CSDLPT_2_Backup;Trust Server Certificate=True;"));

===========================================================================================================================================


```

Sau đó chạy lệnh

```bash

docker compose up -d --build

```