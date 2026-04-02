using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Unstore.ViewModels;
using Unstorekle.Models;

namespace Unstore.Extensions;

public static class MapClientViewModel
{
    public static IQueryable<EditorClientViewModel> MapEditor(this IQueryable<Client> clients)
    {
        return clients.Select(client => new EditorClientViewModel
        {
            Name = client.Name,
            Email =  client.Email,
            Address = client.Address,
            ContactNumber = client.ContactNumber,
        });
    }
}
public static class MapClientViewModelToModel
{
    public static Client MapModel(this EditorClientViewModel clientViewModel)
    {
        return new Client
        { 
            Id = 0,
            Name = clientViewModel.Name,
            Email = clientViewModel.Email,
            Address = clientViewModel.Address,
            ContactNumber = clientViewModel.ContactNumber
        };
    }
    
    public static IEnumerable<Client> MapModel(this IEnumerable<EditorClientViewModel> clientViewModels)
    {
        List<Client> clients = new();
        foreach (var clientViewModel in clientViewModels)
            clients.Add(new Client
            { 
                Id = 0,
                Name = clientViewModel.Name,
                Email = clientViewModel.Email,
                Address = clientViewModel.Address,
                ContactNumber = clientViewModel.ContactNumber
            });
        return clients;
    }
}