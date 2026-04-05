using Unstore.Models;

namespace Unstore.DTO;

public class ServiceResponse
{
    public string ClientName { get; set; }
    public string ClientEmail { get; set; }
    public string ClientContact { get; set; }
    
    public string EmployeeName { get; set; }
    public string EmployeeEmail { get; set; }
    public string EmployeePhone { get; set; }
    
    public string Address { get; set; }
    public string Details { get; set; }
   
    public ICollection<ProductResponse> Products { get; set; }
    public ICollection<ToolResponse> Tools { get; set; }
}