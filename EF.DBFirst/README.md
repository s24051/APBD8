1. Connection issue bo \\ -> \
2. Certyfikaty w łańcuchu mu nie leżą -> TrustServerCertificate; Encrypt 

dotnet ef dbcontext scaffold "Server=.\SQLEXPRESS;Database=apbd8;Integrated Security=True;TrustServerCertificate=Yes;
Encrypt=False" Microsoft.EntityFrameworkCore.SqlServer --output-dir Models --context-dir Context
