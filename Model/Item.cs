namespace WaiterBackend.Models{
  public class Item:Base{
    public string name {get;set;}
    public decimal harga{get;set;}
    public string? deskripsi{get;set;}
    public bool IsAvaible{get;set;} = true;
  }
}


