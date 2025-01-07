Solution
│
├── MyBlazorApp (Blazor Server)
│
├── MyApp.Domain (Class Library - Domínio)
│
├── MyApp.Application (Class Library - Aplicação)
│
└── MyApp.Infrastructure (Class Library - Infraestrutura)

✅ Passo 1
Execute os seguintes comandos para criar a solution e os projetos:
dotnet new sln -n MyApp
dotnet new blazorserver -n MyBlazorApp
dotnet new classlib -n MyApp.Domain
dotnet new classlib -n MyApp.Application
dotnet new classlib -n MyApp.Infrastructure

Adicione todos os projetos à solution:
dotnet sln add MyBlazorApp/MyBlazorApp.csproj
dotnet sln add MyApp.Domain/MyApp.Domain.csproj
dotnet sln add MyApp.Application/MyApp.Application.csproj
dotnet sln add MyApp.Infrastructure/MyApp.Infrastructure.csproj

Referencie os projetos conforme o diagrama abaixo:
dotnet add MyApp.Application reference MyApp.Domain
dotnet add MyApp.Infrastructure reference MyApp.Domain
dotnet add MyBlazorApp reference MyApp.Application
dotnet add MyBlazorApp reference MyApp.Infrastructure

 Passo 2: Implementar a Arquitetura DDD
📁 MyApp.Domain (Domínio)
Crie as classes de domínio e interfaces.

Exemplo: Entities/Customer.cs

csharp
Copiar código
namespace MyApp.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public Customer(string name, string email)
        {
            Id = Guid.NewGuid();
            Name = name;
            Email = email;
        }
    }
}
📁 MyApp.Application (Serviços de Aplicação)
Implemente os serviços que o Blazor Server consumirá.

Exemplo: Services/CustomerService.cs

csharp
Copiar código
using MyApp.Domain.Entities;

namespace MyApp.Application.Services
{
    public class CustomerService
    {
        private readonly List<Customer> _customers = new();

        public void AddCustomer(string name, string email)
        {
            var customer = new Customer(name, email);
            _customers.Add(customer);
        }

        public List<Customer> GetAllCustomers()
        {
            return _customers;
        }
    }
}
📁 MyApp.Infrastructure (Infraestrutura)
Essa camada pode conter o acesso a banco de dados ou integrações externas.

✅ Passo 3: Injetar Dependência no Blazor Server
No projeto MyBlazorApp, configure a injeção de dependência.

Abra Program.cs:
MyBlazorApp/Program.cs

csharp
Copiar código
using MyApp.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// Adicione o serviço CustomerService
builder.Services.AddScoped<CustomerService>();

var app = builder.Build();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
✅ Passo 4: Consumir o Serviço no Blazor Component
Crie um componente chamado Customer.razor.

MyBlazorApp/Pages/Customer.razor

razor
Copiar código
@page "/customer"
@inject MyApp.Application.Services.CustomerService CustomerService

<h3>Customer Management</h3>

<input @bind="name" placeholder="Name" />
<input @bind="email" placeholder="Email" />
<button @onclick="AddCustomer">Add Customer</button>

<ul>
    @foreach (var customer in customers)
    {
        <li>@customer.Name (@customer.Email)</li>
    }
</ul>

@code {
    private string name;
    private string email;
    private List<MyApp.Domain.Entities.Customer> customers = new();

    private void AddCustomer()
    {
        CustomerService.AddCustomer(name, email);
        customers = CustomerService.GetAllCustomers();
    }
}
✅ Passo 5: Rodar o Projeto
No terminal, rode o comando:

bash
Copiar código
dotnet run --project MyBlazorApp
Abra o navegador em https://localhost:5001/customer e veja o componente funcionando.

✅ Passo 6: Melhorias Futuras
Implementar um banco de dados (SQLite ou MySQL) na camada Infrastructure.
Adicionar repositórios e interfaces seguindo o padrão Repository Pattern.
Adicionar validações e regras de negócio na camada Domain.